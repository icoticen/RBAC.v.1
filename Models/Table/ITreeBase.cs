using System;

namespace Table
{
    public interface ITreeBase
    {
        int ID { get; set; }
        int ParentID { get; set; }
        string Name { get; set; }
        String Describe { get; set; }
        int SortNo { get; set; }

        Nullable<System.DateTime> CreateDateTime { get; set; }
        int CreateAdminID { get; set; }
        bool? IsOpen { get; set; }
    }
}