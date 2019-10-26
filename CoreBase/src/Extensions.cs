using System;
using System.Collections.Generic;
using System.Linq;

namespace Wells.Base
{
    public static class Extensions
    {

        public static double StandardDeviation(this IEnumerable<double> source)
        {
            var average = source.Average();
            var sum = Math.Pow(source.Sum(v => v - average), 2.0);
            return Math.Sqrt(sum / source.Count());
        }

        public static double StandardDeviation<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            var list = source.Select(selector);
            return StandardDeviation(list);
        }
    }
}
