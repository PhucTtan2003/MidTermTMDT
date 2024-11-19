using Microsoft.AspNetCore.Mvc;
using MyEStore.Entities;
using MyEStore.Models;

namespace MyEStore.Controllers
{
    public class CartController : Controller
    {
        private readonly MyeStoreContext _ctx;

        public CartController(MyeStoreContext ctx)
        {
            _ctx = ctx;
        }

        public const string CART_NAME = "CART";

        // Lấy giỏ hàng từ Session
        public List<CartItem> CartItems
        {
            get
            {
                return HttpContext.Session.Get<List<CartItem>>(CART_NAME) ?? new List<CartItem>();
            }
            set
            {
                HttpContext.Session.Set(CART_NAME, value);
            }
        }

        // Trang hiển thị giỏ hàng
        public IActionResult Index()
        {
            // Lấy giỏ hàng từ Session hoặc tạo giỏ hàng rỗng
            var cart = CartItems;

            // Tính tổng số lượng sản phẩm trong giỏ
            ViewBag.CartCount = cart.Sum(item => item.SoLuong);

            // Truyền giỏ hàng vào View
            return View(cart);
        }

        // Thêm sản phẩm vào giỏ hàng
        public IActionResult AddToCart(int id, int qty = 1)
        {
            if (qty <= 0)
            {
                TempData["ThongBao"] = "Số lượng phải lớn hơn 0.";
                return RedirectToAction("Index", "Product");
            }

            var cart = CartItems; // Lấy giỏ hàng từ Session

            // Kiểm tra sản phẩm đã tồn tại trong giỏ
            var cartItem = cart.SingleOrDefault(p => p.MaHh == id);
            if (cartItem != null)
            {
                // Sản phẩm đã tồn tại => Cộng thêm số lượng
                cartItem.SoLuong += qty;
            }
            else
            {
                // Lấy thông tin sản phẩm từ cơ sở dữ liệu
                var hangHoa = _ctx.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["ThongBao"] = $"Không tìm thấy sản phẩm có mã {id}";
                    return RedirectToAction("Index", "Product");
                }

                // Thêm sản phẩm mới vào giỏ hàng
                cartItem = new CartItem
                {
                    MaHh = id,
                    TenHh = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh,
                    SoLuong = qty
                };
                cart.Add(cartItem);
            }

            CartItems = cart; // Lưu lại giỏ hàng vào Session
            return RedirectToAction("Index"); // Quay lại trang giỏ hàng
        }

        // Xác nhận xóa sản phẩm
        public IActionResult ConfirmDelete(int id)
        {
            var cart = CartItems;

            // Tìm sản phẩm cần xóa
            var item = cart.SingleOrDefault(p => p.MaHh == id);
            if (item == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm trong giỏ hàng.";
                return RedirectToAction("Index");
            }

            // Truyền sản phẩm vào View
            return View(item);
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var cart = CartItems;

            // Tìm sản phẩm cần xóa
            var item = cart.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                cart.Remove(item); // Xóa sản phẩm khỏi giỏ hàng
                CartItems = cart; // Cập nhật Session
                TempData["SuccessMessage"] = "Sản phẩm đã được xóa khỏi giỏ hàng.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm trong giỏ hàng.";
            }

            return RedirectToAction("Index");
        }

        // Xóa sản phẩm qua AJAX
        [HttpPost]
        [Route("Cart/RemoveCart/{id}")]
        public IActionResult RemoveCart(int id)
        {
            var cart = CartItems;

            // Tìm sản phẩm cần xóa
            var item = cart.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                cart.Remove(item); // Xóa sản phẩm khỏi giỏ hàng
                CartItems = cart; // Cập nhật Session
                return Ok(new { success = true, message = "Xóa sản phẩm thành công." });
            }

            return NotFound(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng." });
        }

        // Cập nhật số lượng sản phẩm
        [HttpPost]
        public IActionResult UpdateCart(int id, int qty)
        {
            if (qty <= 0)
            {
                return BadRequest(new { success = false, message = "Số lượng phải lớn hơn 0." });
            }

            var cart = CartItems;

            // Tìm sản phẩm cần cập nhật
            var item = cart.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                item.SoLuong = qty; // Cập nhật số lượng sản phẩm

                CartItems = cart; // **Cập nhật Session**

                // Tính tổng tiền sản phẩm này
                var updatedSubtotal = item.DonGia * item.SoLuong;

                // Tính tổng tiền của toàn bộ giỏ hàng
                var totalPrice = cart.Sum(p => p.DonGia * p.SoLuong);

                return Ok(new
                {
                    success = true,
                    subtotal = updatedSubtotal.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")),
                    totalPrice = totalPrice.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))
                });
            }

            return NotFound(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng." });
        }
    }
}
