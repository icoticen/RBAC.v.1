using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table
{
    public abstract class pRefBase : IRefBase
    {
        public int ID { get; set; }
        public int KeyID { get; set; }
        public int refKeyID { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public int CreateAdminID { get; set; }

        public Nullable<bool> HasCancle { get; set; }
        public Nullable<System.DateTime> CancleDateTime { get; set; }
    }
}