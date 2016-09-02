using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public class Match<T>
    {
        public int StartIndex { get; }
        public int Length { get; }
        public IList<T> Elements { get; }

        internal Match(int startIndex, int length, IList<T> elements)
        {
            StartIndex = startIndex;
            Length = length;
            Elements = elements;
        }
    }

}
