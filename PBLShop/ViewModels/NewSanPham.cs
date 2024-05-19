namespace PBLShop.ViewModels
{
    public class NewSanPham
    {
        public NewSanPham()
        {
            danhMucs = new List<DanhMucList>();
        }
        public string? TenSp { get; set; }
        public int DonGia { get; set; }
        public string? MoTa { get; set; }
        public int MaDm { get; set; }
        public string? AnhSp { get; set; }
        public string? MauSac { get; set; }
        public string? Size { get; set; }
        public List<DanhMucList> danhMucs { get; set; }
    }
}
