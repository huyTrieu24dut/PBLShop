using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class ChiTietGh
{
    public int MaKh { get; set; }

    public int MaMau { get; set; }

    public int MaKt { get; set; }

    public int? SoLuong { get; set; }

    public virtual NguoiDung MaKhNavigation { get; set; } = null!;

    public virtual KichThuoc MaKtNavigation { get; set; } = null!;

    public virtual MauSac MaMauNavigation { get; set; } = null!;
}
