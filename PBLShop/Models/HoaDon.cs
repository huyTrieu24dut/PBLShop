using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class HoaDon
{
    public int MaHd { get; set; }

    public int MaDh { get; set; }

    public DateTime? NgayHoanThanh { get; set; }

    public string? FileHoaDon { get; set; }

    public virtual DonHang MaDhNavigation { get; set; } = null!;
}
