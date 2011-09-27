#System Requires
require 'rubygems'
require 'csv'
require 'logger'
#require bindata

#Program Requires
require './search_tree.rb'
#Require all Modules
Dir["./modules/*.rb"].each {|file| require file }

class SetupProg
  include BinaryUtils
  #Set our files.
  @data_file = File.open("./data/AtoZCountryData.csv", 'r')
  @log = Logger.new("./logs/prog_out.log")
  
  #Set our data structures.
  @actual_data = []
  @tree = BinarySearchTree.new
  
  #Set our BinaryWriter
  @binary_builder = BinaryUtils::BinaryWriter.new
  
  def self.process_csv
    #Read through the CSV file rows.
    CSV.foreach(@data_file) do |row|
      #Build ActualData by calling method and sending the current row.
      create_DA(row)
      @log.info {"Added #{row[1]} to data array with the id of #{row[0]}"}
      #Insert code into tree.
      @tree.insert(row[1])
      @log.info {"Added #{row[1]} to BST."}
    end
  end
  
  def self.create_DA(row)
    id = row[0].to_i
    countryCode = row[1]
    countryName = row[2]
    continent = row[3]
    region = row[4]
    surfaceArea = row[5]
    yearOfIndep = row[6]
    population = row[7]
    lifeExpectancy = row[8]
    gnp = row[9]
    
    @actual_data[id] = [id, countryCode, countryName, continent, region, surfaceArea, yearOfIndep, population, lifeExpectancy, gnp]
  end
  
  #Equivalent to a Java Main()
  if __FILE__ == $0
    process_csv
    @binary_builder.build_header(@actual_data.length, 0, @actual_data.length)
    @actual_data.each do |record|
      @binary_builder.build_actual_data(record)
    end
    @binary_builder.save
  end
  
  
end