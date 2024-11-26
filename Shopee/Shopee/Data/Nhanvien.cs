using System;
using System.Collections.Generic;

namespace Shopee.Data;

public partial class Nhanvien
{
    public string Manv { get; set; } = null!;

    public string Hoten { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Matkhau { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();

    public virtual ICollection<PhanCong> PhanCongs { get; set; } = new List<PhanCong>();
}
