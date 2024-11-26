using System;
using System.Collections.Generic;

namespace Shopee.Data;

public partial class Hanghoa
{
    public int MaHh { get; set; }

    public string TenHh { get; set; } = null!;

    public string? TenAlias { get; set; }

    public int MaLoai { get; set; }

    public string? MoTaDonVi { get; set; }

    public double? DonGia { get; set; }

    public string? Hinh { get; set; }

    public DateTime NgaySx { get; set; }

    public double GiamGia { get; set; }

    public int SoLanXem { get; set; }

    public string? MoTa { get; set; }

    public string MaNcc { get; set; } = null!;

    public string Danhgia { get; set; } = null!;

    public virtual ICollection<BanBe> BanBes { get; set; } = new List<BanBe>();

    public virtual ICollection<Chitiethd> Chitiethds { get; set; } = new List<Chitiethd>();

    public virtual Loai MaLoaiNavigation { get; set; } = null!;

    public virtual Nhacungcap MaNccNavigation { get; set; } = null!;

    public virtual ICollection<Yeuthich> Yeuthiches { get; set; } = new List<Yeuthich>();
}
