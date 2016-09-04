using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class WithIdExpression<T> : ExpressionBase<T>
    {
        public ExpressionBase<T> Expression { get; }
        public int ExpressionId { get; }

        public WithIdExpression(ExpressionBase<T> expression, int expressionId)
        {
            Expression = expression;
            ExpressionId = expressionId;
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            foreach (var newContext in Expression.Match(context))
            {
                yield return newContext.WithGroupId(ExpressionId, new MatchReference(context.Index, newContext.Index - context.Index));
            }
        }

        public override string ToString() => $"WithId({Expression}, {ExpressionId})";
    }
}
