using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static GenericRegex.Matcher<int>;

namespace GenericRegex.Tests
{
    public class IntSequenceTests
    {
        [Fact]
        public void IntSubsequenceMatch()
        {
            var array = new int[] { 5, 3, 0, 10, 9, 1, 1, 1, 0, 9, 3, 7, 1, -2, 9, 0, 6, 7, 8, 1, 1, 5, 0, 9, 8, 7, 2, 0, 10, 10, 10, 0, 5 };
            var pattern = Seq(0, OneOrMany(Pred(n => n > 5)), Or(0, OneOrMany(1)));

            var match = pattern.FindMatchesIn(array).ToList();

            match.Count.ShouldBe(3);
            match[0].Elements.ShouldBe(new int[] { 0, 10, 9, 1, 1, 1 });
            match[1].Elements.ShouldBe(new int[] { 0, 6, 7, 8, 1, 1 });
            match[2].Elements.ShouldBe(new int[] { 0, 10, 10, 10, 0 });
        }

        [Fact]
        public void IntBackrefMatch1()
        {
            var array = new int[] { 6, 5, 3, 6, 7, 4, 2, 3, 4, 5, 8 };
            var pattern = Seq(AnyElement.WithId(0), Pred((n, groups) => n == groups[0].Elements[0] + 1));

            var match = pattern.FindMatchesIn(array).ToList();

            match.Count.ShouldBe(3);
            match[0].Elements.ShouldBe(new int[] { 6, 7 });
            match[1].Elements.ShouldBe(new int[] { 2, 3 });
            match[2].Elements.ShouldBe(new int[] { 4, 5 });
        }

        [Fact]
        public void IntBackrefMatch2()
        {
            var array = new int[] { 6, 5, 3, 6, 7, 4, 2, 3, 4, 5, 8 };
            var pattern =
                Seq(
                    AnyElement.WithId(0),
                    Pred((n, groups) => n == groups[0].Elements[0] + 1).WithId(1),
                    Pred((n, groups) => n == groups[1].Elements[0] + 1)
                );

            var match = pattern.FindMatchesIn(array).ToList();

            match.Count.ShouldBe(1);
            match[0].Elements.ShouldBe(new int[] { 2, 3, 4 });
        }
    }
}
