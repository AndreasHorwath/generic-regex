# generic-regex 0.0.1

generic-regex is a regular expression engine for .NET. Unlike other such engines, it is not limited to strings but can operate on sequences of any type. For example, you can use it to find subsequences within `IEnumerable<int>` sequences:

```cs
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
```

This code searches within `array` for subsequences consisting of a 0 followed by one or more ints > 5 which in turn are followed by one 0 or one or more 1's.

Since `FindMatchesIn` accepts an `IEnumerable<T>` and returns its results as an `IEnumerable<MatchResult<T>>`, it may be invoked with a potentially infinite sequence. For example, suppose you have a function `DigitsOfPi` that produces the digits of π, you could do this to find the positions of all occurrences of the sequence `[6, 6, 6]`:

```cs
foreach (var match in Seq(6, 6, 6).FindMatchesIn(DigitsOfPi()))
{
    System.Console.WriteLine(match.StartIndex);
}
```

Output:

    2440
    4000
    4435
    5403
    6840
    11911
    13472
    13586
    14740
    15208
    15372
    ...

Here is an example involving a more complex pattern. It searches within the digits of π for 3-tuples of consecutive integers:

```cs
var pattern =
    Seq(
        AnyElement.WithId(0),
        Pred((n, groups) => n == groups[0].Elements[0] + 1).WithId(1),
        Pred((n, groups) => n == groups[1].Elements[0] + 1)
    );

foreach (var match in pattern.FindMatchesIn(DigitsOfPi()))
{
    System.Console.WriteLine($"{match.StartIndex}: [{string.Join(", ", match.Elements)}]");
}
```

Output:

    234: [6, 7, 8]
    251: [4, 5, 6]
    260: [2, 3, 4]
    350: [6, 7, 8]
    466: [5, 6, 7]
    634: [7, 8, 9]
    659: [0, 1, 2]
    1198: [0, 1, 2]
    1244: [6, 7, 8]
    1401: [6, 7, 8]
    1700: [3, 4, 5]
    1874: [4, 5, 6]
    1924: [1, 2, 3]
    ...
