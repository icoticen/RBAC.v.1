//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Table.SYS
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_SYS_DEPARTMENT_POSITION:Table.IBase
    {
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int SortNo { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public int CreateAdminID { get; set; }
        public bool? IsOpen { get; set; }

        public Int32 EmpID { get; set; }
    }
}
