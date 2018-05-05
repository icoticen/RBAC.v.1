using IKUS.LIB;
using IKUS.LIB.MODEL;
using IKUS.LIB.TOOL;
using IKUS.LIB.WEB;
using IKUS.LIB.WEB.MVC;
using IKUS.LIB.WEB.PLUGIN;
using SYS;
using SYS.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VEHICLEDETECTING.Functions;
using VEHICLEDETECTING.Models;
using A = VEHICLEDETECTING.Models.AAuth;

namespace VEHICLEDETECTING.Areas.Auth.Controllers
{
    [myFilterAuth(PublicAction: "_login,_LogOut,T")]
    //[ValidateInput(false)]
    public class MainController : myControllerAuth
    {
        static Functions.fSys FUNCTHIS = new Functions.fSys();
        #region Index


        public ActionResult Index()
        {
            var Area = Config.E_Sys_Menu_Area.Authority;
            var DefaultTab = "";

            var lMenu = FUNCTHIS.Menu_GetAllMenus_InUser(REQ.UserID, Root: "");

            var TopMenu = new Dictionary<String, String>();
            #region TopMenu

            TopMenu.Add("退出", "/Admin/Main/_LogOut");
            foreach (var m in lMenu.Where(p => p.Level == 0).OrderByDescending(p => p.SortNo))
            {
                TopMenu.Add(m.Name, m.Value);
                if (m.Root == Area.ToString()) DefaultTab = m.Name;
            }
            var EmpInfo = FunctionSYS?.Emp_GetEmpInfo(REQ.UserID) ?? new SYS.Table.T_SYS_EMP_INFO { EmpID = -1, Name = "参数配置未初始化" };
            if (REQ.UserID > 0)
                TopMenu.Add("当前：" + EmpInfo.Name + "[" + REQ.UserID + "]", "#");
            else
                TopMenu.Add("未登录", "/Admin/Main/_LogOut");

            #endregion

            var SideMenu = lMenu.Where(p => p.Root == Area.ToString())
                .Select(m => new M_TreeNode
                {
                    NodeDescrib = m.Describe,
                    NodeID = m.ID,
                    NodeLevel = m.Level,
                    NodeName = m.Name,
                    NodeSortNo = m.SortNo,
                    NodeValue = m.Value,
                    ParentNodeID = m.ParentID,
                }).OrderBy(p => p.NodeSortNo)
                .ToList();


            ViewBag.TopMenu = TopMenu;
            ViewBag.SideMenu = SideMenu;
            ViewBag.DefaultTab = DefaultTab;
            ViewBag.DefaultPage = "/../../Content/Static/Image//background.jpg";
            ViewBag.SwitchLong = " <img src=\"/../../Content/Static/Image/logo.png\" style=\"height:30px;\">HIGHER TEST";
            ViewBag.SwitchShort = "<img src=\"/../../Content/Static/Image/logo.png\" style=\"height:30px;\">";
            return View();
        }


        //public ActionResult _LogOut()
        //{
        //    _AdminID = 0;
        //    return Redirect("/Admin/Main/_Login");
        //}
        //public ActionResult _Login(string AccountName, string AccountPsw, String BackURL)
        //{
        //    BackURL = BackURL ?? "/Admin/Main/Index";
        //    if (Request.RequestType == "POST")
        //    {
        //        if (string.IsNullOrWhiteSpace(AccountName) || string.IsNullOrWhiteSpace(AccountPsw))
        //        {
        //            Response.Write("<script>alert('用户名密码不能为空~~~~~！');</script>");
        //            //T_Web.Tmpl_Result_LayUI(new M_Result() { result = 0, msg = "用户名密码不正确" });
        //            //return Content("");                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
        //            return View();
        //        }
        //        var R = Function.Login(AccountName, AccountPsw);
        //        if (R.result != (Int32)E_ERRORCODE.操作成功)
        //        {
        //            Response.Write("<script>alert('用户账号或密码不正确~~~~~！');</script>");
        //            return View();
        //        }
        //        else
        //        {
        //            _AdminID = (Int32)R.data;
        //            return Redirect(BackURL);
        //        }
        //    }
        //    return View();
        //}



        public JsonResult CacheClear()
        {
            //Cache.RemoveAllCache();
            //FCACHE.Actions = new ChiGuaDaShu.Models.Table.CacheEntity.pActionCollection();
            //FCACHE.Items = new ChiGuaDaShu.Models.Table.CacheEntity.pItemCollection();
            var M = new M_Result(E_ERRORCODE.操作成功, DATA: new { DateTime.Now, REQ.UserID, });
            return Json(M, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Clear(Int32 UserID = 0, Int32 ToLevel = 1, Int32 ToWalletRemain = 100000000)
        {
            //int A = 0, B = 0, C = 0, D = 0, Level = 0, WalletRemain = 0, WCost = 0, WGain = 0;
            //if (UserID > 0)
            return Content($"UserID:{UserID}<br/>clear kan liao");
        }

        #endregion

        public ActionResult T()
        {
            var ActionName = (Request.RequestContext.RouteData.Values["Action"] ?? "").ToString().ToLower();
            var ControllerName = (Request.RequestContext.RouteData.Values["Controller"] ?? "").ToString().ToLower();
            var AreaName = (Request.RequestContext.RouteData.DataTokens["Area"] ?? "").ToString().ToLower();
            return Content(new { ActionName, ControllerName, AreaName }.Ex_ToJson());
        }
    }




}
