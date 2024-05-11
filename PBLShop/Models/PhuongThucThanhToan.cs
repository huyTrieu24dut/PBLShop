using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class PhuongThucThanhToan
{
    public int MaPttt { get; set; }

    public string? TenPt { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
