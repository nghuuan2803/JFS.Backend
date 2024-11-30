namespace JFS.Backend.Models
{
    public class MomoOption
    {
        public string MomoApiUrl { get; set; }
        public string SecretKey { get; set; }
        public string AccessKey { get; set; }
        public string ReturnUrl { get; set; }
        public string NotifyUrl { get; set; }
        public string PartnerCode { get; set; }
        public string RequestType { get; set; }
    }
    public class MomoPaymentResponse
    {
        public string RequestId { get; set; }
        public int ErrorCode { get; set; }
        public string OrderId { get; set; }
        public string Message { get; set; }
        public string LocalMessage { get; set; }
        public string RequestType { get; set; }
        public string PayUrl { get; set; }
        public string Signature { get; set; }
        public string QrCodeUrl { get; set; }
        public string Deeplink { get; set; }
        public string DeeplinkWebInApp { get; set; }
    }

    public class MomoIpnRequest
    {
        public string OrderType { get; set; } // Loại đơn hàng (VD: momo_wallet)
        public decimal Amount { get; set; } // Số tiền giao dịch
        public string PartnerCode { get; set; } // Mã đối tác (partnerCode do MoMo cấp)
        public string OrderId { get; set; } // ID đơn hàng từ hệ thống của bạn
        public string ExtraData { get; set; } // Dữ liệu thêm (mã hóa Base64)
        public string Signature { get; set; } // Chữ ký để xác thực
        public long TransId { get; set; } // ID giao dịch từ MoMo
        public long ResponseTime { get; set; } // Thời gian phản hồi
        public int ResultCode { get; set; } // Mã kết quả (0 là thành công, khác 0 là lỗi)
        public string Message { get; set; } // Tin nhắn phản hồi từ MoMo
        public string PayType { get; set; } // Phương thức thanh toán (VD: QR, ATM)
        public string RequestId { get; set; } // ID yêu cầu giao dịch từ hệ thống của bạn
        public string OrderInfo { get; set; } // Thông tin đơn hàng
    }
}
