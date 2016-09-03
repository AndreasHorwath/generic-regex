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

        IEnumerator<T> enumerator;
        readonly List<T> list = new List<T>();
        bool isEndOfSequence;

        public IReadOnlyList<T> CurrentList => list;

        public bool ReadElementsThroughIndex(int index)
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

        public T this[int index]
        {
            get
            {
                if (!ReadElementsThroughIndex(index))
                {
                    throw new InvalidOperationException($"IsEndOfSequence: index = {index} > {list.Count}");
                }

                return list[index];
            }
        }
    }
}
