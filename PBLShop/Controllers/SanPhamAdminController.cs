﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Policy;
using Microsoft.IdentityModel.Tokens;

namespace PBLShop.Controllers
{
    public class SanPhamAdminController : Controller
    {
        private readonly WebShopContext _context;

        public SanPhamAdminController(WebShopContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Index()
        {
            var chitietsp = _context.QuanLySanPhams
                .Include(p => p.MaMauNavigation)
                .Select(p => p.MaMauNavigation.MaSp).Distinct().ToList();
            var sanphams = _context.SanPhams.AsQueryable();
            sanphams = sanphams.Where(p => chitietsp.Contains(p.MaSp) && p.TrangThai);

            //if (DM != null)
            //{
            //    var danhMucCon = _context.DanhMucs.Where(dm => dm.MaDmcha == DM).Select(dm => dm.MaDm).ToList();
            //    sanphams = sanphams.Where(p => p.MaDm == DM || danhMucCon.Contains(p.MaDm));
            //}

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
        [HttpPost]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                RedirectToAction("/404");
            }
            var sanpham = _context.SanPhams
                .FirstOrDefault(p => p.MaSp == id && p.TrangThai == true);
            if (sanpham == null)
            {
                TempData["Message"] = "Không tìm thấy sản phẩm";
                RedirectToAction("/404");
            }
            else
            {
                sanpham.TrangThai = false;
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "SanPhamAdmin");
        }
        [HttpGet]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Update(int id)
        {
            var sp = _context.SanPhams.FirstOrDefault(p => p.MaSp == id);
            if (sp != null)
            {
                var danhmucs = _context.DanhMucs.AsQueryable();
                var sanpham = new SanPhamUpdateVM
                {
                    MaSp = id,
                    TenSp = sp.TenSp,
                    DonGia = sp.DonGia,
                    MoTa = sp.MoTa,
                    MaDanhMuc = sp.MaDm,
                    HinhAnh = sp.AnhSp,
                    DanhMucs = danhmucs
                        .Where(p => p.MaDmcha != null && p.TrangThai == true)
                        .Select(p => new DanhMucList
                        {
                            MaDm = p.MaDm,
                            TenDm = p.TenDanhMuc
                        }).ToList()
                };
                
                return View(sanpham);
            }
            else
            {
                TempData["Message"] = "Không tìm thấy nhân viên";
                RedirectToAction("/404");
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Update(int id, SanPhamUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var sanpham = _context.SanPhams.FirstOrDefault(p => p.MaSp == id);
                if (sanpham == null)
                {
                    TempData["Message"] = "Không tìm thấy nhân viên";
                    RedirectToAction("/404");
                }
                else
                {
                    sanpham.TenSp = model.TenSp;
                    sanpham.DonGia = model.DonGia;
                    sanpham.MoTa = model.MoTa;
                    sanpham.MaDm = model.MaDanhMuc;
                    if (!model.HinhAnh.IsNullOrEmpty())
                        sanpham.AnhSp = model.HinhAnh;
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Create(string error = null)
        {
            var danhmucs = _context.DanhMucs.AsQueryable();
            var sanpham = new NewSanPham
            {
                danhMucs = danhmucs
                    .Where(p => p.MaDmcha != null && p.TrangThai == true)
                    .Select(p => new DanhMucList
                    {
                        MaDm = p.MaDm,
                        TenDm = p.TenDanhMuc
                    }).ToList()
            };
            if (!string.IsNullOrEmpty(error))
            {
                ModelState.AddModelError("loi", error);
            }

            return View(sanpham);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Create(NewSanPham model)
        {
            if (ModelState.IsValid)
            {
                List<string> MauSacs = new List<string>();
                MauSacs.AddRange(model.MauSac.Split(",").Select(s => s.Trim()).Distinct());
                List<string> Sizes = new List<string>();
                Sizes.AddRange(model.Size.Split(",").Select(s => s.Trim()).Distinct());
                bool check = true;
                foreach(var kt in Sizes)
                {
                    if (!_context.KichThuocs.Select(p => p.Size).ToList().Contains(kt))
                    {
                        check = false; 
                        break;
                    }
                }
                if (check)
                {
                    var sanpham = new SanPham
                    {
                        TenSp = model.TenSp,
                        DonGia = model.DonGia,
                        MoTa = model.MoTa,
                        AnhSp = model.AnhSp,
                        MaDm = model.MaDm,
                        TrangThai = true
                    };
                    _context.Add(sanpham);
                    _context.SaveChanges();
                    foreach (var mau in MauSacs)
                    {
                        var Mau = new MauSac
                        {
                            MaSp = sanpham.MaSp,
                            TenMau = mau,
                        };
                        _context.Add(Mau);
                    }
                    _context.SaveChanges();
                    string mauSac = string.Join(",", MauSacs);
                    string size = string.Join(",", Sizes);
                    return RedirectToAction("Create2", new { id = sanpham.MaSp, mauSac = mauSac, size = size });
                }
                else
                {
                    string errorMessage = "Kích thước không hợp lệ";
                    return RedirectToAction("Create", new { error = errorMessage });
                }
            }
            return RedirectToAction("Index", "SanPhamAdmin");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Create2(int id, string mauSac, string size)
        {
            QuanLySoLuongVM ql = new QuanLySoLuongVM();
            ql.MaSp = id;
            ql.MauSacs.AddRange(mauSac.Split(","));
            ql.Sizes.AddRange(size.Split(","));
            for (int m = 0; m < ql.MauSacs.Count; m++)
            {
                ql.SoLuong.Add(new List<int>(new int[ql.Sizes.Count]));
            }
            ql.NewHinhAnhs = new List<IFormFile>(new IFormFile[ql.MauSacs.Count]);
            return View(ql);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Create2(int MaSp, List<string> MauSacs, List<string> Sizes, List<List<int>> SoLuong, List<IFormFile> HinhAnhs)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < MauSacs.Count; i++)
                {
                    bool first = true;
                    for (int j = 0; j < Sizes.Count; j++)
                    {
                        var qlsp = new QuanLySanPham
                        {
                            SoLuong = SoLuong[i][j],
                            MaMau = _context.MauSacs.Where(p => p.MaSp == MaSp && p.TenMau == MauSacs[i]).Select(p => p.MaMau).FirstOrDefault(),
                            MaKichThuoc = _context.KichThuocs.Where(p => p.Size == Sizes[j]).Select(p => p.MaKt).FirstOrDefault(),
                        };
                        _context.Add(qlsp);
                        if (first)
                        {
                            var mau = _context.MauSacs.Where(p => p.MaMau == qlsp.MaMau).FirstOrDefault();
                            if (mau != null && i < HinhAnhs.Count)
                            {
                                if (HinhAnhs != null && HinhAnhs[i] != null && HinhAnhs[i].Length > 0)
                                {
                                    mau.AnhSp = HinhAnhs[i].FileName;
                                }
                            }
                            first = false;
                        }
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Index", "SanPhamAdmin");
            }

            return RedirectToAction("Index", "SanPhamAdmin");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult UpdateQuantity(int id)
        {
            QuanLySoLuongVM ql = new QuanLySoLuongVM();
            ql.MaSp = id;
            var data = _context.QuanLySanPhams
                .Include(p => p.MaMauNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation.MaDmNavigation)
                .Include(p => p.MaKichThuocNavigation)
                .Where(p => p.MaMauNavigation.MaSp == id).ToList();
            if (data != null)
            {
                foreach (var detail in data)
                {
                    if (detail.MaMauNavigation != null && detail.MaMauNavigation.TenMau != null)
                    {
                        if (!ql.MauSacs.Contains(detail.MaMauNavigation.TenMau))
                        {
                            ql.MauSacs.Add(detail.MaMauNavigation.TenMau);
                            ql.HinhAnhs.Add(detail.MaMauNavigation.AnhSp ?? "");
                        }
                    }

                    if (detail.MaKichThuocNavigation != null)
                    {
                        if (!ql.Sizes.Contains(detail.MaKichThuocNavigation.Size))
                        {
                            ql.Sizes.Add(detail.MaKichThuocNavigation.Size);
                        }
                    }
                }
                for (int m = 0; m < ql.MauSacs.Count; m++)
                {
                    ql.SoLuong.Add(new List<int>(new int[ql.Sizes.Count]));
                }
                ql.NewHinhAnhs = new List<IFormFile>(new IFormFile[ql.MauSacs.Count]);
                int i = 0;
                foreach (var mauSac in ql.MauSacs)
                {
                    int j = 0;
                    foreach (var size in ql.Sizes)
                    {
                        var chitiet = data.Where(p => p.MaMauNavigation.TenMau == mauSac && p.MaKichThuocNavigation.Size == size).FirstOrDefault();
                        if (chitiet != null)
                        {
                            ql.SoLuong[i][j] = chitiet.SoLuong ?? 0;
                        }
                        j++;
                    }
                    i++;
                }
            }
            return View(ql);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult UpdateQuantity(QuanLySoLuongVM model)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < model.MauSacs.Count; i++)
                {
                    bool first = true;
                    for (int j = 0; j < model.Sizes.Count; j++)
                    {
                        var chitiet = _context.QuanLySanPhams
                            .Include(p => p.MaMauNavigation)
                            .Where(p => p.MaMauNavigation.TenMau == model.MauSacs[i] && p.MaKichThuocNavigation.Size == model.Sizes[j] && p.MaMauNavigation.MaSp == model.MaSp)
                            .FirstOrDefault();
                        if (chitiet != null)
                        {
                            var maMau = chitiet.MaMau;
                            chitiet.SoLuong = model.SoLuong[i][j];

                            if (first)
                            {
                                var mau = _context.MauSacs.Where(p => p.MaMau == maMau).FirstOrDefault();
                                if(mau != null && i < model.NewHinhAnhs.Count)
                                {
                                    if (model.NewHinhAnhs != null && model.NewHinhAnhs[i] != null && model.NewHinhAnhs[i].Length > 0 )
                                    {
                                        mau.AnhSp = model.NewHinhAnhs[i].FileName;
                                    }
                                }
                                first = false;
                            }
                        }
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Index", "SanPhamAdmin");
            }

            return RedirectToAction("Index", "SanPhamAdmin");
        }

    }
}
