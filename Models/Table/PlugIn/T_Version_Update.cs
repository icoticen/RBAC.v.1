//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Table.PlugIn
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_Version_Update
    {
        public int ID { get; set; }
        public Int32 Channel { get; set; }
        public string VersionName { get; set; }
        public Int32 VersionIndex { get; set; }
        public string VersionDetails { get; set; }
        public string FaceImage { get; set; }
        public float AppSize { get; set; }
        public Nullable<bool> IsMust { get; set; }
        public Nullable<System.DateTime> UpdateDateTime { get; set; }
        public string DownloadLink { get; set; }
    }
}
