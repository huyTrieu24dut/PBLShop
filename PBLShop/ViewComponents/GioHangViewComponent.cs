using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Security.Claims;

namespace PBLShop.ViewComponents
{
    public class GioHangViewComponent : ViewComponent
    {
        private readonly WebShopContext _context;

        public GioHangViewComponent(WebShopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var data = _context.ChiTietGhs.Where(p => p.MaKh == Convert.ToInt32(HttpContext.User.FindFirstValue("MaNguoiDung"))).ToList();
            GioHangVM vm = new GioHangVM
            {
                SoLuong = 0,
                TongGia = 0
            };
            foreach (var item in data)
            {
                vm.SoLuong += 1;
                var product = _context.MauSacs
                    .Include(p => p.MaSpNavigation)
                    .FirstOrDefault(sp => sp.MaSp == item.MaMau);
                if (product != null)
                {
                    vm.TongGia += product.MaSpNavigation.DonGia * item.SoLuong;
                }
            }
            return View(vm);
        }
    }
}
