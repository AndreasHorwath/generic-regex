using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static GenericRegex.Matcher<char>;

namespace GenericRegex.Tests
{
    public class MatcherTests
    {
        [Fact]
        public void MatcherSeqMatch()
        {
            var result = Seq('a', 'b').Match("cabs");

            result.Success.ShouldBe(true);
            result.Groups.Count.ShouldBe(0);
            result.Groups[0].ShouldBeNull();
            result.Groups[1].ShouldBeNull();
        }

        [Fact]
        public void MatcherSeqMatchWithAnchor1()
        {
            var result = Seq(StartAnchor, 'a', 'b').Match("cabs");
            result.Success.ShouldBe(false);
        }

        [Fact]
        public void MatcherSeqMatchWithAnchor2()
        {
            var result = Seq(StartAnchor, 'c', 'a').Match("cabs");
            result.Success.ShouldBe(true);
        }

        [Fact]
        public void MatcherSeqMatchWithAnchor3()
        {
            var result = Seq(StartAnchor, 'c', 'a', EndAnchor).Match("ca");
            result.Success.ShouldBe(true);
        }

        [Fact]
        public void MatcherSeqMatchWithAnchor4()
        {
            var result = Seq('c', 'a', EndAnchor).Match("cab");
            result.Success.ShouldBe(false);
        }

        [Fact]
        public void MatcherSeqNoMatch()
        {
            var pattern = Seq('a', 'b');
            var result = pattern.Match("caas");

            result.Success.ShouldBe(false);
        }

        [Fact]
        public void MatcherSeqMatchWithCapturingGroups()
        {
            var result = Seq('a', 'b').As(1).Match("cabs");

            result.Success.ShouldBe(true);
            result.Groups.Count.ShouldBe(1);
            result.Groups[1].StartIndex.ShouldBe(1);
            result.Groups[1].Length.ShouldBe(2);
            result.Groups[1].Elements.Count.ShouldBe(2);
            result.Groups[1].Elements[0].ShouldBe('a');
            result.Groups[1].Elements[1].ShouldBe('b');
        }

        [Fact]
        public void MatcherSeqMatchAllWithCapturingGroups()
        {
            var results = Seq('a', 'b').As(1).MatchAll("cabsab").ToList();

            results.Count.ShouldBe(2);

            results[0].Success.ShouldBe(true);
            results[0].Groups.Count.ShouldBe(1);
            results[0].Groups[1].StartIndex.ShouldBe(1);
            results[0].Groups[1].Length.ShouldBe(2);
            results[0].Groups[1].Elements.Count.ShouldBe(2);
            results[0].Groups[1].Elements[0].ShouldBe('a');
            results[0].Groups[1].Elements[1].ShouldBe('b');

            results[1].Success.ShouldBe(true);
            results[1].Groups.Count.ShouldBe(1);
            results[1].Groups[1].StartIndex.ShouldBe(4);
            results[1].Groups[1].Length.ShouldBe(2);
            results[1].Groups[1].Elements.Count.ShouldBe(2);
            results[1].Groups[1].Elements[0].ShouldBe('a');
            results[1].Groups[1].Elements[1].ShouldBe('b');
        }

        [Fact]
        public void MatcherZeroOrOne()
        {
            var pattern = Seq('a', ZeroOrOne('b'), 'c');

            pattern.Match("ac").Success.ShouldBe(true);
            pattern.Match("abc").Success.ShouldBe(true);
            pattern.Match("abbc").Success.ShouldBe(false);
        }

        [Fact]
        public void MatcherZeroOrMany()
        {
            var pattern = Seq('a', ZeroOrMany('b'), 'c');

            pattern.Match("ac").Success.ShouldBe(true);
            pattern.Match("abc").Success.ShouldBe(true);
            pattern.Match("abbc").Success.ShouldBe(true);
        }

        [Fact]
        public void MatcherOneOrMany()
        {
            var pattern = Seq('a', OneOrMany('b'), 'c');

            pattern.Match("ac").Success.ShouldBe(false);
            pattern.Match("abc").Success.ShouldBe(true);
            pattern.Match("abbc").Success.ShouldBe(true);
        }

        [Fact]
        public void MatcherZeroOrManyNonGreedy1()
        {
            var pattern = Seq('a', ZeroOrManyNonGreedy('b').As(1), 'c');

            pattern.Match("abbc").CharGroup(1).ShouldBe("bb");
            pattern.Match("abb").CharGroup(1).ShouldBe(null);
        }

        [Fact]
        public void MatcherZeroOrManyNonGreedy2a()
        {
            var pattern = Seq('a', ZeroOrManyNonGreedy(AnyElement).As(1), 'c').As(0);

            pattern.Match("abbcbbbbc").CharGroup(1).ShouldBe("bb");
            pattern.Match("abbcbbbbc").CharGroup(0).ShouldBe("abbc");
        }

        [Fact]
        public void MatcherZeroOrManyNonGreedy2b()
        {
            var pattern = Seq('a', ZeroOrManyNonGreedy(AnyElement).As(1), 'c', 'c');

            pattern.Match("abbcbbbbc").CharGroup(1).ShouldBe(null);
            pattern.Match("abbcbbbbcc").CharGroup(1).ShouldBe("bbcbbbb");
        }

        [Fact]
        public void MatcherZeroOrManyGreedy2()
        {
            var pattern = Seq('a', ZeroOrMany(AnyElement).As(1), 'c').As(0);

            pattern.Match("abbcbbbbc").CharGroup(1).ShouldBe("bbcbbbb");
            pattern.Match("abbcbbbbc").CharGroup(0).ShouldBe("abbcbbbbc");
        }

        [Fact]
        public void MatcherRepeat1()
        {
            var pattern = Seq(Repeat('x', 1, 5).As(1), Repeat('x', 2, 5).As(2)).As(0);
            var match = pattern.Match("axxxxxb");

            match.CharGroup(0).ShouldBe("xxxxx");
            match.CharGroup(1).ShouldBe("xxx");
            match.CharGroup(2).ShouldBe("xx");
            match.Groups[2].StartIndex.ShouldBe(4);
            match.Groups[2].Length.ShouldBe(2);
        }

        [Fact]
        public void MatcherRepeat1a()
        {
            var pattern = Seq(Repeat(Seq('x', 'y'), 1, 5).As(1), Repeat(Seq('x', 'y'), 2, 5).As(2)).As(0);
            var match = pattern.Match("axyxyxyxyxyb");

            match.CharGroup(0).ShouldBe("xyxyxyxyxy");
            match.CharGroup(1).ShouldBe("xyxyxy");
            match.CharGroup(2).ShouldBe("xyxy");
        }

        [Fact]
        public void MatcherRepeat2()
        {
            var pattern = Seq(RepeatNonGreedy('x', 1, 5).As(1), Repeat('x', 2, 5).As(2)).As(0);
            var match = pattern.Match("axxxxxb");

            match.CharGroup(0).ShouldBe("xxxxx");
            match.CharGroup(1).ShouldBe("x");
            match.CharGroup(2).ShouldBe("xxxx");
        }

        [Fact]
        public void MatcherRepeat2a()
        {
            var pattern = Seq(RepeatNonGreedy(Seq('x', 'y'), 1, 5).As(1), Repeat(Seq('x', 'y'), 2, 5).As(2)).As(0);
            var match = pattern.Match("axyxyxyxyxyb");

            match.CharGroup(0).ShouldBe("xyxyxyxyxy");
            match.CharGroup(1).ShouldBe("xy");
            match.CharGroup(2).ShouldBe("xyxyxyxy");
        }

        [Fact]
        public void MatcherRepeat3()
        {
            var pattern = Seq(RepeatNonGreedy('x', 1, 5).As(1), RepeatNonGreedy('x', 2, 5).As(2)).As(0);
            var match = pattern.Match("axxxxxb");

            match.CharGroup(0).ShouldBe("xxx");
            match.CharGroup(1).ShouldBe("x");
            match.CharGroup(2).ShouldBe("xx");
        }

        [Fact]
        public void MatcherRepeat4()
        {
            var pattern = Seq(RepeatNonGreedy('x', 1, 5).As(1), RepeatNonGreedy('x', 2, 5).As(2), 'b').As(0);
            var match = pattern.Match("axxxxxb");

            match.CharGroup(0).ShouldBe("xxxxxb");
            match.CharGroup(1).ShouldBe("x");
            match.CharGroup(2).ShouldBe("xxxx");
        }

        [Fact]
        public void MatcherRepeat5()
        {
            var pattern = Seq(RepeatNonGreedy('x', 1, 5).As(1), RepeatNonGreedy('x', 2, 3).As(2), 'b').As(0);
            var match = pattern.Match("axxxxxb");
            match.CharGroup(0).ShouldBe("xxxxxb");
            match.CharGroup(1).ShouldBe("xx");
            match.CharGroup(2).ShouldBe("xxx");
        }

        [Fact]
        public void MatcherOr()
        {
            Seq('c', Or(OneOrMany('a'), OneOrMany('b')).As(1), OneOrMany('b')).Match("caaabbb").CharGroup(1).ShouldBe("aaa");
            Seq('c', Or(OneOrMany('a'), OneOrMany('b')).As(1), OneOrMany('a')).Match("cbbaaa").CharGroup(1).ShouldBe("bb");

            // regexp: (A.*?A|A.*A)C
            // pattern: AxxAxxAC

            Seq(
                Or(
                    Seq('A', OneOrManyNonGreedy(AnyElement), 'A'),
                    Seq('A', OneOrMany(AnyElement), 'A')
                ),
                'C'
            ).As(1).Match("AxxAxxAC").CharGroup(1).ShouldBe("AxxAxxAC");

            Seq(
                Or(
                    Seq('A', OneOrManyNonGreedy(AnyElement), 'A', 'y'),
                    Seq('A', OneOrManyNonGreedy(AnyElement), 'A')
                ),
                'C'
            ).As(1).Match("AxxAxxAC").CharGroup(1).ShouldBe("AxxAxxAC");

            Seq(
                Or(
                    Seq('A', OneOrManyNonGreedy(AnyElement), 'A', 'y'),
                    Seq('A', OneOrManyNonGreedy(AnyElement), 'B')
                ),
                'C'
            ).As(1).Match("AxxAxxAC").CharGroup(1).ShouldBeNull();
        }
    }

    static class TextExtensions
    {
        public static string CharGroup(this MatchResult<char> matchResult, int groupId)
        {
            Match<char> match = matchResult.Groups[groupId];
            if (match == null)
            {
                return null;
            }

            return new string(match.Elements.ToArray());
        }
    }
}
