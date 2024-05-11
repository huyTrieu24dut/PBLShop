using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class TrangThai
{
    public int MaTrangThai { get; set; }

    public string TenTrangThai { get; set; } = null!;

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
