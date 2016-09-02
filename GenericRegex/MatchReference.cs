using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    struct MatchReference
    {
        public int StartIndex { get; }
        public int Length { get; }

        public MatchReference(int startIndex, int length)
        {
            StartIndex = startIndex;
            Length = length;
        }
    }
}
