using TechFlurry.SparkLedger.ApplicationDomain.Enums;

namespace TechFlurry.SparkLedger.ApplicationDomain.Elements
{
    public class TimelineActivityModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public Colors Color { get; set; }
        public bool IsBold { get; set; }
        public string DateTime { get; set; }
        public string ActivityCode { get; set; }
    }
}
