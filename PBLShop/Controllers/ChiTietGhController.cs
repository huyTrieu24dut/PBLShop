using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Security.Claims;
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
        [Authorize]
        public IActionResult Index()
        {
            var cartItems = _context.ChiTietGhs.AsQueryable();

            var result = cartItems
            .Where(p => p.MaKh == HttpContext.User.FindFirstValue("MaKhachHang"))
            .Select(p => new ChiTietGhVM
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
        [Authorize]
        public IActionResult Add(string masp, int? soluong)
        {
            if (masp != null)
            {
                var existingCartItem = _context.ChiTietGhs.FirstOrDefault(p => p.MaSp == masp && p.MaKh == HttpContext.User.FindFirstValue("MaKhachHang"));
                if (existingCartItem != null)
                {
                    existingCartItem.SoLuong += soluong;
                }
                else 
                {
                    var sampham = _context.SanPhams.FirstOrDefault(sp =>  sp.MaSp == masp);
                    if(sampham != null)
                    {
                        var newCartItem = new ChiTietGh
                        {
                            MaKh = HttpContext.User.FindFirstValue("MaKhachHang"),
                            MaSp = masp,
                            SoLuong = soluong
                        };
                        _context.ChiTietGhs.Add(newCartItem);
                    }
                    else
                    {
                        TempData["Message"] = "Không tìm thấy sản phẩm";
                        return Redirect("/404");
                    }
                }
                _context.SaveChanges();
            }

            var cartItems = _context.ChiTietGhs
                .Where(p => p.MaKh == HttpContext.User.FindFirstValue("MaKhachHang"))
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
        [Authorize]
        public IActionResult Remove(string masp)
        {
            var cartItem = _context.ChiTietGhs.FirstOrDefault(item => item.MaSp == masp && item.MaKh == HttpContext.User.FindFirstValue("MaKhachHang"));

            if (cartItem != null)
            {
                _context.ChiTietGhs.Remove(cartItem);
                _context.SaveChanges();
            }
            else
            {
                TempData["Message"] = "Sản phẩm không có trong giỏ hàng";
                return Redirect("/404");
            }
            var cartItems = _context.ChiTietGhs
                .Where(p => p.MaKh == HttpContext.User.FindFirstValue("MaKhachHang"))
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
