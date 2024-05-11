using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class ChiTietDh
{
    public int MaDh { get; set; }

    public int MaMau { get; set; }

    public int MaKt { get; set; }

    public int? SoLuong { get; set; }

    public virtual DonHang MaDhNavigation { get; set; } = null!;

    public virtual KichThuoc MaKtNavigation { get; set; } = null!;

    public virtual MauSac MaMauNavigation { get; set; } = null!;
}
