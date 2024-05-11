using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class KhachHang
{
    public string MaKh { get; set; } = null!;

    public string MatKhau { get; set; } = null!;
        public string HoTen { get; set; } = null!;

    public bool GioiTinh { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string DiaChi { get; set; } = null!;

    public string DienThoai { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<ChiTietGh> ChiTietGhs { get; set; } = new List<ChiTietGh>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
