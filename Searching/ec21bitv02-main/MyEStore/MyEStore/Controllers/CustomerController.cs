using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyEStore.Entities;
using MyEStore.Models;
using System.Security.Claims;
using MyEStore.MyTool;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Authorization;

namespace MyEStore.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MyeStoreContext _ctx;
        public CustomerController(MyeStoreContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            var kh = _ctx.KhachHangs.SingleOrDefault(p => p.MaKh == model.UserName);
            if (kh == null)
            {
                ViewBag.ThongBaoLoi = "User này không tồn tại";
                return View();
            }

            if (kh.MatKhau != model.Password.ToMd5Hash(kh.RandomKey))
            {
                ViewBag.ThongBaoLoi = "Đăng nhập không thành công";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, kh.Email),
                new Claim("FullName", kh.HoTen),

                new Claim(ClaimTypes.Role, "Administrator"),//động
            };

            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var claimPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimPrincipal);
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            return Redirect("/");
        }
        [Authorize]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM model, IFormFile FileHinh)
        {
            try
            {
                var khachHang = new KhachHang
                {
                    MaKh = model.MaKh,
                    HoTen = model.HoTen,
                    NgaySinh = model.NgaySinh,
                    DiaChi = model.DiaChi,
                    GioiTinh = model.GioiTinh,
                    DienThoai = model.DienThoai,
                    Email = model.Email,
                    Hinh = MyTool.MyTools.UploadFileToFolder(FileHinh, "KhachHang"),

                    HieuLuc = true, //false + gửi mail active tài khoản
                    RandomKey = MyTool.MyTools.GetRandom()
                };
                khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                _ctx.Add(khachHang);
                _ctx.SaveChanges();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}
