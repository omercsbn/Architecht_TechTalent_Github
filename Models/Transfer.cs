using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinansalYonetimWebApi.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }

        [Required]
        public int FromUserId { get; set; }

        [Required]
        public int ToUserId { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime TransferDate { get; set; } = DateTime.UtcNow;
    }
}
