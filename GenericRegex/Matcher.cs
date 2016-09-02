using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public static class Matcher<T>
    {
        public static PatternExpr<T> Seq(params PatternExpr<T>[] expressions)
        {
            return Seq((IEnumerable<PatternExpr<T>>)expressions);
        }

        public static PatternExpr<T> Seq(IEnumerable<PatternExpr<T>> expressions)
        {
            return new SeqExpr<T>(expressions);
        }

        public static PatternExpr<T> Or(params PatternExpr<T>[] expressions)
        {
            return Or((IEnumerable<PatternExpr<T>>)expressions);
        }

        public static PatternExpr<T> Or(IEnumerable<PatternExpr<T>> expressions)
        {
            return new OrExpr<T>(expressions);
        }

        public static PatternExpr<T> AnyElement
        {
            get { return AnyElement<T>.Instance; }
        }

        public static PatternExpr<T> Val(T value)
        {
            return new EqualsPatternExpr<T>(value);
        }

        public static PatternExpr<T> Pred(Func<T, bool> predicate)
        {
            return new PredPatternExpr<T>(predicate);
        }

        public static PatternExpr<T> ZeroOrOne(PatternExpr<T> expression)
        {
            return Repeat(expression, 0, 1);
        }

        public static PatternExpr<T> ZeroOrMany(PatternExpr<T> expression)
        {
            return Repeat(expression, 0, int.MaxValue);
        }

        public static PatternExpr<T> OneOrMany(PatternExpr<T> expression)
        {
            return Repeat(expression, 1, int.MaxValue);
        }

        public static PatternExpr<T> Repeat(PatternExpr<T> expression, int minOccur, int maxOccur = int.MaxValue, bool greedy = true)
        {
            return new RepetitionExpr<T>(expression, minOccur, maxOccur, greedy);
        }

        public static PatternExpr<T> ZeroOrOneNonGreedy(PatternExpr<T> expression)
        {
            return RepeatNonGreedy(expression, 0, 1);
        }

        public static PatternExpr<T> ZeroOrManyNonGreedy(PatternExpr<T> expression)
        {
            return RepeatNonGreedy(expression, 0, int.MaxValue);
        }

        public static PatternExpr<T> OneOrManyNonGreedy(PatternExpr<T> expression)
        {
            return RepeatNonGreedy(expression, 1, int.MaxValue);
        }

        public static PatternExpr<T> RepeatNonGreedy(PatternExpr<T> expression, int minOccur, int maxOccur = int.MaxValue)
        {
            return new RepetitionExpr<T>(expression, minOccur, maxOccur, false);
        }

        public static PatternExpr<T> StartAnchor
        {
            get { return StartAnchor<T>.Instance; }
        }

        public static PatternExpr<T> EndAnchor
        {
            get { return EndAnchor<T>.Instance; }
        }
    }
}
