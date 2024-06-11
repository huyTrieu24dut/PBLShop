using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Security.Claims;

namespace PBLShop.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly WebShopContext _context;

        public SanPhamController(WebShopContext context)
        {
            _context = context;
        }

        //Hien thi theo danh muc
        public IActionResult Index(int? DM)
        {
            var chitietsp = _context.QuanLySanPhams
                .Include(p => p.MaMauNavigation)
                .Select(p => p.MaMauNavigation.MaSp).Distinct().ToList();
            var sanphams = _context.SanPhams.AsQueryable();
            sanphams = sanphams.Where(p => chitietsp.Contains(p.MaSp) && p.TrangThai);

            if (DM != null)
            {
                var danhMucCon = _context.DanhMucs.Where(dm => dm.MaDmcha == DM).Select(dm => dm.MaDm).ToList();
                sanphams = sanphams.Where(p => p.MaDm == DM || danhMucCon.Contains(p.MaDm));
            }

            var result = sanphams.Select(p => new SanPhamVM
            {
                MaSp = p.MaSp,
                TenSp = p.TenSp,
                DonGia = p.DonGia,
                HinhAnh = p.AnhSp,
                DanhMuc = p.MaDmNavigation.TenDanhMuc
            });
            return View(result);
        }

        //Tim san pham
        public IActionResult Search(string SpTofind)
        {
            var chitietsp = _context.QuanLySanPhams
                .Include(p => p.MaMauNavigation)
                .Select(p => p.MaMauNavigation.MaSp).Distinct().ToList();
            var sanphams = _context.SanPhams.AsQueryable();
            sanphams = sanphams.Where(p => chitietsp.Contains(p.MaSp) && p.TrangThai);

            if (SpTofind != null)
            {
                sanphams = sanphams.Where(p => p.TenSp.Contains(SpTofind));
            }

            var result = sanphams.Select(p => new SanPhamVM
            {
                MaSp = p.MaSp,
                TenSp = p.TenSp,
                DonGia = p.DonGia,
                HinhAnh = p.AnhSp,
                DanhMuc = p.MaDmNavigation.TenDanhMuc
            });
            return View(result);
        }
        //chi tiet san pham
        public IActionResult Detail(int id)
        {
            var data = _context.QuanLySanPhams
                .Include(p => p.MaMauNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation.MaDmNavigation)
                .Include(p => p.MaKichThuocNavigation)
                .Where(p => p.MaMauNavigation.MaSp == id).ToList();
            if(data.Count == 0)
            {
                TempData["Message"] = "Không tìm thấy sản phẩm";
                return Redirect("/404");
            }
            var result = new ChiTietSanPhamVM();
            result.MaSp = 0;
            result.MauSac = new List<string>();
            result.KichThuoc = new List<string>();
            result.SoLuong = 0;
            foreach (var detail in data)
            {
                if (result.MaSp == 0)
                {
                    result.MaSp = detail.MaMauNavigation.MaSp;
                    result.TenSp = detail.MaMauNavigation.MaSpNavigation.TenSp;
                    result.DonGia = detail.MaMauNavigation.MaSpNavigation.DonGia;
                    result.HinhAnh = detail.MaMauNavigation.MaSpNavigation.AnhSp;
                    result.MoTa = detail.MaMauNavigation.MaSpNavigation.MoTa ?? string.Empty;
                    result.DanhMuc = detail.MaMauNavigation.MaSpNavigation.MaDmNavigation.TenDanhMuc;
                }

                if (detail.MaMauNavigation != null && detail.MaMauNavigation.TenMau != null)
                {
                    if (!result.MauSac.Contains(detail.MaMauNavigation.TenMau))
                    {
                        result.MauSac.Add(detail.MaMauNavigation.TenMau);
                    }
                }

                if (detail.MaKichThuocNavigation != null)
                {
                    if (!result.KichThuoc.Contains(detail.MaKichThuocNavigation.Size))
                    {
                        result.KichThuoc.Add(detail.MaKichThuocNavigation.Size);
                    }
                }
                result.SoLuong += detail.SoLuong;
            }
            var danhGias = _context.DanhGia
                .Include(p => p.MaNguoiDungNavigation)
                .Include(p => p.MaSpNavigation)
                .Where(p => p.MaSp == id)
                .ToList();
            if (danhGias != null)
            {
                var user = danhGias.Where(p => p.MaNguoiDung == Convert.ToInt32(HttpContext.User.FindFirstValue("MaNguoiDung"))).FirstOrDefault();
                if (user != null)
                {
                    result.DaDanhGia = true;
                }
                result.DanhGia = new List<DanhGiaVM>();
                foreach (var danhGia in danhGias)
                {
                    result.DanhGia.Add(new DanhGiaVM
                    {
                        MaDanhGia = danhGia.MaDg,
                        TenKh = danhGia.MaNguoiDungNavigation.HoTen,
                        SoSao = danhGia.SoSao ?? 0,
                        NoiDung = danhGia.NoiDung,
                    });
                }
            }
            return View(result);
        }

    }
}
