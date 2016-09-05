using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public static class Matcher<T>
    {
        public static ExpressionBase<T> Seq(params ExpressionBase<T>[] expressions)
        {
            return Seq((IEnumerable<ExpressionBase<T>>)expressions);
        }

        public static ExpressionBase<T> Seq(IEnumerable<ExpressionBase<T>> expressions)
        {
            return new SeqExpression<T>(expressions);
        }

        public static ExpressionBase<T> Or(params ExpressionBase<T>[] expressions)
        {
            return Or((IEnumerable<ExpressionBase<T>>)expressions);
        }

        public static ExpressionBase<T> Or(IEnumerable<ExpressionBase<T>> expressions)
        {
            return new OrExpression<T>(expressions);
        }

        public static ExpressionBase<T> AnyElement
        {
            get { return AnyElement<T>.Instance; }
        }

        public static ExpressionBase<T> Val(T value)
        {
            return new ValExpression<T>(value);
        }

        public static ExpressionBase<T> Pred(Func<T, bool> predicate)
        {
            return new PredExpression<T>(predicate);
        }

        public static ExpressionBase<T> Pred(Func<T, CapturingGroupContainer<T>, bool> predicate)
        {
            return new PredWithBackrefsExpressions<T>(predicate);
        }

        public static ExpressionBase<T> ZeroOrOne(ExpressionBase<T> expression)
        {
            return Repeat(expression, 0, 1);
        }

        public static ExpressionBase<T> ZeroOrMany(ExpressionBase<T> expression)
        {
            return Repeat(expression, 0, int.MaxValue);
        }

        public static ExpressionBase<T> OneOrMany(ExpressionBase<T> expression)
        {
            return Repeat(expression, 1, int.MaxValue);
        }

        public static ExpressionBase<T> Repeat(ExpressionBase<T> expression, int minOccur, int? maxOccur = null)
        {
            return new RepetitionExpr<T>(expression, minOccur, maxOccur ?? minOccur, true);
        }

        public static ExpressionBase<T> ZeroOrOneNonGreedy(ExpressionBase<T> expression)
        {
            return RepeatNonGreedy(expression, 0, 1);
        }

        public static ExpressionBase<T> ZeroOrManyNonGreedy(ExpressionBase<T> expression)
        {
            return RepeatNonGreedy(expression, 0, int.MaxValue);
        }

        public static ExpressionBase<T> OneOrManyNonGreedy(ExpressionBase<T> expression)
        {
            return RepeatNonGreedy(expression, 1, int.MaxValue);
        }

        public static ExpressionBase<T> RepeatNonGreedy(ExpressionBase<T> expression, int minOccur, int? maxOccur = null)
        {
            return new RepetitionExpr<T>(expression, minOccur, maxOccur ?? minOccur, false);
        }

        public static ExpressionBase<T> StartAnchor
        {
            get { return StartAnchor<T>.Instance; }
        }

        public static ExpressionBase<T> EndAnchor
        {
            get { return EndAnchor<T>.Instance; }
        }
    }
}
