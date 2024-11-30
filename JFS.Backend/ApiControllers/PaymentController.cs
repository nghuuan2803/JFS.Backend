using System.Security.Cryptography;
using System.Text;
using JFS.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JFS.Backend.ApiControllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController(IOptions<MomoOption> _options) : ControllerBase
    {
        [HttpPost("getpayment")]
        public async Task<IActionResult> CreatePayment([FromBody] string item)
        {
            string requestId = Guid.NewGuid().ToString();
            string OrderId = DateTime.Now.Ticks.ToString();
            double amount = 200000;
            string orderInfo = $"test order {item}";
            string secretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";

            var rawData = $"partnerCode={_options.Value.PartnerCode}&accessKey={_options.Value.AccessKey}&requestId={OrderId}&amount={amount}&orderId={OrderId}&orderInfo={orderInfo}&returnUrl={_options.Value.ReturnUrl}&notifyUrl={_options.Value.NotifyUrl}&extraData=";

            var signature = ComputeHmacSha256(rawData, secretKey);

            // Tạo yêu cầu tới MoMo
            var jsonData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = OrderId,
                amount = amount.ToString(),
                orderInfo = orderInfo,
                requestId = OrderId,
                extraData = "",
                signature = signature
            };

            // Gửi tới API của MoMo
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://test-payment.momo.vn/gw_payment/transactionProcessor", jsonData);
            var responseContent = await response.Content.ReadFromJsonAsync<MomoPaymentResponse>();

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Không thể tạo giao dịch với MoMo.");
            }

            return Ok(responseContent.PayUrl);

            //var responseObj = JsonSerializer.Deserialize<MoMoResponse>(responseContent);

            //// Trả URL thanh toán và OrderId về frontend
            //return Ok(new PaymentResponse { PayUrl = responseObj.payUrl, OrderId = orderId });
        }

        [HttpGet("momocallback")]
        public async Task<IActionResult> MomoCallback()
        {
            var statusCode = HttpContext.Request.Query["errorCode"].ToString();
            if (statusCode == "0")
            {
                var orderId = HttpContext.Request.Query["orderId"].ToString();
                //xử lý logic khi thành công
                //...
                string successUrl = $"https://localhost:5000/orderdetail/{orderId}";
                return Redirect(successUrl);
            }
            return Redirect("https://www.facebook.com/");
        }


        [HttpPost("momonotify")]
        public async Task<IActionResult> MomoNotify([FromBody] MomoIpnRequest request)
        {
            return StatusCode(204);
        }


        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }
    }
    public class MomoPaymentRequest
    {
        public long OrderId { get; set; } = DateTime.Now.Ticks;
        public double Amount { get; set; } = 200000;
    }
}
