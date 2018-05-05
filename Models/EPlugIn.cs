namespace VEHICLEDETECTING.Models
{
    using global::PlugIn.Table;
    using System.Data.Entity;

    public partial class EPlugIn : DbContext
    {
        public EPlugIn()
            : base("name=EVI")
        {
        }

        public virtual DbSet<T_Adv> T_Adv { get; set; }
        public virtual DbSet<T_Adv_ActClick> T_Adv_ActClick { get; set; }
        public virtual DbSet<T_Adv_ActShow> T_Adv_ActShow { get; set; }
        public virtual DbSet<T_Adv_refSite> T_Adv_refSite { get; set; }
        public virtual DbSet<T_Adv_Site> T_Adv_Site { get; set; }
        public virtual DbSet<T_Push> T_Push { get; set; }
        public virtual DbSet<T_Push_ActClick> T_Push_ActClick { get; set; }
        public virtual DbSet<T_Push_ActShow> T_Push_ActShow { get; set; }
        public virtual DbSet<T_Version_Server> T_Version_Server { get; set; }
        public virtual DbSet<T_Version_Update> T_Version_Update { get; set; }
        public virtual DbSet<T_SMS_Verification> T_SMS_Verification { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<VI_Project>().HasMany(p => p.Forms).WithOptional(p => p.Project).HasForeignKey(p => p.ProjectID);
            //modelBuilder.Entity<VI_Project_Form>().HasMany(p => p.Options).WithOptional(p => p.Form).HasForeignKey(p => p.FormID);

            //modelBuilder.Entity<VI_Report>().HasOptional(p => p.Order).WithOptionalDependent(p => p.Report).Map(m => m.MapKey("ReportID"));
            //modelBuilder.Entity<VI_Report>().HasOptional(p => p.Vehicle).WithOptionalDependent(p => p.Report).Map(m => m.MapKey("ReportID"));
            //modelBuilder.Entity<VI_Report>().HasOptional(p => p.Customer).WithOptionalDependent(p => p.Report).Map(m => m.MapKey("ReportID"));

            //modelBuilder.Entity<VI_Report>().HasMany(p => p.Projects).WithOptional(p => p.Report).HasForeignKey(p => p.ReportID);
        }
    }
}
