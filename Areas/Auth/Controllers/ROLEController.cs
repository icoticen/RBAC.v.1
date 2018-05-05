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
    public class ROLEController : myControllerAuth
    {
        // GET: Authority/ROLE
        public ActionResult Index()
        {
            return View();
        }
        #region T_SYS_GROUP

        static Dictionary<String, String> DIC_T_SYS_GROUP = new Dictionary<string, string> {

        {"ID","ID"},
        {"ParentID","父ID"},
        {"Name","名称"},
        {"Describe","描述"},
        {"SortNo","排序"},
        {"CreateDateTime","创建时间"},
        {"CreateAdminID","创建管理员"},
        {"IsOpen","公开"},       };

        public ActionResult T_SYS_GROUP_List(T_SYS_GROUP Model, String KeyWord, DateTime? dt1, DateTime? dt2, Int32? PageSize)
        {

            var LayData = new LayUI.Data("T_SYS_GROUP", btnTableInsertSibling: false, btnTableInsertChild: true, btnTableView: true, btnTableUpdate: true, btnTableDelete: true);
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            //LayData.AddProperty("ID",  LayUI.E_Property_Type.@hidden);
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

            LayData.Translate(DIC_T_SYS_GROUP);

            return View("List", LayData);
        }

        public ActionResult T_SYS_GROUP_Tree()
        {
            //var btnAddSetting = new Models.Static.LayUI.Button("设置职位", Link: "LayUI_Layer_OpeniFrame(layer, \"T_SYS_DEPARTMENT_POSITION_List?DepartmentID=\"+selectedtreenode.id,'设置职位')", ActionType: LayUI.E_Button_Action.function);
            var LayData = new LayUI.Data("T_SYS_GROUP", btnTreeInsertSibling: true, btnTreeInsertChild: true, btnTreeView: true, btnTreeUpdate: true, btnTreeDelete: true);

            LayData.zTreeData.TreeNodes = A.Exec(EF => TreeList<T_SYS_GROUP>(EF)).ToList();
            //LayData.zTreeData.onDoubleClick = btnAddSetting;

            return View("Tree", LayData);
        }

        public ActionResult Ajax_T_SYS_GROUP_List(T_SYS_GROUP Model, String KeyWord, Int32? PageIndex, Int32? PageSize, String SortCode, Boolean? IsAsc, DateTime? dt1, DateTime? dt2)
        {
            SortCode = String.IsNullOrWhiteSpace(SortCode) ? "ID" : SortCode;
            if (PageIndex > 0)
            {
                M_Pagination Page = new M_Pagination() { PageIndex = PageIndex.Value, PageSize = PageSize ?? 10 };
                var iList = A.Exec(EF => EF.T_SYS_GROUP
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
                //LayData.AddProperty("Describe",  LayUI.E_Property_Type.@textarea);
                LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
                LayData.AddProperty("CreateDateTime", LayUI.E_Property_Type.@datetime);
                LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
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
                        {"ParentID",p.ParentID},
                        {"Name",p.Name.Ex_ToString(15, "...")},
                        {"Describe",p.Describe.Ex_ToString(15, "...")},
                        {"SortNo",p.SortNo},
                        {"CreateDateTime",p.CreateDateTime.Ex_ToString()},
                        {"CreateAdminID",p.CreateAdminID},
                        {"IsOpen",p.IsOpen??false},
                    },
                    Buttons = new List<LayUI.Button> {
                        p.ParentID>0?new LayUI.Button("新增同级", Site: LayUI.E_Button_Site.item, Link: "T_SYS_GROUP_Insert", Param: new Dictionary<string, string> { { "ParentID",p.ParentID.ToString()} }):null,
                    }
                }));

                LayData.Translate(DIC_T_SYS_GROUP);

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

        public ActionResult T_SYS_GROUP_Insert(T_SYS_GROUP Model, String refRoles)
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
                    refInsert<T_SYS_GROUP_refROLE>(EF, refRoles, _M.ID);
                    EF.SaveChanges();
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }

            var MP = A.Model<T_SYS_GROUP>(Model.ParentID);
            if (MP == null)
            {
                R.msg = "参数错误 父节点不存在";
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);
            var E_Roles = A.Exec(EF => EF.T_SYS_ROLE.Select(p => new
            {
                p.Name,
                p.ID
            }).ToList())
            .Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString(),
            }).ToList();
            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("ParentID", LayUI.E_Property_Type.hidden);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);
            LayData.AddProperty("refRoles", LayUI.E_Property_Type.list);

            #endregion

            #region Nodes
            LayData.AddNode("refRoles", E_Roles);
            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("ParentID", Model.ParentID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);
            LayData.AddModel("refRoles", ",");

            #endregion
            LayData.Translate(DIC_T_SYS_GROUP);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_GROUP_Update(T_SYS_GROUP Model, String refRoles)
        {
            var M = A.Model<T_SYS_GROUP>(Model.ID);
            var R = new M_Result { msg = "参数错误 对象为空", data = null, result = 0 };
            if (M == null)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {

                R = A.Update<T_SYS_GROUP>(Model.ID, _M =>
                {
                    //_M.ID = Model.ID;
                    //_M.ParentID = Model.ParentID;
                    _M.Name = Model.Name;
                    _M.Describe = Model.Describe;
                    _M.SortNo = Model.SortNo;
                    //_M.CreateDateTime = Model.CreateDateTime;
                    //_M.CreateAdminID = Model.CreateAdminID;
                    _M.IsOpen = Model.IsOpen;
                }, CallBack: (EF, _M) =>
                {
                    refUpdate<T_SYS_GROUP_refROLE>(EF, refRoles, _M.ID);
                    EF.SaveChanges();
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }

            Model = M;

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            var E_Roles = A.Exec(EF => EF.T_SYS_ROLE.Select(p => new
            {
                p.Name,
                p.ID,
                Selected = EF.T_SYS_GROUP_refROLE.Any(r => r.KeyID == Model.ID && r.refKeyID == p.ID && !(r.HasCancle ?? false))
            }).ToList())
            .Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString(),
                Selected = p.Selected,
            }).ToList();
            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            //LayData.AddProperty("ParentID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);
            LayData.AddProperty("refRoles", LayUI.E_Property_Type.list);

            #endregion

            #region Nodes
            LayData.AddNode("refRoles", E_Roles);

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            //LayData.AddModel("ParentID", Model.ParentID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);
            LayData.AddModel("refRoles", string.Join(",", E_Roles.Where(p => p.Selected).Select(p => p.Value)));

            #endregion
            LayData.Translate(DIC_T_SYS_GROUP);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_GROUP_View(Int32 ID = 0)
        {
            var Model = A.Model<T_SYS_GROUP>(ID);
            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
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
            LayData.AddModel("ParentID", Model.ParentID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_GROUP);
            return View("View", LayData);
        }

        public ActionResult T_SYS_GROUP_Delete(Int32 ID = 0)
        {
            var R = A.Delete<T_SYS_GROUP>(ID);
            Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
            return Content("");
        }
        #endregion
        #region T_SYS_ROLE

        static Dictionary<String, String> DIC_T_SYS_ROLE = new Dictionary<string, string> {

        {"ID","ID"},
        {"Name","名称"},
        {"Describe","描述"},
        {"SortNo","排序"},
        {"CreateDateTime","创建时间"},
        {"CreateAdminID","创建管理员"},
        {"IsOpen","公开"},        };

        public ActionResult T_SYS_ROLE_List(T_SYS_ROLE Model, String KeyWord, DateTime? dt1, DateTime? dt2, Int32? PageSize)
        {
            var LayData = new LayUI.Data("T_SYS_ROLE", btnTableInsert: true, btnTableView: true, btnTableUpdate: true, btnTableDelete: true)
                .AddButton("设置权限", Site: LayUI.E_Button_Site.item, Link: "T_SYS_ROLE_SETPRIVIEGE", Param: new Dictionary<string, string> { { "ROLEID", "{{=row.Cells.ID}}" } });
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            //LayData.AddProperty("ID",  LayUI.E_Property_Type.@hidden);
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

            LayData.Translate(DIC_T_SYS_ROLE);

            return View("List", LayData);
        }


        public ActionResult Ajax_T_SYS_ROLE_List(T_SYS_ROLE Model, String KeyWord, Int32? PageIndex, Int32? PageSize, String SortCode, Boolean? IsAsc, DateTime? dt1, DateTime? dt2)
        {
            SortCode = String.IsNullOrWhiteSpace(SortCode) ? "ID" : SortCode;
            if (PageIndex > 0)
            {
                M_Pagination Page = new M_Pagination() { PageIndex = PageIndex.Value, PageSize = PageSize ?? 10 };
                var iList = A.Exec(EF => EF.T_SYS_ROLE
                .F_Search(Model, KeyWord, dt1, dt2)
                .Ex_OrderBy(SortCode, IsAsc ?? false)
                .Ex_GetPagination(Page), false);

                if (iList == null) return Content(new { result = -1, page = Page, model = Model }.Ex_ToJson());
                if (iList.Count == 0) return Content(new { result = 2, page = Page, model = Model }.Ex_ToJson());

                var LayData = new LayUI.Data();
                LayData.AddSort(SortCode, IsAsc);

                #region Properties
                LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
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
                        {"Name",p.Name.Ex_ToString(15, "...")},
                        {"Describe",p.Describe.Ex_ToString(15, "...")},
                        {"SortNo",p.SortNo},
                        //{"CreateDateTime",p.CreateDateTime.Ex_ToString()},
                        //{"CreateAdminID",p.CreateAdminID},
                        {"IsOpen",p.IsOpen??false},
                    },
                }));

                LayData.Translate(DIC_T_SYS_ROLE);

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

        public ActionResult T_SYS_ROLE_Insert(T_SYS_ROLE Model)
        {
            if (Request.RequestType == "POST")
            {
                Model.CreateDateTime = DateTime.Now;
                Model.CreateAdminID = REQ.UserID;
                var R = A.Insert(Model);
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_ROLE);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_ROLE_Update(T_SYS_ROLE Model)
        {
            var M = A.Model<T_SYS_ROLE>(Model.ID);
            var R = new M_Result { msg = "参数错误 对象为空", data = null, result = 0 };
            if (M == null)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {

                R = A.Update<T_SYS_ROLE>(Model.ID, _M =>
                {
                    //_M.ID = Model.ID;
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
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_ROLE);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_ROLE_View(Int32 ID = 0)
        {
            var Model = A.Model<T_SYS_ROLE>(ID);
            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
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
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Describe", Model.Describe);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_ROLE);
            return View("View", LayData);
        }

        public ActionResult T_SYS_ROLE_Delete(Int32 ID = 0)
        {
            var R = A.Delete<T_SYS_ROLE>(ID);
            Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
            return Content("");
        }
        #endregion


        public ActionResult T_SYS_ROLE_SETPRIVIEGE(Int32 ROLEID = 0, String priviegeid = "")
        {
            var M = A.Model<T_SYS_ROLE>(ROLEID);
            var R = new M_Result { msg = "参数错误 对象为空", data = null, result = 0 };
            if (M == null)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {
                R = A.Exec(EF =>
                {
                    refUpdate<T_SYS_ROLE_refPRIVIEGE>(EF, priviegeid, ROLEID);
                    EF.SaveChanges();
                    return new M_Result(E_ERRORCODE.操作成功);
                }) ?? new M_Result(E_ERRORCODE.数据库错误);
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ");</script>");
                return Content("");
            }
            var LayData = new LayUI.Data();

            LayData.AddProperty("RoleID", LayUI.E_Property_Type.hidden);
            LayData.AddProperty("priviegeid", LayUI.E_Property_Type.ztreeextra);

            LayData.Model.Add("RoleID", ROLEID);
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
                .GroupJoin(EF.T_SYS_ROLE_refPRIVIEGE.Where(p => p.KeyID == ROLEID && !(p.HasCancle ?? false)), p => p.priviegeid, q => q.refKeyID, (p, q) => new
                {
                    id = p.id,
                    pId = p.pId,
                    name = p.name,
                    IsOpen = p.IsOpen ?? false,
                    priviegeid = p.priviegeid,
                    Image = p.Image,

                    @checked = q.Count() > 0,
                })

                .ToList().Select(p => new zTree.TreeNodeBase
                {
                    id = p.id,
                    pId = p.pId,
                    name = p.name,
                    open = true,
                    color = p.IsOpen ? null : "#A9A9A9",
                    @checked = p.@checked,
                    data = new Dictionary<string, string> {
                        {"priviegeid",p.priviegeid.ToString() },
                    }
                }));
                return L;
            }, false);

            return View("Choice", LayData);
        }
    }
}