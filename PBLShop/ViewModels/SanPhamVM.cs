namespace PBLShop.ViewModels
{
    public class SanPhamVM
    {
        public int MaSp { get; set; }
        public string TenSp { get; set; } = null!;
        public int DonGia { get; set; }
        public int SoLuong { get; set; }
        public string? HinhAnh { get; set; }
        public string? DanhMuc { get; set; }
    }

    public class SanPhamUpdateVM : SanPhamVM
    {
        public SanPhamUpdateVM()
        {
            DanhMucs = new List<DanhMucList>();
        }
        public int MaDanhMuc { get; set; }
        public string? MoTa { get; set; }
        public List<DanhMucList>? DanhMucs { get; set; }
    }

    public class ChiTietSanPhamVM
    {
        public int MaSp { get; set; }
        public string TenSp { get; set; } = null!;
        public int DonGia { get; set; }
        public int? SoLuong { get; set; }
        public string? HinhAnh { get; set; }
        public string? MoTa { get; set; }
        public decimal? SoSao { get; set; }
        public List<string>? MauSac { get; set; }
        public List<string>? KichThuoc { get; set; }
        public List<List<int>>? SoLuongs { get; set; }
        public List<DanhGiaVM>? DanhGia { get; set; }
        public string DanhMuc { get; set; } = null!;
    }

    public class SanPhamMoi
    {
        public string TenSp { get; set; } = null!;
        public int DonGia { get; set; }
        public string? HinhAnh { get; set; }
        public string? MoTa { get; set; }
        public List<string>? MauSac { get; set; }
        public List<string>? KichThuoc { get; set; }
        public int MaDm { get; set; }
    }
}
