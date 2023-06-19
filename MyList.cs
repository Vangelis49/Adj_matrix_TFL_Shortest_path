using System;
using System.Collections.Generic;
using System.Text;

namespace TFLShortestPathFinder
{
    internal class MyList<T>
    {
        //_count: A private integer field that keeps track of the number of elements in the list
        //count: A public property to get and set the _count field value
        //items: A private array of type T[] that holds the elements of the list
        //this[int index]: An indexer property that allows access to the elements of the list using an index
        private int _count;
        public int count { get { return _count; } set { _count = value; } }
        private T[] items;
        //this added after
        //public int Length
        //{
        //    get { return _size; }
        //}
        //and this 
        public int Size
        {
            get { return count; }
        }


        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                {
                    throw new IndexOutOfRangeException();
                }
                return items[index];
            }
            set
            {
                if (index < 0 || index >= count)
                {
                    throw new IndexOutOfRangeException();
                }
                items[index] = value;
            }
        }
        //DEFAULT_CAPACITY: A constant integer representing the default capacity of the list (4 in this case)
        private const int DEFAULT_CAPACITY = 4;

        //MyList(): Initializes the items array with the default capacity and sets the count to 0.
        public MyList()
        {
            this.items = new T[DEFAULT_CAPACITY];
            this.count = 0;
        }

        //Count(): Returns the number of elements in the list (same as the count property).
        public int Count()
        {
            return count;
        }

        //Add(T item): Adds an item to the list. If the list is full, it resizes the items array by doubling its capacity
        public void Add(T item)
        {
            if (count == items.Length)
            {
                T[] newItems = new T[items.Length * 2];
                for (int i = 0; i < count; i++)
                {
                    newItems[i] = items[i];
                }
                items = newItems;
            }
            items[count++] = item;
        }


        //Indexof(T target): Searches for the specified target and returns the index of the first occurrence within the list. If the target is not found, it returns -1.
        public int Indexof(T target)
        {
            for (int i=0; i < count; i++)
            {
                if (Equals(items[i], target))
                {
                    return i;
                }
               
            }
            return -1;
        }

        //Reverse(): Reverses the order of the elements in the list
        public void Reverse()
        {
            int leftIndex = 0;
            int rightIndex = count - 1;
            while (leftIndex < rightIndex)
            {
                T temp = items[leftIndex];
                items[leftIndex] = items[rightIndex];
                items[rightIndex] = temp;
                leftIndex++;
                rightIndex--;
            }
        }

        //Exists(Func<T, bool> predicate): Determines whether the list contains elements that match the conditions defined by the specified predicate.
        public bool Exists(Func<T, bool> predicate)
        {
            for (int i = 0; i < count; i++)
            {
                if (predicate(items[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            for (int i = 0; i < Count(); i++)
            {
                if (predicate(this[i]))
                {
                    return this[i];
                }
            }

            return default(T);
        }


    }
}
