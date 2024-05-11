using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PBLShop.Models;

public partial class WebShopContext : DbContext
{
    public WebShopContext()
    {
    }

    public WebShopContext(DbContextOptions<WebShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDh> ChiTietDhs { get; set; }

    public virtual DbSet<ChiTietGh> ChiTietGhs { get; set; }

    public virtual DbSet<DanhGia> DanhGia { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GioiTinh> GioiTinhs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KichThuoc> KichThuocs { get; set; }

    public virtual DbSet<MauSac> MauSacs { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }

    public virtual DbSet<QuanLyDh> QuanLyDhs { get; set; }

    public virtual DbSet<QuanLySanPham> QuanLySanPhams { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-P13IU1H\\SQLEXPRESS;Initial Catalog=WebShop;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDh>(entity =>
        {
            entity.HasKey(e => new { e.MaDh, e.MaMau, e.MaKt }).HasName("pk_chitietdh");

            entity.ToTable("ChiTietDH");

            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaKt).HasColumnName("MaKT");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ChiTietDhs)
                .HasForeignKey(d => d.MaDh)
                .HasConstraintName("fk_chitietdh_madh");

            entity.HasOne(d => d.MaKtNavigation).WithMany(p => p.ChiTietDhs)
                .HasForeignKey(d => d.MaKt)
                .HasConstraintName("fk_chitietdh_makt");

            entity.HasOne(d => d.MaMauNavigation).WithMany(p => p.ChiTietDhs)
                .HasForeignKey(d => d.MaMau)
                .HasConstraintName("fk_chitietdh_mamau");
        });

        modelBuilder.Entity<ChiTietGh>(entity =>
        {
            entity.HasKey(e => new { e.MaKh, e.MaMau, e.MaKt }).HasName("pk_chitietgh");

            entity.ToTable("ChiTietGH");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.MaKt).HasColumnName("MaKT");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.ChiTietGhs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("fk_chitietgh_makh");

            entity.HasOne(d => d.MaKtNavigation).WithMany(p => p.ChiTietGhs)
                .HasForeignKey(d => d.MaKt)
                .HasConstraintName("fk_chitietgh_makt");

            entity.HasOne(d => d.MaMauNavigation).WithMany(p => p.ChiTietGhs)
                .HasForeignKey(d => d.MaMau)
                .HasConstraintName("fk_chitietgh_mamau");
        });

        modelBuilder.Entity<DanhGia>(entity =>
        {
            entity.HasKey(e => e.MaDg).HasName("pk_danhgia");

            entity.Property(e => e.MaDg).HasColumnName("MaDG");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");

            entity.Property(e => e.SoSao)
                .HasColumnName("SoSao")
                .HasColumnType("decimal(1, 0)"); ;

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("fk_danhgia_nguoidung");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("fk_danhgia_sanpham");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.MaDm).HasName("pk_danhmuc");

            entity.ToTable("DanhMuc");

            entity.Property(e => e.MaDm).HasColumnName("MaDM");
            entity.Property(e => e.MaDmcha).HasColumnName("MaDMCha");
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenDanhMuc).HasMaxLength(50);

            entity.HasOne(d => d.MaDmchaNavigation).WithMany(p => p.InverseMaDmchaNavigation)
                .HasForeignKey(d => d.MaDmcha)
                .HasConstraintName("fk_DanhMuc_MaDMCha");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDh).HasName("pk_donhang");

            entity.ToTable("DonHang");

            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.MaPttt).HasColumnName("MaPTTT");
            entity.Property(e => e.NgayDatHang).HasColumnType("datetime");
            entity.Property(e => e.SdtnguoiNhan)
                .HasMaxLength(10)
                .HasColumnName("SDTNguoiNhan");
            entity.Property(e => e.TenNguoiNhan).HasMaxLength(50);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("fk_donhang_nguoidung");

            entity.HasOne(d => d.MaPtttNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaPttt)
                .HasConstraintName("fk_donhang_pttt");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaTrangThai)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_donhang_trangthai");
        });

        modelBuilder.Entity<GioiTinh>(entity =>
        {
            entity.HasKey(e => e.MaGioiTinh).HasName("pk_gioitinh");

            entity.ToTable("GioiTinh");

            entity.Property(e => e.TenGioiTinh).HasMaxLength(255);
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("pk_hoadon");

            entity.ToTable("HoaDon");

            entity.HasIndex(e => e.MaDh, "UQ__HoaDon__272586607DB7E11E").IsUnique();

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.FileHoaDon).HasMaxLength(255);
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.NgayHoanThanh).HasColumnType("datetime");

            entity.HasOne(d => d.MaDhNavigation).WithOne(p => p.HoaDon)
                .HasForeignKey<HoaDon>(d => d.MaDh)
                .HasConstraintName("fk_hoadon_madonhang");
        });

        modelBuilder.Entity<KichThuoc>(entity =>
        {
            entity.HasKey(e => e.MaKt).HasName("pk_kichthuoc");

            entity.ToTable("KichThuoc");

            entity.Property(e => e.MaKt).HasColumnName("MaKT");
            entity.Property(e => e.Size)
                .HasMaxLength(50)
                .HasColumnName("KichThuoc");
        });

        modelBuilder.Entity<MauSac>(entity =>
        {
            entity.HasKey(e => e.MaMau).HasName("pk_mausac");

            entity.ToTable("MauSac");

            entity.Property(e => e.AnhSp)
                .HasMaxLength(50)
                .HasColumnName("AnhSP");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.TenMau).HasMaxLength(50);

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.MauSacs)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("fk_mausac_masp");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("pk_nguoidung");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.Email, "UQ__NguoiDun__A9D10534254CF2B7").IsUnique();

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.SoDienThoai).HasMaxLength(10);

            entity.HasOne(d => d.MaGioiTinhNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.MaGioiTinh)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_nguoidung_gioitinh");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.MaVaiTro)
                .HasConstraintName("fk_nguoidung_vaitro");
        });

        modelBuilder.Entity<PhuongThucThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaPttt).HasName("pk_phuongthuctt");

            entity.ToTable("PhuongThucThanhToan");

            entity.Property(e => e.MaPttt).HasColumnName("MaPTTT");
            entity.Property(e => e.TenPt)
                .HasMaxLength(50)
                .HasColumnName("TenPT");
        });

        modelBuilder.Entity<QuanLyDh>(entity =>
        {
            entity.HasKey(e => e.MaQldh).HasName("pk_quanlydh");

            entity.ToTable("QuanLyDH");

            entity.Property(e => e.MaQldh).HasColumnName("MaQLDH");
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.ThoiGian).HasColumnType("datetime");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.QuanLyDhs)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_qldonhang_madonhang");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.QuanLyDhs)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("fk_qldonhang_manhanvien");
        });

        modelBuilder.Entity<QuanLySanPham>(entity =>
        {
            entity.HasKey(e => new { e.MaMau, e.MaKichThuoc }).HasName("pk_qlsoluong");

            entity.ToTable("QuanLySanPham");

            entity.HasOne(d => d.MaKichThuocNavigation).WithMany(p => p.QuanLySanPhams)
                .HasForeignKey(d => d.MaKichThuoc)
                .HasConstraintName("fk_qlsanpham_makt");

            entity.HasOne(d => d.MaMauNavigation).WithMany(p => p.QuanLySanPhams)
                .HasForeignKey(d => d.MaMau)
                .HasConstraintName("fk_qlsanpham_mamau");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("pk_sanpham");

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.AnhSp)
                .HasMaxLength(50)
                .HasColumnName("AnhSP");
            entity.Property(e => e.MaDm).HasColumnName("MaDM");
            entity.Property(e => e.TenSp)
                .HasMaxLength(255)
                .HasColumnName("TenSP");

            entity.HasOne(d => d.MaDmNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaDm)
                .HasConstraintName("fk_sanpham_danhmuc");
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTrangThai).HasName("pk_trangthai");

            entity.ToTable("TrangThai");

            entity.Property(e => e.TenTrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("pk_vaitro");

            entity.ToTable("VaiTro");

            entity.Property(e => e.TenVaiTro).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
