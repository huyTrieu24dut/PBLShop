using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class KichThuoc
{
    public string MaSize { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public string Size { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
