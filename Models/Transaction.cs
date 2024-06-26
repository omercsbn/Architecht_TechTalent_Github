using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinansalYonetimWebApi.Models
{
    public class Transaction
    {
        public Guid UserID { get; set; }
        public int TransactionID { get; set; }

        [Required]
        public int SenderAccountID { get; set; }

        [Required]
        public int ReceiverAccountID { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionType { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required]
        public bool IsHide { get; set; }
    }
}
