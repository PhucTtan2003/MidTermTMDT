using System;
using System.Collections.Generic;

namespace Shopee.Data;

public partial class TrangThai
{
    public int MaTrangThai { get; set; }

    public string TenTrangThai { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();
}
