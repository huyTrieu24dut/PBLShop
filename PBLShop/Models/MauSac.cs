using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class MauSac
{
    public int MaMau { get; set; }

    public int MaSp { get; set; }

    public string TenMau { get; set; } = null!;

    public string? AnhSp { get; set; }

    public virtual ICollection<ChiTietDh> ChiTietDhs { get; set; } = new List<ChiTietDh>();

    public virtual ICollection<ChiTietGh> ChiTietGhs { get; set; } = new List<ChiTietGh>();

    public virtual SanPham MaSpNavigation { get; set; } = null!;

    public virtual ICollection<QuanLySanPham> QuanLySanPhams { get; set; } = new List<QuanLySanPham>();
}
