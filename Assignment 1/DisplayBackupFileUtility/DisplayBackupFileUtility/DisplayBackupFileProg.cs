/* PROGRAM:  DisplayBackupFileProg
 * USED BY:  asgn 1's TestDriveProg (or manually by the developer)
 * DESCRIPTION:  This program reads the BINARY file, Backup.bin, and pretty-prints it to Log.txt file
 *      (appending it to the existinge, if there is one).
 *      It does not use OOP since it's just a quick utility writtem for the developer to view their
 *      file (vs. having to use a hex-edit or dump software for viewing a binary file).
 * ASSUMPTIONS:
 *          - 2-byte short integers (C#'s short), 4-byte regular integers (C#'s int),
 *                  8-byte long integers (C#'s long)
 *          - 4-byte floating point numbers (C#'s float, not double)
 *          - FIXED-LENGTH "string" fields (with length hard-coded in program as specified in the
 *                  requirement specs) - i.e., char arrays of 8-bit ASCII char's without
 *                  preceeding length fields nor null terminators)
 * *****************************************************************************************
 */
using System;
using System.IO;

namespace DisplayBackupFileUtility
{
    class DisplayBackupFileProg
    {
        static void Main(string[] args)
        {
            // > > > >  FIX PATH FOR YOUR PROJECT  < < < <
            string path = ".//..//..//..//";       // i.e., top-level project directory
            string binFileName = "Backup.bin";

            //MakeDummyTestFile.CreateTheFile(path, binFileName);

            StreamWriter log = new StreamWriter(path + "Log.txt", true);

            FileStream fileStream = new FileStream(path + binFileName, FileMode.Open,
                FileAccess.Read);
            BinaryReader binFile = new BinaryReader(fileStream);

            // READ AND DISPLAY HEADER RECORD
            short n = binFile.ReadInt16();
            short rootPtr = binFile.ReadInt16();
            short maxId = binFile.ReadInt16();
            log.WriteLine("N is {0}, RootPtr is {1}, MaxId is {2}\r\n",
                n, rootPtr, maxId);

            if (n != 0)
            {
                ShowIndexRecs(binFile, log, n);
                ShowActualDataRecs(fileStream, binFile, log, n, maxId);
                // this method needs fileStream so it can do random access
            }
            binFile.Close();
            log.Close();
        }
        //*******************************************************************************
        // There are exactly N records for the index.
        //-------------------------------------------------------------------------------
        private static void ShowIndexRecs(BinaryReader binFile, StreamWriter log,
                short n)
        {
            int lChPtr, rChPtr;
            char[] code = new char[3];
            int drp;

            log.WriteLine("--------------------------------------------------------");
            log.WriteLine("THE CODE INDEX (up to the 1st 26 entries)\r\n");

            int nToDisplay = n;
            if (n > 26)
                nToDisplay = 26;

            for (int i = 0; i < nToDisplay; i++)
            {
                lChPtr = binFile.ReadInt16();
                code = binFile.ReadChars(3);
                drp = binFile.ReadInt16();
                rChPtr = binFile.ReadInt16();
                log.WriteLine("[{0:000}]  {1:000;-00;000} | {2} | {3:000} | {4:000;-00;000} |",
                    i, lChPtr, new string(code), drp, rChPtr);
            }
            if (n > 26)
                log.WriteLine("\r\n. . . rest of BST nodes here . . .\r\n");
        }
        //*******************************************************************************
        // There are exactly maxId + 1 records for the actual data (rather than n) because
        //      there are some empty locations (including location 0), and location
        //      numbers go from 0 to maxId (not 0 to maxId - 1).
        //-------------------------------------------------------------------------------
        private static void ShowActualDataRecs(FileStream fileStr, BinaryReader binFile,
                StreamWriter log, short n, short maxId)
        {
            // THESE ARE NEEDED TO CALCULATE THE OFFSET FOR DOING RANDOM ACCESS
            const int sizeOfHeaderRec = 2 + 2 + 2;
            const int sizeOfIndexRec = 2 + 3 + 2 + 2;
            const int sizeOfDataRec = 2 + 3 + 17 + 11 + 10 + 4 + 2 + 8 + 4 + 4;

            // SKIP TO THE START OF THE ACTUAL DATA
            int offset = sizeOfHeaderRec + (n * sizeOfIndexRec);
            fileStr.Seek(offset, SeekOrigin.Begin);

            log.WriteLine("\r\n--------------------------------------------------------");
            log.WriteLine("THE ACTUAL DATA (including empty locations)\r\n");
            log.WriteLine("The first few records (with their location #'s):");

            int maxLocToDisplay = 8;
            if (maxId < maxLocToDisplay)
                maxLocToDisplay = maxId;

            for (int i = 0; i <= maxLocToDisplay; i++)
                ReadPrintDataRec((short)i, binFile, log);

            if (maxId > maxLocToDisplay)
            {
                log.WriteLine("\r\n. . . next records would be here (if there are some) . . .\r\n");

                if (maxId > maxLocToDisplay + 1)
                {
                    log.WriteLine("The 2nd from the last record:");

                    // SKIP TO SECOND FROM LAST ACTUAL DATA RECORD  
                    offset = sizeOfHeaderRec + (n * sizeOfIndexRec) + ((maxId - 1) * sizeOfDataRec);
                    fileStr.Seek(offset, SeekOrigin.Begin);

                    ReadPrintDataRec((short)(maxId - 1), binFile, log);
                }
                log.WriteLine("The last record:");

                // SKIP TO LAST ACTUAL DATA RECORD  
                offset = sizeOfHeaderRec + (n * sizeOfIndexRec) + ((maxId) * sizeOfDataRec);
                fileStr.Seek(offset, SeekOrigin.Begin);

                ReadPrintDataRec((short)(maxId), binFile, log);
            }
        }
        //-------------------------------------------------------------------------------
        private static void ReadPrintDataRec(short i, BinaryReader f, StreamWriter log)
        {
            short id;
            char[] code = new char[3];
            char[] name = new char[17];
            char[] continent = new char[11];
            char[] region = new char[10];
            int surfaceArea;
            short yearOfIndep;
            long population;
            float lifeExp;
            int gnp;

            id = f.ReadInt16();
            code = f.ReadChars(3);
            name = f.ReadChars(17);
            continent = f.ReadChars(11);
            region = f.ReadChars(10);
            surfaceArea = f.ReadInt32();
            yearOfIndep = f.ReadInt16();
            population = f.ReadInt64();
            lifeExp = f.ReadSingle();
            gnp = f.ReadInt32();
            if (id == 0)
                log.WriteLine("[{0:000}] EMPTY SPOT", i);
            else
                log.WriteLine("[{0:000}] {1:000}|{2}|{3}|{4}|{5}|" +
                    "{6,10:N0}|{7,5}|{8,13:N0}|{9,4:N1}|{10,9:N0}|",
                    i, id, new string(code), new string(name),
                    new string(continent), new string(region),
                    surfaceArea, yearOfIndep, population, lifeExp, gnp);
        }
    }
}
