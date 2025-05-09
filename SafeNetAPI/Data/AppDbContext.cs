﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SafeNetAPI.Models;
using System.Collections.Generic;

namespace SafeNetAPI.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<UserModel>(options)
    {

        public DbSet<RequestModel> Request { get; set; }
    }
}
