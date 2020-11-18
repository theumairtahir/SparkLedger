using TechFlurry.SparkLedger.BusinessDomain.Abstractions;

namespace TechFlurry.SparkLedger.BusinessDomain.Info
{
    public class Company : Information<Company>
    {
        public Company()
        {
            CompanyName = "Tech-Flurry";
            EstablishedYear = "2020";
            LogoPath = "/";
            Website = "https://www.techflurry.co/";
        }

        public string CompanyName { get; }
        public string EstablishedYear { get; }
        public string LogoPath { get; }
        public string Website { get; }
    }
}
