using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class QuanLySanPham
{
    public int MaMau { get; set; }

    public int MaKichThuoc { get; set; }

    public int SoLuong { get; set; }

    public virtual KichThuoc MaKichThuocNavigation { get; set; } = null!;

    public virtual MauSac MaMauNavigation { get; set; } = null!;
}
