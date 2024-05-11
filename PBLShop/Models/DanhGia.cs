using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class DanhGia
{
    public int MaDg { get; set; }

    public int MaSp { get; set; }

    public int MaNguoiDung { get; set; }

    public string? NoiDung { get; set; }

    public decimal? SoSao { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
