using IKUS.LIB.WEB.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models
{
    public class myFilterAdmin : myFilter.WEB
    {
        public myFilterAdmin(String LoginURL = "/Admin/Main/_Login", String PublicAction = "_Login,_LogOut", String CookieName = "_Admin")
            : base(LoginURL, PublicAction, CookieName)
        {
            this.VALIDATECFG = new DefaultValidateConfigAdmin();
        }
    }
    public class myFilterAuth : myFilter.WEB
    {
        public myFilterAuth(String LoginURL = "/Admin/Main/_Login", String PublicAction = "_Login,_LogOut", String CookieName = "_Admin")
            : base(LoginURL, PublicAction, CookieName)
        {
            this.VALIDATECFG = new DefaultValidateConfigAuth();
        }
    }
}