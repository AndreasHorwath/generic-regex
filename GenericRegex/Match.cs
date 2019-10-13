using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public class Match<T>
    {
        internal Match(int startIndex, int length, IEnumerable<T> elements)
        {
            StartIndex = startIndex;
            Length = length;
            Elements = elements.ToList();
        }

        public int StartIndex { get; }

        public int EndIndex => StartIndex + Length;

        public int Length { get; }

        public IReadOnlyList<T> Elements { get; }
    }
}
