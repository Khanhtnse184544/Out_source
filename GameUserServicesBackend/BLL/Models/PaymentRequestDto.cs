using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class PaymentRequestDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public int Amount { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public List<PaymentItemDto>? Items { get; set; }


        public string? ReturnUrl { get; set; }

        public string? CancelUrl { get; set; }
    }

    public class PaymentItemDto
    {
        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
