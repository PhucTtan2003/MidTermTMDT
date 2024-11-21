using MyEStore.Models.VnPay;

namespace MyEStore.Services
{
	public interface IVnPayService
	{
		string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);

		VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
	}
}
