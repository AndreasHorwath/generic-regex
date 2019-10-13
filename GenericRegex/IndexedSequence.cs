using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GenericRegex
{
    public class IndexedSequence<T>
    {
        public IndexedSequence(IEnumerable<T> sequence)
        {
            _enumerator = sequence.GetEnumerator();
        }

        readonly IEnumerator<T> _enumerator;

        readonly List<T> _buffer = new List<T>();

        bool _isEndOfSequence;

        public bool TryGetElementAt(int index, out T element)
        {
            if (FillBuffer(index + 1))
            {
                element = _buffer[index];
                return true;
            }

            element = default!; // may be null if T is nullable
            return false;
        }

        public IEnumerable<T> GetSubsequence(int startIndex, int length)
        {
            int endIndex = startIndex + length;
            for (int index = startIndex; index < endIndex; index++)
            {
                if (TryGetElementAt(index, out T element))
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
            if (_isEndOfSequence)
            {
                return _buffer.Count >= size;
            }

            while (_buffer.Count < size)
            {
                _isEndOfSequence = !_enumerator.MoveNext();
                if (_isEndOfSequence)
                {
                    return false;
                }

                _buffer.Add(_enumerator.Current);
            }

            return true;
        }
    }
}
