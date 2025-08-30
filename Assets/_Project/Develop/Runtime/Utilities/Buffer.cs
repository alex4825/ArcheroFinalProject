using static UnityEditor.Progress;

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
    }
}
