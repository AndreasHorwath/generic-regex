using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public abstract class ExpressionBase<T>
    {
        public static implicit operator ExpressionBase<T>(T value) => Matcher<T>.Val(value);

        public static implicit operator ExpressionBase<T>(Func<T, bool> predicate) => Matcher<T>.Pred(predicate);

        public ExpressionBase<T> WithId(int expressionId)
        {
            return new WithIdExpression<T>(this, expressionId);
        }

        public IEnumerable<MatchResult<T>> FindMatchesIn(IEnumerable<T> sequence)
        {
            var indexedSequence = new IndexedSequence<T>(sequence);
            var context = new MatchContext<T>(indexedSequence);

            while (!context.IsEndOfSequence)
            {
                var startIndex = context.Index;
                var newContext = Match(context).FirstOrDefault();
                if (newContext != null)
                {
                    yield return new MatchResult<T>(newContext, startIndex);
                }

                int newIndex = (newContext != null && newContext.Index > context.Index) ? newContext.Index : context.Index + 1;
                context = new MatchContext<T>(indexedSequence).WithIndex(newIndex);
            }
        }

        internal abstract IEnumerable<MatchContext<T>> Match(MatchContext<T> context);
    }
}
