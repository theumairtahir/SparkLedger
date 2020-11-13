using System;
using System.Collections.Generic;
using TechFlurry.SparkLedger.ApplicationDomain.Elements;
using TechFlurry.SparkLedger.ApplicationDomain.EventArgs;
using TechFlurry.SparkLedger.ClientServices.Abstractions;

namespace TechFlurry.SparkLedger.ClientServices.Core
{
    public interface IActivityService : IValueUpdator
    {
        List<TimelineActivityModel> TimelineActivities { get; }
        int TotalTransactions { get; }
        string NewCode { get; }

        void Init();
        void LoadMainActivity(DateTime startDate, DateTime endDate);
    }

    internal class ActivityService : IActivityService
    {
        public ActivityService()
        {
            TimelineActivities = new List<TimelineActivityModel>();
        }
        public List<TimelineActivityModel> TimelineActivities { get; private set; }
        public int TotalTransactions { get; private set; }
        public string NewCode { get; private set; }
        public event EventHandler<OnUpdateEventArgs> OnValueUpdate;
        public void Init()
        {
            LoadMainActivity(DateTime.Today, DateTime.Now);
            TotalTransactions = 100;
            NewCode = "#XRS-45670";
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(Init),
                CallingObject = this
            });
        }
        public void LoadMainActivity(DateTime startDate, DateTime endDate)
        {
            TimelineActivities = new List<TimelineActivityModel>();
            TimelineActivities.Add(new TimelineActivityModel
            {
                ActivityCode = "#x773s",
                Color = ApplicationDomain.Enums.Colors.danger,
                DateTime = DateTime.Now.AddMinutes(-120).ToString("dd/MMM HH:mm"),
                Description = "Gave 150 rupees to Abdul Shakoor",
                Id = 1,
                IsBold = true
            });
            TimelineActivities.Add(new TimelineActivityModel
            {
                ActivityCode = "#xc273z",
                Color = ApplicationDomain.Enums.Colors.success,
                DateTime = DateTime.Now.AddDays(-12).ToString("dd/MMM HH:mm"),
                Description = "Got 1000 rupees from Majid",
                Id = 2,
                IsBold = true
            });
            TimelineActivities.Add(new TimelineActivityModel
            {
                ActivityCode = "#x773s",
                Color = ApplicationDomain.Enums.Colors.success,
                DateTime = DateTime.Now.AddMinutes(-1200).ToString("dd/MMM HH:mm"),
                Description = "Gave 150 rupees to Abdul Shakoor",
                Id = 3,
                IsBold = true
            });
            TimelineActivities.Add(new TimelineActivityModel
            {
                ActivityCode = "#x773s",
                Color = ApplicationDomain.Enums.Colors.danger,
                DateTime = DateTime.Now.AddDays(-120).ToString("dd/MMM HH:mm"),
                Description = "Gave 150 rupees to Abdul Shakoor",
                Id = 4,
                IsBold = true
            });
            TimelineActivities.Add(new TimelineActivityModel
            {
                ActivityCode = "#x773s",
                Color = ApplicationDomain.Enums.Colors.danger,
                DateTime = DateTime.Now.AddDays(-1).ToString("dd/MMM HH:mm"),
                Description = "Gave 150 rupees to Abdul Shakoor",
                Id = 1,
                IsBold = true
            });
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(LoadMainActivity),
                CallingObject = this
            });
        }
    }
}
