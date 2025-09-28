using BLL.Models;

namespace BLL.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreatePaymentOrderAsync(PaymentRequestDto request);
        Task<PaymentStatusDto> GetPaymentStatusAsync(string orderId);
        Task<bool> HandlePaymentCallbackAsync(PaymentCallbackDto callbackData);
        Task<bool> VerifyPaymentAsync(string orderId);
    }
}
