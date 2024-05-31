using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue
{
    public class PriorityQueue<T>
    {
        internal class Node : IComparable<Node>
        {
            public int Priority;
            public T O;
            public int CompareTo(Node other)
            {
                return Priority.CompareTo(other.Priority);
            }
        }

        private MinHeap<Node> minHeap = new MinHeap<Node>();
        public int Count { get { return minHeap.Count; } }

        public void Add(int priority, T element)
        {
            minHeap.Add(new Node() { Priority = priority, O = element });
        }

        public T Remove()
        {
            return minHeap.RemoveMin().O;
        }

        public T Peek()
        {
            return minHeap.Peek().O;
        }

    }

    public class PriorityQueue
    {
        static void mMain(string[] args)
        {
            PriorityQueue<string> myQueue = new PriorityQueue<string>();
            myQueue.Add(10, "ten");
            myQueue.Add(8, "eight");
            myQueue.Add(6, "six");
            myQueue.Add(12, "twelve");
            myQueue.Add(4, "four");
            myQueue.Add(9, "nine");
            myQueue.Add(110, "1-ten");
            myQueue.Add(108, "1-eight");
            myQueue.Add(106, "1-six");
            myQueue.Add(112, "1-twelve");
            myQueue.Add(104, "1-four");
            myQueue.Add(109, "1-nine");
            while (myQueue.Count > 0)
            {
                Console.WriteLine(myQueue.Remove());
            }
            Console.ReadLine();
        }
    }
}
