using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shopee.Data;
using Shopee.Models;

namespace Shopee.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopporContext _context;

        // Constructor nhận DbContext để làm việc với cơ sở dữ liệu
        public CartController(ShopporContext context)
        {
            _context = context;
        }

        // ----------------------
        // Lấy số lượng sản phẩm trong giỏ hàng
        // ----------------------
        public int GetCartItemCount()
        {
            // Lấy giỏ hàng từ Session
            var cartJson = HttpContext.Session.GetString("Cart");

            // Nếu giỏ hàng trống, trả về số lượng là 0
            if (cartJson == null) return 0;

            // Chuyển JSON từ Session thành danh sách sản phẩm và tính tổng số lượng
            var cart = JsonConvert.DeserializeObject<List<CartItemVMViewModel>>(cartJson);
            return cart.Sum(item => item.SoLuong);
        }

        // ----------------------
        // Lấy giỏ hàng từ Session
        // ----------------------
        private List<CartItemVMViewModel> GetCart()
        {
            // Lấy JSON từ Session
            var cartJson = HttpContext.Session.GetString("Cart");

            // Nếu Session trống, trả về danh sách rỗng
            return cartJson == null
                ? new List<CartItemVMViewModel>()
                : JsonConvert.DeserializeObject<List<CartItemVMViewModel>>(cartJson);
        }

        // ----------------------
        // Lưu giỏ hàng vào Session
        // ----------------------
        private void SaveCart(List<CartItemVMViewModel> cart)
        {
            // Chuyển danh sách giỏ hàng sang JSON và lưu vào Session
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        // ----------------------
        // Thêm sản phẩm vào giỏ hàng
        // ----------------------
        public IActionResult AddToCart(int maHh, int soLuong = 1)
        {
            // Tìm sản phẩm trong cơ sở dữ liệu theo mã hàng hóa (maHh)
            var product = _context.Hanghoas.FirstOrDefault(p => p.MaHh == maHh);

            // Nếu không tìm thấy sản phẩm, trả về thông báo lỗi
            if (product == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại!";
                return RedirectToAction("Index", "Product");
            }

            // Lấy giỏ hàng từ Session
            var cart = GetCart();

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
            var existingItem = cart.FirstOrDefault(c => c.MaHh == maHh);
            if (existingItem != null)
            {
                // Nếu đã tồn tại, tăng số lượng sản phẩm
                existingItem.SoLuong += soLuong;
            }
            else
            {
                // Nếu chưa có, thêm sản phẩm mới vào giỏ
                cart.Add(new CartItemVMViewModel
                {
                    MaHh = product.MaHh,
                    TenHh = product.TenHh,
                    SoLuong = soLuong,
                    DonGia = product.DonGia ?? 0,
                    Hinh = product.Hinh
                });
            }

            // Lưu giỏ hàng đã cập nhật vào Session
            SaveCart(cart);

            // Gửi thông báo thành công và chuyển hướng về trang sản phẩm
            TempData["SuccessMessage"] = "Thêm sản phẩm vào giỏ hàng thành công!";
            return RedirectToAction("Index", "Product");
        }

        // ----------------------
        // Hiển thị giỏ hàng
        // ----------------------
        public IActionResult Index()
        {
            // Lấy giỏ hàng từ Session
            var cart = GetCart();

            // Trả về View với danh sách sản phẩm trong giỏ
            return View(cart);
        }

        // ----------------------
        // Cập nhật số lượng sản phẩm trong giỏ hàng
        // ----------------------
        [HttpPost]
        public IActionResult UpdateQuantity(int maHh, int soLuong)
        {
            // Lấy giỏ hàng từ Session
            var cart = GetCart();

            // Tìm sản phẩm trong giỏ hàng
            var item = cart.FirstOrDefault(c => c.MaHh == maHh);
            if (item != null)
            {
                if (soLuong <= 0)
                {
                    // Nếu số lượng <= 0, xóa sản phẩm khỏi giỏ
                    cart.Remove(item);
                }
                else
                {
                    // Cập nhật số lượng sản phẩm
                    item.SoLuong = soLuong;
                }
            }

            // Lưu giỏ hàng đã cập nhật vào Session
            SaveCart(cart);

            // Gửi thông báo thành công và chuyển hướng về trang giỏ hàng
            TempData["SuccessMessage"] = "Cập nhật số lượng thành công!";
            return RedirectToAction("Index");
        }

        // ----------------------
        // Xóa sản phẩm khỏi giỏ hàng
        // ----------------------
        [HttpPost]
        public IActionResult RemoveFromCart(int maHh)
        {
            // Lấy giỏ hàng từ Session
            var cart = GetCart();

            // Tìm sản phẩm cần xóa trong giỏ
            var item = cart.FirstOrDefault(c => c.MaHh == maHh);
            if (item != null)
            {
                // Xóa sản phẩm khỏi giỏ hàng
                cart.Remove(item);
            }

            // Lưu giỏ hàng đã cập nhật vào Session
            SaveCart(cart);

            // Gửi thông báo thành công và chuyển hướng về trang giỏ hàng
            TempData["SuccessMessage"] = "Xóa sản phẩm khỏi giỏ hàng thành công!";
            return RedirectToAction("Index");
        }

        // ----------------------
        // Thanh toán giỏ hàng
        // ----------------------
        public IActionResult Checkout()
        {
            // Lấy giỏ hàng từ Session
            var cart = GetCart();

            // Nếu giỏ hàng trống, trả về thông báo lỗi
            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            // Thực hiện logic thanh toán tại đây (ví dụ: lưu đơn hàng vào cơ sở dữ liệu)

            // Xóa giỏ hàng sau khi thanh toán
            HttpContext.Session.Remove("Cart");

            // Gửi thông báo thành công và chuyển hướng về trang sản phẩm
            TempData["SuccessMessage"] = "Thanh toán thành công!";
            return RedirectToAction("Index", "Product");
        }
    }
}
