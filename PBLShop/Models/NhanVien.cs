using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class NhanVien
{
    public string MaNv { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public bool GioiTinh { get; set; }

    public DateTime NgaySinh { get; set; }

    public int MaChucVu { get; set; }

    public string DienThoai { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ChucVu MaChucVuNavigation { get; set; } = null!;

    public virtual ICollection<QuanLyDh> QuanLyDhs { get; set; } = new List<QuanLyDh>();
}
