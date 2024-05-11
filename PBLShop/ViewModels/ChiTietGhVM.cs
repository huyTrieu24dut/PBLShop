namespace PBLShop.ViewModels
{
    public class ChiTietGhVM
    {
        public int MaKh { get; set; }
        public int MaSp { get; set; }
        public string TenSp { get; set; } = null!;
        public string MauSp { get; set; } = null!;
        public string size {  get; set; } = null!;
        public int DonGia { get; set; }
        public int? SoLuong { get; set; }
        public string? HinhAnh { get; set; }
        public string DanhMuc { get; set; } = null!;
    }
}
