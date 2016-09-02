using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public abstract class PatternExpr<T>
    {
        public static implicit operator PatternExpr<T>(T value) => Matcher<T>.Val(value);
        public static implicit operator PatternExpr<T>(Func<T, bool> predicate) => Matcher<T>.Pred(predicate);

        public PatternExpr<T> As(int groupId) => new As<T>(this, groupId);

        public MatchResult<T> Match(IEnumerable<T> sequence) => MatchAll(sequence).FirstOrDefault() ?? new MatchResult<T>(null);

        public IEnumerable<MatchResult<T>> MatchAll(IEnumerable<T> sequence)
        {
            var list = (sequence as IReadOnlyList<T>) ?? new List<T>(sequence);

            var context = new MatchContext<T>(list);

            while (context.Index < list.Count)
            {
                var newContext = Match(context).FirstOrDefault();
                if (newContext != null)
                {
                    yield return new MatchResult<T>(newContext);
                }

                int newIndex = (newContext != null && newContext.Index > context.Index) ? newContext.Index : context.Index + 1;
                context = new MatchContext<T>(list).WithIndex(newIndex);
            }
        }

        internal abstract IEnumerable<MatchContext<T>> Match(MatchContext<T> context);
    }
}
