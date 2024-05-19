namespace PBLShop.ViewModels
{
    public class QuanLySoLuongVM
    {
        public QuanLySoLuongVM() 
        {
            MauSacs = new List<string>();
            Sizes = new List<string>();
        }
        public int MaSp { get; set; }
        public int SoLuong { get; set; }
        public List<string> MauSacs { get; set; }
        public List<string> Sizes { get; set; }
    }
}
