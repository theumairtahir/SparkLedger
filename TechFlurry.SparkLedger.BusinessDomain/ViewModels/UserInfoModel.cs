using TechFlurry.SparkLedger.BusinessDomain.ValueObjects;

namespace TechFlurry.SparkLedger.BusinessDomain.ViewModels
{
    public class UserInfoModel
    {
        public string UserId { get; set; }
        public Name FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string PicPath { get; set; }
    }
}
