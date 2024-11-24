using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyEStore.Entities;
using MyEStore.Models;
using MyEStore.Models.VnPay;
using MyEStore.Services;
using System.Security.Claims;

namespace MyEStore.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly PaypalClient _paypalClient;
        private readonly MyeStoreContext _ctx;
        private readonly IVnPayService _vnPayService;

        // Constructor
        public PaymentController(PaypalClient paypalClient, MyeStoreContext ctx, IVnPayService vnPayService)
        {
            _paypalClient = paypalClient;
            _ctx = ctx;
            _vnPayService = vnPayService;
        }

        // Lấy thông tin giỏ hàng từ Session
        private const string CART_KEY = "MY_CART";

        public List<CartItem> CartItems
        {
            get
            {
                var carts = HttpContext.Session.Get<List<CartItem>>(CART_KEY);
                return carts ?? new List<CartItem>();
            }
        }

        // Hiển thị giao diện thanh toán
        public IActionResult Index()
        {
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(CartItems);
        }

        #region **Thanh toán PayPal**
        [HttpPost]
        public async Task<IActionResult> PaypalOrder(CancellationToken cancellationToken)
        {
            var totalAmount = CartItems.Sum(p => p.ThanhTien).ToString("F2");
            var currency = "USD";
            var orderReference = "DH" + DateTime.Now.Ticks;

            try
            {
                var response = await _paypalClient.CreateOrder(totalAmount, currency, orderReference);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.GetBaseException().Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PaypalCapture(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);

                if (response.status == "COMPLETED")
                {
                    SaveOrderToDatabase(response, "PayPal");
                    ClearCart();
                    return Ok(response);
                }
                else
                {
                    return BadRequest(new { Message = "Thanh toán không hoàn tất." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.GetBaseException().Message });
            }
        }
        #endregion

        #region **Thanh toán VNPay**
        public IActionResult CreatePaymentUrl(VnPaymentRequestModel model)
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, model);
            return Redirect(paymentUrl);
        }

        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VNPay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            SaveOrderToDatabase(response, "VNPay");
            ClearCart();

            TempData["Message"] = "Thanh toán VNPay thành công.";
            return RedirectToAction("Success");
        }
        #endregion

        #region **Xử lý đơn hàng**
        private void SaveOrderToDatabase(dynamic paymentResponse, string paymentMethod)
        {
            var orderReference = paymentResponse.purchase_units?[0].reference_id ?? "N/A";
            var transactionId = paymentResponse.purchase_units?[0].payments?.captures?[0]?.id ?? "N/A";

            var hoaDon = new HoaDon
            {
                MaKh = User.FindFirstValue("UserId"),
                NgayDat = DateTime.Now,
                HoTen = User.Identity.Name,
                DiaChi = "N/A",
                CachThanhToan = paymentMethod,
                CachVanChuyen = "N/A",
                MaTrangThai = 0, // Mới đặt hàng
                GhiChu = $"Reference ID: {orderReference}, Transaction ID: {transactionId}"
            };

            _ctx.HoaDons.Add(hoaDon);
            _ctx.SaveChanges();

            foreach (var item in CartItems)
            {
                var chiTietHd = new ChiTietHd
                {
                    MaHd = hoaDon.MaHd,
                    MaHh = item.MaHh,
                    DonGia = item.DonGia,
                    SoLuong = item.SoLuong,
                    GiamGia = 1
                };
                _ctx.ChiTietHds.Add(chiTietHd);
            }

            _ctx.SaveChanges();
        }

        private void ClearCart()
        {
            HttpContext.Session.Set(CART_KEY, new List<CartItem>());
        }
        #endregion

        #region **Giao diện thành công và thất bại**
        public IActionResult Success()
        {
            return View();
        }

        public IActionResult PaymentFail()
        {
            return View();
        }
        #endregion

        #region **Cập nhật thông tin thanh toán**
        [HttpPost]
        public IActionResult UpdatePayment(int orderId, string paymentMethod)
        {
            try
            {
                var order = _ctx.HoaDons.FirstOrDefault(o => o.MaHd == orderId);

                if (order == null)
                {
                    return NotFound(new { Message = "Không tìm thấy đơn hàng." });
                }

                order.CachThanhToan = paymentMethod;
                _ctx.SaveChanges();

                return Ok(new { Message = "Cập nhật phương thức thanh toán thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.GetBaseException().Message });
            }
        }
        #endregion
    }
}
