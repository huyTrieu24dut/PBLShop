﻿using Microsoft.AspNetCore.Mvc;
using PBLShop.ViewModels;
using RazorLight;
using HtmlToPdfMaster;
using PBLShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Scriban;
using System.Diagnostics;


namespace PBLShop.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly WebShopContext _context;
        public HoaDonController(WebShopContext context)
        {
            _context = context;
        }

        public dynamic RenderData(DonHang donHang, List<ChiTietDh> chiTietDhs)
        {
            var data = new
            {
                madh = donHang.MaDh,
                makh = donHang.MaNguoiDung,
                tenkh = donHang.MaNguoiDungNavigation.HoTen,
                tennguoinhan = donHang.TenNguoiNhan,
                tennv = _context.NguoiDungs
                        .Where(p => p.MaNguoiDung.ToString() == HttpContext.User.FindFirstValue("MaNguoiDung"))
                        .Select(p => p.HoTen).FirstOrDefault(),
                tongtien = donHang.TongTien.ToString("N0"),
                ngaydat = donHang.NgayDatHang,
                ngayhoanthanh = donHang.HoaDon.NgayHoanThanh.ToString("dd/MM/yyyyy"),
                diachi = donHang.DiaChi,
                sodienthoai = donHang.SdtnguoiNhan,
                sdtnguoidung = donHang.MaNguoiDungNavigation.SoDienThoai,
                email = donHang.MaNguoiDungNavigation.Email,
                trangthai = donHang.MaTrangThaiNavigation.TenTrangThai,
                phuongthuc = donHang.MaPtttNavigation.TenPt,
                chitietdhvms = new List<ChiTietHoaDonVM>(),
            };
            foreach (var item1 in chiTietDhs)
            {
                if (item1.MaMauNavigation != null && item1.MaMauNavigation.MaSpNavigation != null)
                {
                    var chiTietDhVM = new ChiTietHoaDonVM();
                    chiTietDhVM.tensp = item1.MaMauNavigation.MaSpNavigation.TenSp;
                    chiTietDhVM.mausp = item1.MaMauNavigation.TenMau;
                    chiTietDhVM.size = item1.MaKtNavigation.Size;
                    chiTietDhVM.dongia = item1.MaMauNavigation.MaSpNavigation.DonGia;
                    chiTietDhVM.soluong = item1.SoLuong ?? 0;
                    chiTietDhVM.tonggia = (chiTietDhVM.dongia * chiTietDhVM.soluong).ToString("N0");
                    chiTietDhVM.hinhanh = item1.MaMauNavigation.MaSpNavigation.AnhSp ?? "";
                    data.chitietdhvms.Add(chiTietDhVM);
                }
            }
            return data;
        }
        private void GeneratePdfFromHtml(string htmlPath, string pdfPath)
        {
            string content = System.IO.File.ReadAllText(htmlPath);
            var pdf = HtmlConverter.FromHtmlString(content, 210, 297);
            System.IO.File.WriteAllBytes(pdfPath, pdf);
        }

        public IActionResult GenerateInvoicePdf(int id)
        {
            var donHang = _context.DonHangs
                .Include(p => p.MaNguoiDungNavigation)
                .Include(p => p.MaPtttNavigation)
                .Include(p => p.MaTrangThaiNavigation)
                .Include(p => p.HoaDon)
                .Where(p => p.MaDh == id).FirstOrDefault();
            if (donHang == null)
            {
                TempData["Message"] = "Không tìm thấy đơn hàng";
                return View("/404");
            }
            else
            {
                var chitietdhs = _context.ChiTietDhs
                    .Include(p => p.MaMauNavigation)
                    .Include(p => p.MaKtNavigation)
                    .Include(p => p.MaMauNavigation.MaSpNavigation)
                    .Where(p => p.MaDh == donHang.MaDh)
                    .ToList();

                var model = RenderData(donHang, chitietdhs);
                var templateContent = System.IO.File.ReadAllText(@"Views\Shared\InvoiceHtml\Invoice_Template.html");
                var template = Template.Parse(templateContent);

                var pageContent = template.Render(model);
                System.IO.File.WriteAllText(@$"Views\Shared\InvoiceHtml\Invoice_{model.madh}.html", pageContent);

                var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Invoices", $"Invoice_{model.madh}.pdf");
                GeneratePdfFromHtml(@$"Views\Shared\InvoiceHtml\Invoice_{model.madh}.html", pdfPath);

                Process.Start(new ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                });
                return RedirectToAction("Index", "DonHangAdmin");
            }
        }

        public IActionResult GetInvoice(int id)
        {
            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Invoices", $"Invoice_{id}.pdf");

            Process.Start(new ProcessStartInfo
            {
                FileName = pdfPath,
                UseShellExecute = true
            });
            return RedirectToAction("Index", "DonHang");
        }
    }
}