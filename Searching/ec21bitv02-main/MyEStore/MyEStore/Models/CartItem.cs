
namespace MyEStore.Models
{
    public class CartItem
    {
        public int MaHh { get; set; }
        public string TenHh { get; set; }
        public double DonGia { get; set; }
        public string? Hinh { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien => SoLuong * DonGia;

		internal static object Sum(Func<object, object> value)
		{
			throw new NotImplementedException();
		}
	}
}
