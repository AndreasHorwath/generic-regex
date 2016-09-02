using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class As<T> : PatternExpr<T>
    {
        public PatternExpr<T> Expression { get; }
        public int GroupId { get; }

        public As(PatternExpr<T> expression, int groupId)
        {
            Expression = expression;
            GroupId = groupId;
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            foreach (var newContext in Expression.Match(context))
            {
                yield return newContext.WithGroupId(GroupId, new MatchReference(context.Index, newContext.Index - context.Index));
            }
        }

        public override string ToString() => $"As({Expression}, {GroupId})";
    }
}
