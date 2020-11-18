using TechFlurry.SparkLedger.BusinessDomain.Enums;

namespace TechFlurry.SparkLedger.BusinessDomain.ViewModels
{
    public class LedgerAccountModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public LedgerCategories Category { get; set; }
        public string Phone { get; set; }
    }
}
