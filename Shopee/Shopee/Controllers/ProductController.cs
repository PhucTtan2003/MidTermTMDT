using Microsoft.AspNetCore.Mvc;
using Shopee.Data;
using System.Linq;

namespace Shopee.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopporContext _context;

        public ProductController(ShopporContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Lấy tên người dùng đã đăng nhập (nếu có)
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;

            // Lấy danh sách hàng hóa từ cơ sở dữ liệu
            var hanghoaList = _context.Hanghoas
                .OrderByDescending(hh => hh.SoLanXem) // Sắp xếp theo số lượt xem
                .ToList();

            // Truyền tên người dùng xuống View qua ViewBag
            ViewBag.UserName = userName;

            return View(hanghoaList);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            // Lấy thông tin hàng hóa theo ID
            var hanghoa = _context.Hanghoas.FirstOrDefault(hh => hh.MaHh == id);
            if (hanghoa == null)
            {
                return NotFound();
            }

            return View(hanghoa);
        }
    }
}
