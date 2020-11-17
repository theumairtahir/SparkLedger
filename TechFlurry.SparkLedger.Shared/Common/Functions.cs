using System;
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
    }
}
