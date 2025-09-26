using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Context;

public partial class db_userservicesContext : DbContext
{
    public db_userservicesContext()
    {
    }

    public db_userservicesContext(DbContextOptions<db_userservicesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorydetail> Categorydetails { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Memberpackage> Memberpackages { get; set; }

    public virtual DbSet<Plantedlog> Plantedlogs { get; set; }

    public virtual DbSet<Scene> Scenes { get; set; }

    public virtual DbSet<Scenedetail> Scenedetails { get; set; }

    public virtual DbSet<Transactionhistory> Transactionhistories { get; set; }

    public virtual DbSet<User> Users { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];
        return strConn;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Categorydetail>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ItemId }).HasName("categorydetails_pkey");

            entity.ToTable("categorydetails", "userservices");

            entity.Property(e => e.UserId).HasMaxLength(50);
            entity.Property(e => e.ItemId).HasMaxLength(20);

            entity.HasOne(d => d.Item).WithMany(p => p.Categorydetails)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item");

            entity.HasOne(d => d.User).WithMany(p => p.Categorydetails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("item_pkey");

            entity.ToTable("item", "userservices");

            entity.Property(e => e.ItemId).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<Memberpackage>(entity =>
        {
            entity.HasKey(e => e.MemberTypeId).HasName("memberpackage_pkey");

            entity.ToTable("memberpackage", "userservices");

            entity.Property(e => e.MemberTypeId).HasMaxLength(20);
            entity.Property(e => e.NameType).HasMaxLength(100);
        });

        modelBuilder.Entity<Plantedlog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("plantedlog", "userservices");

            entity.Property(e => e.ItemId).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(50);

            entity.HasOne(d => d.Item).WithMany()
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("fk_planted_item");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_planted_user");
        });

        modelBuilder.Entity<Scene>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("scene_pkey");

            entity.ToTable("scene", "unityservices");

            entity.Property(e => e.UserId).HasMaxLength(20);
            entity.Property(e => e.DateSave).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<Scenedetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("scenedetails", "unityservices");

            entity.Property(e => e.ItemId).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UserId).HasMaxLength(20);

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_scene");
        });

        modelBuilder.Entity<Transactionhistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transactionhistory_pkey");

            entity.ToTable("transactionhistory", "userservices");

            entity.Property(e => e.Id).HasMaxLength(20);
            entity.Property(e => e.DateTrade).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.UserId).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Transactionhistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_tx_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user", "userservices");

            entity.Property(e => e.UserId).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.MemberTypeId).HasMaxLength(20);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UserName).HasMaxLength(200);

            entity.HasOne(d => d.MemberType).WithMany(p => p.Users)
                .HasForeignKey(d => d.MemberTypeId)
                .HasConstraintName("fk_member");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
