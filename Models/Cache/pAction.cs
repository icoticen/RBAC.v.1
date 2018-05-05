using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models.Cache
{
    public class pAction
    {
        public string this[String ExtraKey]
        {
            get { return ExtraData.ContainsKey(ExtraKey ?? "-") ? ExtraData[ExtraKey ?? "-"] : null; }
            set { ExtraData[ExtraKey ?? "-"] = value; }
        }
        public Int32 ID { get; set; }
        public Int32 KeyID { get; set; }
        public Int32 UserID { get; set; }
        public DateTime? ActionDateTime { get; set; }

        public Dictionary<String, String> ExtraData { get; set; } = new Dictionary<string, string>();
    }
}