using System;
using TechFlurry.SparkLedger.ApplicationDomain.EventArgs;
using TechFlurry.SparkLedger.BusinessDomain.Enums;
using TechFlurry.SparkLedger.ClientServices.Abstractions;

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
        public string NewAccountCode { get; set; }

        public event EventHandler<OnUpdateEventArgs> OnValueUpdate;
        public event EventHandler<SuccessfullOperationEventArgs> OnAccountSuccessfullyAdded;
        public event EventHandler<ErrorOperationEventArgs> OnErrorAdding;

        public void LoadNewCode()
        {
            NewAccountCode = "10001223445";
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(LoadNewCode),
                CallingObject = this
            });
        }
        public void AddNewAccount(string accountTitle, string accountCode, LedgerCategories category, string phone)
        {
            OnAccountSuccessfullyAdded.Invoke(this, new SuccessfullOperationEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(AddNewAccount),
                CallingObject = this,
                Message = "New account has been created"
            });
        }
    }
}
