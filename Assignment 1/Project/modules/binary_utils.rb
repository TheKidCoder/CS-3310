require 'rubygems'
require 'bindata'

module BinaryUtils
  class BinaryWriter
    @@data_actual = []
    def initialize
    end
    
    def build_header(n, rootPtr, maxID)
      @N = BinData::Int16le.new(n)
      @rootPtr = BinData::Int16le.new(rootPtr)
      @maxID = BinData::Int16le.new(maxID)
    end
    
    def build_actual_data(row)
      if row != nil
        country_code = BinData::String.new(:read_length => 3)
        country_code.assign(row[1])
        
        name = BinData::String.new(:read_length => 17)
        name.assign(row[2])
        
        continent = BinData::String.new(:read_length => 11)
        continent.assign(row[3])
        
        region = BinData::String.new(:read_length => 10)
        region.assign(row[4])    
        
        @@data_actual << [
              BinData::Int16le.new(row[0].to_i),
              country_code,
              name,
              continent,
              region,
              BinData::Int32le.new(row[5].to_i),
              BinData::Int16le.new(row[6].to_i),
              BinData::Int64le.new(row[7].to_i),
              BinData::FloatLe.new(row[8].to_i),
              BinData::Int32le.new(row[9].to_i),
          ]
        end
    end
    
    def save
      File.open("backup.bin", "w") do |io|
        #Write Header
        @N.write(io)
        @rootPtr.write(io)
        @maxID.write(io)
        #Write Code Index
        
        @@data_actual.each do |record|
          record.each do |column|
            column.write(io)
          end
        end
      end
    end
  end
end