using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class QuanLyDh
{
    public int MaQldh { get; set; }

    public int MaNv { get; set; }

    public int MaDh { get; set; }

    public DateTime ThoiGian { get; set; }

    public int MaTrangThai { get; set; }

    public virtual DonHang MaDhNavigation { get; set; } = null!;

    public virtual NguoiDung MaNvNavigation { get; set; } = null!;
}
