using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class DanhGia
{
    public string MaDanhGia { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public decimal SoSao { get; set; }

    public string? NoiDung { get; set; }

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
