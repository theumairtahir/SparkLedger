using System;
using System.Linq;
using TechFlurry.SparkLedger.ApplicationDomain.EventArgs;
using TechFlurry.SparkLedger.ClientServices.Abstractions;

namespace TechFlurry.SparkLedger.ClientServices.Core
{
    public interface IMenuService : IValueUpdator
    {
        int Active { get; }

        void ChangeActiveTab(int tab);
    }

    internal class MenuService : IMenuService
    {
        private readonly int[] _tabsCount = { 1, 2, 3 };
        public int Active { get; private set; }

        public event EventHandler<OnUpdateEventArgs> OnValueUpdate;

        public void ChangeActiveTab(int tab)
        {
            if (_tabsCount.Any(x => x == tab))
            {
                Active = tab;
            }
            OnValueUpdate.Invoke(this, new OnUpdateEventArgs
            {
                CallerType = GetType(),
                CallingMethod = nameof(ChangeActiveTab),
                CallingObject = this
            });
        }
    }
}
