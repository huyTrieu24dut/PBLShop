using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public string Email { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? HoTen { get; set; }

    public int? MaGioiTinh { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public int MaVaiTro { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<ChiTietGh> ChiTietGhs { get; set; } = new List<ChiTietGh>();

    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual GioiTinh? MaGioiTinhNavigation { get; set; }

    public virtual VaiTro MaVaiTroNavigation { get; set; } = null!;

    public virtual ICollection<QuanLyDh> QuanLyDhs { get; set; } = new List<QuanLyDh>();
}
