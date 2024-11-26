using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Shopee.Data;

public partial class ShopporContext : DbContext
{
    public ShopporContext()
    {
    }

    public ShopporContext(DbContextOptions<ShopporContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BanBe> BanBes { get; set; }

    public virtual DbSet<Chitiethd> Chitiethds { get; set; }

    public virtual DbSet<Gopy> Gopies { get; set; }

    public virtual DbSet<Hanghoa> Hanghoas { get; set; }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Loai> Loais { get; set; }

    public virtual DbSet<Nhacungcap> Nhacungcaps { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<PhanCong> PhanCongs { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }

    public virtual DbSet<Yeuthich> Yeuthiches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-ALUJNDO\\SQLEXPRESS;Initial Catalog=shoppor;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BanBe>(entity =>
        {
            entity.HasKey(e => e.MaBb).HasName("PK__BanBe__272475B9CFD1FEFF");

            entity.ToTable("BanBe");

            entity.Property(e => e.MaBb).HasColumnName("MaBB");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.NgayGui).HasColumnType("datetime");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.BanBes)
                .HasForeignKey(d => d.MaHh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BanBe__MaHH__5441852A");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.BanBes)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__BanBe__MaKH__534D60F1");
        });

        modelBuilder.Entity<Chitiethd>(entity =>
        {
            entity.HasKey(e => e.MaCt).HasName("PK__CHITIETH__27258E74AA14DBB3");

            entity.ToTable("CHITIETHD");

            entity.Property(e => e.MaCt).HasColumnName("MaCT");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.Chitiethds)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETHD__MaHD__49C3F6B7");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.Chitiethds)
                .HasForeignKey(d => d.MaHh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETHD__MaHH__4AB81AF0");
        });

        modelBuilder.Entity<Gopy>(entity =>
        {
            entity.HasKey(e => e.MaGy).HasName("PK__GOPY__2725AEF42EC467E7");

            entity.ToTable("GOPY");

            entity.Property(e => e.MaGy)
                .HasMaxLength(50)
                .HasColumnName("MaGY");
            entity.Property(e => e.DienThoai).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaCd).HasColumnName("MaCD");
            entity.Property(e => e.NgayGy).HasColumnName("NgayGY");
            entity.Property(e => e.NgayTl).HasColumnName("NgayTL");
            entity.Property(e => e.TraLoi).HasMaxLength(50);
        });

        modelBuilder.Entity<Hanghoa>(entity =>
        {
            entity.HasKey(e => e.MaHh).HasName("PK__HANGHOA__2725A6E430C12A97");

            entity.ToTable("HANGHOA");

            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.Danhgia).HasMaxLength(50);
            entity.Property(e => e.Hinh).HasMaxLength(50);
            entity.Property(e => e.MaNcc)
                .HasMaxLength(50)
                .HasColumnName("MaNCC");
            entity.Property(e => e.MoTaDonVi).HasMaxLength(50);
            entity.Property(e => e.NgaySx)
                .HasColumnType("datetime")
                .HasColumnName("NgaySX");
            entity.Property(e => e.TenAlias).HasMaxLength(50);
            entity.Property(e => e.TenHh)
                .HasMaxLength(50)
                .HasColumnName("TenHH");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.Hanghoas)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HANGHOA__MaLoai__3F466844");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.Hanghoas)
                .HasForeignKey(d => d.MaNcc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HANGHOA__MaNCC__403A8C7D");
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HOADON__2725A6E07628DD72");

            entity.ToTable("HOADON");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.CachThanhToan).HasMaxLength(50);
            entity.Property(e => e.CachVanChuyen).HasMaxLength(50);
            entity.Property(e => e.DiaChi).HasMaxLength(60);
            entity.Property(e => e.GhiChu).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.NgayCan).HasColumnType("datetime");
            entity.Property(e => e.NgayDat).HasColumnType("datetime");
            entity.Property(e => e.NgayGiao).HasColumnType("datetime");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HOADON__MaKH__44FF419A");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK__HOADON__MaNV__46E78A0C");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.MaTrangThai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HOADON__MaTrangT__45F365D3");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.Makh).HasName("PK__KHACHHAN__603F592C381E9D98");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.Makh)
                .HasMaxLength(20)
                .HasColumnName("MAKH");
            entity.Property(e => e.Diachi)
                .HasMaxLength(60)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Dienthoai)
                .HasMaxLength(24)
                .HasColumnName("DIENTHOAI");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Gioitinh).HasColumnName("GIOITINH");
            entity.Property(e => e.Hieuluc).HasColumnName("HIEULUC");
            entity.Property(e => e.Hinh)
                .HasMaxLength(50)
                .HasColumnName("HINH");
            entity.Property(e => e.Hoten)
                .HasMaxLength(50)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Matkhau)
                .HasMaxLength(50)
                .HasColumnName("MATKHAU");
            entity.Property(e => e.Ngaysinh)
                .HasColumnType("datetime")
                .HasColumnName("NGAYSINH");
            entity.Property(e => e.Randomkey)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RANDOMKEY");
            entity.Property(e => e.Vaitro).HasColumnName("VAITRO");
        });

        modelBuilder.Entity<Loai>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LOAI__730A575958B40E3D");

            entity.ToTable("LOAI");

            entity.Property(e => e.Hinh).HasMaxLength(50);
            entity.Property(e => e.TenLoai).HasMaxLength(50);
            entity.Property(e => e.TenLoaiAlias).HasMaxLength(50);
        });

        modelBuilder.Entity<Nhacungcap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK__NHACUNGC__3A185DEB70719ECC");

            entity.ToTable("NHACUNGCAP");

            entity.Property(e => e.MaNcc)
                .HasMaxLength(50)
                .HasColumnName("MaNCC");
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.DienThoai).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Logo).HasMaxLength(50);
            entity.Property(e => e.NguoiLienLac).HasMaxLength(50);
            entity.Property(e => e.TenCongTy).HasMaxLength(50);
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.Manv).HasName("PK__NHANVIEN__603F5114156361CC");

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.Manv)
                .HasMaxLength(50)
                .HasColumnName("MANV");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Hoten)
                .HasMaxLength(50)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Matkhau)
                .HasMaxLength(50)
                .HasColumnName("MATKHAU");
        });

        modelBuilder.Entity<PhanCong>(entity =>
        {
            entity.HasKey(e => e.MaPc).HasName("PK__PhanCong__2725E7E542DE4675");

            entity.ToTable("PhanCong");

            entity.Property(e => e.MaPc).HasColumnName("MaPC");
            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.MaPb)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("MaPB");
            entity.Property(e => e.NgayPc)
                .HasColumnType("datetime")
                .HasColumnName("NgayPC");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.PhanCongs)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhanCong__MaNV__571DF1D5");
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.MaPq).HasName("PK__PhanQuye__2725E7F38A173F10");

            entity.ToTable("PhanQuyen");

            entity.Property(e => e.MaPq).HasColumnName("MaPQ");
            entity.Property(e => e.MaPb)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("MaPB");
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTrangThai).HasName("PK__TrangTha__AADE41389B5098CC");

            entity.ToTable("TrangThai");

            entity.Property(e => e.MaTrangThai).ValueGeneratedNever();
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.TenTrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<Yeuthich>(entity =>
        {
            entity.HasKey(e => e.MaYt).HasName("PK__YEUTHICH__272330D45C5BFEDF");

            entity.ToTable("YEUTHICH");

            entity.Property(e => e.MaYt).HasColumnName("MaYT");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.NgayChon).HasColumnType("datetime");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.Yeuthiches)
                .HasForeignKey(d => d.MaHh)
                .HasConstraintName("FK__YEUTHICH__MaHH__4D94879B");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Yeuthiches)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__YEUTHICH__MaKH__4E88ABD4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
