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

        public ExpressionBase<T> WithId(int groupId) => new WithIdExpression<T>(this, groupId);

        public MatchResult<T> Match(IEnumerable<T> sequence) => FindMatchesIn(sequence).FirstOrDefault() ?? new MatchResult<T>(null);

        public IEnumerable<MatchResult<T>> FindMatchesIn(IEnumerable<T> sequence)
        {
            //var list = (sequence as IReadOnlyList<T>) ?? new List<T>(sequence);

            var indexedSequence = new IndexedSequence<T>(sequence);
            var context = new MatchContext<T>(indexedSequence);

            //while (context.Index < list.Count)
            while (!context.IsEndOfSequence)
            {
                var newContext = Match(context).FirstOrDefault();
                if (newContext != null)
                {
                    yield return new MatchResult<T>(newContext);
                }

                int newIndex = (newContext != null && newContext.Index > context.Index) ? newContext.Index : context.Index + 1;
                context = new MatchContext<T>(indexedSequence).WithIndex(newIndex);
            }
        }

        internal abstract IEnumerable<MatchContext<T>> Match(MatchContext<T> context);
    }
}
