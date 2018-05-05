using IKUS.LIB;
using IKUS.LIB.MODEL;
using IKUS.LIB.WEB.PLUGIN;
using SYS;
using SYS.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VEHICLEDETECTING.Models;
using A = VEHICLEDETECTING.Models.AAuth;

namespace VEHICLEDETECTING.Areas.Authority.Controllers
{
    [myFilterAuth(PublicAction: "_login,_LogOut,T")]
    //[ValidateInput(false)]
    public class MainController : myControllerAuth
    {        // GET: Authority/Main
        #region Index


        public ActionResult Index()
        {
            var TopMenu = new Dictionary<String, String>();
            #region TopMenu

            TopMenu.Add("退出", "/Admin/Main/_LogOut");
            TopMenu.Add("统计后台", "/Analysis/Main/Index");
            TopMenu.Add("权限后台", "/Authority/Main/Index");
            TopMenu.Add("管理后台", "/Admin/Main/Index");
            var EmpInfo = FunctionSYS?.Emp_GetEmpInfo(REQ.UserID)??new SYS.Table.T_SYS_EMP_INFO {EmpID=-1,Name="参数配置未初始化" };
            if (REQ.UserID > 0)
                TopMenu.Add("当前：" + EmpInfo.Name + "[" + REQ.UserID + "]", "#");
            else
                TopMenu.Add("未登录", "/Admin/Main/_LogOut?BackURL=" + Url.Encode("/Authority/Main/Index"));

            #endregion

            var SideMenu = new List<M_TreeNode>();
            #region SideMenu

            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=10,NodeName="员工[EMP]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=1021,ParentNodeID=10,NodeValue="/Authority/EMP/T_SYS_EMP_List",NodeName="员工列表",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=20,NodeName="部门[DEPARTMENT]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=202101,ParentNodeID=20,NodeValue="/Authority/DEPARTMENT/T_SYS_DEPARTMENT_List",NodeName="部门",NodeLevel=2},
                new M_TreeNode(){NodeID=202102,ParentNodeID=20,NodeValue="/Authority/DEPARTMENT/T_SYS_DEPARTMENT_Tree",NodeName="部门-树",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=30,NodeName="角色[ROLE]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=302101,ParentNodeID=30,NodeValue="/Authority/ROLE/T_SYS_GROUP_List",NodeName="角色组",NodeLevel=2},
                new M_TreeNode(){NodeID=302102,ParentNodeID=30,NodeValue="/Authority/ROLE/T_SYS_GROUP_Tree",NodeName="角色组-树",NodeLevel=2},
                new M_TreeNode(){NodeID=3022,ParentNodeID=30,NodeValue="/Authority/ROLE/T_SYS_ROLE_List",NodeName="角色",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=40,NodeName="权限[PRIVIEGE]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=4023,ParentNodeID=40,NodeValue="/Authority/PRIVIEGE/T_SYS_PRIVIEGE_Tree",NodeName="当前所有权限",NodeLevel=2},
                //new M_TreeNode(){NodeID=4024,ParentNodeID=40,NodeValue="/Authority/PRIVIEGE/T_SYS_PRIVIEGE_ACTION_List",NodeName="这是啥",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=70,NodeName="目录[PRIVIEGE]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=702101,ParentNodeID=70,NodeValue="/Authority/MENU/T_SYS_MENU_List",NodeName="目录",NodeLevel=2},
                new M_TreeNode(){NodeID=702102,ParentNodeID=70,NodeValue="/Authority/MENU/T_SYS_MENU_Tree",NodeName="目录-树",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=90,NodeName="配置[SETTING]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=902201,ParentNodeID=90,NodeValue="/Authority/SETTING/T_SYS_NODE_List",NodeName="节点",NodeLevel=2},
                new M_TreeNode(){NodeID=902202,ParentNodeID=90,NodeValue="/Authority/SETTING/T_SYS_NODE_Tree",NodeName="节点-树",NodeLevel=2},
            });

            #endregion

            ViewBag.TopMenu = TopMenu;
            ViewBag.SideMenu = SideMenu;
            ViewBag.DafaultTab = "权限后台";
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
