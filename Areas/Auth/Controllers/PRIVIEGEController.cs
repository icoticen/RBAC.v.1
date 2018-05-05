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
    [myFilterAuth]
    public class PRIVIEGEController : myControllerAuth
    {
        // GET: Authority/PRIVIRGR
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult T_SYS_PRIVIEGE_Tree()
        {
            var LayData = new LayUI.Data();

            LayData.zTreeData.TreeNodes = A.Exec(EF =>
            {
                var L = new List<zTree.ITreeNodeBase>();
                L.Add(new zTree.TreeNodeBase { id = -1, pId = 0, name = "ROOT", color = "#FF4500" });
                L.Add(new zTree.TreeNodeBase { id = (Int32)Config.E_Sys_Priviege_Type.MENU, pId = -1, name = "MENU", color = "#FF4500" });
                L.AddRange(EF.T_SYS_MENU.Join(EF.T_SYS_PRIVIEGE.Where(p => p.Type == (Int32)Config.E_Sys_Priviege_Type.MENU), p => p.ID, q => q.KeyID, (p, q) => new
                {
                    id = p.ID + (Int32)Config.E_Sys_Priviege_Type.MENU,
                    pId = p.ParentID + (Int32)Config.E_Sys_Priviege_Type.MENU,
                    name = "[MENU]" + p.Name,
                    IsOpen = p.IsOpen,
                    priviegeid = q.ID,
                    Image = "/Content/Static/Image/EasyIcon/1116512.png",
                }).Concat(EF.T_SYS_BUTTON.Join(EF.T_SYS_PRIVIEGE.Where(p => p.Type == (Int32)Config.E_Sys_Priviege_Type.BUTTON), p => p.ID, q => q.KeyID, (p, q) => new
                {
                    id = p.ID + (Int32)Config.E_Sys_Priviege_Type.BUTTON,
                    pId = p.MenuID + (Int32)Config.E_Sys_Priviege_Type.MENU,
                    name = "[BUTTON]" + p.Name,
                    IsOpen = p.IsOpen,
                    priviegeid = q.ID,
                    Image = "/Content/Static/Image/EasyIcon/1116060.png",
                }))
                //.GroupJoin(EF.T_SYS_ROLE_refPRIVIEGE.Where(p => p.KeyID == ROLEID && !(p.HasCancle ?? false)), p => p.priviegeid, q => q.refKeyID, (p, q) => new
                //{
                //    id = p.id,
                //    pId = p.pId,
                //    name = p.name,
                //    IsOpen = p.IsOpen,
                //    canclick = p.canclick,
                //    priviegeid = p.priviegeid,
                //    Image = p.Image,

                //    @checked = q.Count() > 0,
                //})

                .ToList().Select(p => new zTree.TreeNodeBase
                {
                    id = p.id,
                    pId = p.pId,
                    name = p.name,
                    open = true,
                    color = (p.IsOpen ?? false) ? null : "#A9A9A9",
                    //  icon = Config.LocalHostAuthority + p.Image,
                    data = new Dictionary<string, string> {
                        {"priviegeid",p.priviegeid.ToString() },
                    }
                }));
                return L;
            }, false);

            return View("Tree", LayData);
        }
    }
}