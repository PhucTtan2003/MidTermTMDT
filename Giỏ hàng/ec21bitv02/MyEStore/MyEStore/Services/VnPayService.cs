using MyEStore.Models.VnPay;

namespace MyEStore.Services
{
	public class VnPayService : IVnPayService
	{
		private IConfiguration _config;

		public VnPayService(IConfiguration config) { _config = config; }
		public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
		{
			var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_config["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _config["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _config["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _config["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _config["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _config["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _config["vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", "Thanh Toán Đơn Hàng:" + model.OrderId);
            pay.AddRequestData("vnp_OrderType", "Order");
            pay.AddRequestData("vnp_ReturnUrl", _config["Vnpay:PaymentBackReturnUrl"]);
			pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = pay.CreateRequestUrl(_config["Vnpay:BaseUrl"], _config["Vnpay:HashSecret"]);

            return paymentUrl;
		}

		public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
		{
			var vnpay = new VnPayLibrary();
            foreach(var (key, value) in  collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
					vnpay.AddRequestData(key, value.ToString());
                }
            }

            var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
			var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
			var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
			
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
			if (!checkSignature)
			{
				return new VnPaymentResponseModel
				{
					Success = false
				};
			}

			return new VnPaymentResponseModel
			{
				Success = true,
				PaymentMethod = "VnPay",
				OrderDescription = vnp_OrderInfo,
				OrderId = vnp_orderId.ToString(),
				TransactionId = vnp_TransactionId.ToString(),
				Token = vnp_SecureHash,
				VnPayResponseCode = vnp_ResponseCode.ToString()
			};
		}
	}
}
