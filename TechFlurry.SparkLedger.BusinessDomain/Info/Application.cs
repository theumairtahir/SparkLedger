using TechFlurry.SparkLedger.BusinessDomain.Abstractions;

namespace TechFlurry.SparkLedger.BusinessDomain.Info
{
    public class Application : Information<Application>
    {
        public Application()
        {
            AppName = "Spark Ledger";
            Version = "Dev 1.0";
            Build = 1;
        }

        public string AppName { get; set; }
        public string Version { get; set; }
        public int Build { get; set; }
    }
}
