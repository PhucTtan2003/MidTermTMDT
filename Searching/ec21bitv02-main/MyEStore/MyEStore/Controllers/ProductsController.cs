using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEStore.Entities;
using MyEStore.Models;

namespace MyEStore.Controllers
{
	public class ProductsController : Controller
	{
		private readonly MyeStoreContext _ctx;

		public ProductsController(MyeStoreContext ctx)
		{
			_ctx = ctx;
		}

		public IActionResult Detail(int id)
		{
			var hangHoa = _ctx.HangHoas.SingleOrDefault(p => p.MaHh == id);
			if (hangHoa == null)
			{
				return NotFound();
			}
			return View(hangHoa);
		}

		public IActionResult Index(int? cateid)
		{
			var data = _ctx.HangHoas.AsQueryable();
			if (cateid.HasValue)
			{
				data = data.Where(hh => hh.MaLoai == cateid.Value);
			}

			var result = data.Select(hh => new HangHoaVM
			{
				MaHh = hh.MaHh,
				TenHh = hh.TenHh,
				DonGia = hh.DonGia ?? 0,
				Hinh = hh.Hinh
			});
			return View(result);
		}

        public IActionResult Edit(int id)
        {
            var hangHoa = _ctx.HangHoas.SingleOrDefault(p => p.MaHh == id);
            if (hangHoa == null)
            {
                return NotFound();
            }
            return View(hangHoa); // Sử dụng trực tiếp entity HangHoa
        }

        [HttpPost]
        public IActionResult Edit(HangHoa model)
        {
            if (ModelState.IsValid)
            {
                var hangHoa = _ctx.HangHoas.SingleOrDefault(p => p.MaHh == model.MaHh);
                if (hangHoa == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin sản phẩm
                hangHoa.TenHh = model.TenHh;
                hangHoa.DonGia = model.DonGia;
                hangHoa.Hinh = model.Hinh;

                _ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Search(string Searchterm)
        {
            var products = await _ctx.HangHoas
                  .Where(p => p.TenHh.Contains(Searchterm) ||
                              p.MoTa.Contains(Searchterm) ||
                              p.Hinh.Contains(Searchterm) ||
                              p.DonGia.ToString().Contains(Searchterm)) // Chuyển DonGia thành chuỗi
                  .ToListAsync();

            var productVMs = products.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                Hinh = p.Hinh,
                DonGia = p.DonGia ??0,
                MoTa = p.MoTa
            }).ToList();

            ViewBag.KeyWord = Searchterm;

            return View(productVMs);
        }

    }
}