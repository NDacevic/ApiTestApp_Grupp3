using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiTestApp_Grupp3.Models;

namespace ApiTestApp_Grupp3.Data
{
    public class ApiTestApp_Grupp3Context : DbContext
    {
        public ApiTestApp_Grupp3Context (DbContextOptions<ApiTestApp_Grupp3Context> options)
            : base(options)
        {
        }

        public DbSet<ApiTestApp_Grupp3.Models.Test> Test { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.Employee> Employee { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.Student> Student { get; set; }

        public DbSet<ApiTestApp_Grupp3.Models.Question> Question { get; set; }
    }
}
