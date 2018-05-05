using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Table
{
    public class pActBase : IActBase
    {
        public int ID { get; set; }
        public int AppSource { get; set; }
        public int AccountType { get; set; }
        public string Mac { get; set; }
        public string IP { get; set; }
        public int KeyID { get; set; }
        public int UserID { get; set; }
        public string PhoneModel { get; set; }
        public DateTime? ActionDateTime { get; set; }
        public bool? HasCancle { get; set; }
        public DateTime? CancleDateTime { get; set; }
    }
}