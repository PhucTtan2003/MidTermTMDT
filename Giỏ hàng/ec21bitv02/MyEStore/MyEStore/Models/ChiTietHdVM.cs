namespace MyEStore.Models
{
    public class ChiTietHdVM
    {
        public int MaHH { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double GiamGia { get; set; }
        public double ThanhTien => DonGia * SoLuong * (1 - GiamGia / 100);
    }
}
