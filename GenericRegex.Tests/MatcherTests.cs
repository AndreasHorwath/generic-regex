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
            var result = Seq('a', 'b').FindMatchesIn("cabs").FirstOrDefault();

            result.ShouldNotBeNull();
            result.StartIndex.ShouldBe(1);
            result.Length.ShouldBe(2);
            result.Elements.ShouldBe("ab".ToArray());
            result.Groups.Count.ShouldBe(0);
            result.Groups[0].ShouldBeNull();
            result.Groups[1].ShouldBeNull();
        }

        [Fact]
        public void MatcherSeqMatchWithAnchor1()
        {
            Seq(StartAnchor, 'a', 'b').FindMatchesIn("cabs").ShouldBeEmpty();
        }

        [Fact]
        public void MatcherSeqMatchWithAnchor2()
        {
            Seq(StartAnchor, 'c', 'a').FindMatchesIn("cabs").ShouldNotBeEmpty();
        }

        [Fact]
        public void MatcherSeqMatchWithAnchor3()
        {
            Seq(StartAnchor, 'c', 'a', EndAnchor).FindMatchesIn("ca").ShouldNotBeEmpty();
        }

        [Fact]
        public void MatcherSeqMatchWithAnchor4()
        {
            Seq('c', 'a', EndAnchor).FindMatchesIn("cab").ShouldBeEmpty();
        }

        [Fact]
        public void MatcherSeqNoMatch()
        {
            Seq('a', 'b').FindMatchesIn("caas").ShouldBeEmpty();
        }

        [Fact]
        public void MatcherSeqMatchWithCapturingGroups()
        {
            var result = Seq('a', 'b').WithId(1).FindMatchesIn("cabs").FirstOrDefault();

            result.ShouldNotBeNull();
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
            var results = Seq('a', 'b').WithId(1).FindMatchesIn("cabsab").ToList();

            results.Count.ShouldBe(2);

            results[0].Groups.Count.ShouldBe(1);
            results[0].Groups[1].StartIndex.ShouldBe(1);
            results[0].Groups[1].Length.ShouldBe(2);
            results[0].Groups[1].Elements.Count.ShouldBe(2);
            results[0].Groups[1].Elements[0].ShouldBe('a');
            results[0].Groups[1].Elements[1].ShouldBe('b');

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

            pattern.FindMatchesIn("ac").ShouldNotBeEmpty();
            pattern.FindMatchesIn("abc").ShouldNotBeEmpty();
            pattern.FindMatchesIn("abbc").ShouldBeEmpty();
        }

        [Fact]
        public void MatcherZeroOrMany()
        {
            var pattern = Seq('a', ZeroOrMany('b'), 'c');

            pattern.FindMatchesIn("ac").ShouldNotBeEmpty();
            pattern.FindMatchesIn("abc").ShouldNotBeEmpty();
            pattern.FindMatchesIn("abbc").ShouldNotBeEmpty();
        }

        [Fact]
        public void MatcherOneOrMany()
        {
            var pattern = Seq('a', OneOrMany('b'), 'c');

            pattern.FindMatchesIn("ac").ShouldBeEmpty();
            pattern.FindMatchesIn("abc").ShouldNotBeEmpty();
            pattern.FindMatchesIn("abbc").ShouldNotBeEmpty();
        }

        [Fact]
        public void MatcherZeroOrManyNonGreedy1()
        {
            var pattern = Seq('a', ZeroOrManyNonGreedy('b').WithId(1), 'c');

            pattern.FindMatchesIn("abbc").First().Groups[1].Elements.ShouldBe("bb".ToArray());
            pattern.FindMatchesIn("abb").ShouldBeEmpty();
        }

        [Fact]
        public void MatcherZeroOrManyNonGreedy2a()
        {
            var pattern = Seq('a', ZeroOrManyNonGreedy(AnyElement).WithId(1), 'c').WithId(0);

            pattern.FindMatchesIn("abbcbbbbc").First().Groups[1].Elements.ShouldBe("bb".ToArray());
            pattern.FindMatchesIn("abbcbbbbc").First().Groups[0].Elements.ShouldBe("abbc".ToArray());
        }

        [Fact]
        public void MatcherZeroOrManyNonGreedy2b()
        {
            var pattern = Seq('a', ZeroOrManyNonGreedy(AnyElement).WithId(1), 'c', 'c');

            pattern.FindMatchesIn("abbcbbbbc").ShouldBeEmpty();
            pattern.FindMatchesIn("abbcbbbbcc").First().Groups[1].Elements.ShouldBe("bbcbbbb".ToArray());
        }

        [Fact]
        public void MatcherZeroOrManyGreedy2()
        {
            var pattern = Seq('a', ZeroOrMany(AnyElement).WithId(1), 'c').WithId(0);

            pattern.FindMatchesIn("abbcbbbbc").First().Groups[1].Elements.ShouldBe("bbcbbbb".ToArray());
            pattern.FindMatchesIn("abbcbbbbc").First().Groups[0].Elements.ShouldBe("abbcbbbbc".ToArray());
        }

        [Fact]
        public void MatcherRepeat1()
        {
            var pattern = Seq(Repeat('x', 1, 5).WithId(1), Repeat('x', 2, 5).WithId(2)).WithId(0);
            var match = pattern.FindMatchesIn("axxxxxb").First();

            match.Groups[0].Elements.ShouldBe("xxxxx".ToArray());
            match.Groups[1].Elements.ShouldBe("xxx".ToArray());
            match.Groups[2].Elements.ShouldBe("xx".ToArray());
            match.Groups[2].StartIndex.ShouldBe(4);
            match.Groups[2].Length.ShouldBe(2);
        }

        [Fact]
        public void MatcherRepeat1a()
        {
            var pattern = Seq(Repeat(Seq('x', 'y'), 1, 5).WithId(1), Repeat(Seq('x', 'y'), 2, 5).WithId(2)).WithId(0);
            var match = pattern.FindMatchesIn("axyxyxyxyxyb").First();

            match.Groups[0].Elements.ShouldBe("xyxyxyxyxy".ToArray());
            match.Groups[1].Elements.ShouldBe("xyxyxy".ToArray());
            match.Groups[2].Elements.ShouldBe("xyxy".ToArray());
        }

        [Fact]
        public void MatcherRepeat2()
        {
            var pattern = Seq(RepeatNonGreedy('x', 1, 5).WithId(1), Repeat('x', 2, 5).WithId(2)).WithId(0);
            var match = pattern.FindMatchesIn("axxxxxb").First();

            match.Groups[0].Elements.ShouldBe("xxxxx".ToArray());
            match.Groups[1].Elements.ShouldBe("x".ToArray());
            match.Groups[2].Elements.ShouldBe("xxxx".ToArray());
        }

        [Fact]
        public void MatcherRepeat2a()
        {
            var pattern = Seq(RepeatNonGreedy(Seq('x', 'y'), 1, 5).WithId(1), Repeat(Seq('x', 'y'), 2, 5).WithId(2)).WithId(0);
            var match = pattern.FindMatchesIn("axyxyxyxyxyb").First();

            match.Groups[0].Elements.ShouldBe("xyxyxyxyxy".ToArray());
            match.Groups[1].Elements.ShouldBe("xy".ToArray());
            match.Groups[2].Elements.ShouldBe("xyxyxyxy".ToArray());
        }

        [Fact]
        public void MatcherRepeat3()
        {
            var pattern = Seq(RepeatNonGreedy('x', 1, 5).WithId(1), RepeatNonGreedy('x', 2, 5).WithId(2)).WithId(0);
            var match = pattern.FindMatchesIn("axxxxxb").First();

            match.Groups[0].Elements.ShouldBe("xxx".ToArray());
            match.Groups[1].Elements.ShouldBe("x".ToArray());
            match.Groups[2].Elements.ShouldBe("xx".ToArray());
        }

        [Fact]
        public void MatcherRepeat4()
        {
            var pattern = Seq(RepeatNonGreedy('x', 1, 5).WithId(1), RepeatNonGreedy('x', 2, 5).WithId(2), 'b').WithId(0);
            var match = pattern.FindMatchesIn("axxxxxb").First();

            match.Groups[0].Elements.ShouldBe("xxxxxb".ToArray());
            match.Groups[1].Elements.ShouldBe("x".ToArray());
            match.Groups[2].Elements.ShouldBe("xxxx".ToArray());
        }

        [Fact]
        public void MatcherRepeat5()
        {
            var pattern = Seq(RepeatNonGreedy('x', 1, 5).WithId(1), RepeatNonGreedy('x', 2, 3).WithId(2), 'b').WithId(0);
            var match = pattern.FindMatchesIn("axxxxxb").First();
            match.Groups[0].Elements.ShouldBe("xxxxxb".ToArray());
            match.Groups[1].Elements.ShouldBe("xx".ToArray());
            match.Groups[2].Elements.ShouldBe("xxx".ToArray());
        }

        [Fact]
        public void MatcherOr()
        {
            Seq('c', Or(OneOrMany('a'), OneOrMany('b')).WithId(1), OneOrMany('b')).FindMatchesIn("caaabbb").First().Groups[1].Elements.ShouldBe("aaa".ToArray());
            Seq('c', Or(OneOrMany('a'), OneOrMany('b')).WithId(1), OneOrMany('a')).FindMatchesIn("cbbaaa").First().Groups[1].Elements.ShouldBe("bb".ToArray());

            // regexp: (A.*?A|A.*A)C
            // pattern: AxxAxxAC

            Seq(
                Or(
                    Seq('A', OneOrManyNonGreedy(AnyElement), 'A'),
                    Seq('A', OneOrMany(AnyElement), 'A')
                ),
                'C'
            ).WithId(1).FindMatchesIn("AxxAxxAC").First().Groups[1].Elements.ShouldBe("AxxAxxAC".ToArray());

            Seq(
                Or(
                    Seq('A', OneOrManyNonGreedy(AnyElement), 'A', 'y'),
                    Seq('A', OneOrManyNonGreedy(AnyElement), 'A')
                ),
                'C'
            ).WithId(1).FindMatchesIn("AxxAxxAC").First().Groups[1].Elements.ShouldBe("AxxAxxAC".ToArray());

            Seq(
                Or(
                    Seq('A', OneOrManyNonGreedy(AnyElement), 'A', 'y'),
                    Seq('A', OneOrManyNonGreedy(AnyElement), 'B')
                ),
                'C'
            ).WithId(1).FindMatchesIn("AxxAxxAC").ShouldBeEmpty();
        }
    }
}
