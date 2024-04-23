using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class QuanLyDh
{
    public string MaNv { get; set; } = null!;

    public string MaDh { get; set; } = null!;

    public DateTime ThoiGian { get; set; }

    public string TrangThaiCapNhat { get; set; } = null!;

    public virtual DonHang MaDhNavigation { get; set; } = null!;

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
