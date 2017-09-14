using System;

namespace typeahead
{
    static class Timer
    {
        public static T Time<T>(Func<T> x, string log="")
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var res = x();
            Console.WriteLine($"{log} Time: {timer.ElapsedMilliseconds}");
            return res;
        }
    }
}
