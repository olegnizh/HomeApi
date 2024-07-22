﻿using HomeApi.Data.Models;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace HomeApi.Data
{
    public sealed class HomeApiContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Device> Devices { get; set; }

        public HomeApiContext(DbContextOptions<HomeApiContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Room>().ToTable("Rooms");
            builder.Entity<Device>().ToTable("Devices");
        }
    }
}
