using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PBLShop.Controllers
{
    public class ChiTietGhController : Controller
    {
        private readonly WebShopContext _context;

        public ChiTietGhController(WebShopContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "KhachHang")]
        public IActionResult Index()
        {
            var cartItems = _context.ChiTietGhs
                .Include(p => p.MaMauNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation)
                .Include(p => p.MaKtNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation.MaDmNavigation)
                .AsQueryable();

            var result = cartItems
            .Where(p => p.MaKh.ToString() == HttpContext.User.FindFirstValue("MaNguoiDung"))
            .Select(p => new ChiTietGhVM
            {
                MaKh = p.MaKh,
                MaSp = p.MaMauNavigation.MaSp,
                TenSp = p.MaMauNavigation.MaSpNavigation.TenSp,
                MauSp = p.MaMauNavigation.TenMau,
                size = p.MaKtNavigation.Size,
                DonGia = p.MaMauNavigation.MaSpNavigation.DonGia,
                SoLuong = p.SoLuong,
                HinhAnh = p.MaMauNavigation.MaSpNavigation.AnhSp,
                DanhMuc = p.MaMauNavigation.MaSpNavigation.MaDmNavigation.TenDanhMuc,
            });
            return View(result);
        }

        [Authorize(Roles = "KhachHang")]
        public IActionResult Add(int id, string? mausac, string? size, int? soluong)
        {
            var chitietsp = _context.QuanLySanPhams
                .Include(p => p.MaMauNavigation)
                .Include(p => p.MaKichThuocNavigation)
                .FirstOrDefault(p => p.MaMauNavigation.TenMau == mausac && p.MaKichThuocNavigation.Size == size && p.MaMauNavigation.MaSp == id);
            if (chitietsp != null)
            {
                if (chitietsp.SoLuong < soluong || chitietsp.SoLuong == 0)
                {
                    TempData["ErrorMessage"] = "Không đủ số lượng";
                    return RedirectToAction("Detail", "SanPham", new { id = chitietsp.MaMauNavigation.MaSp });
                }
                var existingCartItem = _context.ChiTietGhs.FirstOrDefault(p => p.MaMau == chitietsp.MaMau && p.MaKt == chitietsp.MaKichThuoc && p.MaKh.ToString() == HttpContext.User.FindFirstValue("MaNguoiDung"));
                if (existingCartItem != null)
                {
                    existingCartItem.SoLuong += soluong;
                }
                else
                {
                    var newCartItem = new ChiTietGh
                    {
                        MaKh = Convert.ToInt32(HttpContext.User.FindFirstValue("MaNguoiDung")),
                        MaMau = chitietsp.MaMau,
                        MaKt = chitietsp.MaKichThuoc,
                        SoLuong = soluong
                    };
                    _context.ChiTietGhs.Add(newCartItem);

                }
            }
            else
            {
                TempData["Message"] = "Không tìm thấy sản phẩm";
                return Redirect("/404");
            }
            _context.SaveChanges();

            var cartItems = _context.ChiTietGhs
                .Include(p => p.MaMauNavigation)
                .Include(p => p.MaKhNavigation)
                .Include(p => p.MaKtNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation.MaDmNavigation)
                .Where(p => p.MaKh.ToString() == HttpContext.User.FindFirstValue("MaNguoiDung"))
                .Select(p => new ChiTietGhVM
                {
                    MaKh = p.MaKh,
                    MaSp = p.MaMauNavigation.MaSp,
                    TenSp = p.MaMauNavigation.MaSpNavigation.TenSp,
                    DonGia = p.MaMauNavigation.MaSpNavigation.DonGia,
                    MauSp = p.MaMauNavigation.TenMau,
                    size = p.MaKtNavigation.Size,
                    SoLuong = p.SoLuong,
                    HinhAnh = p.MaMauNavigation.MaSpNavigation.AnhSp,
                    DanhMuc = p.MaMauNavigation.MaSpNavigation.MaDmNavigation.TenDanhMuc
                })
                .ToList();

            return View(cartItems);
        }

        [Authorize(Roles = "KhachHang")]
        public IActionResult Remove(int id, string mausac, string size)
        {
            var cartItem = _context.ChiTietGhs
                .FirstOrDefault(item => item.MaMauNavigation.MaSp == id && item.MaMauNavigation.TenMau == mausac && item.MaKtNavigation.Size == size && item.MaKh.ToString() == HttpContext.User.FindFirstValue("MaNguoiDung"));

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
                .Include(p => p.MaMauNavigation)
                .Include(p => p.MaKhNavigation)
                .Include(p => p.MaKtNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation.MaDmNavigation)
                .Where(p => p.MaKh.ToString() == HttpContext.User.FindFirstValue("MaNguoiDung"))
                .Select(p => new ChiTietGhVM
                {
                    MaKh = p.MaKh,
                    MaSp = p.MaMauNavigation.MaSp,
                    TenSp = p.MaMauNavigation.MaSpNavigation.TenSp,
                    DonGia = p.MaMauNavigation.MaSpNavigation.DonGia,
                    MauSp = p.MaMauNavigation.TenMau,
                    size = p.MaKtNavigation.Size,
                    SoLuong = p.SoLuong,
                    HinhAnh = p.MaMauNavigation.MaSpNavigation.AnhSp,
                    DanhMuc = p.MaMauNavigation.MaSpNavigation.MaDmNavigation.TenDanhMuc
                })
                .ToList();
            return View(cartItems);
        }
    }
    }
