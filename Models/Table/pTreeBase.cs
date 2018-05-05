using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Table
{
    public abstract class pTreeBase : ITreeBase
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public String Describe { get; set; }
        public int SortNo { get; set; }

        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public int CreateAdminID { get; set; }
        public bool? IsOpen { get; set; }
    }

}