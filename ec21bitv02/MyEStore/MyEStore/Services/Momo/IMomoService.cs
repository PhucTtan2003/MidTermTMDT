using MyEStore.Models;
using MyEStore.Models.Momo;

namespace MyEStore.Services.Momo
{
	public interface IMomoService
	{
		Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfo model);
		MomoExecuteResponseModel PaymentExcuteAsync(IQueryCollection collection);
	}
}
