using System;
using TechFlurry.SparkLedger.ApplicationDomain.EventArgs;
using TechFlurry.SparkLedger.BusinessDomain.ViewModels;
using TechFlurry.SparkLedger.ClientServices.Abstractions;

namespace TechFlurry.SparkLedger.ClientServices.Core
{
    public interface ILedgerItemService : IValueUpdator
    {
        event EventHandler<SuccessfullOperationEventArgs> OnItemAdded;
        event EventHandler<ErrorOperationEventArgs> OnItemAddError;

        void AddNewItem(LedgerItemModel model);
    }

    internal class LedgerItemService : ILedgerItemService
    {
        public event EventHandler<OnUpdateEventArgs> OnValueUpdate;
        public event EventHandler<SuccessfullOperationEventArgs> OnItemAdded;
        public event EventHandler<ErrorOperationEventArgs> OnItemAddError;

        public void AddNewItem(LedgerItemModel model)
        {
            OnItemAdded.Invoke(this, new SuccessfullOperationEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(AddNewItem),
                CallingObject = this,
                Message = "Item added successfully"
            });
        }
    }
}
