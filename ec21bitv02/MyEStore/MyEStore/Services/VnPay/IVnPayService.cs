using MyEStore.Models.VnPay;

namespace MyEStore.Services.VnPay
{
	public interface IVnPayService
	{
		string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);

		VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
	}
}
