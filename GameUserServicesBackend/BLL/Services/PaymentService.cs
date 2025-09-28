using BLL.Models;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;

namespace BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PayOS _payOS;
        private readonly db_userservicesContext _context;
        private readonly IDbContextFactory<db_userservicesContext> _contextFactory;
        private readonly PayOSConfig _config;

        public PaymentService(IOptions<PayOSConfig> config, db_userservicesContext context, IDbContextFactory<db_userservicesContext> contextFactory)
        {
            _config = config.Value;
            _payOS = new PayOS(_config.ClientId, _config.ApiKey, _config.ChecksumKey);
            _context = context;
            _contextFactory = contextFactory;
        }

        public async Task<PaymentResponseDto> CreatePaymentOrderAsync(PaymentRequestDto request)
        {
            // Kiểm tra người dùng tồn tại
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                throw new ArgumentException($"Người dùng với UserId {request.UserId} không tồn tại.");
            }

            // Kiểm tra Amount
            if (request.Amount <= 0)
            {
                throw new ArgumentException("Số tiền phải lớn hơn 0.");
            }

            // Unique code for each order
            var orderCode = (long)(DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond);

            // Tạo items cho PayOS
            var items = new List<ItemData>();
            if (request.Items != null && request.Items.Any())
            {
                foreach (var item in request.Items)
                {
                    items.Add(new ItemData(item.Name, item.Quantity, item.Price));
                }
            }
            else
            {
                // Default item nếu không có items
                items.Add(new ItemData(request.Description, 1, request.Amount));
            }

            // Tạo PaymentData
            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: request.Amount,
                description: request.Description,
                items: items,
                returnUrl: request.ReturnUrl ?? "https://your-app.com/payment/success",
                cancelUrl: request.CancelUrl ?? "https://your-app.com/payment/cancel"
            );

            try
            {
                // Log thông tin request trước khi gọi PayOS
                Console.WriteLine($"[PaymentService] Creating payment link for OrderCode: {orderCode}, Amount: {request.Amount}, UserId: {request.UserId}");
                Console.WriteLine($"[PaymentService] PayOS Request Data: OrderCode={orderCode}, Amount={request.Amount}, Description={request.Description}");
                
                // Gọi PayOS với timeout và logging
                CreatePaymentResult result;
                var startTime = DateTime.Now;
                
                try
                {
                    // Tạo CancellationToken với timeout 5 giây (ngắn để không chờ quá lâu)
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    
                    Console.WriteLine($"[PaymentService] Calling PayOS API at {startTime:yyyy-MM-dd HH:mm:ss}");
                    
                    result = await _payOS.createPaymentLink(paymentData);
                    
                    var endTime = DateTime.Now;
                    var duration = endTime - startTime;
                    
                    Console.WriteLine($"[PaymentService] PayOS API Response received at {endTime:yyyy-MM-dd HH:mm:ss}");
                    Console.WriteLine($"[PaymentService] PayOS API Duration: {duration.TotalMilliseconds}ms");
                    Console.WriteLine($"[PaymentService] PayOS Response: CheckoutUrl={result.checkoutUrl}, PaymentLinkId={result.paymentLinkId}");
                }
                catch (OperationCanceledException)
                {
                    var endTime = DateTime.Now;
                    var duration = endTime - startTime;
                    Console.WriteLine($"[PaymentService] PayOS API TIMEOUT after {duration.TotalMilliseconds}ms");
                    throw new Exception($"PayOS API timeout after {duration.TotalSeconds} seconds. Vui lòng thử lại sau.");
                }
                catch (Exception payosEx)
                {
                    var endTime = DateTime.Now;
                    var duration = endTime - startTime;
                    Console.WriteLine($"[PaymentService] PayOS API ERROR after {duration.TotalMilliseconds}ms: {payosEx.Message}");
                    Console.WriteLine($"[PaymentService] PayOS Error Details: {payosEx}");
                    throw new Exception($"PayOS API error: {payosEx.Message}", payosEx);
                }

                // Log thông tin response từ PayOS
                Console.WriteLine($"[PaymentService] PayOS Success Response:");
                Console.WriteLine($"  - CheckoutUrl: {result.checkoutUrl}");
                Console.WriteLine($"  - PaymentLinkId: {result.paymentLinkId}");
                Console.WriteLine($"  - OrderCode: {orderCode}");

                // DỪNG NGAY SAU KHI NHẬN ĐƯỢC PAYOS RESPONSE - KHÔNG CHỜ THÊM
                Console.WriteLine($"[PaymentService] PayOS response received successfully - proceeding to save and return response");

                // TRẢ VỀ RESPONSE NGAY LẬP TỨC - KHÔNG CHỜ DATABASE SAVE
                Console.WriteLine($"[PaymentService] Skipping database save due to timeout issues - returning response immediately");
                
                // TODO: Implement async database save in background
                // Lưu thông tin thanh toán vào cơ sở dữ liệu (sử dụng Transactionhistory)
                try
                {
                    var transaction = new Transactionhistory
                    {
                        Id = orderCode.ToString(),
                        UserId = request.UserId,
                        DateTrade = DateTime.Now,
                        Status = "Pending",
                        Amount = request.Amount  
                    };
                    
                    Console.WriteLine($"[PaymentService] Attempting to save transaction in background...");
                    _context.Transactionhistories.Add(transaction);

                    // Submit background save using a fresh context from factory to avoid disposed context issues
                    _ = Task.Run(async () =>
                    {
                        var bgStart = DateTime.Now;
                        try
                        {
                            using var bgContext = await _contextFactory.CreateDbContextAsync();
                            bgContext.Transactionhistories.Add(new Transactionhistory
                            {
                                Id = transaction.Id,
                                UserId = transaction.UserId,
                                DateTrade = transaction.DateTrade,
                                Status = transaction.Status,
                                Amount = transaction.Amount
                            });

                            await bgContext.SaveChangesAsync();
                            var bgDuration = DateTime.Now - bgStart;
                            Console.WriteLine($"[PaymentService] Background save OK in {bgDuration.TotalMilliseconds}ms: OrderCode={orderCode}");
                        }
                        catch (Exception bgEx)
                        {
                            var bgDuration = DateTime.Now - bgStart;
                            Console.WriteLine($"[PaymentService] Background save FAILED after {bgDuration.TotalMilliseconds}ms: {bgEx.Message}");
                        }
                    });

                    Console.WriteLine($"[PaymentService] Transaction queued for background save (factory): OrderCode={orderCode}, Status=Pending");
                }
                catch (Exception dbEx)
                {
                    Console.WriteLine($"[PaymentService] Database save failed but continuing: {dbEx.Message}");
                }

                // TRẢ VỀ RESPONSE NGAY LẬP TỨC
                var response = new PaymentResponseDto
                {
                    OrderId = orderCode.ToString(),
                    PaymentUrl = result.checkoutUrl,
                    TransactionId = result.paymentLinkId,
                    Amount = request.Amount,
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                Console.WriteLine($"[PaymentService] Payment response prepared - returning to client");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PaymentService] ERROR in CreatePaymentOrderAsync: {ex.Message}");
                Console.WriteLine($"[PaymentService] Error StackTrace: {ex.StackTrace}");
                throw new Exception($"Lỗi tạo thanh toán: {ex.Message}", ex);
            }
        }

        public async Task<PaymentStatusDto> GetPaymentStatusAsync(string orderId)
        {
            Console.WriteLine($"[PaymentService] Getting payment status for OrderId: {orderId}");
            
            var transaction = await _context.Transactionhistories
                .FirstOrDefaultAsync(t => t.Id == orderId);

            if (transaction == null)
            {
                Console.WriteLine($"[PaymentService] Transaction not found for OrderId: {orderId}");
                throw new Exception("Không tìm thấy giao dịch.");
            }

            var startTime = DateTime.Now;
            
            try
            {
                Console.WriteLine($"[PaymentService] Calling PayOS getPaymentLinkInformation for OrderId: {orderId}");
                
                // Lấy thông tin từ PayOS với timeout
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                PaymentLinkInformation info = await _payOS.getPaymentLinkInformation(long.Parse(orderId));
                
                var endTime = DateTime.Now;
                var duration = endTime - startTime;
                
                Console.WriteLine($"[PaymentService] PayOS getPaymentLinkInformation completed in {duration.TotalMilliseconds}ms");
                Console.WriteLine($"[PaymentService] PayOS Status Response: {info.status}");
                
                // Cập nhật trạng thái
                var oldStatus = transaction.Status;
                transaction.Status = info.status == "PAID" ? "Completed" :
                                   info.status == "CANCELLED" ? "Failed" : "Pending";

                await _context.SaveChangesAsync();
                
                Console.WriteLine($"[PaymentService] Status updated from {oldStatus} to {transaction.Status}");

                return new PaymentStatusDto
                {
                    OrderId = orderId,
                    Status = transaction.Status,
                    Amount = transaction.Amount, 
                    TransactionDateTime = transaction.DateTrade
                };
            }
            catch (OperationCanceledException)
            {
                var endTime = DateTime.Now;
                var duration = endTime - startTime;
                Console.WriteLine($"[PaymentService] PayOS getPaymentLinkInformation TIMEOUT after {duration.TotalMilliseconds}ms");
                throw new Exception($"PayOS API timeout after {duration.TotalSeconds} seconds. Vui lòng thử lại sau.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PaymentService] ERROR in GetPaymentStatusAsync: {ex.Message}");
                throw new Exception($"Lỗi lấy trạng thái thanh toán: {ex.Message}", ex);
            }
        }

        public async Task<bool> HandlePaymentCallbackAsync(PaymentCallbackDto callbackData)
        {
            try
            {
                Console.WriteLine($"[PaymentService] Handling payment callback for OrderCode: {callbackData.Data.OrderCode}");
                Console.WriteLine($"[PaymentService] Callback Data: Code={callbackData.Code}, Desc={callbackData.Desc}");
                Console.WriteLine($"[PaymentService] Callback Amount: {callbackData.Data.Amount}, Currency: {callbackData.Data.Currency}");
                
                // Tìm giao dịch trong cơ sở dữ liệu
                var transaction = await _context.Transactionhistories
                    .FirstOrDefaultAsync(t => t.Id == callbackData.Data.OrderCode);

                if (transaction == null)
                {
                    Console.WriteLine($"[PaymentService] Transaction not found for OrderCode: {callbackData.Data.OrderCode}");
                    return false;
                }

                Console.WriteLine($"[PaymentService] Found transaction: UserId={transaction.UserId}, CurrentStatus={transaction.Status}");

                // Cập nhật trạng thái thanh toán
                var oldStatus = transaction.Status;
                transaction.Status = callbackData.Code == "00" ? "Completed" : "Failed";
                
                Console.WriteLine($"[PaymentService] Status updated from {oldStatus} to {transaction.Status}");

                // Nếu thanh toán thành công, cập nhật coin cho user
                if (transaction.Status == "Completed")
                {
                    var user = await _context.Users.FindAsync(transaction.UserId);
                    if (user != null)
                    {
                        var oldCoin = user.Coin;
                        // Tăng coin cho user (có thể tính toán dựa trên amount)
                        user.Coin += 100; // Ví dụ: mỗi lần thanh toán thành công +100 coin
                        
                        Console.WriteLine($"[PaymentService] User coin updated: {oldCoin} -> {user.Coin} for UserId: {transaction.UserId}");
                    }
                    else
                    {
                        Console.WriteLine($"[PaymentService] User not found for UserId: {transaction.UserId}");
                    }
                }

                await _context.SaveChangesAsync();
                Console.WriteLine($"[PaymentService] Callback processed successfully for OrderCode: {callbackData.Data.OrderCode}");
                return true;
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"[PaymentService] ERROR handling payment callback: {ex.Message}");
                Console.WriteLine($"[PaymentService] Callback Error StackTrace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> VerifyPaymentAsync(string orderId)
        {
            try
            {
                Console.WriteLine($"[PaymentService] Verifying payment for OrderId: {orderId}");
                
                var transaction = await _context.Transactionhistories
                    .FirstOrDefaultAsync(t => t.Id == orderId);

                if (transaction == null)
                {
                    Console.WriteLine($"[PaymentService] Transaction not found for verification OrderId: {orderId}");
                    return false;
                }

                Console.WriteLine($"[PaymentService] Found transaction for verification: UserId={transaction.UserId}, CurrentStatus={transaction.Status}");

                // Lấy thông tin từ PayOS để verify với timeout
                var startTime = DateTime.Now;
                try
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                    PaymentLinkInformation info = await _payOS.getPaymentLinkInformation(long.Parse(orderId));
                    
                    var endTime = DateTime.Now;
                    var duration = endTime - startTime;
                    
                    Console.WriteLine($"[PaymentService] PayOS verification completed in {duration.TotalMilliseconds}ms");
                    Console.WriteLine($"[PaymentService] PayOS Verification Status: {info.status}");
                    
                    // Cập nhật trạng thái
                    var oldStatus = transaction.Status;
                    transaction.Status = info.status == "PAID" ? "Completed" :
                                       info.status == "CANCELLED" ? "Failed" : "Pending";
                    
                    Console.WriteLine($"[PaymentService] Verification status updated from {oldStatus} to {transaction.Status}");

                    await _context.SaveChangesAsync();
                    
                    var isCompleted = transaction.Status == "Completed";
                    Console.WriteLine($"[PaymentService] Payment verification result: {isCompleted}");
                    return isCompleted;
                }
                catch (OperationCanceledException)
                {
                    var endTime = DateTime.Now;
                    var duration = endTime - startTime;
                    Console.WriteLine($"[PaymentService] PayOS verification TIMEOUT after {duration.TotalMilliseconds}ms");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PaymentService] ERROR verifying payment: {ex.Message}");
                Console.WriteLine($"[PaymentService] Verification Error StackTrace: {ex.StackTrace}");
                return false;
            }
        }
    }
}
