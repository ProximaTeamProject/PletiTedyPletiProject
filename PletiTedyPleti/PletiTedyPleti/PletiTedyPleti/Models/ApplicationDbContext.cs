﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PletiTedyPleti.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CreationData> CreationDataTable { get; set; }




        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<PletiTedyPleti.Models.Images> Images { get; set; }

        public System.Data.Entity.DbSet<PletiTedyPleti.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<PletiTedyPleti.Models.Order> Orders { get; set; }
    }
}