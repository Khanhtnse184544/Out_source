namespace BLL.Models
{
    public class PaymentResponseDto
    {
        public string OrderId { get; set; } = string.Empty;
        public string PaymentUrl { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class PaymentStatusDto
    {
        public string OrderId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public double? Amount { get; set; }
        public DateTime? TransactionDateTime { get; set; }
    }

    public class PaymentCallbackDto
    {
        public string Code { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public PaymentCallbackDataDto Data { get; set; } = new();
        public string Signature { get; set; } = string.Empty;
    }

    public class PaymentCallbackDataDto
    {
        public string OrderCode { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public string TransactionDateTime { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string PaymentLinkId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}
