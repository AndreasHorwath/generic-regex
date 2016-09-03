using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GenericRegex.Matcher<char>;

namespace GenericRegex.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var pattern = Seq('a', ZeroOrMany(AnyElement).WithId(1), 'c').WithId(0);

            //var m = pattern.Match("abbcbbbbc");

            //pattern.Match("abbcbbbbc").CharGroup(1).ShouldBe("bbcbbbb");
            //pattern.Match("abbcbbbbc").CharGroup(0).ShouldBe("abbcbbbbc");
        }
    }
}
