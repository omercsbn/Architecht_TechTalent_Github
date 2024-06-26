using System.ComponentModel.DataAnnotations;

namespace FinansalYonetimWebApi.Models
{
    public class DepositOption
    {
        public int DepositOptionId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal InterestRate { get; set; }
        public int Term { get; set; }
    }
}
