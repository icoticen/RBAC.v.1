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
    public class MENUController : myControllerAuth
    {
        // GET: Authority/PRIVIEGE
        public ActionResult Index()
        {
            return View();
        }

        #region T_SYS_BUTTON

        static Dictionary<String, String> DIC_T_SYS_BUTTON = new Dictionary<string, string> {

                {"ID","ID"},
                {"MenuID","目录ID"},
                {"Name","名称"},
                {"Value","值"},
                {"Image","图片"},
                {"Link","链接"},
                {"Describe","描述"},
                {"SortNo","排序"},
                {"CreateDateTime","创建时间"},
                {"CreateAdminID","创建管理员"},
                {"IsOpen","是否公开"},        };

        public ActionResult T_SYS_BUTTON_List(T_SYS_BUTTON Model, String KeyWord, DateTime? dt1, DateTime? dt2, Int32? PageSize)
        {
            //var Page = new M_Pagination(1, PageSize ?? 10);
            //A.Exec(EF => EF.T_SYS_BUTTON
            //.F_Search(Model, KeyWord, dt1, dt2)
            //.OrderBy(p => p.ID)
            //.Ex_GetPagination(Page), false);

            var LayData = new LayUI.Data("T_SYS_BUTTON", btnTableInsert: false, btnTableView: true, btnTableUpdate: true, btnTableDelete: true);
            if (Model.MenuID > 0)
            {
                LayData.AddButton("新增", Link: "T_SYS_BUTTON_Insert", Param: new Dictionary<string, string> { { "MenuID", Model.MenuID.ToString() } });
                LayData.AddButton("默认新增", Link: "T_SYS_BUTTON_Insert_DefaultGroup", Param: new Dictionary<string, string> { { "MenuID", Model.MenuID.ToString() } });
            }
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            //LayData.AddProperty("ID",  LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("MenuID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
            //LayData.AddProperty("Image",  LayUI.E_Property_Type.@image);
            LayData.AddProperty("Link", LayUI.E_Property_Type.@text);
            //LayData.AddProperty("Describe",  LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            //LayData.AddProperty("CreateDateTime",  LayUI.E_Property_Type.@datetime);
            LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);
            #endregion

            #region Nodes

            #endregion

            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("MenuID", Model.MenuID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Value", Model.Value);
            //LayData.AddModel("Image", Model.Image);
            LayData.AddModel("Link", Model.Link);
            //LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen);


            LayData.AddModel("KeyWord", KeyWord);
            //LayData.AddModel("PageCount", Page.PageCount);
            LayData.AddModel("dt1", dt1.HasValue ? dt1.Ex_ToString("yyyy-MM-dd") : "");
            LayData.AddModel("dt2", dt2.HasValue ? dt2.Ex_ToString("yyyy-MM-dd") : "");

            #endregion

            LayData.Translate(DIC_T_SYS_BUTTON);

            return View("List", LayData);
        }

        public ActionResult Ajax_T_SYS_BUTTON_List(T_SYS_BUTTON Model, String KeyWord, Int32? PageIndex, Int32? PageSize, String SortCode, Boolean? IsAsc, DateTime? dt1, DateTime? dt2)
        {
            SortCode = String.IsNullOrWhiteSpace(SortCode) ? "ID" : SortCode;
            if (PageIndex > 0)
            {
                M_Pagination Page = new M_Pagination() { PageIndex = PageIndex.Value, PageSize = PageSize ?? 10 };
                var iList = A.Exec(EF => EF.T_SYS_BUTTON
                .F_Search(Model, KeyWord, dt1, dt2)
                .Ex_OrderBy(SortCode, IsAsc ?? false)
                .Ex_GetPagination(Page), false);

                if (iList == null) return Content(new { result = -1, page = Page, model = Model }.Ex_ToJson());
                if (iList.Count == 0) return Content(new { result = 2, page = Page, model = Model }.Ex_ToJson());

                var LayData = new LayUI.Data();
                LayData.AddSort(SortCode, IsAsc);

                #region Properties
                LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("MenuID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
                LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
                //LayData.AddProperty("Image", LayUI.E_Property_Type.@image);
                //LayData.AddProperty("Link", LayUI.E_Property_Type.@text);
                //LayData.AddProperty("Describe",  LayUI.E_Property_Type.@textarea);
                LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
                //LayData.AddProperty("CreateDateTime", LayUI.E_Property_Type.@datetime);
                //LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

                #endregion
                #region Model
                LayData.AddModel("PageCount", Page.PageCount);
                LayData.AddModel("RowCount", Page.RowCount);
                LayData.AddModel("PageIndex", Page.PageIndex);
                LayData.AddModel("PageSize", Page.PageSize);
                #endregion
                iList.ForEach(p => LayData.TableData.Add(new LayUI.Row
                {
                    Cells = new Dictionary<string, object>
                    {
                        {"ID",p.ID},
                        {"MenuID",p.MenuID},
                        {"Name",p.Name.Ex_ToString(15, "...")},
                        {"Value",p.Value.Ex_ToString(15, "...")},
                        //{"Image",p.Image},
                        //{"Link",p.Link.Ex_ToString(15, "...")},
                        //{"Describe",p.Describe.Ex_ToString(15, "...")},
                        {"SortNo",p.SortNo},
                        //{"CreateDateTime",p.CreateDateTime.Ex_ToString()},
                        //{"CreateAdminID",p.CreateAdminID},
                        {"IsOpen",p.IsOpen??false},
                    },
                }));

                LayData.Translate(DIC_T_SYS_BUTTON);

                return Content(new
                M_Result
                {
                    result = 1,
                    msg = "",
                    data = LayData,
                }.Ex_ToJson());
            }
            return Content(new { result = 0 }.Ex_ToJson());
        }

        public ActionResult T_SYS_BUTTON_Insert(T_SYS_BUTTON Model)
        {
            var R = new M_Result { msg = "参数错误 部门未指定", data = null, result = 0 };
            if (Model.MenuID <= 0)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {
                Model.CreateDateTime = DateTime.Now;
                Model.CreateAdminID = REQ.UserID;
                 R = A.Insert(Model, CallBack: (EF, _M) =>
                {
                    EF.T_SYS_PRIVIEGE.Add(new T_SYS_PRIVIEGE
                    {
                        CreateAdminID = REQ.UserID,
                        CreateDateTime = DateTime.Now,
                        IsOpen = true,
                        KeyID = _M.ID,
                        Type = (Int32)Config.E_Sys_Priviege_Type.BUTTON,
                    });
                    EF.SaveChanges();
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);
            var MP = A.Model<T_SYS_MENU>(Model.MenuID);
            if (MP == null)
            {
                R.msg = "参数错误 父节点不存在";
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            Model.Value = MP.Value + "/";
            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("MenuID", LayUI.E_Property_Type.hidden);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Image", LayUI.E_Property_Type.@image);
            LayData.AddProperty("Link", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("MenuID", Model.MenuID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Value", Model.Value);
            LayData.AddModel("Image", Model.Image);
            LayData.AddModel("Link", Model.Link);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_BUTTON);
            return View("Save", LayData);
        }
        public ActionResult T_SYS_BUTTON_Insert_DefaultGroup(T_SYS_BUTTON Model,
                bool btnTableInsert = false,
                bool btnTableView = false, bool btnTableUpdate = false, bool btnTableDelete = false,
                bool btnTableInsertSibling = false, bool btnTableInsertChild = false,
                bool btnExcelExport = false,
                bool btnTreeInsertSibling = false, bool btnTreeInsertChild = false,
                bool btnTreeView = false, bool btnTreeUpdate = false, bool btnTreeDelete = false)
        {
            var R = new M_Result { msg = "参数错误 部门未指定", data = null, result = 0 };
            if (Model.MenuID <= 0)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {
                R = A.Exec(EF =>
                {
                    Action<String, String> FADD = (enname, cnname) =>
                     {
                         var M = EF.T_SYS_BUTTON.Add(new T_SYS_BUTTON
                         {
                             CreateAdminID = REQ.UserID,
                             CreateDateTime = DateTime.Now,
                             IsOpen = true,
                             Value = Model.Value + enname,
                             MenuID = Model.MenuID,
                             SortNo = 1,
                             Name = cnname,
                         });
                         EF.SaveChanges();
                         EF.T_SYS_PRIVIEGE.Add(new T_SYS_PRIVIEGE
                         {
                             CreateAdminID = REQ.UserID,
                             CreateDateTime = DateTime.Now,
                             IsOpen = true,
                             KeyID = M.ID,
                             Type = (Int32)Config.E_Sys_Priviege_Type.BUTTON,
                         });
                         EF.SaveChanges();
                     };
                    if (btnTableInsert) FADD("btnTableInsert", "列表新增");
                    if (btnTableView) FADD("btnTableView", "列表查看");
                    if (btnTableUpdate) FADD("btnTableUpdate", "列表编辑");
                    if (btnTableDelete) FADD("btnTableDelete", "列表删除");
                    if (btnTableInsertSibling) FADD("btnTableInsertSibling", "列表新增同级");
                    if (btnTableInsertChild) FADD("btnTableInsertChild", "列表新增子级");
                    if (btnExcelExport) FADD("btnExcelExport", "Excel导出");
                    if (btnTreeInsertSibling) FADD("btnTreeInsertSibling", "树新增同级");
                    if (btnTreeInsertChild) FADD("btnTreeInsertChild", "树新增子级");
                    if (btnTreeView) FADD("btnTreeView", "树查看");
                    if (btnTreeUpdate) FADD("btnTreeUpdate", "树编辑");
                    if (btnTreeDelete) FADD("btnTreeDelete", "树删除");

                    return new M_Result(E_ERRORCODE.操作成功);
                }) ?? new M_Result(E_ERRORCODE.数据库错误);
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);
            var MP = A.Model<T_SYS_MENU>(Model.MenuID);
            if (MP == null)
            {
                R.msg = "参数错误 父节点不存在";
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            Model.Value = MP.Value + "/";
            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("MenuID", LayUI.E_Property_Type.hidden);
            LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
            LayData.AddProperty("btnTableInsert", LayUI.E_Property_Type.@bool, "列表新增");
            LayData.AddProperty("btnTableView", LayUI.E_Property_Type.@bool, "列表查看");
            LayData.AddProperty("btnTableUpdate", LayUI.E_Property_Type.@bool, "列表编辑");
            LayData.AddProperty("btnTableDelete", LayUI.E_Property_Type.@bool, "列表删除");
            LayData.AddProperty("btnTableInsertSibling", LayUI.E_Property_Type.@bool, "列表新增同级");
            LayData.AddProperty("btnTableInsertChild", LayUI.E_Property_Type.@bool, "列表新增子级");
            LayData.AddProperty("btnExcelExport", LayUI.E_Property_Type.@bool, "Excel导出");
            LayData.AddProperty("btnTreeInsertSibling", LayUI.E_Property_Type.@bool, "树新增同级");
            LayData.AddProperty("btnTreeInsertChild", LayUI.E_Property_Type.@bool, "树新增子级");
            LayData.AddProperty("btnTreeView", LayUI.E_Property_Type.@bool, "树查看");
            LayData.AddProperty("btnTreeUpdate", LayUI.E_Property_Type.@bool, "树编辑");
            LayData.AddProperty("btnTreeDelete", LayUI.E_Property_Type.@bool, "树删除");

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("MenuID", Model.MenuID);
            LayData.AddModel("Value", Model.Value);
            LayData.AddModel("btnTableInsert", btnTableInsert);
            LayData.AddModel("btnTableView", btnTableView);
            LayData.AddModel("btnTableUpdate", btnTableUpdate);
            LayData.AddModel("btnTableDelete", btnTableDelete);
            LayData.AddModel("btnTableInsertSibling", btnTableInsertSibling);
            LayData.AddModel("btnTableInsertChild", btnTableInsertChild);
            LayData.AddModel("btnExcelExport", btnExcelExport);
            LayData.AddModel("btnTreeInsertSibling", btnTreeInsertSibling);
            LayData.AddModel("btnTreeInsertChild", btnTreeInsertChild);
            LayData.AddModel("btnTreeView", btnTreeView);
            LayData.AddModel("btnTreeUpdate", btnTreeUpdate);
            LayData.AddModel("btnTreeDelete", btnTreeDelete);

            #endregion
            LayData.Translate(DIC_T_SYS_BUTTON);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_BUTTON_Update(T_SYS_BUTTON Model)
        {
            var M = A.Model<T_SYS_BUTTON>(Model.ID);
            var R = new M_Result { msg = "参数错误 对象为空", data = null, result = 0 };
            if (M == null)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {

                R = A.Update<T_SYS_BUTTON>(Model.ID, _M =>
                {
                    //_M.ID = Model.ID;
                    //_M.MenuID = Model.MenuID;
                    _M.Name = Model.Name;
                    _M.Value = Model.Value;
                    _M.Image = Model.Image;
                    _M.Link = Model.Link;
                    _M.Describe = Model.Describe;
                    _M.SortNo = Model.SortNo;
                    //_M.CreateDateTime = Model.CreateDateTime;
                    //_M.CreateAdminID = Model.CreateAdminID;
                    _M.IsOpen = Model.IsOpen;
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }

            Model = M;

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            //LayData.AddProperty("MenuID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Image", LayUI.E_Property_Type.@image);
            LayData.AddProperty("Link", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            //LayData.AddModel("MenuID", Model.MenuID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Value", Model.Value);
            LayData.AddModel("Image", Model.Image);
            LayData.AddModel("Link", Model.Link);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_BUTTON);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_BUTTON_View(Int32 ID = 0)
        {
            var Model = A.Model<T_SYS_BUTTON>(ID);
            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("MenuID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Image", LayUI.E_Property_Type.@image);
            LayData.AddProperty("Link", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("CreateDateTime", LayUI.E_Property_Type.@datetime);
            LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("MenuID", Model.MenuID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Value", Model.Value);
            LayData.AddModel("Image", Model.Image);
            LayData.AddModel("Link", Model.Link);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_BUTTON);
            return View("View", LayData);
        }

        public ActionResult T_SYS_BUTTON_Delete(Int32 ID = 0)
        {
            var R = A.Delete<T_SYS_BUTTON>(ID, CallBack: (EF, _M) =>
            {
                EF.T_SYS_PRIVIEGE.RemoveRange(EF.T_SYS_PRIVIEGE.Where(p => p.KeyID == _M.ID && p.Type == (Int32)Config.E_Sys_Priviege_Type.BUTTON));
                EF.SaveChanges();
            });
            Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
            return Content("");
        }
        #endregion
        #region T_SYS_MENU

        static Dictionary<String, String> DIC_T_SYS_MENU = new Dictionary<string, string> {
            {"ID","ID"},
            {"Name","名称"},
            {"Value","值"},
            {"Image","图片"},
            {"Link","链接"},
            {"Describe","描述"},
            {"ParentID","父ID"},
            {"Level","层级"},
            {"Root","所属"},
            {"SortNo","排序"},
            {"CreateDateTime","创建时间"},
            {"CreateAdminID","创建管理员"},
            {"IsOpen","是否公开"},        };

        public ActionResult T_SYS_MENU_List(T_SYS_MENU Model, String KeyWord, DateTime? dt1, DateTime? dt2, Int32? PageSize)
        {


            var LayData = new LayUI.Data("T_SYS_MENU", btnTableInsertSibling: false, btnTableInsertChild: true, btnTableView: true, btnTableUpdate: true, btnTableDelete: true)
                .AddButton("设置按钮", Site: LayUI.E_Button_Site.item, Link: "T_SYS_BUTTON_List", Param: new Dictionary<string, string> { { "MenuID", "{{=row.Cells.ID}}" } });

            //LayData.AddSort(Code, IsAsc);

            #region Properties
            //LayData.AddProperty("ID",  LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
            //LayData.AddProperty("Image",  LayUI.E_Property_Type.@image);
            LayData.AddProperty("Link", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Level", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Root", LayUI.E_Property_Type.@text);
            LayData.AddProperty("ParentID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            //LayData.AddProperty("Describe",  LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            //LayData.AddProperty("CreateDateTime",  LayUI.E_Property_Type.@datetime);
            LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);
            #endregion

            #region Nodes

            #endregion

            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("Value", Model.Value);
            //LayData.AddModel("Image", Model.Image);
            LayData.AddModel("Link", Model.Link);
            LayData.AddModel("Level", Model.Level);
            LayData.AddModel("Root", Model.Root);
            LayData.AddModel("ParentID", Model.ParentID);
            LayData.AddModel("Name", Model.Name);
            //LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen);


            LayData.AddModel("KeyWord", KeyWord);
            //LayData.AddModel("PageCount", Page.PageCount);
            LayData.AddModel("dt1", dt1.HasValue ? dt1.Ex_ToString("yyyy-MM-dd") : "");
            LayData.AddModel("dt2", dt2.HasValue ? dt2.Ex_ToString("yyyy-MM-dd") : "");

            #endregion

            LayData.Translate(DIC_T_SYS_MENU);

            return View("List", LayData);
        }

        public ActionResult T_SYS_MENU_Tree()
        {
            var btnAddSetting = new LayUI.Button("设置按钮", Link: "LayUI_Layer_OpeniFrame(layer, \"T_SYS_BUTTON_List?MenuID=\"+selectedtreenode.id,'设置按钮')", ActionType: LayUI.E_Button_Action.function);
            var LayData = new LayUI.Data("T_SYS_MENU", btnTreeInsertSibling: true, btnTreeInsertChild: true, btnTreeView: true, btnTreeUpdate: true, btnTreeDelete: true);
            LayData.AddButton("默认新增", Link: "LayUI_Layer_OpeniFrame(layer, \"T_SYS_MENU_Insert_DefaultGroup?ParentID=\"+selectedtreenode.id,'默认新增')", ActionType: LayUI.E_Button_Action.function);

            LayData.AddButtons(btnAddSetting);

            LayData.zTreeData.TreeNodes = A.Exec(EF => TreeList<T_SYS_MENU>(EF)).ToList();
            LayData.zTreeData.onDoubleClick = btnAddSetting.ClickScript;

            return View("Tree", LayData);
        }

        public ActionResult Ajax_T_SYS_MENU_List(T_SYS_MENU Model, String KeyWord, Int32? PageIndex, Int32? PageSize, String SortCode, Boolean? IsAsc, DateTime? dt1, DateTime? dt2)
        {
            SortCode = String.IsNullOrWhiteSpace(SortCode) ? "ID" : SortCode;
            if (PageIndex > 0)
            {
                M_Pagination Page = new M_Pagination() { PageIndex = PageIndex.Value, PageSize = PageSize ?? 10 };
                var iList = A.Exec(EF => EF.T_SYS_MENU
                .F_Search(Model, KeyWord, dt1, dt2)
                .Ex_OrderBy(SortCode, IsAsc ?? false)
                .Ex_GetPagination(Page), false);

                if (iList == null) return Content(new { result = -1, page = Page, model = Model }.Ex_ToJson());
                if (iList.Count == 0) return Content(new { result = 2, page = Page, model = Model }.Ex_ToJson());

                var LayData = new LayUI.Data();
                LayData.AddSort(SortCode, IsAsc);

                #region Properties
                LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("ParentID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
                LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
                //LayData.AddProperty("Image", LayUI.E_Property_Type.@image);
                //LayData.AddProperty("Link", LayUI.E_Property_Type.@text);
                LayData.AddProperty("Level", LayUI.E_Property_Type.@text);
                LayData.AddProperty("Root", LayUI.E_Property_Type.@text);
                //LayData.AddProperty("Describe",  LayUI.E_Property_Type.@textarea);
                LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
                //LayData.AddProperty("CreateDateTime", LayUI.E_Property_Type.@datetime);
                //LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

                #endregion
                #region Model
                LayData.AddModel("PageCount", Page.PageCount);
                LayData.AddModel("RowCount", Page.RowCount);
                LayData.AddModel("PageIndex", Page.PageIndex);
                LayData.AddModel("PageSize", Page.PageSize);
                #endregion
                iList.ForEach(p => LayData.TableData.Add(new LayUI.Row
                {
                    Cells = new Dictionary<string, object>
                    {
                        {"ID",p.ID},
                        {"Value",p.Value.Ex_ToString(15, "...")},
                        //{"Image",p.Image},
                        //{"Link",p.Link.Ex_ToString(15, "...")},
                        {"Level",p.Level},
                        {"Root",p.Root.Ex_ToString(15, "...")},
                        {"ParentID",p.ParentID},
                        {"Name",p.Name.Ex_ToString(15, "...")},
                        //{"Describe",p.Describe.Ex_ToString(15, "...")},
                        {"SortNo",p.SortNo},
                        {"CreateDateTime",p.CreateDateTime.Ex_ToString()},
                        {"CreateAdminID",p.CreateAdminID},
                        {"IsOpen",p.IsOpen??false},
                    },
                    Buttons = new List<LayUI.Button> {
                        p.ParentID>0?new LayUI.Button("新增同级", Site: LayUI.E_Button_Site.item, Link: "T_SYS_MENU_Insert", Param: new Dictionary<string, string> { { "ParentID",p.ParentID.ToString()} }):null,
                    }
                }));

                LayData.Translate(DIC_T_SYS_MENU);

                return Content(new
                M_Result
                {
                    result = 1,
                    msg = "",
                    data = LayData,
                }.Ex_ToJson());
            }
            return Content(new { result = 0 }.Ex_ToJson());
        }

        public ActionResult T_SYS_MENU_Insert(T_SYS_MENU Model)
        {
            var R = new M_Result { msg = "参数错误 父节点未指定", data = null, result = 0 };
            if (Model.ParentID <= 0)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {
                Model.CreateDateTime = DateTime.Now;
                Model.CreateAdminID = REQ.UserID;
                R = A.Insert(Model, CallBack: (EF, _M) =>
                {
                    EF.T_SYS_PRIVIEGE.Add(new T_SYS_PRIVIEGE
                    {
                        CreateAdminID = REQ.UserID,
                        CreateDateTime = DateTime.Now,
                        IsOpen = true,
                        KeyID = _M.ID,
                        Type = (Int32)Config.E_Sys_Priviege_Type.MENU,
                    });
                    EF.SaveChanges();
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            var MP = A.Model<T_SYS_MENU>(Model.ParentID);
            if (MP == null)
            {
                R.msg = "参数错误 父节点不存在";
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            Model.Root = MP.Root;
            Model.Level = MP.Level + 1;
            Model.Value = MP.Value + @"/";
            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("Image", LayUI.E_Property_Type.@image);
            LayData.AddProperty("Link", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Level", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Root", LayUI.E_Property_Type.@text);
            LayData.AddProperty("ParentID", LayUI.E_Property_Type.hidden);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("Value", Model.Value);
            LayData.AddModel("Image", Model.Image);
            LayData.AddModel("Link", Model.Link);
            LayData.AddModel("Level", Model.Level);
            LayData.AddModel("Root", Model.Root);
            LayData.AddModel("ParentID", Model.ParentID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_MENU);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_MENU_Update(T_SYS_MENU Model)
        {
            var M = A.Model<T_SYS_MENU>(Model.ID);
            var R = new M_Result { msg = "参数错误 对象为空", data = null, result = 0 };
            if (M == null)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {

                R = A.Update<T_SYS_MENU>(Model.ID, _M =>
                {
                    //_M.ID = Model.ID;
                    _M.Value = Model.Value;
                    _M.Image = Model.Image;
                    _M.Link = Model.Link;
                    _M.Level = Model.Level;
                    _M.Root = Model.Root;
                    //_M.ParentID = Model.ParentID;
                    _M.Name = Model.Name;
                    _M.Describe = Model.Describe;
                    _M.SortNo = Model.SortNo;
                    //_M.CreateDateTime = Model.CreateDateTime;
                    //_M.CreateAdminID = Model.CreateAdminID;
                    _M.IsOpen = Model.IsOpen;
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }

            Model = M;

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("Image", LayUI.E_Property_Type.@image);
            LayData.AddProperty("Link", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Level", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Root", LayUI.E_Property_Type.@text);
            //LayData.AddProperty("ParentID", LayUI.E_Property_Type.hidden);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("Value", Model.Value);
            LayData.AddModel("Image", Model.Image);
            LayData.AddModel("Link", Model.Link);
            LayData.AddModel("Level", Model.Level);
            LayData.AddModel("Root", Model.Root);
            //LayData.AddModel("ParentID", Model.ParentID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_MENU);
            return View("Save", LayData);
        }
        public ActionResult T_SYS_MENU_Insert_DefaultGroup(T_SYS_MENU Model, String TableName, String TableNamecn,
        bool PageTree = false,
        bool PageInsert = false, bool PageUpdate = false, bool PageDelete = false, bool PageView = false)
        {
            var R = new M_Result { msg = "参数错误 父节点未指定", data = null, result = 0 };
            if (Model.ParentID <= 0)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {
                R = A.Exec(EF =>
                {
                    var MList = EF.T_SYS_MENU.Add(new T_SYS_MENU
                    {
                        CreateAdminID = REQ.UserID,
                        CreateDateTime = DateTime.Now,
                        IsOpen = true,
                        Value = Model.Value + TableName + "_List",
                        ParentID = Model.ParentID,
                        SortNo = 1,
                        Name = TableNamecn,
                        Link = Model.Value + TableName + "_List",
                        Root = Model.Root,
                        Level = Model.Level,
                    });
                    EF.SaveChanges();
                    EF.T_SYS_PRIVIEGE.Add(new T_SYS_PRIVIEGE
                    {
                        CreateAdminID = REQ.UserID,
                        CreateDateTime = DateTime.Now,
                        IsOpen = true,
                        KeyID = MList.ID,
                        Type = (Int32)Config.E_Sys_Priviege_Type.MENU,
                    });
                    EF.SaveChanges();


                    Action<String, String, bool> FADD = (enname, cnname, IsChild) =>
                     {
                         var M = EF.T_SYS_MENU.Add(new T_SYS_MENU
                         {
                             CreateAdminID = REQ.UserID,
                             CreateDateTime = DateTime.Now,
                             IsOpen = true,
                             Value = Model.Value + enname,
                             ParentID =IsChild?MList.ID: Model.ParentID,
                             SortNo = 1,
                             Name = cnname,
                             Link = Model.Value + enname,
                             Root = Model.Root,
                             Level = IsChild ? Model.Level + 1 : Model.Level,
                         });
                         EF.SaveChanges();
                         EF.T_SYS_PRIVIEGE.Add(new T_SYS_PRIVIEGE
                         {
                             CreateAdminID = REQ.UserID,
                             CreateDateTime = DateTime.Now,
                             IsOpen = true,
                             KeyID = M.ID,
                             Type = (Int32)Config.E_Sys_Priviege_Type.MENU,
                         });
                         EF.SaveChanges();
                     };
                    if (PageTree) FADD(TableName + "_Tree", TableNamecn + "-树", false);
                    if (PageInsert) FADD(TableName + "_Insert", "新增页面", true);
                    if (PageView) FADD(TableName + "_View", "查看页面", true);
                    if (PageUpdate) FADD(TableName + "_Update", "编辑页面", true);
                    if (PageDelete) FADD(TableName + "_Delete", "删除页面", true);


                    return new M_Result(E_ERRORCODE.操作成功);
                }) ?? new M_Result(E_ERRORCODE.数据库错误);
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);
            var MP = A.Model<T_SYS_MENU>(Model.ParentID);
            if (MP == null)
            {
                R.msg = "参数错误 父节点不存在";
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            Model.Root = MP.Root;
            Model.Level = MP.Level + 1;
            Model.Value = MP.Value + @"/";
            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("ParentID", LayUI.E_Property_Type.hidden);
            LayData.AddProperty("Root", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Level", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
            LayData.AddProperty("TableName", LayUI.E_Property_Type.@text,"表名en");
            LayData.AddProperty("TableNamecn", LayUI.E_Property_Type.@text,"表名cn");
            LayData.AddProperty("PageTree", LayUI.E_Property_Type.@bool, TableNamecn + "-树");
            LayData.AddProperty("PageInsert", LayUI.E_Property_Type.@bool, "新增页面");
            LayData.AddProperty("PageView", LayUI.E_Property_Type.@bool, "查看页面");
            LayData.AddProperty("PageUpdate", LayUI.E_Property_Type.@bool, "编辑页面");
            LayData.AddProperty("PageDelete", LayUI.E_Property_Type.@bool, "删除页面");

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("ParentID", Model.ParentID);
            LayData.AddModel("Root", Model.Root);
            LayData.AddModel("Level", Model.Level);
            LayData.AddModel("Value", Model.Value);
            LayData.AddModel("TableName", TableName);
            LayData.AddModel("TableNamecn", TableNamecn);
            LayData.AddModel("PageTree", PageTree);
            LayData.AddModel("PageInsert", PageInsert);
            LayData.AddModel("PageView", PageView);
            LayData.AddModel("PageUpdate", PageUpdate);
            LayData.AddModel("PageDelete", PageDelete);

            #endregion
            LayData.Translate(DIC_T_SYS_BUTTON);
            return View("Save", LayData);
        }
        public ActionResult T_SYS_MENU_View(Int32 ID = 0)
        {
            var Model = A.Model<T_SYS_MENU>(ID);
            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Value", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Image", LayUI.E_Property_Type.@image);
            LayData.AddProperty("Link", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Level", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Root", LayUI.E_Property_Type.@text);
            LayData.AddProperty("ParentID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("CreateDateTime", LayUI.E_Property_Type.@datetime);
            LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("Value", Model.Value);
            LayData.AddModel("Image", Model.Image);
            LayData.AddModel("Link", Model.Link);
            LayData.AddModel("Level", Model.Level);
            LayData.AddModel("Root", Model.Root);
            LayData.AddModel("ParentID", Model.ParentID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_MENU);
            return View("View", LayData);
        }

        public ActionResult T_SYS_MENU_Delete(Int32 ID = 0)
        {
            var R = A.Delete<T_SYS_MENU>(ID, CallBack: (EF, _M) =>
            {
                EF.T_SYS_PRIVIEGE.RemoveRange(EF.T_SYS_PRIVIEGE.Where(p => p.KeyID == _M.ID && p.Type == (Int32)Config.E_Sys_Priviege_Type.MENU));
                EF.SaveChanges();
            });
            Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
            return Content("");
        }
        #endregion

    }
}