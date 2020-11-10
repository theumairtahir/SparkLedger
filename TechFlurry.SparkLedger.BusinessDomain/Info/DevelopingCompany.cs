using TechFlurry.SparkLedger.BusinessDomain.Abstractions;

namespace TechFlurry.SparkLedger.BusinessDomain.Info
{
    public class DevelopingCompany : Information<DevelopingCompany>
    {
        public DevelopingCompany()
        {
            Name = "Tech-Flurry";
            LogoPath = "/";
            Website = "https://techflurry.co/";
        }

        public string Name { get; }
        public string LogoPath { get; }
        public string Website { get; }
    }
}
