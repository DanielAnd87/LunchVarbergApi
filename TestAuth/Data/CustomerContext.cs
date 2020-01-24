﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestAuth.Models;

namespace TestAuth.Data
{
    public class CustomerContext : DbContext
    {
        private readonly IConfiguration _config;

        public CustomerContext(DbContextOptions<CustomerContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<Customer> Customer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnectionString"));
            optionsBuilder.EnableSensitiveDataLogging();
        }

       
    }

}

