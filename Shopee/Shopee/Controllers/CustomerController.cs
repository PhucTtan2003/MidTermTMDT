using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopee.Data;
using Shopee.Models;
using System.Security.Claims;

namespace Shopee.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ShopporContext _context;

        public CustomerController(ShopporContext context)
        {
            _context = context;
        }

        // ---------------------
        // ĐĂNG NHẬP
        // ---------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Hiển thị form đăng nhập tại /Views/Customer/Login.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            // Xác thực người dùng
            var kh = _context.Khachhangs.SingleOrDefault(p => p.Makh == model.UserName && p.Matkhau == model.Password);
            if (kh == null)
            {
                ViewBag.ThongBao = "Sai thông tin đăng nhập.";
                return View();
            }

            // Tạo claims cho người dùng
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, kh.Hoten),
                new Claim(ClaimTypes.Email, kh.Email),
                new Claim("UserId", kh.Makh),
                new Claim(ClaimTypes.Role, kh.Vaitro == 1 ? "Administrator" : "User") // Kiểm tra vai trò
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            // Điều hướng dựa trên vai trò
            if (kh.Vaitro == 1) // Nếu là Admin
            {
                return RedirectToAction("Index", "ProductAdmin"); // Chuyển đến trang quản lý sản phẩm dành cho Admin
            }

            // Người dùng thông thường
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index", "Product");
        }

        // ---------------------
        // ĐĂNG KÝ
        // ---------------------
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Customer/Register.cshtml");
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            const string adminActivationCode = "ADMIN12345"; // Mã kích hoạt cố định cho Admin

            if (ModelState.IsValid)
            {
                // Kiểm tra mã kích hoạt nếu người dùng chọn vai trò Admin
                if (model.Vaitro == 1) // Vai trò Admin
                {
                    if (model.ActivationCode != adminActivationCode)
                    {
                        ModelState.AddModelError("ActivationCode", "Mã kích hoạt không hợp lệ.");
                        return View(model);
                    }
                }

                // Tạo khách hàng mới từ RegisterVM
                var khachHang = new Khachhang
                {
                    Makh = model.Makh,
                    Matkhau = model.Matkhau,
                    Hoten = model.Hoten,
                    Gioitinh = model.Gioitinh,
                    Ngaysinh = model.Ngaysinh ?? DateTime.Now,
                    Diachi = model.Diachi,
                    Dienthoai = model.Dienthoai,
                    Email = model.Email,
                    Hinh = model.Hinh,
                    Hieuluc = true, // Tài khoản kích hoạt mặc định
                    Vaitro = model.Vaitro // Vai trò 0: User, 1: Admin
                };

                // Lưu khách hàng vào cơ sở dữ liệu
                _context.Khachhangs.Add(khachHang);
                _context.SaveChanges();

                // Hiển thị thông báo thành công và chuyển hướng đến trang đăng nhập
                TempData["SuccessMessage"] = "Đăng ký thành công! Hãy đăng nhập.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // ---------------------
        // ĐĂNG XUẤT
        // ---------------------
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // ---------------------
        // TRANG CÁ NHÂN
        // ---------------------
        [Authorize]
        public IActionResult Profile()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var user = _context.Khachhangs.SingleOrDefault(kh => kh.Makh == userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // ---------------------
        // QUẢN LÝ DÀNH RIÊNG CHO ADMIN
        // ---------------------
        [Authorize(Roles = "Administrator")]
        public IActionResult AdminDashboard()
        {
            return View(); // Hiển thị dashboard của Admin tại /Views/Customer/AdminDashboard.cshtml
        }
    }
}
