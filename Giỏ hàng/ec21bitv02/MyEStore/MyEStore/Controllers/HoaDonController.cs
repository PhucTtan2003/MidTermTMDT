using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEStore.Entities;
using MyEStore.Models;

namespace MyEStore.Controllers
{
    [Authorize]
    public class HoaDonController : Controller
    {
        private readonly MyeStoreContext _ctx;
        public HoaDonController(MyeStoreContext ctx)
        {
            _ctx = ctx;
        }
        public IActionResult LichSuHoaDon()
        {
            var maKH = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(maKH))
            {
                return Unauthorized();
            }

            // Truy vấn danh sách hóa đơn
            var hoaDons = (from hd in _ctx.HoaDons join tt in _ctx.TrangThais on hd.MaTrangThai equals tt.MaTrangThai
                           where hd.MaKh.ToString() == maKH
                           select new HoaDonVM()
                           {
                               MaHD = hd.MaHd,
                               NgayDat = hd.NgayDat,
                               HoTen = hd.HoTen,
                               DiaChi = hd.DiaChi,
                               PhiVanChuyen = (decimal)hd.PhiVanChuyen,
                               CachThanhToan = hd.CachThanhToan,
                               TenTrangThai = tt.TenTrangThai,
                               MoTa = tt.MoTa,
                               GhiChu = hd.GhiChu
                           }).ToList();
            return View(hoaDons); // Truyền danh sách vào view
        }
        public IActionResult ChiTietHoaDon(int id)
        {
            var chiTietHoaDons = _ctx.ChiTietHds
                .Where(ct => ct.MaHd == id)
                .Select(ct => new ChiTietHdVM()
                {
                    MaHH = ct.MaHh,
                    DonGia = ct.DonGia,
                    SoLuong = ct.SoLuong,
                    GiamGia = ct.GiamGia
                }).ToList();

            return View(chiTietHoaDons);
        }

    }
}
