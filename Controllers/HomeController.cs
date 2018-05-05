using IKUS.LIB;
using IKUS.LIB.CACHE;
using IKUS.LIB.MODEL;
using IKUS.LIB.TOOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VEHICLEDETECTING.Models;
using static VEHICLEDETECTING.Functions.fReport;

namespace VEHICLEDETECTING.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var ActionName = (Request.RequestContext.RouteData.Values["Action"] ?? "").ToString().ToLower();
            var ControllerName = (Request.RequestContext.RouteData.Values["Controller"] ?? "").ToString().ToLower();
            var AreaName = (Request.RequestContext.RouteData.DataTokens["Area"] ?? "").ToString().ToLower();
            var URL = Request.Url.AbsoluteUri;
            Response.Write($"URL：{URL}<br/>");
            Response.Write($"ActionName：{ActionName}<br/>");
            Response.Write($"ControllerName：{ControllerName}<br/>");
            Response.Write($"AreaName：{AreaName}<br/>");
            Response.Write($"+++++++++++++++++++++++++++++++++++++<br/>");
            Response.Write($"DateTime.Now：{DateTime.Now.Ex_ToString()}<br/>");
            Response.Write($"DateTime.UtcNow：{DateTime.UtcNow.Ex_ToString()}<br/>");
            Response.Write($"DateTimeOffset.Now：{DateTimeOffset.Now.Ex_ToString()}<br/>");
            Response.Write($"DateTimeOffset.UtcNow：{DateTimeOffset.UtcNow.Ex_ToString()}<br/>");

            return Content("WELCOME BE BACK");
        }


        public ActionResult T(Int32 seconds = 0)
        {
            var Area = Config.E_Sys_Menu_Area.Administrator;
            var SideMenu = new List<M_TreeNode>();
            #region SideMenu
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=10,NodeName="检测项[Project]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=1001,ParentNodeID=10,NodeValue="/Admin/Project/VI_Project_List",NodeName="大项",NodeLevel=2},
                new M_TreeNode(){NodeID=100101,ParentNodeID=10,NodeValue="/Admin/Project/VI_Project_Form_List",NodeName="大项表单",NodeLevel=2},
                new M_TreeNode(){NodeID=10010101,ParentNodeID=10,NodeValue="/Admin/Project/VI_Project_Form_Option_List",NodeName="表单选项",NodeLevel=2},
                new M_TreeNode(){NodeID=1011,ParentNodeID=10,NodeValue="/Admin/Project/VI_Project_Pack_List",NodeName="套餐",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=20,NodeName="检测报告[Report]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=2000,ParentNodeID=20,NodeValue="/Admin/Report/VI_Report_List",NodeName="检测报告",NodeLevel=2},
                new M_TreeNode(){NodeID=2010,ParentNodeID=20,NodeValue="/Admin/Report/VI_Report_Customer_List",NodeName="客户信息",NodeLevel=2},
                new M_TreeNode(){NodeID=2020,ParentNodeID=20,NodeValue="/Admin/Report/VI_Report_Vehicle_List",NodeName="车辆信息",NodeLevel=2},
                new M_TreeNode(){NodeID=2030,ParentNodeID=20,NodeValue="/Admin/Report/VI_Report_Accessory_List",NodeName="附件信息",NodeLevel=2},
                new M_TreeNode(){NodeID=2040,ParentNodeID=20,NodeValue="/Admin/Report/VI_Report_Order_List",NodeName="订单信息",NodeLevel=2},
                new M_TreeNode(){NodeID=2051,ParentNodeID=20,NodeValue="/Admin/Report/VI_Report_Project_List",NodeName="报告-大项",NodeLevel=2},
                new M_TreeNode(){NodeID=2052,ParentNodeID=20,NodeValue="/Admin/Report/VI_Report_Project_Form_List",NodeName="报告-大项-表单",NodeLevel=2},
                new M_TreeNode(){NodeID=2053,ParentNodeID=20,NodeValue="/Admin/Report/VI_Report_Project_Form_Option_List",NodeName="报告-大项-表单-小项",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=30,NodeName="订单管理[Order]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=3021,ParentNodeID=30,NodeValue="/Admin/Report/VI_Report_Order_List",NodeName="订单管理",NodeLevel=2},
            });

            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=90,NodeName="插件[PlugIn]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=90101,ParentNodeID=90,NodeValue="/Admin/Adv/T_Adv_List",NodeName="广告列表",NodeLevel=2},
                new M_TreeNode(){NodeID=90102,ParentNodeID=90,NodeValue="/Admin/Adv/T_Adv_Site_List",NodeName="广告位列表",NodeLevel=2},
                new M_TreeNode(){NodeID=90301,ParentNodeID=90,NodeValue="/Admin/Version/T_Version_Server_List",NodeName="服务器",NodeLevel=2},
                new M_TreeNode(){NodeID=90302,ParentNodeID=90,NodeValue="/Admin/Version/T_Version_Update_List",NodeName="自动更新",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=99,NodeName="设置[Setting]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=99101,ParentNodeID=99,NodeValue="/Admin/Main/CacheClear",NodeName="清除所有缓存",NodeLevel=2},
                new M_TreeNode(){NodeID=99999,ParentNodeID=99,NodeValue="/Admin/Main/_ResetPsw",NodeName="重置密码",NodeLevel=2},
            });

            #endregion
            Init(SideMenu, Area);

            Area = Config.E_Sys_Menu_Area.Authority;
            SideMenu = new List<M_TreeNode>();
            #region SideMenu
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=10,NodeName="员工[EMP]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=1021,ParentNodeID=10,NodeValue="/Auth/EMP/T_SYS_EMP_List",NodeName="员工列表",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=20,NodeName="部门[DEPARTMENT]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=202101,ParentNodeID=20,NodeValue="/Auth/DEPARTMENT/T_SYS_DEPARTMENT_List",NodeName="部门",NodeLevel=2},
                new M_TreeNode(){NodeID=202102,ParentNodeID=20,NodeValue="/Auth/DEPARTMENT/T_SYS_DEPARTMENT_Tree",NodeName="部门-树",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=30,NodeName="角色[ROLE]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=302101,ParentNodeID=30,NodeValue="/Auth/ROLE/T_SYS_GROUP_List",NodeName="角色组",NodeLevel=2},
                new M_TreeNode(){NodeID=302102,ParentNodeID=30,NodeValue="/Auth/ROLE/T_SYS_GROUP_Tree",NodeName="角色组-树",NodeLevel=2},
                new M_TreeNode(){NodeID=3022,ParentNodeID=30,NodeValue="/Auth/ROLE/T_SYS_ROLE_List",NodeName="角色",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=40,NodeName="权限[PRIVIEGE]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=4023,ParentNodeID=40,NodeValue="/Auth/PRIVIEGE/T_SYS_PRIVIEGE_Tree",NodeName="当前所有权限",NodeLevel=2},
                //new M_TreeNode(){NodeID=4024,ParentNodeID=40,NodeValue="/Auth/PRIVIEGE/T_SYS_PRIVIEGE_ACTION_List",NodeName="这是啥",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=70,NodeName="目录[PRIVIEGE]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=702101,ParentNodeID=70,NodeValue="/Auth/MENU/T_SYS_MENU_List",NodeName="目录",NodeLevel=2},
                new M_TreeNode(){NodeID=702102,ParentNodeID=70,NodeValue="/Auth/MENU/T_SYS_MENU_Tree",NodeName="目录-树",NodeLevel=2},
            });
            SideMenu.AddRange(new List<M_TreeNode>
            {
                new M_TreeNode(){NodeID=90,NodeName="配置[SETTING]",NodeLevel=1,NodeValue="",ParentNodeID=0},
                new M_TreeNode(){NodeID=902201,ParentNodeID=90,NodeValue="/Auth/SETTING/T_SYS_NODE_List",NodeName="节点",NodeLevel=2},
                new M_TreeNode(){NodeID=902202,ParentNodeID=90,NodeValue="/Auth/SETTING/T_SYS_NODE_Tree",NodeName="节点-树",NodeLevel=2},
            });

            #endregion
            Init(SideMenu, Area);

            //new Functions.fReport().Report_Create(1, 1, 1);K:\CODE\YD\OUT\VEHICLEDETECTING\VEHICLEDETECTING\Content\JavaScript.json
            //var tDic3 = T_File.File_OpenText(@"K:\CODE\YD\OUT\VEHICLEDETECTING\VEHICLEDETECTING\Content\JavaScript-tmp3.json").Ex_ToEntity<_Tmp_Dic>();
            //var R3 = new Functions.fReport().Report_OverView_RandomInit(1, 3, tDic3);
            //var tDic5 = T_File.File_OpenText(@"K:\CODE\YD\OUT\VEHICLEDETECTING\VEHICLEDETECTING\Content\JavaScript-tmp5.json").Ex_ToEntity<_Tmp_Dic>();
            //var R5 = new Functions.fReport().Report_OverView_RandomInit(2, 5, tDic5);
            var tDic6 = T_File.File_OpenText(@"K:\CODE\YD\OUT\VEHICLEDETECTING\VEHICLEDETECTING\Content\JavaScript-tmp6.json").Ex_ToEntity<_Tmp_Dic>();
            var R6 = new Functions.fReport().Report_OverView_RandomInit(1, 6, tDic6);
            return Content($"Functions.fReport().Report_GetStatus(2,5)<br/><br/>" + new { R6 }.Ex_ToJson());
        }

        void Init(List<M_TreeNode> SideMenu, Config.E_Sys_Menu_Area Area)
        {
            AAuth.Exec(EF =>
            {
                var Root = Area.ToString();
                var LMenu = EF.T_SYS_MENU.Where(p => p.Root == Root)
                .Select(p => p.ID).ToList();
                var LButton = EF.T_SYS_BUTTON.Where(p => LMenu.Contains(p.MenuID))
                .Select(p => p.ID).ToList();

                EF.T_SYS_MENU.RemoveRange(EF.T_SYS_MENU
                    .Where(p => LMenu.Contains(p.ID)));
                EF.T_SYS_PRIVIEGE.RemoveRange(EF.T_SYS_PRIVIEGE
                    .Where(p => p.Type == (Int32)Config.E_Sys_Priviege_Type.MENU)
                    .Where(p => LMenu.Contains(p.KeyID)));

                EF.T_SYS_BUTTON.RemoveRange(EF.T_SYS_BUTTON
                    .Where(p => LButton.Contains(p.ID)));
                EF.T_SYS_PRIVIEGE.RemoveRange(EF.T_SYS_PRIVIEGE
                    .Where(p => p.Type == (Int32)Config.E_Sys_Priviege_Type.BUTTON)
                    .Where(p => LButton.Contains(p.KeyID)));

                EF.SaveChanges();
            });
            AAuth.Exec(EF =>
            {
                var RMenu = EF.T_SYS_MENU.Add(new SYS.Table.T_SYS_MENU
                {
                    CreateAdminID = 1,
                    CreateDateTime = DateTime.UtcNow,
                    Describe = "",
                    Image = "",
                    IsOpen = true,
                    Level = 0,
                    Link = "/Admin/Main/Index",
                    Name = "管理后台",
                    ParentID = 0,
                    Root = Area.ToString(),
                    SortNo = (Int32)Area,
                    Value = "/Admin/Main/Index",
                });
                EF.SaveChanges();
                EF.T_SYS_PRIVIEGE.Add(new SYS.Table.T_SYS_PRIVIEGE
                {
                    CreateAdminID = 1,
                    CreateDateTime = DateTime.Now,
                    IsOpen = true,
                    Type = (Int32)Config.E_Sys_Priviege_Type.MENU,
                    KeyID = RMenu.ID,
                });
                EF.SaveChanges();
                SideMenu.Where(p => p.NodeLevel == 1).ToList().ForEach(p =>
                {
                    var P = EF.T_SYS_MENU.Add(new SYS.Table.T_SYS_MENU
                    {
                        CreateAdminID = 1,
                        CreateDateTime = DateTime.UtcNow,
                        Describe = "",
                        Image = "",
                        IsOpen = true,
                        Level = p.NodeLevel,
                        Link = p.NodeValue,
                        Name = p.NodeName,
                        ParentID = RMenu.ID,
                        Root = Area.ToString(),
                        SortNo = p.NodeID,
                        Value = p.NodeValue,
                    });
                    EF.SaveChanges();
                    EF.T_SYS_PRIVIEGE.Add(new SYS.Table.T_SYS_PRIVIEGE
                    {
                        CreateAdminID = 1,
                        CreateDateTime = DateTime.Now,
                        IsOpen = true,
                        Type = (Int32)Config.E_Sys_Priviege_Type.MENU,
                        KeyID = P.ID,
                    });
                    EF.SaveChanges();
                    SideMenu.Where(m => m.ParentNodeID == p.NodeID).ToList().ForEach(m =>
                    {
                        var M = EF.T_SYS_MENU.Add(new SYS.Table.T_SYS_MENU
                        {
                            CreateAdminID = 1,
                            CreateDateTime = DateTime.UtcNow,
                            Describe = "",
                            Image = "",
                            IsOpen = true,
                            Level = m.NodeLevel,
                            Link = m.NodeValue,
                            Name = m.NodeName,
                            ParentID = P.ID,
                            Root = Area.ToString(),
                            SortNo = m.NodeID,
                            Value = m.NodeValue,
                        });
                        EF.SaveChanges();
                        EF.T_SYS_PRIVIEGE.Add(new SYS.Table.T_SYS_PRIVIEGE
                        {
                            CreateAdminID = 1,
                            CreateDateTime = DateTime.Now,
                            IsOpen = true,
                            Type = (Int32)Config.E_Sys_Priviege_Type.MENU,
                            KeyID = M.ID,
                        });
                        EF.SaveChanges();
                    });
                });
            });
        }
    }
}