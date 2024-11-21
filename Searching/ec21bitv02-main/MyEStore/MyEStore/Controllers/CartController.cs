using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyEStore.Entities;
using MyEStore.Models;
using System.Runtime.CompilerServices;
using static MyEStore.Models.ECommerceDemo;

namespace MyEStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public const string CART_NAME = "CART";
        private readonly MyeStoreContext _ctx;
        private readonly PaypalClient _paypalClient;

        public CartController(MyeStoreContext ctx, PaypalClient paypalClient) { _ctx = ctx; _paypalClient = paypalClient; }

       
        public List<CartItem> CartItems
        {
            get
            {
                var carts = HttpContext.Session.Get<List<CartItem>>(CART_NAME) ?? new List<CartItem>();
                return carts;
            }
        }

        public IActionResult Index()
        {
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(CartItems);
        }
		[HttpPost]
		public async Task<IActionResult> PaypalOrder(CancellationToken cancellationToken)
		{
            // Tạo đơn hàng (thông tin lấy từ Session???)
            var tongTien = "1000";
			var donViTienTe = "USD";
			// OrderId - mã tham chiếu (duy nhất)
			var orderIdref = "DH" + DateTime.Now.Ticks.ToString();

			try
			{
				// a. Create paypal order
				var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, orderIdref);

				return Ok(response);
			}
			catch (Exception e)
			{
				var error = new
				{
					e.GetBaseException().Message
				};

				return BadRequest(error);
			}
		}

        public async Task<IActionResult> PaypalCapture(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);

                var reference = response.purchase_units[0].reference_id;

                // Put your logic to save the transaction here
                // You can use the "reference" variable as a transaction key
                // Lưu đơn hàng vô database

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }
			public IActionResult AddToCart(int id, int qty = 1)
        {
            var cart = CartItems;
            //kiểm tra xem có ko?
            var cartItem = cart.SingleOrDefault(p => p.MaHh == id);
            if (cartItem != null)
            {
                cartItem.SoLuong += qty;
            }
            else
            {
                var hangHoa = _ctx.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["ThongBao"] = $"Tìm không thấy sản phẩm có mã {id}";
                    return RedirectToAction("Index", "Products");
                }
                cartItem = new CartItem
                {
                    MaHh = id,
                    SoLuong = qty,
                    TenHh = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh
                };
                cart.Add(cartItem);
            }
            //set session
            HttpContext.Session.Set<List<CartItem>>(CART_NAME, cart);
            return RedirectToAction("Index");
        }
        public IActionResult RemoveCart(int id)
        {
            var gioHang = CartItems;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set("CART", gioHang);
            }
            return RedirectToAction("Index");
        }
	


	}
}