using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TechFlurry.SparkLedger.Shared.Common
{
    public static class Functions
    {
        public static void RunOnThread(Action action)
        {
            var timer = new Timer(new TimerCallback(_ =>
            {
                action.Invoke();
            }), null, TimeSpan.FromMilliseconds(100), Timeout.InfiniteTimeSpan);
        }
        public static void SetTimeout(Action action, TimeSpan delay)
        {
            var timer = new Timer(new TimerCallback(_ =>
            {
                action.Invoke();
            }), null, delay, Timeout.InfiniteTimeSpan);
        }
        public static void SetInterval(Action action, TimeSpan interval)
        {
            var timer = new Timer(new TimerCallback(_ =>
            {
                action.Invoke();
            }), null, TimeSpan.FromMilliseconds(100), interval);
        }
        public static void SetInterval(Action action, TimeSpan interval, TimeSpan delay)
        {
            var timer = new Timer(new TimerCallback(_ =>
            {
                action.Invoke();
            }), null, delay, interval);
        }
        public static string ValueToId(long value)
        {
            var parts = new List<string>();
            long numberPart = value % 10000;
            parts.Add(numberPart.ToString("0000"));
            value /= 10000;
            var alphaParts = new List<string>();
            for (int i = 0; i < 3 || value > 0; ++i)
            {
                alphaParts.Add(((char)(65 + (value % 26))).ToString());
                value /= 26;
            }
            parts.Add(string.Join("", alphaParts));
            return string.Join("-", parts.AsEnumerable().Reverse().ToArray());
        }
    }
}
