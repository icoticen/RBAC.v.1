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
    public class DEPARTMENTController : myControllerAuth
    {
        // GET: Authority/DEPARTMENT
        public ActionResult Index()
        {
            return View();
        }


        #region T_SYS_DEPARTMENT

        static Dictionary<String, String> DIC_T_SYS_DEPARTMENT = new Dictionary<string, string> {

        {"ID","ID"},
        {"ParentID","父ID"},
        {"Name","名称"},
        {"Describe","描述"},
        {"SortNo","排序"},
        {"CreateDateTime","创建时间"},
        {"CreateAdminID","创建管理员"},
        {"IsOpen","公开"},        };

        public ActionResult T_SYS_DEPARTMENT_List(T_SYS_DEPARTMENT Model, String KeyWord, DateTime? dt1, DateTime? dt2, Int32? PageSize)
        {
         

            var LayData = new LayUI.Data("T_SYS_DEPARTMENT", btnTableInsertSibling: false, btnTableInsertChild: true, btnTableView: true, btnTableUpdate: true, btnTableDelete: true)
                .AddButton("设置职位", Site: LayUI.E_Button_Site.item, Link: "T_SYS_DEPARTMENT_POSITION_List", Param: new Dictionary<string, string> { { "DepartmentID", "{{=row.Cells.ID}}" } });
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
            LayData.AddModel("dt1", dt1.HasValue ? dt1.Ex_ToString("yyyy-MM-dd") : "");
            LayData.AddModel("dt2", dt2.HasValue ? dt2.Ex_ToString("yyyy-MM-dd") : "");

            #endregion

            LayData.Translate(DIC_T_SYS_DEPARTMENT);

            return View("List", LayData);
        }

        public ActionResult T_SYS_DEPARTMENT_Tree()
        {
            var btnAddSetting = new LayUI.Button("设置职位", Link: "LayUI_Layer_OpeniFrame(layer, \"T_SYS_DEPARTMENT_POSITION_List?DepartmentID=\"+selectedtreenode.id,'设置职位')", ActionType: LayUI.E_Button_Action.function);
            var LayData = new LayUI.Data("T_SYS_DEPARTMENT", btnTreeInsertSibling: true, btnTreeInsertChild: true, btnTreeView: true, btnTreeUpdate: true, btnTreeDelete: true);
            LayData.AddButtons(btnAddSetting);

            LayData.zTreeData.TreeNodes = A.Exec(EF => TreeList<T_SYS_DEPARTMENT>(EF)).ToList();
            LayData.zTreeData.onDoubleClick = btnAddSetting.ClickScript;

            return View("Tree", LayData);
        }

        public ActionResult Ajax_T_SYS_DEPARTMENT_List(T_SYS_DEPARTMENT Model, String KeyWord, Int32? PageIndex, Int32? PageSize, String SortCode, Boolean? IsAsc, DateTime? dt1, DateTime? dt2)
        {
            SortCode = String.IsNullOrWhiteSpace(SortCode) ? "ID" : SortCode;
            if (PageIndex > 0)
            {
                M_Pagination Page = new M_Pagination() { PageIndex = PageIndex.Value, PageSize = PageSize ?? 10 };
                var iList = A.Exec(EF => EF.T_SYS_DEPARTMENT
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
                        {"ParentID",p.ParentID},
                        {"Name",p.Name.Ex_ToString(15, "...")},
                        {"Describe",p.Describe.Ex_ToString(15, "...")},
                        {"SortNo",p.SortNo},
                        //{"CreateDateTime",p.CreateDateTime.Ex_ToString()},
                        //{"CreateAdminID",p.CreateAdminID},
                        {"IsOpen",p.IsOpen??false},
                    },
                    Buttons = new List<LayUI.Button> {
                        p.ParentID>0?new LayUI.Button("新增同级", Site: LayUI.E_Button_Site.item, Link: "T_SYS_GROUP_Insert", Param: new Dictionary<string, string> { { "ParentID",p.ParentID.ToString()} }):null,
                    }
                }));

                LayData.Translate(DIC_T_SYS_DEPARTMENT);

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

        public ActionResult T_SYS_DEPARTMENT_Insert(T_SYS_DEPARTMENT Model)
        {
            var R = new M_Result { msg = "参数错误 父节点未指定", data = null, result = 0 };
            if (Model.ParentID <= 0)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {
                Model.CreateDateTime = DateTime.Now;
                Model.CreateAdminID = REQ.UserID;
                R = A.Insert(Model);
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }

            var MP = A.Model<T_SYS_DEPARTMENT>(Model.ParentID);
            if (MP == null)
            {
                R.msg = "参数错误 父节点不存在";
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("ParentID", LayUI.E_Property_Type.hidden);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
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
            LayData.Translate(DIC_T_SYS_DEPARTMENT);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_DEPARTMENT_Update(T_SYS_DEPARTMENT Model)
        {
            var M = A.Model<T_SYS_DEPARTMENT>(Model.ID);
            var R = new M_Result { msg = "参数错误 对象为空", data = null, result = 0 };
            if (M == null)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {

                R = A.Update<T_SYS_DEPARTMENT>(Model.ID, _M =>
                {
                    //_M.ID = Model.ID;
                    //_M.ParentID = Model.ParentID;
                    _M.Name = Model.Name;
                    _M.Describe = Model.Describe;
                    _M.SortNo = Model.SortNo;
                    //_M.CreateDateTime = Model.CreateDateTime;
                    //_M.CreateAdminID = Model.CreateAdminID;
                    _M.IsOpen = Model.IsOpen;
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }

            Model = M;

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            //LayData.AddProperty("ParentID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Describe", LayUI.E_Property_Type.@textarea);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

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

            #endregion
            LayData.Translate(DIC_T_SYS_DEPARTMENT);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_DEPARTMENT_View(Int32 ID = 0)
        {
            var Model = A.Model<T_SYS_DEPARTMENT>(ID);
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
            LayData.Translate(DIC_T_SYS_DEPARTMENT);
            return View("View", LayData);
        }

        public ActionResult T_SYS_DEPARTMENT_Delete(Int32 ID = 0)
        {
            var R = A.Delete<T_SYS_DEPARTMENT>(ID);
            Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
            return Content("");
        }
        #endregion
        #region T_SYS_DEPARTMENT_POSITION

        static Dictionary<String, String> DIC_T_SYS_DEPARTMENT_POSITION = new Dictionary<string, string> {

        {"ID","ID"},
        {"DepartmentID","部门ID"},
        {"Name","名称"},
        {"Level","等级"},
        {"SortNo","排序"},
        {"CreateDateTime","创建时间"},
        {"CreateAdminID","创建管理员"},
        {"IsOpen","公开"},        };

        public ActionResult T_SYS_DEPARTMENT_POSITION_List(T_SYS_DEPARTMENT_POSITION Model, String KeyWord, DateTime? dt1, DateTime? dt2, Int32? PageSize)
        {

            var LayData = new LayUI.Data("T_SYS_DEPARTMENT_POSITION", btnTableView: true, btnTableUpdate: true, btnTableDelete: true);
            if (Model.DepartmentID > 0)
                LayData.AddButton("新增", Link: "T_SYS_DEPARTMENT_POSITION_Insert", Param: new Dictionary<string, string> { { "DepartmentID", Model.DepartmentID.ToString() } });
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            //LayData.AddProperty("ID",  LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("DepartmentID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Level", LayUI.E_Property_Type.@text);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            //LayData.AddProperty("CreateDateTime",  LayUI.E_Property_Type.@datetime);
            LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);
            #endregion

            #region Nodes

            #endregion

            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("DepartmentID", Model.DepartmentID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Level", Model.Level);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen);


            LayData.AddModel("KeyWord", KeyWord);         
            LayData.AddModel("dt1", dt1.HasValue ? dt1.Ex_ToString("yyyy-MM-dd") : "");
            LayData.AddModel("dt2", dt2.HasValue ? dt2.Ex_ToString("yyyy-MM-dd") : "");

            #endregion

            LayData.Translate(DIC_T_SYS_DEPARTMENT_POSITION);

            return View("List", LayData);
        }


        public ActionResult Ajax_T_SYS_DEPARTMENT_POSITION_List(T_SYS_DEPARTMENT_POSITION Model, String KeyWord, Int32? PageIndex, Int32? PageSize, String SortCode, Boolean? IsAsc, DateTime? dt1, DateTime? dt2)
        {
            SortCode = String.IsNullOrWhiteSpace(SortCode) ? "ID" : SortCode;
            if (PageIndex > 0)
            {
                M_Pagination Page = new M_Pagination() { PageIndex = PageIndex.Value, PageSize = PageSize ?? 10 };
                var iList = A.Exec(EF => EF.T_SYS_DEPARTMENT_POSITION
                .F_Search(Model, KeyWord, dt1, dt2)
                .Ex_OrderBy(SortCode, IsAsc ?? false)
                .Ex_GetPagination(Page), false);

                if (iList == null) return Content(new { result = -1, page = Page, model = Model }.Ex_ToJson());
                if (iList.Count == 0) return Content(new { result = 2, page = Page, model = Model }.Ex_ToJson());

                var LayData = new LayUI.Data();
                LayData.AddSort(SortCode, IsAsc);

                #region Properties
                LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("DepartmentID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
                LayData.AddProperty("Level", LayUI.E_Property_Type.@text);
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
                        {"DepartmentID",p.DepartmentID},
                        {"Name",p.Name.Ex_ToString(15, "...")},
                        {"Level",p.Level},
                        {"SortNo",p.SortNo},
                        //{"CreateDateTime",p.CreateDateTime.Ex_ToString()},
                        //{"CreateAdminID",p.CreateAdminID},
                        {"IsOpen",p.IsOpen??false},
                    },
                }));

                LayData.Translate(DIC_T_SYS_DEPARTMENT_POSITION);

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

        public ActionResult T_SYS_DEPARTMENT_POSITION_Insert(T_SYS_DEPARTMENT_POSITION Model)
        {
            var R = new M_Result { msg = "参数错误 部门未指定", data = null, result = 0 };
            if (Model.DepartmentID <= 0)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {
                Model.CreateDateTime = DateTime.Now;
                Model.CreateAdminID = REQ.UserID;
                R = A.Insert(Model);
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("DepartmentID", LayUI.E_Property_Type.hidden);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Level", LayUI.E_Property_Type.@text);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("DepartmentID", Model.DepartmentID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Level", Model.Level);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_DEPARTMENT_POSITION);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_DEPARTMENT_POSITION_Update(T_SYS_DEPARTMENT_POSITION Model)
        {
            var M = A.Model<T_SYS_DEPARTMENT_POSITION>(Model.ID);
            var R = new M_Result { msg = "参数错误 对象为空", data = null, result = 0 };
            if (M == null)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {

                R = A.Update<T_SYS_DEPARTMENT_POSITION>(Model.ID, _M =>
                {
                    //_M.ID = Model.ID;
                    //_M.DepartmentID = Model.DepartmentID;
                    _M.Name = Model.Name;
                    _M.Level = Model.Level;
                    _M.SortNo = Model.SortNo;
                    //_M.CreateDateTime = Model.CreateDateTime;
                    //_M.CreateAdminID = Model.CreateAdminID;
                    _M.IsOpen = Model.IsOpen;
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }

            Model = M;

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            //LayData.AddProperty("DepartmentID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Level", LayUI.E_Property_Type.@text);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            //LayData.AddModel("DepartmentID", Model.DepartmentID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Level", Model.Level);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_DEPARTMENT_POSITION);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_DEPARTMENT_POSITION_View(Int32 ID = 0)
        {
            var Model = A.Model<T_SYS_DEPARTMENT_POSITION>(ID);
            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("DepartmentID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Level", LayUI.E_Property_Type.@text);
            LayData.AddProperty("SortNo", LayUI.E_Property_Type.@text);
            LayData.AddProperty("CreateDateTime", LayUI.E_Property_Type.@datetime);
            LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("DepartmentID", Model.DepartmentID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("Level", Model.Level);
            LayData.AddModel("SortNo", Model.SortNo);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_DEPARTMENT_POSITION);
            return View("View", LayData);
        }

        public ActionResult T_SYS_DEPARTMENT_POSITION_Delete(Int32 ID = 0)
        {
            var R = A.Delete<T_SYS_DEPARTMENT_POSITION>(ID);
            Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
            return Content("");
        }

        #endregion
    }
}