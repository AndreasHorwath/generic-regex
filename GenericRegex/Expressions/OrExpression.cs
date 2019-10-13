using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class OrExpression<T> : ExpressionBase<T>
    {
        public OrExpression(IEnumerable<ExpressionBase<T>> expressions)
        {
            _expressions = expressions.ToList();
        }

        readonly List<ExpressionBase<T>> _expressions;

        public IReadOnlyCollection<ExpressionBase<T>> Expressions => _expressions;

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            return _expressions.SelectMany(e => e.Match(context));
        }

        public override string ToString() => $"Or({string.Join(", ", _expressions)})";
    }
}
