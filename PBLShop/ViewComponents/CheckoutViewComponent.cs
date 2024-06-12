using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Security.Claims;

namespace PBLShop.ViewComponents
{
    public class CheckoutViewComponent : ViewComponent
    {
        private readonly WebShopContext _context;

        public CheckoutViewComponent(WebShopContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var cartItems = _context.ChiTietGhs
                .Include(p => p.MaMauNavigation)
                .Include(p => p.MaKtNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation)
                .AsQueryable();

            var cart = cartItems
            .Where(p => p.MaKh.ToString() == HttpContext.User.FindFirstValue("MaNguoiDung"))
            .Select(p => new ChiTietGhVM
            {
                MaKh = p.MaKh,
                TenSp = p.MaMauNavigation.MaSpNavigation.TenSp,
                MauSp = p.MaMauNavigation.TenMau,
                size = p.MaKtNavigation.Size,
                DonGia = p.MaMauNavigation.MaSpNavigation.DonGia,
                SoLuong = p.SoLuong,
                HinhAnh = p.MaMauNavigation.AnhSp,
            });
            return View(cart);
        }
    }
}
