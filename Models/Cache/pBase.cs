using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models.Cache
{
    public abstract class pBase
    {
        public Int32 SortIndex { get; set; } = 0;
        public Int32 ID { get; set; } = 0;

        public pActionCollection Actions { get; protected set; } = new pActionCollection();
        public pItemCollection Items { get; protected set; } = new pItemCollection();

    }
    public abstract class pBase<T>
    {

        public Int32 SortIndex { get; set; } = 0;
        public Int32 ID { get; set; } = 0;

        public pActionCollection Actions { get; protected set; } = new pActionCollection();
        public pItemCollection Items { get; protected set; } = new pItemCollection();

    }
}