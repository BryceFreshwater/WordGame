using System;
using System.Collections.Generic;

namespace PriorityQueue
{
    public class MinHeap<T> where T : IComparable<T>
    {
        private List<T> nodeList = new List<T>();
        public int Count { get { return nodeList.Count; } }

        public int NumCompares { get; private set; }
        public int NumMoves { get; private set; }

        public void Add(T element)
        {
            // add element at end
            nodeList.Add(element);

            // move up as far as possible
            MoveUp(nodeList.Count - 1);
        }

        public T RemoveMin()
        {
            T ret = default(T);
            // We could just let this throw an exception rather than checking Count
            if (nodeList.Count > 0)
            {
                // set the return value to the top of the heap
                ret = nodeList[0];

                // move the last element to the top
                nodeList[0] = nodeList[nodeList.Count - 1];
                NumMoves++;
                // get rid of the bottom entry in the array
                nodeList.RemoveAt(nodeList.Count - 1);

                // move the top element down as far as possible
                MoveDown(0);
            }

            return ret;
        }

        public T Peek()
        {
            return nodeList[0];
        }

        public void ClearStats()
        {
            NumCompares = 0;
            NumMoves = 0;
        }

        private int getLeftChildNdx(int parent)
        {
            return 2 * parent + 1;
        }

        private int getRightChildNdx(int parent)
        {
            return 2 * parent + 2;
        }

        private int getParentNdx(int child)
        {
            return (child - 1) / 2;
        }

        private void swap(int ndx1, int ndx2)
        {
            NumMoves += 2;
            T tmp = nodeList[ndx1];
            nodeList[ndx1] = nodeList[ndx2];
            nodeList[ndx2] = tmp;
        }


        public void MoveDown(int parent)
        {
            int leftChild = getLeftChildNdx(parent);
            int rightChild = getRightChildNdx(parent);
            int min = parent; // assume parent is minimum of (parent, left, right)

            // if we are at a leaf, we're done
            if (leftChild >= Count)
                return;

            // We want to find out which child is the smallest

            NumCompares++;
            if (nodeList[leftChild].CompareTo(nodeList[parent]) < 0)
                min = leftChild; // for now, left is less

            // not an else; we use the previous min value
            NumCompares++;
            if ((rightChild < Count) &&  (nodeList[rightChild].CompareTo(nodeList[min]) < 0))
                min = rightChild; // we have a right child and it is smaller than the current min

            // if the parent was not the smallest, move it down
            if (min != parent)
            {
                swap(min, parent);
                // let the child determine whether we need to continue
                MoveDown(min);
            }
        }

        private void MoveUp(int child)
        {
            // calculate our parents index based on our index
            int parent = getParentNdx(child);

            // if our value is less than that of our parent, we need to move the value up
            // otherwise we are done.
            NumCompares++;
            if (nodeList[child].CompareTo(nodeList[parent]) < 0)
            {
                swap(child, parent);
                // let our parent decide whether it needs to go up further
                MoveUp(parent);
            }
        }
    }
}
