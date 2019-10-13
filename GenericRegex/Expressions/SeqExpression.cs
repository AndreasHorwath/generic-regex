using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class SeqExpression<T> : ExpressionBase<T>
    {
        public SeqExpression(IEnumerable<ExpressionBase<T>> expressions)
        {
            _expressions = expressions.ToList();
        }

        readonly List<ExpressionBase<T>> _expressions;

        public IReadOnlyCollection<ExpressionBase<T>> Expressions => _expressions;

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            return Match(context, 0);
        }

        IEnumerable<MatchContext<T>> Match(MatchContext<T> context, int position)
        {
            if (position < _expressions.Count)
            {
                foreach (var newContext in _expressions[position].Match(context))
                {
                    foreach (var resultingContext in Match(newContext, position + 1))
                    {
                        yield return resultingContext;
                    }
                }
            }
            else
            {
                yield return context;
            }
        }

        public override string ToString() => $"Seq({string.Join(", ", _expressions)})";
    }
}
