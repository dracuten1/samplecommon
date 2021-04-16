using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SampleCommonInfo.Contexts
{
    public class ETL_Worker
    {
        [JsonProperty("id_worker")]
        public int id_worker { get; set; }
        public string file_name { get; set; }
        public string version_name { get; set; }
        public string checksum { get; set; }
        public int type { get; set; }
        public string location { get; set; }

        
    }
    public class Company_Worker
    {
        public int id { get; set; }
        public int quantity { get; set; }
        public int id_worker { get; set; }
        public int id_company { get; set; }
        public bool deleted { get; set; }
    }
    public class ETLWorkerContext:DbContext
    {
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(GlobalResouce.DbConnection.ToString());
        }
        public DbSet<ETL_Worker> ETL_Workers { get; set; }
        public DbSet<Company_Worker> Company_Workers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ETL_Worker>().ToTable("etl_workers");
            modelBuilder.Entity<Company_Worker>().ToTable("worker_config");
            modelBuilder.Entity<ETL_Worker>().HasKey(e => e.id_worker);
            modelBuilder.Entity<Company_Worker>().HasKey(e => e.id);
            modelBuilder.Entity<ETL_Worker>().Property(e => e.id_worker).HasColumnName("id_version");
        }
        public override void Dispose()
        {
            Console.WriteLine("ContextDispose");
            base.Dispose();
        }
    }
}
