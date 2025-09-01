using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Utilities
{
    public class Buffer<T>
    {
        public T[] Items;
        public int Count;

        public Buffer(int initialSize)
        {
            Items = new T[initialSize];
            Count = 0;
        }

        public void Clear()
        {
            for (int i = 0; i < Items.Length; i++)
                Items[i] = default(T);

            Count = 0;
        }

        public bool TryRemove(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(Items[i], item))
                {
                    RemoveItemAt(i);
                    return true;
                }
            }

            return false;
        }

        public void RemoveItemAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException($"{nameof(index)}");

            for (int i = index; i < Count - 1; i++)
            {
                Items[i] = Items[i + 1];
            }

            Items[Count - 1] = default(T);
            Count--;
        }
    }
}
