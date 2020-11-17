using System;
using System.Collections.Generic;
using TechFlurry.SparkLedger.ApplicationDomain.Elements;
using TechFlurry.SparkLedger.ApplicationDomain.EventArgs;
using TechFlurry.SparkLedger.BusinessDomain.Enums;
using TechFlurry.SparkLedger.ClientServices.Abstractions;

namespace TechFlurry.SparkLedger.ClientServices.Core
{
    public interface IActivityService : IValueUpdator
    {
        List<TimelineActivityModel> TimelineActivities { get; }
        int TotalTransactions { get; }
        string NewCode { get; }

        event EventHandler<SuccessfullOperationEventArgs> OnSuccessAddNew;
        event EventHandler<ErrorOperationEventArgs> OnErrorAddNew;

        void AddNewActivity(long accountId, long itemId, TransactionType transactionType, decimal value);
        List<DropDownModel<long>> GetLedgerAccounts();
        List<DropDownModel<long>> GetTransactionItems();
        void Init();
        void LoadMainActivity(DateTime startDate, DateTime endDate);
        void LoadMainActivity(DateTime startDate, DateTime endDate, LedgerCategories ledgerCategory);
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
        public event EventHandler<SuccessfullOperationEventArgs> OnSuccessAddNew;
        public event EventHandler<ErrorOperationEventArgs> OnErrorAddNew;
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
        public List<DropDownModel<long>> GetLedgerAccounts()
        {
            List<DropDownModel<long>> model = new List<DropDownModel<long>>();
            model.Add(new DropDownModel<long>
            {
                Text = "101000100-Dodh Wala",
                Value = 1
            });
            model.Add(new DropDownModel<long>
            {
                Text = "101000102-Sabzi Wala",
                Value = 2
            });
            model.Add(new DropDownModel<long>
            {
                Text = "101000103-Muhammad Ahmad",
                Value = 3
            });
            return model;
        }
        public List<DropDownModel<long>> GetTransactionItems()
        {
            List<DropDownModel<long>> model = new List<DropDownModel<long>>();
            model.Add(new DropDownModel<long>
            {
                Text = "Rupees",
                Value = 1
            });
            model.Add(new DropDownModel<long>
            {
                Text = "Goods",
                Value = 2
            });
            return model;
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
        public void LoadMainActivity(DateTime startDate, DateTime endDate, LedgerCategories ledgerCategory)
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
        public void AddNewActivity(long accountId, long itemId, TransactionType transactionType, decimal value)
        {
            OnSuccessAddNew.Invoke(this, new SuccessfullOperationEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(AddNewActivity),
                CallingObject = this,
                Message = "New activity has been added successfully"
            });
        }
    }
}
