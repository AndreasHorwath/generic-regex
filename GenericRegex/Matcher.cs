using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Ahosoft.GenericRegex
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

    public abstract class PatternExpr<T>
    {
        internal abstract IEnumerable<MatchContext<T>> Match(MatchContext<T> context);

        public static implicit operator PatternExpr<T>(T value) => Matcher<T>.Val(value);
        public static implicit operator PatternExpr<T>(Func<T, bool> predicate) => Matcher<T>.Pred(predicate);

        public PatternExpr<T> As(int groupId) => new As<T>(this, groupId);

        public MatchResult<T> Match(IEnumerable<T> sequence) => Match(sequence.ToList());

        public MatchResult<T> Match(IList<T> sequence) => MatchAll(sequence).FirstOrDefault() ?? new MatchResult<T>(null);

        public IEnumerable<MatchResult<T>> MatchAll(IEnumerable<T> sequence) => MatchAll(sequence.ToList());

        public IEnumerable<MatchResult<T>> MatchAll(IReadOnlyList<T> sequence)
        {
            var context = new MatchContext<T>(sequence);

            while (context.Index < sequence.Count)
            {
                var newContext = Match(context).FirstOrDefault();
                if (newContext != null)
                {
                    yield return new MatchResult<T>(newContext);
                }

                int newIndex = (newContext != null && newContext.Index > context.Index) ? newContext.Index : context.Index + 1;
                context = new MatchContext<T>(sequence).WithIndex(newIndex);
            }
        }
    }

    public class CapturingGroupContainer<T>
    {
        readonly Dictionary<int, Match<T>> groups;

        internal CapturingGroupContainer(MatchContext<T> context)
        {
            if (context != null)
            {
                groups = context.MatchReferences.ToDictionary(e => e.Key, e =>
                {
                    MatchReference mr = e.Value;
                    return new Match<T>(mr.StartIndex, mr.Length, context.InputSequence.Skip(mr.StartIndex).Take(mr.Length).ToList());
                });
            }
            else
            {
                groups = new Dictionary<int, Match<T>>();
            }
        }

        public Match<T> this[int groupId]
        {
            get
            {
                Match<T> match;
                groups.TryGetValue(groupId, out match);
                return match;
            }
        }

        public int Count => groups.Count;

        public ICollection<int> Keys => groups.Keys;

        public ICollection<Match<T>> Values => groups.Values;
    }

    public class MatchResult<T>
    {
        public bool Success { get; }
        public CapturingGroupContainer<T> Groups { get; }

        internal MatchResult(MatchContext<T> context)
        {
            Success = context != null;
            Groups = new CapturingGroupContainer<T>(context);
        }
    }

    public class Match<T>
    {
        public int StartIndex { get; }
        public int Length { get; }
        public IList<T> Elements { get; }

        internal Match(int startIndex, int length, IList<T> elements)
        {
            StartIndex = startIndex;
            Length = length;
            Elements = elements;
        }
    }

    struct MatchReference
    {
        public int StartIndex { get; }
        public int Length { get; }

        public MatchReference(int startIndex, int length)
        {
            StartIndex = startIndex;
            Length = length;
        }
    }

    class MatchContext<T>
    {
        public IReadOnlyList<T> InputSequence { get; }
        public int Index { get; }
        public ImmutableDictionary<int, MatchReference> MatchReferences { get; }

        public MatchContext(IReadOnlyList<T> inputSequence)
        {
            InputSequence = inputSequence;
            MatchReferences = ImmutableDictionary<int, MatchReference>.Empty;
        }

        MatchContext(MatchContext<T> context, int index)
        {
            InputSequence = context.InputSequence;
            Index = index;
            MatchReferences = context.MatchReferences;
        }

        MatchContext(MatchContext<T> context, int id, MatchReference matchReference)
        {
            InputSequence = context.InputSequence;
            Index = context.Index;
            MatchReferences = context.MatchReferences.Add(id, matchReference);
        }

        public MatchContext<T> WithIndex(int index) => new MatchContext<T>(this, index);
        public MatchContext<T> WithGroupId(int id, MatchReference matchReference) => new MatchContext<T>(this, id, matchReference);

        public bool IsEndOfSequence => Index >= InputSequence.Count;
        public T CurrentItem => InputSequence[Index];

        public override string ToString() => $"Index = {Index}, InputSequence = {string.Join(", ", InputSequence.ToArray())}";
    }

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

    class EqualsPatternExpr<T> : PatternExpr<T>
    {
        public T Value { get; }

        public EqualsPatternExpr(T value)
        {
            Value = value;
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            if (!context.IsEndOfSequence && EqualityComparer<T>.Default.Equals(context.CurrentItem, Value))
            {
                yield return context.WithIndex(context.Index + 1);
            }
        }

        public override string ToString() => $"Val({Value?.ToString()})";
    }

    class PredPatternExpr<T> : PatternExpr<T>
    {
        public Func<T, bool> Predicate { get; }

        public PredPatternExpr(Func<T, bool> predicate)
        {
            Predicate = predicate;
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            if (!context.IsEndOfSequence && Predicate(context.CurrentItem))
            {
                yield return context.WithIndex(context.Index + 1);
            }
        }

        public override string ToString() => $"Pred({Predicate})";
    }

    class AnyElement<T> : PatternExpr<T>
    {
        public static AnyElement<T> Instance = new AnyElement<T>();

        public AnyElement()
        {
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            if (!context.IsEndOfSequence)
            {
                yield return context.WithIndex(context.Index + 1);
            }
        }

        public override string ToString() => "AnyElement";
    }

    class StartAnchor<T> : PatternExpr<T>
    {
        public static StartAnchor<T> Instance = new StartAnchor<T>();

        public StartAnchor()
        {
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            if (context.Index == 0)
            {
                yield return context;
            }
        }

        public override string ToString() => "StartAnchor";
    }

    class EndAnchor<T> : PatternExpr<T>
    {
        public static EndAnchor<T> Instance = new EndAnchor<T>();

        public EndAnchor()
        {
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            if (context.Index == context.InputSequence.Count)
            {
                yield return context;
            }
        }

        public override string ToString() => "EndAnchor";
    }

    class OrExpr<T> : PatternExpr<T>
    {
        readonly List<PatternExpr<T>> expressions;

        public OrExpr(IEnumerable<PatternExpr<T>> expressions)
        {
            this.expressions = expressions.ToList();
        }

        public IReadOnlyCollection<PatternExpr<T>> Expressions => expressions;

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            return expressions.SelectMany(e => e.Match(context));
        }

        public override string ToString() => $"Or({string.Join(", ", expressions)})";
    }

    class SeqExpr<T> : PatternExpr<T>
    {
        readonly List<PatternExpr<T>> expressions;

        public SeqExpr(IEnumerable<PatternExpr<T>> expressions)
        {
            this.expressions = expressions.ToList();
        }

        public IReadOnlyCollection<PatternExpr<T>> Expressions => expressions;

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            return Match(context, 0);
        }

        IEnumerable<MatchContext<T>> Match(MatchContext<T> context, int position)
        {
            if (position < expressions.Count)
            {
                foreach (var newContext in expressions[position].Match(context))
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

        public override string ToString() => $"Seq({string.Join(", ", expressions)})";
    }

    class RepetitionExpr<T> : PatternExpr<T>
    {
        public PatternExpr<T> Expression { get; }
        public int MinOccur { get; }
        public int MaxOccur { get; }
        public bool Greedy { get; private set; }

        public RepetitionExpr(PatternExpr<T> expression, int minOccur, int maxOccur, bool greedy)
        {
            Expression = expression;
            MinOccur = minOccur;
            MaxOccur = maxOccur;
            Greedy = greedy;
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            return Match(context, 0);
        }

        IEnumerable<MatchContext<T>> Match(MatchContext<T> context, int position)
        {
            if (position <= MaxOccur)
            {
                if (position >= MinOccur && !Greedy)
                {
                    yield return context;
                }

                foreach (var newContext in Expression.Match(context))
                {
                    foreach (var resultingContext in Match(newContext, position + 1))
                    {
                        yield return resultingContext;
                    }
                }

                if (position >= MinOccur && Greedy)
                {
                    yield return context;
                }
            }
        }

        public override string ToString()
        {
            var greedyText = Greedy ? "greedy" : "non-greedy";
            var maxText = MaxOccur == int.MaxValue ? "∞" : MaxOccur.ToString();
            return $"Rep({Expression}, {MinOccur}-{maxText}, {greedyText})";
        }
    }
}
