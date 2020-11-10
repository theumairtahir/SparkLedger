using System;

namespace TechFlurry.SparkLedger.BusinessDomain.Abstractions
{
    public class Information<T>
    {
        public static T GetInformation()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
