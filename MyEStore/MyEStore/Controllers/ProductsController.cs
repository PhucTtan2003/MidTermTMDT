using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult Edit(HangHoa model)
        {
            if (!ModelState.IsValid)
            {
                // Log or inspect model state errors
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model);
            }

            var hangHoa = _ctx.HangHoas.SingleOrDefault(p => p.MaHh == model.MaHh);
            if (hangHoa == null)
            {
                return NotFound();
            }

            // Update the properties
            hangHoa.TenHh = model.TenHh;
            hangHoa.DonGia = model.DonGia;
            hangHoa.Hinh = model.Hinh;

            _ctx.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}