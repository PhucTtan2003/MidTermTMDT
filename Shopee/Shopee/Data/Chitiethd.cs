using System;
using System.Collections.Generic;

namespace Shopee.Data;

public partial class Chitiethd
{
    public int MaCt { get; set; }

    public int MaHd { get; set; }

    public int MaHh { get; set; }

    public double DonGia { get; set; }

    public int SoLuong { get; set; }

    public double GiamGia { get; set; }

    public virtual Hoadon MaHdNavigation { get; set; } = null!;

    public virtual Hanghoa MaHhNavigation { get; set; } = null!;
}
