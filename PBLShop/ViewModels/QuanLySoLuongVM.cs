namespace PBLShop.ViewModels
{
    public class QuanLySoLuongVM
    {
        public QuanLySoLuongVM() 
        {
            SoLuong = new List<List<int>>();
            MauSacs = new List<string>();
            Sizes = new List<string>();
            HinhAnhs = new List<string>();
        }
        public int MaSp { get; set; }
        public List<List<int>> SoLuong { get; set; }
        public List<string> MauSacs { get; set; }
        public List<string> Sizes { get; set; }
        public List<string> HinhAnhs { get; set; }
        public List<IFormFile> NewHinhAnhs { get; set; }
    }
}
