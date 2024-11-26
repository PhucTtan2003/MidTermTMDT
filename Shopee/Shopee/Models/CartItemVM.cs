using System;

namespace Shopee.Models
{
    public class CartItemVMViewModel
    {
        public int MaHh { get; set; } // Mã hàng hóa
        public string TenHh { get; set; } = null!; // Tên hàng hóa
        public string? Hinh { get; set; } // Hình ảnh sản phẩm
        public double? DonGia { get; set; } // Giá sản phẩm
        public int SoLuong { get; set; } // Số lượng
        public double ThanhTien => (DonGia ?? 0) * SoLuong; // Thành tiền
    }
}
