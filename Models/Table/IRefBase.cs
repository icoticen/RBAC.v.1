using System;

namespace Table
{
    public interface IRefBase
    {
        int ID { get; set; }
        int KeyID { get; set; }
        int refKeyID { get; set; }
        int CreateAdminID { get; set; }
        DateTime? CreateDateTime { get; set; }
        bool? HasCancle { get; set; }
        DateTime? CancleDateTime { get; set; }
    }
}