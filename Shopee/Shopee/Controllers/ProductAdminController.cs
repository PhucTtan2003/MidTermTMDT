using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopee.Data;

namespace Shopee.Controllers
{
    [Authorize(Roles = "Administrator")] // Chỉ Admin mới được truy cập
    public class ProductAdminController : Controller
    {
        private readonly ShopporContext _context;

        public ProductAdminController(ShopporContext context)
        {
            _context = context;
        }

        // ---------------------
        // Danh sách sản phẩm
        // ---------------------
        [HttpGet]
        public IActionResult Index()
        {
            var productList = _context.Hanghoas.ToList(); // Lấy danh sách sản phẩm từ database
            return View(productList); // Hiển thị danh sách sản phẩm
        }

        // ---------------------
        // Thêm sản phẩm
        // ---------------------
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Hanghoa model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Hanghoas.Add(model); // Thêm sản phẩm vào DbSet
                    _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                    TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Ghi lỗi nếu xảy ra
                    Console.WriteLine("Lỗi khi thêm sản phẩm: " + ex.Message);
                    ModelState.AddModelError("", "Lỗi khi thêm sản phẩm: " + ex.Message);
                }
            }
            return View(model); // Trả lại view với model nếu có lỗi
        }




        // ---------------------
        // Sửa sản phẩm
        // ---------------------
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Hanghoas.FirstOrDefault(p => p.MaHh == id); // Tìm sản phẩm theo ID
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm cần sửa!";
                return RedirectToAction("Index");
            }
            return View(product); // Hiển thị form chỉnh sửa
        }

        [HttpPost]
        public IActionResult Edit(Hanghoa model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Hanghoas.Update(model); // Cập nhật sản phẩm
                    _context.SaveChanges();          // Lưu thay đổi vào database
                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật sản phẩm: " + ex.Message);
                }
            }
            return View(model); // Nếu lỗi, trả lại form sửa
        }

        // ---------------------
        // Xóa sản phẩm
        // ---------------------
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Hanghoas.FirstOrDefault(p => p.MaHh == id); // Tìm sản phẩm theo ID
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm cần xóa!";
                return RedirectToAction("Index");
            }
            return View(product); // Hiển thị form xác nhận xóa
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            var product = _context.Hanghoas.FirstOrDefault(p => p.MaHh == id); // Tìm sản phẩm
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm cần xóa!";
                return RedirectToAction("Index");
            }

            try
            {
                _context.Hanghoas.Remove(product); // Xóa sản phẩm
                _context.SaveChanges();           // Lưu thay đổi vào database
                TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa sản phẩm: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
