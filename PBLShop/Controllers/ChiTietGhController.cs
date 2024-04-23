using Microsoft.AspNetCore.Mvc;
using PBLShop.Models;
using PBLShop.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PBLShop.Controllers
{
    public class ChiTietGhController : Controller
    {
        private readonly PblshopContext _context;

        public ChiTietGhController(PblshopContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            var cartItems = _context.ChiTietGhs.AsQueryable();

            var result = cartItems.Select(p => new ChiTietGhVM
            {
                MaSp = p.MaSp,
                MaKh = p.MaKh,
                TenSp = p.MaSpNavigation.TenSp,
                DonGia = p.MaSpNavigation.DonGia,
                SoLuong = p.SoLuong,
                HinhAnh = p.MaSpNavigation.HinhAnh,
                DanhMuc = p.MaSpNavigation.MaDmNavigation.TenDm
            });
            return View(result);
        }
        public IActionResult Add(string masp)
        {
            var existingCartItem = _context.ChiTietGhs.FirstOrDefault(p => p.MaSp == masp);
            if (masp != null)
            {
                if (existingCartItem != null)
                {
                    existingCartItem.SoLuong += 1;
                }
                else
                {
                    var newCartItem = new ChiTietGh
                    {
                        MaKh = "KH1",
                        MaSp = masp,
                        SoLuong = 1
                    };
                    _context.ChiTietGhs.Add(newCartItem);
                }
                _context.SaveChanges();
            }

            var cartItems = _context.ChiTietGhs
                .Select(p => new ChiTietGhVM
                {
                    MaSp = p.MaSp,
                    MaKh = p.MaKh,
                    TenSp = p.MaSpNavigation.TenSp,
                    DonGia = p.MaSpNavigation.DonGia,
                    SoLuong = p.SoLuong,
                    HinhAnh = p.MaSpNavigation.HinhAnh,
                    DanhMuc = p.MaSpNavigation.MaDmNavigation.TenDm
                })
                .ToList();

            return View(cartItems);
        }
        public IActionResult Remove(string masp)
        {
            var cartItem = _context.ChiTietGhs.FirstOrDefault(item => item.MaSp == masp);

            if (cartItem != null)
            {
                _context.ChiTietGhs.Remove(cartItem);
                _context.SaveChanges();
            }
            var cartItems = _context.ChiTietGhs
                .Select(p => new ChiTietGhVM
                {
                    MaSp = p.MaSp,
                    MaKh = p.MaKh,
                    TenSp = p.MaSpNavigation.TenSp,
                    DonGia = p.MaSpNavigation.DonGia,
                    SoLuong = p.SoLuong,
                    HinhAnh = p.MaSpNavigation.HinhAnh,
                    DanhMuc = p.MaSpNavigation.MaDmNavigation.TenDm
                })
                .ToList();
            return View(cartItems);
        }
    }
}
