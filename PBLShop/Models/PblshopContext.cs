using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PBLShop.Models;

public partial class PblshopContext : DbContext
{
    public PblshopContext()
    {
    }

    public PblshopContext(DbContextOptions<PblshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDh> ChiTietDhs { get; set; }

    public virtual DbSet<ChiTietGh> ChiTietGhs { get; set; }

    public virtual DbSet<ChucVu> ChucVus { get; set; }

    public virtual DbSet<DanhGia> DanhGias { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KichThuoc> KichThuocs { get; set; }

    public virtual DbSet<MauSac> MauSacs { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }

    public virtual DbSet<QuanLyDh> QuanLyDhs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-P13IU1H\\SQLEXPRESS;Initial Catalog=PBLShop;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDh>(entity =>
        {
            entity.HasKey(e => new { e.MaDh, e.MaSp }).HasName("pk_chitietHD");

            entity.ToTable("ChiTietDH");

            entity.Property(e => e.MaDh)
                .HasMaxLength(20)
                .HasColumnName("MaDH");
            entity.Property(e => e.MaSp)
                .HasMaxLength(20)
                .HasColumnName("MaSP");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ChiTietDhs)
                .HasForeignKey(d => d.MaDh)
                .HasConstraintName("fk_chitietHD_mahd");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietDhs)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("fk_chitietHD_masp");
        });

        modelBuilder.Entity<ChiTietGh>(entity =>
        {
            entity.HasKey(e => new { e.MaSp, e.MaKh }).HasName("pk_chitietGH");

            entity.ToTable("ChiTietGH");

            entity.Property(e => e.MaSp)
                .HasMaxLength(20)
                .HasColumnName("MaSP");
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.ChiTietGhs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("fk_chitietGH_makh");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietGhs)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("fk_chitietGH_masp");
        });

        modelBuilder.Entity<ChucVu>(entity =>
        {
            entity.HasKey(e => e.MaChucVu).HasName("pk_chucvu");

            entity.ToTable("ChucVu");

            entity.Property(e => e.MaChucVu).HasColumnName("MaChucVu");
            entity.Property(e => e.TenChucVu).HasMaxLength(50).HasColumnName("TenChucVu");
        });

        modelBuilder.Entity<DanhGia>(entity =>
        {
            entity.HasKey(e => new { e.MaDanhGia, e.MaSp }).HasName("pk_danhgia");

            entity.Property(e => e.MaDanhGia).HasMaxLength(20);
            entity.Property(e => e.MaSp)
                .HasMaxLength(20)
                .HasColumnName("MaSP");
            entity.Property(e => e.SoSao).HasColumnType("decimal(1, 1)");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.DanhGias)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("fk_danhgia_masp");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.MaDm).HasName("pk_danhmuc");

            entity.ToTable("DanhMuc");

            entity.Property(e => e.MaDm)
                .HasMaxLength(20)
                .HasColumnName("MaDM");
            entity.Property(e => e.TenDm)
                .HasMaxLength(30)
                .HasColumnName("TenDM");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDh).HasName("pk_hoadon");

            entity.ToTable("DonHang");

            entity.Property(e => e.MaDh)
                .HasMaxLength(20)
                .HasColumnName("MaDH");
            entity.Property(e => e.DiaChi).HasMaxLength(60);
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.NgayDat).HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasMaxLength(20);
            entity.Property(e => e.MaPhuongThuc)
                .HasMaxLength(10)
                .HasColumnName("MaPhuongThuc");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("fk_hoadon_makh");
            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaPhuongThuc)
                .HasConstraintName("fk_phuongthucthanhtoan_mahd");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon);

            entity.ToTable("Hoa_don");

            entity.Property(e => e.MaHoaDon).HasMaxLength(20);
            entity.Property(e => e.MaDh)
                .HasMaxLength(20)
                .HasColumnName("MaDH");
            entity.Property(e => e.NgayHoanThanh).HasColumnType("date");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_madonhang");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("pk_khachhang");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.DiaChi).HasMaxLength(60);
            entity.Property(e => e.DienThoai).HasMaxLength(10);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
        });

        modelBuilder.Entity<KichThuoc>(entity =>
        {
            entity.HasKey(e => new { e.MaSize, e.MaSp }).HasName("pk_size");

            entity.ToTable("KichThuoc");

            entity.Property(e => e.MaSize).HasMaxLength(20);
            entity.Property(e => e.MaSp)
                .HasMaxLength(20)
                .HasColumnName("MaSP");
            entity.Property(e => e.Size).HasMaxLength(30);

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.KichThuocs)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("fk_size_masp");
        });

        modelBuilder.Entity<MauSac>(entity =>
        {
            entity.HasKey(e => new { e.MaMau, e.MaSp }).HasName("pk_mausac");

            entity.ToTable("MauSac");

            entity.Property(e => e.MaMau).HasMaxLength(20);
            entity.Property(e => e.MaSp)
                .HasMaxLength(20)
                .HasColumnName("MaSP");
            entity.Property(e => e.TenMau).HasMaxLength(30);

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.MauSacs)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("fk_mausac_masp");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("pk_nhanvien");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv)
                .HasMaxLength(20)
                .HasColumnName("MaNV");
            entity.Property(e => e.DienThoai).HasMaxLength(10);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.HasOne(d => d.MaChucVuNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaChucVu)
                .HasConstraintName("fk_nhanvien_machucvu");
        });

        modelBuilder.Entity<PhuongThucThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaPhuongThuc).HasName("pk_phuongthucthanhtoan");

            entity.ToTable("PhuongThucThanhToan");

            entity.Property(e => e.MaPhuongThuc).HasMaxLength(20);
            entity.Property(e => e.TenPhuongThuc).HasMaxLength(50);
        });

        modelBuilder.Entity<QuanLyDh>(entity =>
        {
            entity.HasKey(e => new { e.MaNv, e.MaDh }).HasName("pk_quanlyHD");

            entity.ToTable("QuanLyDH");

            entity.Property(e => e.MaNv)
                .HasMaxLength(20)
                .HasColumnName("MaNV");
            entity.Property(e => e.MaDh)
                .HasMaxLength(20)
                .HasColumnName("MaDH");
            entity.Property(e => e.ThoiGian).HasColumnType("datetime");
            entity.Property(e => e.TrangThaiCapNhat).HasMaxLength(20);

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.QuanLyDhs)
                .HasForeignKey(d => d.MaDh)
                .HasConstraintName("fk_quanlyHD_mahd");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.QuanLyDhs)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("fk_quanlyHD_manv");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("pk_sanpham");

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSp)
                .HasMaxLength(20)
                .HasColumnName("MaSP");
            entity.Property(e => e.MaDm)
                .HasMaxLength(20)
                .HasColumnName("MaDM");
            entity.Property(e => e.TenSp)
                .HasMaxLength(50)
                .HasColumnName("TenSP");

            entity.HasOne(d => d.MaDmNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaDm)
                .HasConstraintName("fk_sanpham_madm");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
