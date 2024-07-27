using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using project.roles;
using project.users;

namespace AvionesBackNet.Models;

public partial class AvionesContext : IdentityDbContext<userEntity, rolEntity, string>
{
    IConfiguration _configuration;
    public AvionesContext(DbContextOptions<AvionesContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }


    public DbSet<Aerolinea> Aerolineas { get; set; }

    public DbSet<AerolineaAeropuerto> AerolineaAeropuertos { get; set; }

    public DbSet<Aeropuerto> Aeropuertos { get; set; }

    public DbSet<Asiento> Asientos { get; set; }

    public DbSet<Avione> Aviones { get; set; }

    public DbSet<BitacoraCuerpo> BitacoraCuerpos { get; set; }

    public DbSet<BitacoraEncabezado> BitacoraEncabezados { get; set; }

    public DbSet<Boleto> Boletos { get; set; }

    public DbSet<Catalogo> Catalogos { get; set; }

    public DbSet<CatalogoTipo> CatalogoTipos { get; set; }

    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Empleado> Empleados { get; set; }



    public DbSet<Paise> Paises { get; set; }


    public DbSet<Tripulacione> Tripulaciones { get; set; }



    public DbSet<Vuelo> Vuelos { get; set; }

    public DbSet<VueloClase> VueloClases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de las relaciones
        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.CodigoTelefonoEmergenciaObj)
            .WithMany()
            .HasForeignKey(c => c.CodigoTelefonoEmergencia)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.CodigoTelefonoObj)
            .WithMany()
            .HasForeignKey(c => c.CodigoTelefono)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Pais)
            .WithMany()
            .HasForeignKey(c => c.PaisId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

}
