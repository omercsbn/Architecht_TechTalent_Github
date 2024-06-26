using System;
using System.ComponentModel.DataAnnotations;

namespace FinansalYonetimWebApi.Models
{
    public class CurrencyRate
    {
        public int id { get; set; }

        [Required]
        public string CurrencyCode { get; set; }

        [Required]
        public decimal SaleRate { get; set; }

        [Required]
        public decimal PurchaseRate { get; set; }

        [Required]
        public decimal ChangeAmount { get; set; }

        [Required]
        public decimal ChangeRate { get; set; }

        [Required]
        public string ChangeDirection { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
