using System;
using System.Collections.Generic;

namespace Shopee.Data;

public partial class PhanCong
{
    public int MaPc { get; set; }

    public string MaNv { get; set; } = null!;

    public string MaPb { get; set; } = null!;

    public DateTime? NgayPc { get; set; }

    public bool? HieuLuc { get; set; }

    public virtual Nhanvien MaNvNavigation { get; set; } = null!;
}
