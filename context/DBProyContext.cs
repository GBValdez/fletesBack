using System;
using System.Collections.Generic;
using fletesProyect.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using project.roles;
using project.users;
using project.users.Models;
using project.utils.catalogue;

namespace AvionesBackNet.Models;

public partial class DBProyContext : IdentityDbContext<userEntity, rolEntity, string>
{
    IConfiguration _configuration;
    public DBProyContext(DbContextOptions<DBProyContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Orden> Orders { get; set; }
    public DbSet<ordenDetail> OrderDetails { get; set; }
    public DbSet<product> Products { get; set; }
    public DbSet<productProvider> productProviders { get; set; }
    public DbSet<vehicleProduct> VehicleProducts { get; set; }
    public DbSet<stationProduct> stationProducts { get; set; }
    public DbSet<modelGasoline> modelGasolines { get; set; }
    public DbSet<Provider> providers { get; set; }
    public DbSet<Station> stations { get; set; }
    public DbSet<Visit> visits { get; set; }

    public DbSet<binnacleBody> BinnacleBodies { get; set; }
    public DbSet<binnacleHeader> BinnacleHeaders { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Catalogue> catalogues { get; set; }
    public DbSet<catalogueType> catalogueTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

}
