using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class ChiTietGh
{
    public string MaSp { get; set; } = null!;

    public string MaKh { get; set; } = null!;

    public int? SoLuong { get; set; }

    public virtual KhachHang MaKhNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
