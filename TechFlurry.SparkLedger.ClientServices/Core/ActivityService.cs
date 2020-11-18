using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechFlurry.SparkLedger.ApplicationDomain.Elements;
using TechFlurry.SparkLedger.ApplicationDomain.EventArgs;
using TechFlurry.SparkLedger.BusinessDomain.Enums;
using TechFlurry.SparkLedger.BusinessDomain.ViewModels;
using TechFlurry.SparkLedger.ClientServices.Abstractions;
using TechFlurry.SparkLedger.Shared.Common;
using TG.Blazor.IndexedDB;

namespace TechFlurry.SparkLedger.ClientServices.Core
{
    public interface IActivityService : IValueUpdator
    {
        List<TimelineActivityModel> TimelineActivities { get; }
        int TotalTransactions { get; }
        string NewCode { get; }
        List<DropDownModel<long>> TransactionItems { get; }
        List<DropDownModel<long>> LedgerAccounts { get; }
        decimal TotalCurrency { get; }
        decimal TotalGoods { get; }
        int CurrencyIncrease { get; }
        int GoodsIncrease { get; }

        event EventHandler<SuccessfullOperationEventArgs> OnSuccessAddNew;
        event EventHandler<ErrorOperationEventArgs> OnErrorAddNew;

        void AddNewActivity(long accountId, long itemId, TransactionType transactionType, decimal value, string description);
        void GetLedgerAccounts(long itemId);
        void GetTransactionItemsAsync();
        void Init();
        void LoadMainActivity(DateTime startDate, DateTime endDate);
        void LoadMainActivity(DateTime startDate, DateTime endDate, LedgerCategories ledgerCategory);
        void LoadNewCode();
        void LoadSummary();
    }

    internal class ActivityService : IActivityService
    {
        private readonly IndexedDBManager _dbManager;

        public ActivityService(IndexedDBManager dbManager)
        {
            TimelineActivities = new List<TimelineActivityModel>();
            _dbManager = dbManager;
            TransactionItems = new List<DropDownModel<long>>();
            LedgerAccounts = new List<DropDownModel<long>>();
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
            TotalTransactions = 0;
            LoadTotalTransactions();
            LoadSummary();
        }
        private async void LoadTotalTransactions()
        {
            var count = (await _dbManager.GetRecords<ActivityModel>("LedgerActivites")).Count;
            TotalTransactions = count;
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(LoadTotalTransactions),
                CallingObject = this
            });
        }
        public async void LoadNewCode()
        {
            var lastRecord = (await _dbManager.GetRecords<ActivityModel>("LedgerActivites")).LastOrDefault();
            var prevId = lastRecord != null ? lastRecord.Id + 1 : 1;
            NewCode = $"#{Functions.ValueToId(prevId)}".ToUpper();
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(LoadNewCode),
                CallingObject = this
            });
        }
        public List<DropDownModel<long>> LedgerAccounts { get; private set; }
        public async void GetLedgerAccounts(long itemId)
        {
            var category = (await _dbManager.GetRecordById<long, LedgerItemModel>("LedgerItems", itemId)).Category;
            List<DropDownModel<long>> model = (await _dbManager.GetRecords<LedgerAccountModel>("LedgerAccounts"))
                                                        .Where(x => x.Category == category)
            .Select(x => new DropDownModel<long>
            {
                Text = x.Code + "-" + x.Title,
                Value = x.Id
            }).ToList();
            LedgerAccounts = model;
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(GetLedgerAccounts),
                CallingObject = this
            });
        }
        public List<DropDownModel<long>> TransactionItems { get; private set; }
        public async void GetTransactionItemsAsync()
        {
            List<DropDownModel<long>> model = (await _dbManager.GetRecords<LedgerItemModel>("LedgerItems")).Select(x => new DropDownModel<long>
            {
                Text = x.ItemName,
                Value = x.Id
            }).ToList();
            TransactionItems = model;
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(GetTransactionItemsAsync),
                CallingObject = this
            });
        }
        public void LoadMainActivity(DateTime startDate, DateTime endDate)
        {
            Functions.RunOnThread(async () =>
            {
                var activites = (await GetActivitesAsync(startDate, endDate))
                                                        .Select(x => new TimelineActivityModel
                                                        {
                                                            ActivityCode = x.Code,
                                                            Id = x.Id,
                                                            Color = x.TransactionType == TransactionType.In ? ApplicationDomain.Enums.Colors.success : ApplicationDomain.Enums.Colors.danger,
                                                            DateTime = x.ActivityTime.ToString("dd/MMM HH:mm"),
                                                            IsBold = x.TransactionType == TransactionType.Out
                                                        }).ToList();
                activites.ForEach(async x =>
                {
                    x.Description = await GetActivityDescriptionAsync(x.Id);
                    OnValueUpdate.Invoke(this, new OnUpdateEventArgs
                    {
                        CallerType = GetType(),
                        CallingMethod = nameof(LoadMainActivity),
                        CallingObject = this
                    });
                });
                TimelineActivities = activites;
                OnValueUpdate.Invoke(this, new OnUpdateEventArgs
                {
                    CallerType = GetType(),
                    CallingMethod = nameof(LoadMainActivity),
                    CallingObject = this
                });
            });
        }
        public void LoadMainActivity(DateTime startDate, DateTime endDate, LedgerCategories ledgerCategory)
        {
            Functions.RunOnThread(async () =>
            {
                var items = (await _dbManager.GetRecords<LedgerItemModel>("LedgerItems")).Where(x => x.Category == ledgerCategory).ToList();
                TimelineActivities = new List<TimelineActivityModel>();
                foreach (var item in items)
                {
                    var activites = (await GetActivitesAsync(startDate, endDate)).Where(x => x.ItemId == item.Id)
                                                            .Select(x => new TimelineActivityModel
                                                            {
                                                                ActivityCode = x.Code,
                                                                Id = x.Id,
                                                                Color = x.TransactionType == TransactionType.In ? ApplicationDomain.Enums.Colors.success : ApplicationDomain.Enums.Colors.danger,
                                                                DateTime = x.ActivityTime.ToString("dd/MMM HH:mm"),
                                                                IsBold = x.TransactionType == TransactionType.Out
                                                            }).ToList();
                    activites.ForEach(async x =>
                    {
                        x.Description = await GetActivityDescriptionAsync(x.Id);
                        OnValueUpdate.Invoke(this, new OnUpdateEventArgs
                        {
                            CallerType = GetType(),
                            CallingMethod = nameof(LoadMainActivity),
                            CallingObject = this
                        });
                    });
                    TimelineActivities.AddRange(activites);
                }
                OnValueUpdate.Invoke(this, new OnUpdateEventArgs
                {
                    CallerType = GetType(),
                    CallingMethod = nameof(LoadMainActivity),
                    CallingObject = this
                });
            });
        }
        public int CurrencyIncrease { get; private set; }
        public int GoodsIncrease { get; private set; }
        public decimal TotalCurrency { get; private set; }
        public decimal TotalGoods { get; private set; }
        public async void LoadSummary()
        {
            var activities = await _dbManager.GetRecords<ActivityModel>("LedgerActivites");
            var items = await _dbManager.GetRecords<LedgerItemModel>("LedgerItems");
            var currencyItems = items.Where(x => x.Category == LedgerCategories.Currency).ToList();
            var goodsItems = items.Where(x => x.Category == LedgerCategories.Goods);
            decimal? currencyTodayTotal = activities
                                                .Where(x => currencyItems.Any(y => x.ItemId == y.Id) && x.ActivityTime >= DateTime.Today
                                                        && x.ActivityTime < DateTime.Today.AddDays(1))
                                                ?.Sum(x =>
                                                {
                                                    return x.TransactionType == TransactionType.In ? x.Value : -1 * x.Value;
                                                });
            decimal? goodsTodayTotal = activities
                                                .Where(x => goodsItems.Any(y => x.ItemId == y.Id) && x.ActivityTime >= DateTime.Today
                                                        && x.ActivityTime < DateTime.Today.AddDays(1))
                                                ?.Sum(x =>
                                                {
                                                    return x.TransactionType == TransactionType.In ? x.Value : -1 * x.Value;
                                                });
            decimal? currencyYesterdayTotal = activities
                                                .Where(x => currencyItems.Any(y => x.ItemId == y.Id) && x.ActivityTime >= DateTime.Today.AddDays(-1)
                                                        && x.ActivityTime < DateTime.Today)
                                                ?.Sum(x =>
                                                {
                                                    return x.TransactionType == TransactionType.In ? x.Value : -1 * x.Value;
                                                });
            decimal? goodsYesterdayTotal = activities
                                                .Where(x => goodsItems.Any(y => x.ItemId == y.Id) && x.ActivityTime >= DateTime.Today.AddDays(-1)
                                                        && x.ActivityTime < DateTime.Today)
                                                ?.Sum(x =>
                                                {
                                                    return x.TransactionType == TransactionType.In ? x.Value : -1 * x.Value;
                                                });
            if (currencyTodayTotal > currencyYesterdayTotal)
            {
                try
                {
                    CurrencyIncrease = (int)decimal.Round((currencyTodayTotal / currencyYesterdayTotal) ?? 0 * 100);
                }
                catch (DivideByZeroException)
                {
                    CurrencyIncrease = 0;
                }
            }
            else
            {
                try
                {
                    CurrencyIncrease = (int)decimal.Round((currencyYesterdayTotal / currencyTodayTotal) ?? 0 * 100) * -1;
                }
                catch (DivideByZeroException)
                {
                    CurrencyIncrease = 0;
                }
            }
            if (goodsTodayTotal > goodsYesterdayTotal)
            {
                try
                {
                    GoodsIncrease = (int)decimal.Round((goodsTodayTotal / goodsYesterdayTotal) ?? 0 * 100);
                }
                catch (DivideByZeroException)
                {
                    GoodsIncrease = 0;
                }
            }
            else
            {
                try
                {
                    GoodsIncrease = (int)decimal.Round((goodsYesterdayTotal / goodsTodayTotal) ?? 0 * 100) * -1;
                }
                catch (DivideByZeroException) { GoodsIncrease = 0; }
            }
            TotalCurrency = currencyTodayTotal ?? 0;
            TotalGoods = goodsTodayTotal ?? 0;
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(LoadSummary),
                CallingObject = this
            });
        }
        private async Task<string> GetActivityDescriptionAsync(long id)
        {
            var activity = await _dbManager.GetRecordById<long, ActivityModel>("LedgerActivites", id);
            var account = await _dbManager.GetRecordById<long, LedgerAccountModel>("LedgerAccounts", activity.AccountId);
            var item = await _dbManager.GetRecordById<long, LedgerItemModel>("LedgerItems", activity.ItemId);
            return $"{(activity.TransactionType == TransactionType.In ? "Got" : "Gave")} {activity.Value} {item.ItemName} {(activity.TransactionType == TransactionType.In ? "from" : "to")} {account.Title}({account.Code}). Reason: {activity.Description}";
        }
        private async Task<List<ActivityModel>> GetActivitesAsync(DateTime startDate, DateTime endDate)
        {
            var data = await _dbManager.GetRecords<ActivityModel>("LedgerActivites");
            data = data.Where(x => x.ActivityTime >= startDate && x.ActivityTime <= endDate).OrderByDescending(x => x.ActivityTime).ToList();
            return data;
        }
        public void AddNewActivity(long accountId, long itemId, TransactionType transactionType, decimal value, string description)
        {
            Functions.RunOnThread(async () =>
            {
                try
                {
                    await _dbManager.AddRecord(new StoreRecord<ActivityModel>
                    {
                        Data = new ActivityModel
                        {
                            AccountId = accountId,
                            Id = 0,
                            ItemId = itemId,
                            TransactionType = transactionType,
                            Value = value,
                            Code = NewCode,
                            ActivityTime = DateTime.Now,
                            Description = description
                        },
                        Storename = "LedgerActivites"
                    });
                    LoadTotalTransactions();
                    OnSuccessAddNew.Invoke(this, new SuccessfullOperationEventArgs
                    {
                        CallerType = GetType(),
                        CallingMethod = nameof(AddNewActivity),
                        CallingObject = this,
                        Message = "New activity has been added successfully"
                    });
                    Console.WriteLine($"Success: New activity has been added successfully");
                    LoadSummary();
                }
                catch (Exception ex)
                {
                    OnErrorAddNew.Invoke(this, new ErrorOperationEventArgs
                    {
                        CallerType = GetType(),
                        CallingMethod = nameof(AddNewActivity),
                        CallingObject = this,
                        Message = "Oops! something went wrong. Please contact to the administrator"
                    });
                    Console.WriteLine("Error: " + ex.Message);
                }
            });
        }
    }
}
