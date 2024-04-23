using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class HoaDon
{
    public string MaHoaDon { get; set; } = null!;

    public string MaDh { get; set; } = null!;

    public DateTime? NgayHoanThanh { get; set; }

    public string? File { get; set; }

    public virtual DonHang MaDhNavigation { get; set; } = null!;
}
