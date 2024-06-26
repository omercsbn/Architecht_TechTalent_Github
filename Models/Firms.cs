using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinansalYonetimWebApi.Models
{
    public class Firm
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string company_name { get; set; }

        [Required]
        public int subscription_id { get; set; }

        [Required]
        public int payment_value { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(6)")]
        public string unique_company_id { get; set; }

        public int interest_rate_overdue_payment { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(7)")]
        public string payment_status { get; set; }

        [Required]
        public DateTime subscription_start_date { get; set; }

        [Required]
        public DateTime subscription_end_date { get; set; }

        [Required]
        public DateTime createdAt { get; set; }

        [Required]
        public DateTime updatedAt { get; set; }
    }
}
