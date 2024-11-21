using Microsoft.AspNetCore.Mvc;
using MyEStore.Entities;
using MyEStore.Models;

namespace MyEStore.Controllers
{
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
			return View(CartItems);
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
			return RedirectToAction("Index", "Products");
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
				TempData["ThongBao"] = "Số lượng phải lớn hơn 0.";
				return RedirectToAction("Index");
			}

			var cart = CartItems;
			var cartItem = cart.SingleOrDefault(p => p.MaHh == id);

			if (cartItem != null)
			{
				cartItem.SoLuong = qty;
				HttpContext.Session.Set(CART_KEY, cart);
				TempData["ThongBao"] = "Số lượng sản phẩm đã được cập nhật.";
			}
			else
			{
				TempData["ThongBao"] = "Không tìm thấy sản phẩm trong giỏ hàng.";
			}

			return RedirectToAction("Index");
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