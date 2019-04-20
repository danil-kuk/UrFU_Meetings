using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;

namespace WebApp.Models.DataModels
{
    public class EFDBContext: DbContext
    {
        public EFDBContext(DbContextOptions<EFDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<EmailValid> EmailValid { get; set; }
        public DbSet<PasswordReset> PasswordReset { get; set; }
    }
}
