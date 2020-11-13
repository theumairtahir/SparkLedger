using System;

namespace TechFlurry.SparkLedger.ApplicationDomain.EventArgs
{
    public class ApplicationEventArgs
    {
        public object CallingObject { get; set; }
        public Type CallerType { get; set; }
        public string CallingMethod { get; set; }
    }
}
