using System;
using System.Linq;
using TechFlurry.SparkLedger.ApplicationDomain.EventArgs;
using TechFlurry.SparkLedger.BusinessDomain.Enums;
using TechFlurry.SparkLedger.BusinessDomain.ViewModels;
using TechFlurry.SparkLedger.ClientServices.Abstractions;
using TechFlurry.SparkLedger.Shared.Common;
using TG.Blazor.IndexedDB;

namespace TechFlurry.SparkLedger.ClientServices.Core
{
    public interface ILedgerAccountsService : IValueUpdator
    {
        string NewAccountCode { get; set; }

        event EventHandler<SuccessfullOperationEventArgs> OnAccountSuccessfullyAdded;
        event EventHandler<ErrorOperationEventArgs> OnErrorAdding;

        void AddNewAccount(string accountTitle, string accountCode, LedgerCategories category, string phone);
        void LoadNewCode();
    }

    internal class LedgerAccountsService : ILedgerAccountsService
    {
        private readonly IndexedDBManager _dbManager;
        private readonly IAuthenticationService _authenticationService;

        public LedgerAccountsService(IndexedDBManager dbManager, IAuthenticationService authenticationService)
        {
            _dbManager = dbManager;
            _authenticationService = authenticationService;
        }
        public string NewAccountCode { get; set; }

        public event EventHandler<OnUpdateEventArgs> OnValueUpdate;
        public event EventHandler<SuccessfullOperationEventArgs> OnAccountSuccessfullyAdded;
        public event EventHandler<ErrorOperationEventArgs> OnErrorAdding;

        public void LoadNewCode()
        {
            var prevCode = NewAccountCode;
            if (!string.IsNullOrEmpty(NewAccountCode))
            {
                NewAccountCode = (Convert.ToInt64(prevCode) + 1).ToString();
                OnValueUpdate.Invoke(this, new OnUpdateEventArgs
                {
                    CallerType = GetType(),
                    CallingMethod = nameof(LoadNewCode),
                    CallingObject = this
                });
            }
            else
            {
                Functions.RunOnThread(async () =>
                {
                    var lastRecord = (await _dbManager.GetRecords<LedgerAccountModel>("LedgerAccounts")).LastOrDefault();
                    var lastCode = lastRecord != null ? lastRecord.Code : GetStartingCode();
                    NewAccountCode = (Convert.ToInt64(lastCode) + 1).ToString();
                    OnValueUpdate.Invoke(this, new OnUpdateEventArgs
                    {
                        CallerType = GetType(),
                        CallingMethod = nameof(LoadNewCode),
                        CallingObject = this
                    });
                });
            }
        }

        private string GetStartingCode()
        {
            var code = "10000000000";
            return code;
        }

        public void AddNewAccount(string accountTitle, string accountCode, LedgerCategories category, string phone)
        {
            Functions.RunOnThread(async () =>
            {
                await _dbManager.AddRecord(new StoreRecord<LedgerAccountModel>
                {
                    Data = new LedgerAccountModel
                    {
                        Category = category,
                        Code = accountCode,
                        Id = 0,
                        Phone = phone,
                        Title = accountTitle
                    },
                    Storename = "LedgerAccounts"
                });
                OnAccountSuccessfullyAdded.Invoke(this, new SuccessfullOperationEventArgs
                {
                    CallerType = GetType(),
                    CallingMethod = nameof(AddNewAccount),
                    CallingObject = this,
                    Message = "New account has been created"
                });
            });
        }
    }
}
