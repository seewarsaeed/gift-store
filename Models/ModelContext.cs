using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GiftStore.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategortR> CategortRs { get; set; }

    public virtual DbSet<CustomerR> CustomerRs { get; set; }

    public virtual DbSet<Departmentemployee> Departmentemployees { get; set; }

    public virtual DbSet<LoginR> LoginRs { get; set; }

    public virtual DbSet<ProductCustomerR> ProductCustomerRs { get; set; }

    public virtual DbSet<ProductR> ProductRs { get; set; }

    public virtual DbSet<Projectaboutu> Projectaboutus { get; set; }

    public virtual DbSet<Projectbackground> Projectbackgrounds { get; set; }

    public virtual DbSet<Projectcategory> Projectcategories { get; set; }

    public virtual DbSet<Projectcategoryuser> Projectcategoryusers { get; set; }

    public virtual DbSet<Projectcontactu> Projectcontactus { get; set; }

    public virtual DbSet<Projectfooter> Projectfooters { get; set; }

    public virtual DbSet<Projectgift> Projectgifts { get; set; }

    public virtual DbSet<Projectheader> Projectheaders { get; set; }

    public virtual DbSet<Projecthome> Projecthomes { get; set; }

    public virtual DbSet<Projectpayment> Projectpayments { get; set; }

    public virtual DbSet<Projectpresent> Projectpresents { get; set; }

    public virtual DbSet<Projectrole> Projectroles { get; set; }

    public virtual DbSet<Projectstatus> Projectstatuses { get; set; }

    public virtual DbSet<Projecttestimonial> Projecttestimonials { get; set; }

    public virtual DbSet<Projectuser> Projectusers { get; set; }

    public virtual DbSet<RolesR> RolesRs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=JOR18_USER309;PASSWORD=seewar;DATA SOURCE=94.56.229.181:3488/traindb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("JOR18_USER309")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<CategortR>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00372362");

            entity.ToTable("CATEGORT_R");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEGORY_NAME");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
        });

        modelBuilder.Entity<CustomerR>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00372369");

            entity.ToTable("CUSTOMER_R");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LNAME");
        });

        modelBuilder.Entity<Departmentemployee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DEPARTMENTEMPLOYEES");

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Departmentname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DEPARTMENTNAME");
            entity.Property(e => e.Employeeid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("EMPLOYEEID");
            entity.Property(e => e.Firstname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("FIRSTNAME");
            entity.Property(e => e.Lastname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LASTNAME");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
            entity.Property(e => e.Salary)
                .HasColumnType("NUMBER")
                .HasColumnName("SALARY");
        });

        modelBuilder.Entity<LoginR>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00372364");

            entity.ToTable("LOGIN_R");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CustomerId)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.RoleId)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.UserName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");

            entity.HasOne(d => d.Customer).WithMany(p => p.LoginRs)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("LOGIN_CUSTOMER");

            entity.HasOne(d => d.Role).WithMany(p => p.LoginRs)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("LOGIN_ROLES");
        });

        modelBuilder.Entity<ProductCustomerR>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00372355");

            entity.ToTable("PRODUCT_CUSTOMER_R");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CudtomerId)
                .HasColumnType("NUMBER")
                .HasColumnName("CUDTOMER_ID");
            entity.Property(e => e.DateFrom)
                .HasColumnType("DATE")
                .HasColumnName("DATE_FROM");
            entity.Property(e => e.DateTo)
                .HasColumnType("DATE")
                .HasColumnName("DATE_TO");
            entity.Property(e => e.ProductId)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCT_ID");
            entity.Property(e => e.Quantity)
                .HasColumnType("NUMBER")
                .HasColumnName("QUANTITY");

            entity.HasOne(d => d.Cudtomer).WithMany(p => p.ProductCustomerRs)
                .HasForeignKey(d => d.CudtomerId)
                .HasConstraintName("PRODUCT_CUSTOMER_FK");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductCustomerRs)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("PRODUCR_FK");
        });

        modelBuilder.Entity<ProductR>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00372360");

            entity.ToTable("PRODUCT_R");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.Namee)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAMEE");
            entity.Property(e => e.Price)
                .HasColumnType("FLOAT")
                .HasColumnName("PRICE");
            entity.Property(e => e.Sale)
                .HasColumnType("NUMBER")
                .HasColumnName("SALE");

            entity.HasOne(d => d.Category).WithMany(p => p.ProductRs)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("PRODUCT_CATEGORY_FK");
        });

        modelBuilder.Entity<Projectaboutu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377031");

            entity.ToTable("PROJECTABOUTUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Image)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
        });

        modelBuilder.Entity<Projectbackground>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377037");

            entity.ToTable("PROJECTBACKGROUND");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
        });

        modelBuilder.Entity<Projectcategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377056");

            entity.ToTable("PROJECTCATEGORIES");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Projectcategoryuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377456");

            entity.ToTable("PROJECTCATEGORYUSER");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Categoryid)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Category).WithMany(p => p.Projectcategoryusers)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("CATEGORYFK");

            entity.HasOne(d => d.User).WithMany(p => p.Projectcategoryusers)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("USERFK");
        });

        modelBuilder.Entity<Projectcontactu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377033");

            entity.ToTable("PROJECTCONTACTUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Pnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PNUMBER");
        });

        modelBuilder.Entity<Projectfooter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377023");

            entity.ToTable("PROJECTFOOTER");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Copyright)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COPYRIGHT");
        });

        modelBuilder.Entity<Projectgift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377058");

            entity.ToTable("PROJECTGIFTS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CategoryUsrId).HasColumnType("NUMBER");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER")
                .HasColumnName("PRICE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.CategoryUsr).WithMany(p => p.Projectgifts)
                .HasForeignKey(d => d.CategoryUsrId)
                .HasConstraintName("CATEGORYUSERGIFTFK");

            entity.HasOne(d => d.User).WithMany(p => p.Projectgifts)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("GIFTUSERFK");
        });

        modelBuilder.Entity<Projectheader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377029");

            entity.ToTable("PROJECTHEADER");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Projecthome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377019");

            entity.ToTable("PROJECTHOME");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Aboutusid)
                .HasColumnType("NUMBER")
                .HasColumnName("ABOUTUSID");
            entity.Property(e => e.Backgroundid)
                .HasColumnType("NUMBER")
                .HasColumnName("BACKGROUNDID");
            entity.Property(e => e.Contactusid)
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACTUSID");
            entity.Property(e => e.Footerid)
                .HasColumnType("NUMBER")
                .HasColumnName("FOOTERID");
            entity.Property(e => e.Headerid)
                .HasColumnType("NUMBER")
                .HasColumnName("HEADERID");
            entity.Property(e => e.Logo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("LOGO");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Testimonialid)
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALID");

            entity.HasOne(d => d.Aboutus).WithMany(p => p.Projecthomes)
                .HasForeignKey(d => d.Aboutusid)
                .HasConstraintName("HOMEABOUTFK");

            entity.HasOne(d => d.Background).WithMany(p => p.Projecthomes)
                .HasForeignKey(d => d.Backgroundid)
                .HasConstraintName("HOMEBACKGROUNDFK");

            entity.HasOne(d => d.Contactus).WithMany(p => p.Projecthomes)
                .HasForeignKey(d => d.Contactusid)
                .HasConstraintName("HOMWCONTACTFK");

            entity.HasOne(d => d.Footer).WithMany(p => p.Projecthomes)
                .HasForeignKey(d => d.Footerid)
                .HasConstraintName("HOMEFOOTERFK");

            entity.HasOne(d => d.Header).WithMany(p => p.Projecthomes)
                .HasForeignKey(d => d.Headerid)
                .HasConstraintName("HOMEHEADERFK");

            entity.HasOne(d => d.Testimonial).WithMany(p => p.Projecthomes)
                .HasForeignKey(d => d.Testimonialid)
                .HasConstraintName("HOMETESTIMONIALFK");
        });

        modelBuilder.Entity<Projectpayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377052");

            entity.ToTable("PROJECTPAYMENT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Availablebalance)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AVAILABLEBALANCE");
            entity.Property(e => e.Cardnumber)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cvvcvc)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("CVVCVC");
            entity.Property(e => e.Expirationdate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIRATIONDATE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Projectpayments)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("USERPAYMENT");
        });

        modelBuilder.Entity<Projectpresent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377060");

            entity.ToTable("PROJECTPRESENT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Giftsid)
                .HasColumnType("NUMBER")
                .HasColumnName("GIFTSID");
            entity.Property(e => e.Reciveraddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("RECIVERADDRESS");
            entity.Property(e => e.Requestdate)
                .HasColumnType("DATE")
                .HasColumnName("REQUESTDATE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Gifts).WithMany(p => p.Projectpresents)
                .HasForeignKey(d => d.Giftsid)
                .HasConstraintName("GIFTPRESENTFK");

            entity.HasOne(d => d.User).WithMany(p => p.Projectpresents)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("USERPRESENT");
        });

        modelBuilder.Entity<Projectrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377050");

            entity.ToTable("PROJECTROLES");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Projectstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377448");

            entity.ToTable("PROJECTSTATUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Arrivedstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ARRIVEDSTATUS");
            entity.Property(e => e.Notifecationstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NOTIFECATIONSTATUS");
            entity.Property(e => e.Paidstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PAIDSTATUS");
            entity.Property(e => e.Presentid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRESENTID");
            entity.Property(e => e.Requeststatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("REQUESTSTATUS");

            entity.HasOne(d => d.Present).WithMany(p => p.Projectstatuses)
                .HasForeignKey(d => d.Presentid)
                .HasConstraintName("STATUSPRESENTFK");
        });

        modelBuilder.Entity<Projecttestimonial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377035");

            entity.ToTable("PROJECTTESTIMONIALS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Satatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SATATUS");
            entity.Property(e => e.UserId).HasColumnType("NUMBER");

            entity.HasOne(d => d.User).WithMany(p => p.Projecttestimonials)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("USERTESTIMONIALS");
        });

        modelBuilder.Entity<Projectuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00377054");

            entity.ToTable("PROJECTUSER");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Fname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.Lname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Pnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PNUMBER");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Role).WithMany(p => p.Projectusers)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("ROLEUSERFK");
        });

        modelBuilder.Entity<RolesR>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C00372366");

            entity.ToTable("ROLES_R");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ROLE_NAME");
        });
        modelBuilder.HasSequence("SEQ_CUSTOMER_ID");
        modelBuilder.HasSequence("SEQ_DRAWING_ID");
        modelBuilder.HasSequence("SEQ_EMPLOYEE_ID");
        modelBuilder.HasSequence("SEQ_PURCHASE_ID");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
