namespace PBLShop.ViewModels
{
    public class QuanLySoLuongVM
    {
        public QuanLySoLuongVM() 
        {
            SoLuong = new List<List<int>>();
            for (int i = 0; i < 10; i++)
            {
                SoLuong.Add(new List<int>(new int[10]));
            }
            MauSacs = new List<string>();
            Sizes = new List<string>();
        }
        public int MaSp { get; set; }
        public List<List<int>> SoLuong { get; set; }
        public List<string> MauSacs { get; set; }
        public List<string> Sizes { get; set; }
    }
}
