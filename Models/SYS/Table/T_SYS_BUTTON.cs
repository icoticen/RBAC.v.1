//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SYS.Table
{
    using IKUS.LIB.TABLE;
    using System;

    public partial class T_SYS_BUTTON:IAdminCreateBase
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Describe { get; set; }
        public int SortNo { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public int CreateAdminID { get; set; }
        public bool? IsOpen { get; set; }
    }
}
