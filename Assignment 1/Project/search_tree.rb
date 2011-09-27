class BinarySearchTree
 
   @root = nil
   
   public 
 
      def initialize
      end
 
      def find(target)
         self[locate(target, @root)]
      end
 
      def insert(element)
         @root = put(element, @root)
      end
 
      def print
        output(@root)
      end
 
   private
   #begin: private members
 
      def [](leaf)
         return leaf.contents unless leaf.nil?
         nil
      end
 
      def locate(target, leaf)
         if leaf.nil? then return nil
         elsif leaf.contents < target then return locate(target, leaf.right)
         elsif leaf.contents > target then return locate(target, leaf.left)
         elsif leaf.contents == target then return leaf
         end
      end   
 
      def put(element, leaf)
         if leaf.nil? then leaf = BinaryNode.new(element, nil, nil)
         elsif leaf.contents < element then leaf.right = put(element, leaf.right)
         elsif leaf.contents > element then leaf.left  = put(element, leaf.left)
         end
         return leaf
      end
      
      def to_a

        
      end
 
      def output(leaf)
         if (!leaf.nil?) then
            output(leaf.left)
            puts leaf.contents
            output(leaf.right)
         end
      end   
 
   #end: private members
 
end



class BinaryNode
 
   @contents = nil
   @left = nil
   @right = nil
 
   public
      def initialize(data, leftChild, rightChild)
         @contents = data
         @left = leftChild
         @right = rightChild
      end
 
      def contents
         @contents
      end
 
      def left
         @left
      end
 
      def right
         @right
      end
 
      def contents=(data)
         @contents = data
      end
 
      def left=(leaf)
         @left = leaf
      end
 
      def right=(leaf)
         @right = leaf
      end
 
end