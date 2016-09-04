using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public class IndexedSequence<T>
    {
        public IndexedSequence(IEnumerable<T> sequence)
        {
            enumerator = sequence.GetEnumerator();
        }

        readonly IEnumerator<T> enumerator;
        readonly List<T> list = new List<T>();
        bool isEndOfSequence;

        public IReadOnlyList<T> CurrentList => list;

        public bool TryGetElementAt(int index, out T element)
        {
            if (ReadElementsThroughIndex(index))
            {
                element = list[index];
                return true;
            }

            element = default(T);
            return false;
        }

        public IEnumerable<T> GetSubsequence(int startIndex, int length)
        {
            int endIndex = startIndex + length;
            for (int index = startIndex; index < endIndex; index++)
            {
                T element;
                if (TryGetElementAt(index, out element))
                {
                    yield return element;
                }
                else
                {
                    break;
                }
            }
        }

        bool ReadElementsThroughIndex(int index)
        {
            if (isEndOfSequence)
            {
                return index < list.Count;
            }

            while (list.Count <= index)
            {
                isEndOfSequence = !enumerator.MoveNext();
                if (isEndOfSequence)
                {
                    return false;
                }

                list.Add(enumerator.Current);
            }

            return true;
        }

        //public T this[int index]
        //{
        //    get
        //    {
        //        if (!ReadElementsThroughIndex(index))
        //        {
        //            throw new InvalidOperationException($"IsEndOfSequence: index = {index} > {list.Count}");
        //        }

        //        return list[index];
        //    }
        //}
    }
}
