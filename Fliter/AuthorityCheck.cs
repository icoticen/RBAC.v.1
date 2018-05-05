using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VEHICLEDETECTING.Fliter
{
    public class AuthorityCheck
    {


        public static Boolean FAuthorityCheckAdmin(String AreaName, String ControllerName, String ActionName, Int32 AdminID)
        {
            return true;
        }

        public class Admin : AuthorizeAttribute
        {

            private Int32 __AdminID = 0;
            protected Int32 _AdminID
            {
                get
                {
                    if (__AdminID > 0)
                        return __AdminID;
                    var M = M_Identify.Get(_CookieName);
                    if (M == null) __AdminID = 0;
                    else __AdminID = M.ID;
                    return __AdminID;
                }
                set
                {
                    if (value <= 0)
                    {
                        __AdminID = 0;
                        M_Identify.Clear(_CookieName);
                    }
                    else
                    {
                        M_Identify.Set(new M_Identify { ID = value, Identify = 1, IdentifyCode = "_Admin" });
                        __AdminID = value;
                    }
                }
            }

            private String _CookieName = "_Admin";
            private String _LoginURL = "/Admin/Main/_Login";
            private List<String> _PublicAction = new List<string> { "_Login", "_LogOut" };


            public Admin(String LoginURL = "/Admin/Main/_Login", String PublicAction = "_Login,_LogOut", String CookieName = "_Admin")
            {
                _LoginURL = LoginURL;
                _PublicAction = PublicAction.ToLower().Ex_ToList(',');
                _CookieName = CookieName;
            }
            #region IAuthorizationFilter 成员



            /// <summary>
            /// 验证登录
            /// </summary>
            /// <param name="filterContext"></param>
            public override void OnAuthorization(AuthorizationContext filterContext)
            {

                var ActionName = filterContext.RouteData.Values["Action"].ToString().ToLower();
                var ControllerName = filterContext.RouteData.Values["Controller"].ToString().ToLower();
                var AreaName = filterContext.RouteData.DataTokens["Area"].ToString().ToLower();

                //特别允许 Action
                if (_PublicAction.Contains(ActionName.ToLower())) return;


                //未登录
                if (_AdminID <= 0)
                {
                    if (ActionName.StartsWith("AJAX_"))
                        filterContext.HttpContext.Response.Write(new M_Result { result = 2, msg = "未登录 无权访问", data = null }.ToJson());
                    else //获取返回页面url
                        filterContext.HttpContext.Response.Write("<script type=\"text/javascript\">alert('ERROR:[" + _AdminID + "]该页面暂无权访问');top.location=\"" + _LoginURL + "?BackURL=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri) + "\";</script>");
                    filterContext.HttpContext.Response.End();
                    filterContext.Result = new EmptyResult();
                }
                else
                {
                    var IsValiable = AuthorityCheck.FAuthorityCheckAdmin(AreaName, ControllerName, ActionName, _AdminID);
                    if (!IsValiable)
                    {
                        filterContext.HttpContext.Response.Write("<script type=\"text/javascript\">alert('ERROR:[" + _AdminID + "]该页面暂无权访问');top.location=\"" + _LoginURL + "?BackURL=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri) + "\";</script>");
                        filterContext.HttpContext.Response.End();
                        filterContext.Result = new EmptyResult();
                    }
                    //判断用户是否有访问改页面的权限
                    //var R = IdolInfo.Models.Static.AuthorityCheck.IsAllow_MENU(ActionName, _AdminID);
                    //if (!R)
                    //{
                    //    filterContext.HttpContext.Response.Write("<script type=\"text/javascript\">alert('该页面暂无权访问');</script>");
                    //    filterContext.HttpContext.Response.End();
                    //    filterContext.Result = new EmptyResult();
                    //}
                }
            }
            #endregion
        }
    }
}