/* CLASS:  MakeDummyTestFile    used by   DisplayBackupFileProg
 * DESCRIPTION:  
 *      This program produces a "dummy" BINARY file, Backup.bin, used to test my utility program.
 *      The data in this file:
 *          1) is in the CORRECT FORMAT in terms of data types, field sizes, the order of the
 *              3 different parts of the file (header, code index, actual data), spacing, 
 *              no <CR><LF>'s, etc.
 *      BUT 2) the DATA ITSELF is NOT CORRECT in terms of actual data values (for asgn 1)- e.g.,
 *          - the records are in the wrong spots for the actual data (i.e., not Direct Address)
 *          - the left & right child ptrs and data ptrs are all wrong in the BST
 *          - the header records' data isn't all correct for the data.
 * *****************************************************************************************
 */
using System;
using System.IO;

namespace DisplayBackupFileUtility
{
    class MakeDummyTestFile
    {
        public static void CreateTheFile(string path, string binfileName)
        {
            FileStream fileStream = new FileStream(path + binfileName, FileMode.Create,
                FileAccess.Write);
            BinaryWriter f = new BinaryWriter(fileStream);

            // WRITE FAKE HEADER RECORD
            const int fakeN = 26;
            const int fakeMaxId = fakeN + 4;
            f.Write(Convert.ToInt16(fakeN));                // n
            f.Write(Convert.ToInt16(0));                    // rootPtr
            f.Write(Convert.ToInt16(fakeMaxId));            // maxId
            
            // USE THE CountryData FILE TO GET VALUES FOR INDEX & ACTUAL DATA RECORDS
            StreamReader inFile = new StreamReader(path + "AToZCountryData.csv");
            string aLine;
            string[] field = new string[15];
            char[] code = new char[3];

            int sizeOfDataRec = 2 + 3 + 17 + 11 + 10 + 4 + 2 + 8 + 4 + 4;
            char[] all0Bits = new char[sizeOfDataRec];
 
            // WRITE FAKE CODE INDEX
            for (int i = 0; i < fakeN; i++)
            {
                if (i % 10 == 4 || i % 10 == 7)             // leftChPtr (fake)
                    f.Write(Convert.ToInt16(-1));
                else
                    f.Write(Convert.ToInt16(i));
                aLine = inFile.ReadLine();
                field = aLine.Split(',');
                f.Write(field[1].ToCharArray());            // code (from file)
                f.Write(Convert.ToInt16(field[0]));         // dataPtr (i.e., id from file)
                if (i % 10 == 3 || i % 10 == 5)             // rightChPtr (fake)
                    f.Write(Convert.ToInt16(-1));
                else
                    f.Write(Convert.ToInt16(i * 10));
            }
            // WRITE FAKE ACTUAL DATA
            inFile.Close();
            inFile = new StreamReader(path + "AToZCountryData.csv"); // start at front again

            for (int i = 0; i < fakeN; i++)
            {
                // WRITE 4 "RANDOM" EMPTY LOCATIONS PLUS AT LOCATION 0
                if (i == 0 || i == 2 || i == 5 || i == 6 || i == 17)
                    f.Write(all0Bits);

                // WRITE fakeN GOOD RECORDS BASED ON DATA FROM THE CountryData FILE
                aLine = inFile.ReadLine();
                field = aLine.Split(',');
        
                f.Write(Convert.ToInt16(field[0]));             // id
                f.Write(field[1].ToCharArray());
                f.Write(field[2].PadRight(17).Substring(0, 17).ToCharArray());
                f.Write(field[3].PadRight(11).Substring(0, 11).ToCharArray());
                f.Write(field[4].PadRight(10).Substring(0, 10).ToCharArray());
                for (int j = 5; j < 10; j++)
                    if (field[j] == "NULL")
                        field[j] = "0";
                f.Write(Convert.ToInt32(field[5]));             // surfaceArea
                f.Write(Convert.ToInt16(field[6]));             // yearOfIndep
                f.Write(Convert.ToInt64(field[7]));             // population
                f.Write(Convert.ToSingle(field[8]));            // lifeExp
                f.Write(Convert.ToInt32(field[9]));             // gnp          
            }
            inFile.Close();
            fileStream.Close();
            Console.WriteLine("OK, the dummy BINARY file was just created");
            Console.WriteLine("    <Hit Enter to quit>");
            Console.ReadLine();
        }
    }
}
