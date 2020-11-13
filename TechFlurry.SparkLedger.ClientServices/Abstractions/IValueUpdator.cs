using System;
using TechFlurry.SparkLedger.ApplicationDomain.EventArgs;

namespace TechFlurry.SparkLedger.ClientServices.Abstractions
{
    public interface IValueUpdator
    {
        event EventHandler<OnUpdateEventArgs> OnValueUpdate;
    }
}
