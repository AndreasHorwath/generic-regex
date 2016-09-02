using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class RepetitionExpr<T> : PatternExpr<T>
    {
        public PatternExpr<T> Expression { get; }
        public int MinOccur { get; }
        public int MaxOccur { get; }
        public bool Greedy { get; private set; }

        public RepetitionExpr(PatternExpr<T> expression, int minOccur, int maxOccur, bool greedy)
        {
            Expression = expression;
            MinOccur = minOccur;
            MaxOccur = maxOccur;
            Greedy = greedy;
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            return Match(context, 0);
        }

        IEnumerable<MatchContext<T>> Match(MatchContext<T> context, int position)
        {
            if (position <= MaxOccur)
            {
                if (position >= MinOccur && !Greedy)
                {
                    yield return context;
                }

                foreach (var newContext in Expression.Match(context))
                {
                    foreach (var resultingContext in Match(newContext, position + 1))
                    {
                        yield return resultingContext;
                    }
                }

                if (position >= MinOccur && Greedy)
                {
                    yield return context;
                }
            }
        }

        public override string ToString()
        {
            var greedyText = Greedy ? "greedy" : "non-greedy";
            var maxText = MaxOccur == int.MaxValue ? "∞" : MaxOccur.ToString();
            return $"Rep({Expression}, {MinOccur}-{maxText}, {greedyText})";
        }
    }
}
