﻿using Microsoft.EntityFrameworkCore;
using UserManagementBack.Models;

namespace UserManagementBack.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
    }
}
