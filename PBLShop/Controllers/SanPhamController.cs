using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;

namespace PBLShop.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly PblshopContext _context;

        public SanPhamController(PblshopContext context)
        {
            _context = context;
        }

        //Hien thi theo danh muc
        public IActionResult Index(string? DM)
        {
            var sanphams = _context.SanPhams.AsQueryable();

            if(DM != null)
            {
                sanphams = sanphams.Where(p => p.MaDm == DM);
            }

            var result = sanphams.Select(p => new SanPhamVM
            {
                MaSp = p.MaSp,
                TenSp = p.TenSp,
                DonGia = p.DonGia,
                SoLuong = p.SoLuong,
                HinhAnh = p.HinhAnh,
                DanhMuc = p.MaDmNavigation.TenDm
            });
            return View(result);
        }

        //Tim san pham
        public IActionResult Search(string SpTofind)
        {
            var sanphams = _context.SanPhams.AsQueryable();

            if (SpTofind != null)
            {
                sanphams = sanphams.Where(p => p.TenSp.Contains(SpTofind));
            }

            var result = sanphams.Select(p => new SanPhamVM
            {
                MaSp = p.MaSp,
                TenSp = p.TenSp,
                DonGia = p.DonGia,
                SoLuong = p.SoLuong,
                HinhAnh = p.HinhAnh,
                DanhMuc = p.MaDmNavigation.TenDm
            });
            return View(result);
        }
        //chi tiet san pham
        public IActionResult Detail(string id)
        {
            var data = _context.SanPhams
                .Include(p => p.MaDmNavigation)
                .SingleOrDefault(p => p.MaSp == id);
            if(data == null)
            {
                TempData["Message"] = "Không tìm thấy sản phẩm";
                return Redirect("/404");
            }
            var result = new ChiTietSanPhamVM
            {
                MaSp = data.MaSp,
                TenSp = data.TenSp,
                DonGia = data.DonGia,
                SoLuong = data.SoLuong,
                MoTa = data.MoTa ?? string.Empty,
                HinhAnh = data.HinhAnh ?? string.Empty,
                DanhMuc = data.MaDmNavigation.TenDm,
                SoSao = 5
            };
            return View(result);
        }
    }
}
