require 'rubygems'

class Node
  attr_reader :key, :left, :right

  def initialize(key, left = nil, right = nil)
    @key = key
    @left = left
    @right = right
  end

  def insert(key)
    #Compare the passed key to the
    if key < @key
      # insert left
      @left.nil? ? @left = Node.new(key) : @left.insert(key)
    elsif key > @key
      # insert right
      @right.nil? ? @right = Node.new(key) : @right.insert(key)
    end
  end

end

class Tree
  attr_reader :root

  def initialize
    @root = nil
  end
  
  
  def insert(key)
    if @root.nil?
      @root = Node.new(key)
    else
      @root.insert(key)
    end
    self
  end

end
