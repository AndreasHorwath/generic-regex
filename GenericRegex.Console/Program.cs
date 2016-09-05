using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GenericRegex.Matcher<int>;

namespace GenericRegex.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //int i = 0;
            //foreach (var digit in DigitsOfPi().Take(40))
            //{
            //    System.Console.WriteLine($"{i,6}: {digit}");
            //    i++;
            //}

            foreach (var match in Seq(6, 6, 6).FindMatchesIn(DigitsOfPi()))
            {
                System.Console.WriteLine(match.StartIndex);
            }

            //var pattern =
            //    Seq(
            //        AnyElement.WithId(0),
            //        Pred((n, groups) => n == groups[0].Elements[0] + 1).WithId(1),
            //        Pred((n, groups) => n == groups[1].Elements[0] + 1)
            //    );

            //foreach (var match in pattern.FindMatchesIn(DigitsOfPi()))
            //{
            //    System.Console.WriteLine($"{match.StartIndex}: [{string.Join(", ", match.Elements)}]");
            //}
        }

        /// <summary>
        /// Produces the digits of π as an IEnumerable&lt;int&gt;.
        /// <para>
        /// Sources:
        /// http://stackoverflow.com/questions/11677369/how-to-calculate-pi-to-n-number-of-places-in-c-sharp-using-loops
        /// and http://www.mathpropress.com/stan/bibliography/spigot.pdf
        /// </para>
        /// </summary>
        static public IEnumerable<int> DigitsOfPi()
        {
            int[] x = new int[short.MaxValue];
            int[] r = new int[short.MaxValue];

            for (int j = 0; j < short.MaxValue; j++)
            {
                x[j] = 20;
            }

            int prev = 0;
            for (int i = 0; i < short.MaxValue; i++)
            {
                int carry = 0;
                for (int j = 0; j < x.Length; j++)
                {
                    int num = (int)(x.Length - j - 1);
                    int dem = num * 2 + 1;
                    x[j] += carry;
                    int q = x[j] / dem;
                    r[j] = x[j] % dem;
                    carry = q * num;
                }

                // calculate the digit, but don't add to the list right away:
                int digit = (int)(x[x.Length - 1] / 10);
                // handle overflow:
                if (digit >= 10)
                {
                    digit -= 10;
                    prev++;
                }
                if (i > 0)
                {
                    yield return prev;
                }

                // store the digit for next time, when it will be the prev value:
                prev = digit;

                r[x.Length - 1] = x[x.Length - 1] % 10;
                for (int j = 0; j < x.Length; j++)
                {
                    x[j] = r[j] * 10;
                }
            }
        }
    }
}
