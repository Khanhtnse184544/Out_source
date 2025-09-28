using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameUserServicesBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        /// <summary>
        /// Tạo đơn hàng thanh toán
        /// </summary>
        [HttpPost("create-order")]
        public async Task<IActionResult> CreatePaymentOrder([FromBody] PaymentRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _paymentService.CreatePaymentOrderAsync(request);
                return Ok(new { status = "success", data = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid payment request: {Message}", ex.Message);
                return BadRequest(new { status = "error", message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment order");
                return StatusCode(500, new { status = "error", message = "Lỗi tạo thanh toán" });
            }
        }

        /// <summary>
        /// Lấy trạng thái thanh toán
        /// </summary>
        [HttpGet("status/{orderId}")]
        public async Task<IActionResult> GetPaymentStatus(string orderId)
        {
            try
            {
                var result = await _paymentService.GetPaymentStatusAsync(orderId);
                return Ok(new { status = "success", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment status for order {OrderId}", orderId);
                return StatusCode(500, new { status = "error", message = "Lỗi lấy trạng thái thanh toán" });
            }
        }

        /// <summary>
        /// Xác minh thanh toán
        /// </summary>
        [HttpGet("verify/{orderId}")]
        public async Task<IActionResult> VerifyPayment(string orderId)
        {
            try
            {
                var isVerified = await _paymentService.VerifyPaymentAsync(orderId);
                return Ok(new { status = "success", data = new { verified = isVerified } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying payment for order {OrderId}", orderId);
                return StatusCode(500, new { status = "error", message = "Lỗi xác minh thanh toán" });
            }
        }

        /// <summary>
        /// Webhook callback từ PayOS
        /// </summary>
        [HttpPost("callback")]
        public async Task<IActionResult> PaymentCallback([FromBody] PaymentCallbackDto callbackData)
        {
            try
            {
                var result = await _paymentService.HandlePaymentCallbackAsync(callbackData);
                
                if (result)
                {
                    return Ok(new { status = "success", message = "Payment processed successfully" });
                }
                else
                {
                    return BadRequest(new { status = "error", message = "Failed to process payment" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling payment callback");
                return StatusCode(500, new { status = "error", message = "Lỗi xử lý callback thanh toán" });
            }
        }
    }
}
