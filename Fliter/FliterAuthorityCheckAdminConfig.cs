using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Fliter
{
    public class FliterAuthorityCheckAdminConfig 
    {
       
        public static Boolean AuthorityCheck(String ControllerName, String ActionName, Int32 AdminID)
        {
            return true;
        }        
    }
}