using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinansalYonetimWebApi.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        public int? WithdrawalAccountId { get; set; }

        public String DepositOption { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(20)] // Önerilen maksimum uzunluk, en uzun değer olan "Deposit" için 7 karakterdir.
        public string AccountType { get; set; }

        [Required]
        [StringLength(26)]
        public string Iban { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Balance { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal AvailableBalance { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        public DateTime OpenDate { get; set; }

        // constructor for IBAN generation
        public Account()
        {
            Iban = GenerateIBAN();
           
        }

        // Iban generation method
        private string GenerateIBAN()
        {
            const string countryCode = "TR";
            const string controlDigits = "96";
            const string bankCode = "12345";
            const string reserveArea = "0";

            // random part of IBAN
            string randomPart = GenerateRandomDigits(16); // remaining 16 digits

 
            string iban = $"{countryCode}{controlDigits}{bankCode}{reserveArea}{randomPart}";

            return iban;
        }

        // generator for random digits
        private string GenerateRandomDigits(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
