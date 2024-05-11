namespace PBLShop.ViewModels
{
    public class DonHangVM
    {
        public DonHangVM()
        {
            chiTietDhVMs = new List<ChiTietDhVM>(); // Initialize chiTietDhVMs in the constructor
        }
        public int MaDh { get; set; }
        public string TenKh { get; set; }
        public string TrangThai { get; set; }
        public int TongTien { get; set; }
        public List<ChiTietDhVM> chiTietDhVMs { get; set; }
    }
}
