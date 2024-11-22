using Microsoft.AspNetCore.Mvc;
using MyEStore.Entities;
using MyEStore.Models;

namespace MyEStore.Controllers
{
    public class CartController : Controller
    {
        const string CART_KEY = "MY_CART";
        private readonly MyeStoreContext _ctx;

        public CartController(MyeStoreContext ctx)
        {
            _ctx = ctx;
        }

        // Lấy danh sách sản phẩm trong giỏ hàng từ Session
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

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            ViewBag.TotalPrice = CartItems.Sum(item => item.SoLuong * item.DonGia);
            return View(CartItems);
        }

        // Thêm sản phẩm vào giỏ hàng
        public IActionResult AddToCart(int id, int qty = 1)
        {
            if (qty <= 0)
            {
                TempData["ThongBao"] = "Số lượng phải lớn hơn 0.";
                return RedirectToAction("Index", "Products");
            }

            var cart = HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>();

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng hay chưa
            var cartItem = cart.SingleOrDefault(p => p.MaHh == id);
            if (cartItem != null)
            {
                // Nếu đã có, tăng số lượng sản phẩm
                cartItem.SoLuong += qty;
            }
            else
            {
                // Lấy thông tin sản phẩm từ cơ sở dữ liệu
                var hangHoa = _ctx.HangHoas.SingleOrDefault(h => h.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["ThongBao"] = $"Không tìm thấy hàng hóa có mã {id}.";
                    return RedirectToAction("Index", "Products");
                }

                // Thêm sản phẩm mới vào giỏ hàng
                cartItem = new CartItem
                {
                    MaHh = id,
                    TenHh = hangHoa.TenHh,
                    Hinh = hangHoa.Hinh,
                    DonGia = hangHoa.DonGia ?? 0,
                    SoLuong = qty
                };
                cart.Add(cartItem);
            }

            // Cập nhật giỏ hàng vào Session
            HttpContext.Session.Set(CART_KEY, cart);

            TempData["ThongBao"] = "Sản phẩm đã được thêm vào giỏ hàng.";
            return RedirectToAction("Index", "Products");
        }


        // Xóa sản phẩm khỏi giỏ hàng
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

        // Cập nhật số lượng sản phẩm
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
