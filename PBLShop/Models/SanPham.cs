using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class SanPham
{
    public string MaSp { get; set; } = null!;

    public string MaDm { get; set; } = null!;

    public string TenSp { get; set; } = null!;

    public int DonGia { get; set; }

    public string? MoTa { get; set; }

    public int SoLuong { get; set; }

    public string? HinhAnh { get; set; }

    public virtual ICollection<ChiTietDh> ChiTietDhs { get; set; } = new List<ChiTietDh>();

    public virtual ICollection<ChiTietGh> ChiTietGhs { get; set; } = new List<ChiTietGh>();

    public virtual ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();

    public virtual ICollection<KichThuoc> KichThuocs { get; set; } = new List<KichThuoc>();

    public virtual DanhMuc MaDmNavigation { get; set; } = null!;

    public virtual ICollection<MauSac> MauSacs { get; set; } = new List<MauSac>();
}
