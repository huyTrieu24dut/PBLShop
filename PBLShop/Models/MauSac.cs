using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class MauSac
{
    public string MaMau { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public string TenMau { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
