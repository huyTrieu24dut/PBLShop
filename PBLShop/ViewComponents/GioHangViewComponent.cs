using Microsoft.AspNetCore.Mvc;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Security.Claims;

namespace PBLShop.ViewComponents
{
    public class GioHangViewComponent : ViewComponent
    {
        private readonly PblshopContext _context;

        public GioHangViewComponent(PblshopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var data = _context.ChiTietGhs.Where(p => p.MaKh == HttpContext.User.FindFirstValue("MaKhachHang")).ToList();
            GioHangVM vm = new GioHangVM
            {
                SoLuong = 0,
                TongGia = 0
            };
            foreach(var item in data)
            {
                vm.SoLuong += item.SoLuong;
                var product = _context.SanPhams.FirstOrDefault(sp => sp.MaSp == item.MaSp);
                if (product != null)
                {
                    // Tính tổng giá bằng cách nhân đơn giá của sản phẩm với số lượng
                    vm.TongGia += product.DonGia * item.SoLuong;
                }
            }
            return View(vm);
        }
    }
}
