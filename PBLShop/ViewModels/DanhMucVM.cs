namespace PBLShop.ViewModels
{
    public class DanhMucVM
    {
        public int? MaDM {  get; set; }
        public string? TenDM { get; set; }
        public int SoLuong { get; set; }
        public int? MaDMCha { get; set; }
        public List<DanhMucVM> DanhMucCon {  get; set; }
    }
    public class DanhMucList
    {
        public int MaDm { get; set; }
        public string? TenDm { get; set; }
    }
}
