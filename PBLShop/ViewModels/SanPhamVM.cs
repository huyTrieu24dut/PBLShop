namespace PBLShop.ViewModels
{
    public class SanPhamVM
    {
        public string MaSp { get; set; } = null!;
        public string TenSp { get; set; } = null!;
        public int DonGia { get; set; }
        public int SoLuong { get; set; }
        public string? HinhAnh { get; set; }
        public string? DanhMuc { get; set; }
    }

    public class ChiTietSanPhamVM
    {
        public string MaSp { get; set; } = null!;
        public string TenSp { get; set; } = null!;
        public int DonGia { get; set; }
        public int SoLuong { get; set; }
        public string? HinhAnh { get; set; }
        public string? MoTa {  get; set; }
        public string DanhMuc { get; set; } = null!;
        public decimal? SoSao { get; set; }
    }
}
