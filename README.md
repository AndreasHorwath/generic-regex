# generic-regex 0.0.1

generic-regex is a regular expression engine for .NET. Unlike other such engines, it is not limited to strings but can operate on sequences of any type. For example, you can use it to find subpatterns within `IEnumerable<int>` sequences:

    using static GenericRegex.Matcher<int>;
  
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

This code searches within `array` for sub-sequences consisting of a 0 followed by one or more ints > 5 which in turn are followed by one 0 or one or more 1's.
