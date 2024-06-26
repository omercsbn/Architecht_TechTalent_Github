using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinansalYonetimWebApi.Models
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int invoiceID { get; set; }

        [Required]
        public Guid userID { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string senderAccountID { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string receiverAccountID { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string receiverName { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string transactionType { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal amount { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime invoiceDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime dueDate { get; set; }

        [Column(TypeName = "bit")]
        public bool paid { get; set; } = false;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime date { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "datetime")] 
        public DateTime UpdatedAt { get; set; }
    }
}
