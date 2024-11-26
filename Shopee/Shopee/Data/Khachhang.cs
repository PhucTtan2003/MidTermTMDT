using System;
using System.Collections.Generic;

namespace Shopee.Data;

public partial class Khachhang
{
    public string Makh { get; set; } = null!;

    public string? Matkhau { get; set; }

    public string Hoten { get; set; } = null!;

    public bool Gioitinh { get; set; }

    public DateTime Ngaysinh { get; set; }

    public string? Diachi { get; set; }

    public string? Dienthoai { get; set; }

    public string Email { get; set; } = null!;

    public string? Hinh { get; set; }

    public bool Hieuluc { get; set; }

    public int Vaitro { get; set; }

    public string? Randomkey { get; set; }

    public virtual ICollection<BanBe> BanBes { get; set; } = new List<BanBe>();

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();

    public virtual ICollection<Yeuthich> Yeuthiches { get; set; } = new List<Yeuthich>();
}
