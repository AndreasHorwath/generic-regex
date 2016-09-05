using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericRegex
{
    public class IndexedSequence<T>
    {
        public IndexedSequence(IEnumerable<T> sequence)
        {
            enumerator = sequence.GetEnumerator();
        }

        readonly IEnumerator<T> enumerator;
        readonly List<T> buffer = new List<T>();
        bool isEndOfSequence;

        public bool TryGetElementAt(int index, out T element)
        {
            if (FillBuffer(index + 1))
            {
                element = buffer[index];
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

        bool FillBuffer(int size)
        {
            if (isEndOfSequence)
            {
                return buffer.Count >= size;
            }

            while (buffer.Count < size)
            {
                isEndOfSequence = !enumerator.MoveNext();
                if (isEndOfSequence)
                {
                    return false;
                }

                buffer.Add(enumerator.Current);
            }

            return true;
        }
    }
}
