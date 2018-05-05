namespace VEHICLEDETECTING.Models
{
    using SYS.Table;
    using System.Data.Entity;

    public partial class EAuth : DbContext
    {
        public EAuth()
            : base("name=EAuth")
        {
        }
        public virtual DbSet<T_SYS_BUTTON> T_SYS_BUTTON { get; set; }
        public virtual DbSet<T_SYS_DEPARTMENT> T_SYS_DEPARTMENT { get; set; }
        public virtual DbSet<T_SYS_DEPARTMENT_POSITION> T_SYS_DEPARTMENT_POSITION { get; set; }
        public virtual DbSet<T_SYS_DEPARTMENT_POSITION_refEMP> T_SYS_DEPARTMENT_POSITION_refEMP { get; set; }
        public virtual DbSet<T_SYS_EMP> T_SYS_EMP { get; set; }
        public virtual DbSet<T_SYS_EMP_INFO> T_SYS_EMP_INFO { get; set; }
        public virtual DbSet<T_SYS_EMP_refGROUP> T_SYS_EMP_refGROUP { get; set; }
        public virtual DbSet<T_SYS_EMP_refROLE> T_SYS_EMP_refROLE { get; set; }
        public virtual DbSet<T_SYS_GROUP> T_SYS_GROUP { get; set; }
        public virtual DbSet<T_SYS_GROUP_refROLE> T_SYS_GROUP_refROLE { get; set; }
        public virtual DbSet<T_SYS_MENU> T_SYS_MENU { get; set; }
        public virtual DbSet<T_SYS_NODE> T_SYS_NODE { get; set; }
        public virtual DbSet<T_SYS_NODE_ELEMENT> T_SYS_NODE_ELEMENT { get; set; }
        public virtual DbSet<T_SYS_PRIVIEGE> T_SYS_PRIVIEGE { get; set; }
        public virtual DbSet<T_SYS_PRIVIEGE_ACTION> T_SYS_PRIVIEGE_ACTION { get; set; }
        public virtual DbSet<T_SYS_ROLE> T_SYS_ROLE { get; set; }
        public virtual DbSet<T_SYS_ROLE_refPRIVIEGE> T_SYS_ROLE_refPRIVIEGE { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
