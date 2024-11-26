using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyEStore.Entities;
using MyEStore.Models;

namespace MyEStore.Controllers
{
    [Authorize]
    public class CartController : Controller
	{
		const string CART_KEY = "MY_CART";
		private readonly MyeStoreContext _ctx;

		public List<CartItem> CartItems
		{
			get
			{
				var carts = HttpContext.Session.Get<List<CartItem>>(CART_KEY);
				if (carts == null)
				{
					carts = new List<CartItem>();
				}
				return carts;
			}
		}

		public CartController(MyeStoreContext ctx)
		{
			_ctx = ctx;
		}

		public IActionResult Index()
		{
            var gioHang = CartItems;

            // Kiểm tra giỏ hàng rỗng
            if (gioHang == null || !gioHang.Any())
            {
                return View("Empty"); // Hiển thị view EmptyCart nếu giỏ hàng trống
            }

            return View(gioHang); ;
        }

		public IActionResult AddToCart(int id, int qty = 1)
		{
			var cart = CartItems;
			var cartItem = cart.SingleOrDefault(p => p.MaHh == id);
			if (cartItem != null)
			{
				cartItem.SoLuong += qty;
			}
			else
			{
				var hangHoa = _ctx.HangHoas.SingleOrDefault(h => h.MaHh == id);
				if (hangHoa == null)
				{
					//không có trong database
					TempData["ThongBao"] = $"Không tìm thấy hàng hóa có mã {id}";
					return RedirectToAction("Index", "Products");
				}
				cartItem = new CartItem
				{
					MaHh = id,
					SoLuong = qty,
					TenHh = hangHoa.TenHh,
					Hinh = hangHoa.Hinh,
					DonGia = hangHoa.DonGia ?? 0
				};
				cart.Add(cartItem);
			}
			HttpContext.Session.Set(CART_KEY, cart);
			return RedirectToAction("Index");
		}

        // Thêm sản phẩm vào giỏ và chuyển tới trang thanh toán
        public IActionResult AddToCartAndPurchase(int id, int qty = 1)
        {
            var cart = CartItems;
            var cartItem = cart.SingleOrDefault(p => p.MaHh == id);
            if (cartItem != null)
            {
                cartItem.SoLuong += qty;
            }
            else
            {
                var hangHoa = _ctx.HangHoas.SingleOrDefault(h => h.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["ThongBao"] = $"Không tìm thấy hàng hóa có mã {id}";
                    return RedirectToAction("Index", "Products");
                }
                cartItem = new CartItem
                {
                    MaHh = id,
                    SoLuong = qty,
                    TenHh = hangHoa.TenHh,
                    Hinh = hangHoa.Hinh,
                    DonGia = hangHoa.DonGia ?? 0
                };
                cart.Add(cartItem);
            }
            HttpContext.Session.Set(CART_KEY, cart);
            // Chuyển tới trang thanh toán
            return RedirectToAction("Index", "Payment");
        }

        public IActionResult RemoveCartItem(int id)
		{
			var cart = CartItems;
			var cartItem = cart.SingleOrDefault(p => p.MaHh == id);

			if (cartItem != null)
			{
				cart.Remove(cartItem);
				HttpContext.Session.Set(CART_KEY, cart);
				TempData["ThongBao"] = "Sản phẩm đã được xóa khỏi giỏ hàng.";
			}
			else
			{
				TempData["ThongBao"] = "Không tìm thấy sản phẩm trong giỏ hàng.";
			}

			return RedirectToAction("Index");
		}
		[HttpPost]
		public IActionResult UpdateCart(int id, int qty)
		{
			if (qty <= 0)
			{
				return Json(new { success = false, message = "Số lượng phải lớn hơn 0." });
			}

			var cart = CartItems;
			var cartItem = cart.SingleOrDefault(p => p.MaHh == id);

			if (cartItem != null)
			{
				cartItem.SoLuong = qty;
				HttpContext.Session.Set(CART_KEY, cart);

				// Tính toán lại tổng tiền của mặt hàng
				var newTotal = cartItem.DonGia * cartItem.SoLuong;

				// Tính tổng giá trị giỏ hàng
				var cartTotal = cart.Sum(item => item.DonGia * item.SoLuong);

				return Json(new { success = true, newTotal, cartTotal });
			}

			return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng." });
		}
		// Xóa toàn bộ giỏ hàng
		public IActionResult ClearCart()
		{
			HttpContext.Session.Remove(CART_KEY);
			TempData["ThongBao"] = "Giỏ hàng đã được làm trống.";
			return RedirectToAction("Index");
		}
	}
}