using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Table
{

    public interface IShelveBase : IBase
    {
        DateTime? ShelveDateTime { get; set; }
        DateTime? UnShelveDateTime { get; set; }
    }
}