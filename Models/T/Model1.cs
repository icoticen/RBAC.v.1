namespace VEHICLEDETECTING.Models.T
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Table.PlugIn;
    using Table.Project;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }
        public virtual DbSet<VI_Project_Pack> T_Adv { get; set; }
        public virtual DbSet<VI_Project_Pack_refProject> T_Adv_ActClick { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
