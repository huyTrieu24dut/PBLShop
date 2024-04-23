using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class ChiTietDh
{
    public string MaDh { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public int SoLuong { get; set; }

    public int SoTien { get; set; }

    public virtual DonHang MaDhNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
