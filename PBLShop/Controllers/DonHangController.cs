using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Security.Claims;

namespace PBLShop.Controllers
{
    public class DonHangController : Controller
    {
        private readonly PblshopContext _context;

        public DonHangController(PblshopContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var cartItems = _context.ChiTietGhs.AsQueryable();

            var cart = cartItems
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
            if (cart.Count() == 0)
            {
                RedirectToAction("Index", "Home");
            }
            return View(cart);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create(string? DiaChi)
        {
            var cartItems = _context.ChiTietGhs.AsQueryable();

            var cart = cartItems
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
            if (ModelState.IsValid)
            {
                string customerId = HttpContext.User.FindFirstValue("MaKhachHang");
                var KhachHang = _context.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
                var ctdh = new List<ChiTietDh>();
                int Total = 0;
                foreach (var item in cart)
                {
                    ctdh.Add(new ChiTietDh
                    {
                        MaDh = "DH1",
                        SoLuong = (int)item.SoLuong,
                        SoTien = (int)(item.SoLuong * item.DonGia),
                        MaSp = item.MaSp,
                    });
                    Total += (int)(item.SoLuong * item.DonGia);
                    var cartItem = _context.ChiTietGhs.FirstOrDefault(p => p.MaSp == item.MaSp);

                    if (cartItem != null)
                    {
                        _context.ChiTietGhs.Remove(cartItem);
                    }
                }
                _context.AddRange(ctdh);
                _context.SaveChanges();
                var donhang = new DonHang
                {
                    MaDh = "DH1",
                    MaKh = customerId,
                    NgayDat = DateTime.Now,
                    DiaChi = DiaChi ?? KhachHang.DiaChi,
                    TongTien = Total,
                    MaPhuongThuc = "PT1",
                    TrangThai = "Đang chờ xác nhận"
                };
                _context.Add(donhang);
                _context.SaveChanges();

                return RedirectToAction("Success");
            }

            return View(cart);
        }
    }
}
