using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class WithIdExpression<T> : ExpressionBase<T>
    {
        public ExpressionBase<T> Expression { get; }
        public int Id { get; }

        public WithIdExpression(ExpressionBase<T> expression, int id)
        {
            Expression = expression;
            Id = id;
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            foreach (var newContext in Expression.Match(context))
            {
                yield return newContext.WithGroupId(Id, new MatchReference(context.Index, newContext.Index - context.Index));
            }
        }

        public override string ToString() => $"WithId({Expression}, {Id})";
    }
}
