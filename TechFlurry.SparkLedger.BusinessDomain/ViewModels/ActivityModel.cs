using System;
using TechFlurry.SparkLedger.BusinessDomain.Enums;

namespace TechFlurry.SparkLedger.BusinessDomain.ViewModels
{
    public class ActivityModel
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public long ItemId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Value { get; set; }
        public string Code { get; set; }
        public DateTime ActivityTime { get; set; }
        public string Description { get; set; }
    }
}
