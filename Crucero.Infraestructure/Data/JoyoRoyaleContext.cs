using System;
using System.Collections.Generic;
using JoyoRoyale.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

namespace JoyoRoyale.Infraestructure.Data;

public partial class JoyoRoyaleContext : DbContext
{
    public JoyoRoyaleContext(DbContextOptions<JoyoRoyaleContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BarcoHabitaciones> BarcoHabitaciones { get; set; }

    public virtual DbSet<Barcos> Barcos { get; set; }

    public virtual DbSet<Complementos> Complementos { get; set; }

    public virtual DbSet<Cruceros> Cruceros { get; set; }

    public virtual DbSet<Destinos> Destinos { get; set; }

    public virtual DbSet<FechasCruceros> FechasCruceros { get; set; }

    public virtual DbSet<Habitaciones> Habitaciones { get; set; }

    public virtual DbSet<Huespedes> Huespedes { get; set; }

    public virtual DbSet<Itinerarios> Itinerarios { get; set; }

    public virtual DbSet<PreciosHabitaciones> PreciosHabitaciones { get; set; }

    public virtual DbSet<Puertos> Puertos { get; set; }

    public virtual DbSet<Reservas> Reservas { get; set; }

    public virtual DbSet<ReservasComplementos> ReservasComplementos { get; set; }

    public virtual DbSet<ReservasHabitaciones> ReservasHabitaciones { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BarcoHabitaciones>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BarcoHab__3214EC271F4C5769");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BarcoId).HasColumnName("BarcoID");
            entity.Property(e => e.HabitacionId).HasColumnName("HabitacionID");

            entity.HasOne(d => d.Barco).WithMany(p => p.BarcoHabitaciones)
                .HasForeignKey(d => d.BarcoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BarcoHabi__Barco__4222D4EF");

            entity.HasOne(d => d.Habitacion).WithMany(p => p.BarcoHabitaciones)
                .HasForeignKey(d => d.HabitacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BarcoHabi__Habit__4316F928");
        });

        modelBuilder.Entity<Barcos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Barcos__3214EC27DB7BA512");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Complementos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Compleme__3214EC2753F9B2AC");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TipoAplicacion).HasMaxLength(50);
        });

        modelBuilder.Entity<Cruceros>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cruceros__3214EC2791C1CDB8");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BarcoId).HasColumnName("BarcoID");
            entity.Property(e => e.Descripcion).HasMaxLength(180);
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.Barco).WithMany(p => p.Cruceros)
                .HasForeignKey(d => d.BarcoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cruceros__BarcoI__440B1D61");
        });

        modelBuilder.Entity<Destinos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Destinos__3214EC27FD593414");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<FechasCruceros>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FechasCr__3214EC2731D6E69C");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CruceroId).HasColumnName("CruceroID");

            entity.HasOne(d => d.Crucero).WithMany(p => p.FechasCruceros)
                .HasForeignKey(d => d.CruceroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FechasCru__Cruce__44FF419A");
        });

        modelBuilder.Entity<Habitaciones>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Habitaci__3214EC2755D00E10");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Huespedes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Huespede__3214EC27A99B7039");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.DocumentoIdentidad).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.ReservaId).HasColumnName("ReservaID");
            entity.Property(e => e.Telefono).HasMaxLength(50);

            entity.HasOne(d => d.Reserva).WithMany(p => p.Huespedes)
                .HasForeignKey(d => d.ReservaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Huespedes__Reser__45F365D3");
        });

        modelBuilder.Entity<Itinerarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Itinerar__3214EC273A0E2C18");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CruceroId).HasColumnName("CruceroID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.PuertoId).HasColumnName("PuertoID");

            entity.HasOne(d => d.Crucero).WithMany(p => p.Itinerarios)
                .HasForeignKey(d => d.CruceroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Itinerari__Cruce__46E78A0C");

            entity.HasOne(d => d.Puerto).WithMany(p => p.Itinerarios)
                .HasForeignKey(d => d.PuertoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Itinerari__Puert__47DBAE45");
        });

        modelBuilder.Entity<PreciosHabitaciones>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PreciosH__3214EC27A71B22DE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FechaCruceroId).HasColumnName("FechaCruceroID");
            entity.Property(e => e.HabitacionId).HasColumnName("HabitacionID");
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.FechaCrucero).WithMany(p => p.PreciosHabitaciones)
                .HasForeignKey(d => d.FechaCruceroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PreciosHa__Fecha__48CFD27E");

            entity.HasOne(d => d.Habitacion).WithMany(p => p.PreciosHabitaciones)
                .HasForeignKey(d => d.HabitacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PreciosHa__Habit__49C3F6B7");
        });

        modelBuilder.Entity<Puertos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Puertos__3214EC271CD03B00");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DestinoId).HasColumnName("DestinoID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Pais).HasMaxLength(50);

            entity.HasOne(d => d.Destino).WithMany(p => p.Puertos)
                .HasForeignKey(d => d.DestinoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Puertos__Destino__4AB81AF0");
        });

        modelBuilder.Entity<Reservas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservas__3214EC27B9D6F77B");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CruceroId).HasColumnName("CruceroID");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Crucero).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.CruceroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservas__Crucer__4BAC3F29");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservas__Usuari__4CA06362");
        });

        modelBuilder.Entity<ReservasComplementos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservas__3214EC2708729797");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComplementoId).HasColumnName("ComplementoID");
            entity.Property(e => e.ReservaId).HasColumnName("ReservaID");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Complemento).WithMany(p => p.ReservasComplementos)
                .HasForeignKey(d => d.ComplementoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReservasC__Compl__4D94879B");

            entity.HasOne(d => d.Reserva).WithMany(p => p.ReservasComplementos)
                .HasForeignKey(d => d.ReservaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReservasC__Reser__4E88ABD4");
        });

        modelBuilder.Entity<ReservasHabitaciones>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservas__3214EC2702EC0A58");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.HabitacionId).HasColumnName("HabitacionID");
            entity.Property(e => e.ReservaId).HasColumnName("ReservaID");

            entity.HasOne(d => d.Habitacion).WithMany(p => p.ReservasHabitaciones)
                .HasForeignKey(d => d.HabitacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReservasH__Habit__4F7CD00D");

            entity.HasOne(d => d.Reserva).WithMany(p => p.ReservasHabitaciones)
                .HasForeignKey(d => d.ReservaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReservasH__Reser__5070F446");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC27A598EFA8");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC27BCD7E396");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A1934BA40EA").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Contrasena).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Pais).HasMaxLength(50);
            entity.Property(e => e.RolId).HasColumnName("RolID");
            entity.Property(e => e.Telefono).HasMaxLength(15);

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__RolID__5165187F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
