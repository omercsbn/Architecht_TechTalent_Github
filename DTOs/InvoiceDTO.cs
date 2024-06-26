using System;

namespace FinansalYonetimWebApi.Models
{
    public class InvoiceDto
    {
        public Guid userID { get; set; }
        public string senderAccountID { get; set; }
        public string receiverAccountID { get; set; }
        public string receiverName { get; set; }
        public string transactionType { get; set; }
        public string description { get; set; }
        public decimal amount { get; set; }
        public DateTime invoiceDate { get; set; }
        public DateTime dueDate { get; set; }
        public bool paid { get; set; }
    }
}
