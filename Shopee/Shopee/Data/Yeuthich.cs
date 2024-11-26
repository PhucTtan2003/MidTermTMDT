using System;
using System.Collections.Generic;

namespace Shopee.Data;

public partial class Yeuthich
{
    public int MaYt { get; set; }

    public int? MaHh { get; set; }

    public string? MaKh { get; set; }

    public DateTime? NgayChon { get; set; }

    public string? MoTa { get; set; }

    public virtual Hanghoa? MaHhNavigation { get; set; }

    public virtual Khachhang? MaKhNavigation { get; set; }
}
