using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class SanPham
{
    public int MaSp { get; set; }

    public int MaDm { get; set; }

    public string TenSp { get; set; } = null!;

    public int DonGia { get; set; }

    public string? MoTa { get; set; }

    public string? AnhSp { get; set; }
    public bool TrangThai { get; set; }

    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();

    public virtual DanhMuc MaDmNavigation { get; set; } = null!;

    public virtual ICollection<MauSac> MauSacs { get; set; } = new List<MauSac>();
}
