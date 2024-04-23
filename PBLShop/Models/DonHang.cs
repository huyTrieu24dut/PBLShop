using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class DonHang
{
    public string MaDh { get; set; } = null!;

    public string MaKh { get; set; } = null!;

    public DateTime NgayDat { get; set; }

    public string DiaChi { get; set; } = null!;

    public int? TongTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public string MaPhuongThuc { get; set; } = null!;

    public virtual ICollection<ChiTietDh> ChiTietDhs { get; set; } = new List<ChiTietDh>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual KhachHang MaKhNavigation { get; set; } = null!;

    public virtual PhuongThucThanhToan MaPhuongThucNavigation { get; set; } = null!;

    public virtual ICollection<QuanLyDh> QuanLyDhs { get; set; } = new List<QuanLyDh>();
}
