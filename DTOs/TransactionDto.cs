using System.ComponentModel.DataAnnotations;

namespace FinansalYonetimWebApi.Dtos
{
    public class TransactionDto
    {
        public int TransactionID { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsHide { get; set; }
        public Guid UserID { get; set; }
        public int SenderAccountID { get; set; }
        public int ReceiverAccountID { get; set; }
        public string TransactionType { get; set; }
    }
}
