using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class DonHang
{
    public int MaDh { get; set; }

    public int MaNguoiDung { get; set; }

    public string? TenNguoiNhan { get; set; }

    public string? DiaChi { get; set; }

    public string? SdtnguoiNhan { get; set; }

    public int TongTien { get; set; }

    public int MaPttt { get; set; }

    public DateTime? NgayDatHang { get; set; }

    public int? MaTrangThai { get; set; }

    public virtual ICollection<ChiTietDh> ChiTietDhs { get; set; } = new List<ChiTietDh>();

    public virtual HoaDon? HoaDon { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual PhuongThucThanhToan MaPtttNavigation { get; set; } = null!;

    public virtual TrangThai? MaTrangThaiNavigation { get; set; }

    public virtual ICollection<QuanLyDh> QuanLyDhs { get; set; } = new List<QuanLyDh>();
}
