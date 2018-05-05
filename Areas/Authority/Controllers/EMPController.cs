using IKUS.LIB;
using IKUS.LIB.MODEL;
using IKUS.LIB.TOOL;
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

namespace VEHICLEDETECTING.Areas.Authority.Controllers
{
    [myFilterAuth]
    [ValidateInput(false)]
    public class EMPController : myControllerAuth
    {
        fSys FunctionThis = new fSys();
        // GET: Authority/EMP
        public ActionResult Index()
        {
            return View();
        }


        #region T_SYS_EMP

        static Dictionary<String, String> DIC_T_SYS_EMP = new Dictionary<string, string> {

        {"ID","ID"},
        {"AccountName","账号名称"},
        {"AccountPsw","账号密码"},
        {"HasFailure","已失效"},
        {"CreateDateTime","创建时间"},
        {"CreateAdminID","创建管理员"},
        {"IsOpen","是否公开"},
        {"Name","名称"},
        {"IsSexMan","性别[M/F]"},
        {"refRoles","关联角色"},
        {"refGroups","关联角色组"},        };
        public ActionResult T_SYS_EMP_List(T_SYS_EMP Model, String KeyWord, DateTime? dt1, DateTime? dt2, Int32? PageSize)
        {


            var LayData = new LayUI.Data("T_SYS_EMP", btnTableInsert: true, btnTableView: true, btnTableUpdate: true)
                .AddButton("信息", Site: LayUI.E_Button_Site.item, Link: "T_SYS_EMP_INFO_Update_ByEmpID", Param: new Dictionary<string, string> { { "EmpID", "{{=row.Cells.ID}}" } })
                .AddButton("拥有权限", Site: LayUI.E_Button_Site.item, Link: "T_SYS_RMP_PRIVIEGE_TREE", Param: new Dictionary<string, string> { { "ID", "{{=row.Cells.ID}}" } })
                .AddButton("职位", Site: LayUI.E_Button_Site.item, Link: "T_SYS_EMP_DEPARTMENT_POSITION_TREE", Param: new Dictionary<string, string> { { "ID", "{{=row.Cells.ID}}" } })
                .AddButton("重置密码",Site:LayUI.E_Button_Site.item,Link: "T_SYS_EMP_Update_AccountPsw", Param:new Dictionary<string, string> { { "ID", "{{=row.Cells.ID}}" } },Confirm:true,ConfirmText: "<strong>确认重置密码？</strong><br/>[重置密码后密码将重置为登陆账号名称]");
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            //LayData.AddProperty("ID",  LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("AccountName", LayUI.E_Property_Type.@text);
            //LayData.AddProperty("AccountPsw", LayUI.E_Property_Type.@text);
            LayData.AddProperty("HasFailure", LayUI.E_Property_Type.@bool);
            //LayData.AddProperty("CreateDateTime",  LayUI.E_Property_Type.@datetime);
            LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);
            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("AccountName", Model.AccountName);
            //LayData.AddModel("AccountPsw", Model.AccountPsw);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("HasFailure", Model.HasFailure);
            LayData.AddModel("IsOpen", Model.IsOpen);


            LayData.AddModel("KeyWord", KeyWord);
            //LayData.AddModel("PageCount", Page.PageCount);
            LayData.AddModel("dt1", dt1.HasValue ? dt1.Ex_ToString("yyyy-MM-dd") : "");
            LayData.AddModel("dt2", dt2.HasValue ? dt2.Ex_ToString("yyyy-MM-dd") : "");

            #endregion

            LayData.Translate(DIC_T_SYS_EMP);

            return View("List", LayData);
        }
        public ActionResult Ajax_T_SYS_EMP_List(T_SYS_EMP Model, String KeyWord, Int32? PageIndex, Int32? PageSize, String Code, Boolean? IsAsc, DateTime? dt1, DateTime? dt2)
        {
            Code = String.IsNullOrWhiteSpace(Code) ? "ID" : Code;
            if (PageIndex > 0)
            {
                M_Pagination Page = new M_Pagination() { PageIndex = PageIndex.Value, PageSize = PageSize ?? 10 };
                var iList = A.Exec(EF => EF.T_SYS_EMP
                .F_Search(Model, KeyWord, dt1, dt2)
                .Ex_OrderBy(Code, IsAsc ?? false)
                .Ex_GetPagination(Page), false);

                if (iList == null) return Content(new { result = -1, page = Page, model = Model }.Ex_ToJson());
                if (iList.Count == 0) return Content(new { result = 2, page = Page, model = Model }.Ex_ToJson());

                var LayData = new LayUI.Data("T_SYS_EMP");
                LayData.AddSort(Code, IsAsc);

                #region Properties
                LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("AccountName", LayUI.E_Property_Type.@text);
                //LayData.AddProperty("AccountPsw", LayUI.E_Property_Type.@text);
                LayData.AddProperty("CreateDateTime", LayUI.E_Property_Type.@datetime);
                LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("HasFailure", LayUI.E_Property_Type.@bool);
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
                        { "ID",p.ID},
                        {"AccountName",p.AccountName.Ex_ToString(15, "...")},
                        //{"AccountPsw",p.AccountPsw.Ex_ToString(15, "...")},
                        {"HasFailure",p.HasFailure??false},
                        {"CreateDateTime",p.CreateDateTime.Ex_ToString()},
                        {"CreateAdminID",p.CreateAdminID},
                        {"IsOpen",p.IsOpen??false},
                    },
                }));

                LayData.Translate(DIC_T_SYS_EMP);

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
        public ActionResult T_SYS_EMP_Insert(T_SYS_EMP Model, String Name, Boolean? IsSexMan, String refRoles, String refGroups)
        {
            if (Request.RequestType == "POST")
            {
                Model.CreateAdminID = REQ.UserID;;
                Model.CreateDateTime = DateTime.Now;
                Model.AccountPsw = T_Crypt.MD5_String(Model.AccountPsw).ToUpper();
                var R = A.Insert(Model, CallBack: (EF, _M) =>
                {
                    refInsert<T_SYS_EMP_refROLE>(EF, refRoles, _M.ID);
                    refInsert<T_SYS_EMP_refGROUP>(EF, refGroups, _M.ID);
                    EF.T_SYS_EMP_INFO.Add(new T_SYS_EMP_INFO
                    {
                        Name = Name,
                        IsSexMan = IsSexMan,
                        EmpID = _M.ID,
                    });
                    EF.SaveChanges();
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);
            var E_ROLE = A.Exec(EF => EF.T_SYS_ROLE
            .Select(p => new { ID = p.ID, Name = p.Name })
            .ToList())
            .Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString(),
            }).ToList();
            var E_GROUP = A.Exec(EF => EF.T_SYS_GROUP
            .Select(p => new { ID = p.ID, Name = p.Name })
            .ToList())
            .Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString(),
            }).ToList();
            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("AccountName", LayUI.E_Property_Type.@text);
            LayData.AddProperty("AccountPsw", LayUI.E_Property_Type.@text);
            LayData.AddProperty("HasFailure", LayUI.E_Property_Type.@bool);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);
            LayData.AddProperty("Name", LayUI.E_Property_Type.text);
            LayData.AddProperty("IsSexMan", LayUI.E_Property_Type.@bool);
            LayData.AddProperty("refRoles", LayUI.E_Property_Type.list);
            LayData.AddProperty("refGroups", LayUI.E_Property_Type.list);

            #endregion

            #region Nodes
            LayData.AddNode("refRoles", E_ROLE);
            LayData.AddNode("refGroups", E_GROUP);
            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("AccountName", Model.AccountName);
            LayData.AddModel("AccountPsw", Model.AccountPsw);
            LayData.AddModel("HasFailure", Model.HasFailure ?? false);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen);
            LayData.AddModel("Name", Name);
            LayData.AddModel("IsSexMan", IsSexMan);
            LayData.AddModel("refRoles", ",");
            LayData.AddModel("refGroups", ",");

            #endregion
            LayData.Translate(DIC_T_SYS_EMP);
            return View("Save", LayData);
        }
        public ActionResult T_SYS_EMP_Update(T_SYS_EMP Model, String refRoles, String refGroups)
        {
            var M = A.Model<T_SYS_EMP>(Model.ID);
            var R = new M_Result { msg = "参数错误 对象为空", data = null, result = 0 };
            if (M == null)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {

                //Model.AccountPsw = K.Y.DLL.Tool.T_Crypt.MD5_String(Model.AccountPsw).ToUpper();
                R = A.Update<T_SYS_EMP>(Model.ID, _M =>
                {
                    //_M.ID = Model.ID;
                    //_M.AccountName = Model.AccountName;
                    //_M.AccountPsw = Model.AccountPsw;
                    _M.HasFailure = Model.HasFailure;
                    //_M.CreateDateTime = Model.CreateDateTime;
                    //_M.CreateAdminID = Model.CreateAdminID;
                    _M.IsOpen = Model.IsOpen;
                }, CallBack: (EF, _M) =>
                {

                    refUpdate<T_SYS_EMP_refROLE>(EF, refRoles, _M.ID);
                    refUpdate<T_SYS_EMP_refGROUP>(EF, refGroups, _M.ID);
                    EF.SaveChanges();
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }

            Model = M;




            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);
            var E_ROLE = A.Exec(EF => EF.T_SYS_ROLE
            .Select(p => new
            {
                ID = p.ID,
                Name = p.Name,
                Selected = EF.T_SYS_EMP_refROLE.Any(r => r.KeyID == Model.ID && r.refKeyID == p.ID && !(r.HasCancle ?? false))
            })
            .ToList())
            .Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString(),
                Selected = p.Selected,
            }).ToList();
            var E_GROUP = A.Exec(EF => EF.T_SYS_GROUP
            .Select(p => new
            {
                ID = p.ID,
                Name = p.Name,
                Selected = EF.T_SYS_EMP_refGROUP.Any(r => r.KeyID == Model.ID && r.refKeyID == p.ID && !(r.HasCancle ?? false))
            })
            .ToList())
            .Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString(),
                Selected = p.Selected,
            }).ToList();
            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            //LayData.AddProperty("AccountName", LayUI.E_Property_Type.@text);
            //LayData.AddProperty("AccountPsw", LayUI.E_Property_Type.@text);
            LayData.AddProperty("HasFailure", LayUI.E_Property_Type.@bool);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);
            LayData.AddProperty("refRoles", LayUI.E_Property_Type.list);
            LayData.AddProperty("refGroups", LayUI.E_Property_Type.list);

            #endregion

            #region Nodes
            LayData.AddNode("refRoles", E_ROLE);
            LayData.AddNode("refGroups", E_GROUP);
            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            //LayData.AddModel("AccountName", Model.AccountName);
            //LayData.AddModel("AccountPsw", Model.AccountPsw);
            LayData.AddModel("HasFailure", Model.HasFailure ?? false);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen);
            LayData.AddModel("refRoles", string.Join(",", E_ROLE.Where(p => p.Selected).Select(p => p.Value)));
            LayData.AddModel("refGroups", string.Join(",", E_GROUP.Where(p => p.Selected).Select(p => p.Value)));

            #endregion
            LayData.Translate(DIC_T_SYS_EMP);
            return View("Save", LayData);
        }
        public ActionResult T_SYS_EMP_Update_AccountPsw(T_SYS_EMP Model)
        {
            var M = A.Model<T_SYS_EMP>(Model.ID);
            var R = new M_Result { msg = "参数错误 对象为空", data = null, result = 0 };
            if (M == null)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }

            var PSW = T_Crypt.MD5_String(M.AccountName).ToUpper();
            R = A.Update<T_SYS_EMP>(Model.ID, _M =>
            {
                _M.AccountPsw = PSW;
            });
            if (REQ.UserID == Model.ID && R.result == 1)
                //Redirect($"/Admin/Main/_LogOut?BackURL={Url.Encode(Request.Url.AbsoluteUri)}");
                return _LogOut(Request.Url.AbsoluteUri);
            Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
            return Content("");
        }

        public ActionResult T_SYS_EMP_View(Int32 ID = 0)
        {
            var Model = A.Model<T_SYS_EMP>(ID);
            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("AccountName", LayUI.E_Property_Type.@text);
            LayData.AddProperty("AccountPsw", LayUI.E_Property_Type.@text);
            LayData.AddProperty("HasFailure", LayUI.E_Property_Type.@bool);
            LayData.AddProperty("CreateDateTime", LayUI.E_Property_Type.@datetime);
            LayData.AddProperty("CreateAdminID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsOpen", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("AccountName", Model.AccountName);
            LayData.AddModel("AccountPsw", Model.AccountPsw);
            LayData.AddModel("HasFailure", Model.HasFailure ?? false);
            LayData.AddModel("CreateDateTime", Model.CreateDateTime.Ex_ToString());
            LayData.AddModel("CreateAdminID", Model.CreateAdminID);
            LayData.AddModel("IsOpen", Model.IsOpen);

            #endregion
            LayData.Translate(DIC_T_SYS_EMP);
            return View("View", LayData);
        }
        [NonAction]
        public ActionResult T_SYS_EMP_Delete(Int32 ID = 0)
        {
            var R = A.Delete<T_SYS_EMP>(ID, CallBack: (EF, _M) =>
            {
                EF.T_SYS_EMP_INFO.RemoveRange(EF.T_SYS_EMP_INFO.Where(p => p.EmpID == _M.ID));
                EF.SaveChanges();
            });
            Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
            return Content("");
        }

        public ActionResult T_SYS_RMP_PRIVIEGE_TREE(Int32 ID = 0, String priviegeid = "")
        {
            var LayData = new LayUI.Data();
            LayData.zTreeData.TreeNodes = A.Exec(EF =>
            {
                var L = new List<zTree.ITreeNodeBase>();
                L.Add(new zTree.TreeNodeBase { id = -1, pId = 0, name = "ROOT", color = "#FF4500", @checked=true, chkDisabled = true, });
                L.Add(new zTree.TreeNodeBase { id = (Int32)Config.E_Sys_Priviege_Type.MENU, pId = -1, name = "MENU", color = "#FF4500", @checked = true, chkDisabled = true, });

                var LPRIVIEGEs = FunctionThis.Emp_GetAllPriviegeByEmpID(ID);


                L.AddRange(EF.T_SYS_MENU.Join(EF.T_SYS_PRIVIEGE.Where(p => p.Type == (Int32)Config.E_Sys_Priviege_Type.MENU), p => p.ID, q => q.KeyID, (p, q) => new
                {
                    id = p.ID + (Int32)Config.E_Sys_Priviege_Type.MENU,
                    pId = p.ParentID + (Int32)Config.E_Sys_Priviege_Type.MENU,
                    name = "[MENU]" + p.Name,
                    IsOpen = p.IsOpen,
                    priviegeid = q.ID,
                }).Concat(EF.T_SYS_BUTTON.Join(EF.T_SYS_PRIVIEGE.Where(p => p.Type == (Int32)Config.E_Sys_Priviege_Type.BUTTON), p => p.ID, q => q.KeyID, (p, q) => new
                {
                    id = p.ID + (Int32)Config.E_Sys_Priviege_Type.BUTTON,
                    pId = p.MenuID + (Int32)Config.E_Sys_Priviege_Type.MENU,
                    name = "[BUTTON]" + p.Name,
                    IsOpen = p.IsOpen,
                    priviegeid = q.ID,
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
                    @checked = LPRIVIEGEs.Any(pr => pr.ID == p.priviegeid),
                    chkDisabled = true,
                }));
                return L;
            }, false);

            return View("Choice", LayData);
        }

        public ActionResult T_SYS_EMP_DEPARTMENT_POSITION_TREE(Int32 ID = 0, String POSITIONIDS = "")
        {
            var LayData = new LayUI.Data();

            LayData.AddProperty("POSITIONIDS", Type: LayUI.E_Property_Type.ztreeextra);

            if (Request.RequestType == "POST")
            {
                A.Exec(EF =>
                {
                    var ExistItems = EF.T_SYS_DEPARTMENT_POSITION_refEMP.Where(p => p.refKeyID == ID && !(p.HasCancle ?? false)).Select(p => p.KeyID).Where(p => p > 0).Distinct().ToList();
                    var CurrentItems = POSITIONIDS.Ex_ToList().Ex_ToInt32().Where(p => p > 0).Distinct().ToList();
                    var ToInsertItems = CurrentItems.Where(p => !ExistItems.Contains(p)).ToList();
                    var ToRemoveItems = ExistItems.Where(p => !CurrentItems.Contains(p)).ToList();
                    foreach (var m in EF.T_SYS_DEPARTMENT_POSITION_refEMP.Where(p => !(p.HasCancle ?? false) && p.refKeyID == ID && ToRemoveItems.Contains(p.KeyID)))
                    {
                        m.HasCancle = true;
                        m.CancleDateTime = DateTime.Now;
                    }
                    foreach (var m in ToInsertItems)
                    {
                        EF.T_SYS_DEPARTMENT_POSITION_refEMP
                            .Add(new T_SYS_DEPARTMENT_POSITION_refEMP
                            {
                                CreateAdminID = REQ.UserID,
                                KeyID = m,
                                CreateDateTime = DateTime.Now,
                                refKeyID = ID,
                                HasCancle = false,
                            });
                    }
                    EF.SaveChanges();
                });
                var R = new M_Result(E_ERRORCODE.操作成功);
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }
            LayData.zTreeData.TreeNodes = A.Exec(EF =>
            {
                var L = new List<zTree.ITreeNodeBase>();
                var lPositionrefemp = EF.T_SYS_DEPARTMENT_POSITION_refEMP.Where(p => p.refKeyID == ID && !(p.HasCancle ?? false)).Select(p => p.KeyID).ToList();
                L.AddRange(EF.T_SYS_DEPARTMENT.Select(p => new { ID = (p.ID + 1000000), p.Name, ParentID = (p.ParentID + 1000000), p.IsOpen, })
                    .Concat(EF.T_SYS_DEPARTMENT_POSITION.Select(p => new { p.ID, p.Name, ParentID = (p.DepartmentID + 1000000), p.IsOpen }))
                    .ToList().Select(p => new zTree.TreeNodeBase
                    {
                        id = p.ID,
                        pId = p.ParentID,
                        name = p.Name,
                        open = true,
                        color = (p.IsOpen ?? false) ? null : "#A9A9A9",
                        @checked = lPositionrefemp.Any(pr => pr == p.ID),
                        chkDisabled = p.ID > 1000000,
                        data=new Dictionary<string, string> {
                            {"POSITIONIDS",p.ID.ToString()}
                        }
                    }));

                LayData.AddModel("POSITIONIDS", string.Join(",", lPositionrefemp));
                return L;
            }, false);
            return View("Choice", LayData);
        }
        #endregion

        #region T_SYS_EMP_INFO

        static Dictionary<String, String> DIC_T_SYS_EMP_INFO = new Dictionary<string, string> {

        {"ID","ID"},
        {"EmpID","EmpID"},
        {"Name","名称"},
        {"IsSexMan","性别[M/F]"},        };
        [NonAction]
        public ActionResult T_SYS_EMP_INFO_List(T_SYS_EMP_INFO Model, String KeyWord, DateTime? dt1, DateTime? dt2, Int32? PageSize)
        {
            var Page = new M_Pagination(1, PageSize ?? 10);
            A.Exec(EF => EF.T_SYS_EMP_INFO
            .F_Search(Model, KeyWord, dt1, dt2)
            .OrderBy(p => p.ID)
            .Ex_GetPagination(Page), false);

            var LayData = new LayUI.Data("T_SYS_EMP_INFO");
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            //LayData.AddProperty("ID",  LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("EmpID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsSexMan", LayUI.E_Property_Type.@bool);
            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("EmpID", Model.EmpID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("IsSexMan", Model.IsSexMan);


            LayData.AddModel("KeyWord", KeyWord);
            LayData.AddModel("PageCount", Page.PageCount);
            LayData.AddModel("dt1", dt1.HasValue ? dt1.Ex_ToString("yyyy-MM-dd") : "");
            LayData.AddModel("dt2", dt2.HasValue ? dt2.Ex_ToString("yyyy-MM-dd") : "");

            #endregion

            LayData.Translate(DIC_T_SYS_EMP_INFO);

            return View("List", LayData);
        }
        [NonAction]
        public ActionResult Ajax_T_SYS_EMP_INFO_List(T_SYS_EMP_INFO Model, String KeyWord, Int32? PageIndex, Int32? PageSize, String Code, Boolean? IsAsc, DateTime? dt1, DateTime? dt2)
        {
            Code = String.IsNullOrWhiteSpace(Code) ? "ID" : Code;
            if (PageIndex > 0)
            {
                M_Pagination Page = new M_Pagination() { PageIndex = PageIndex.Value, PageSize = PageSize ?? 10 };
                var iList = A.Exec(EF => EF.T_SYS_EMP_INFO
                .F_Search(Model, KeyWord, dt1, dt2)
                .Ex_OrderBy(Code, IsAsc ?? false)
                .Ex_GetPagination(Page), false);

                if (iList == null) return Content(new { result = -1, page = Page, model = Model }.Ex_ToJson());
                if (iList.Count == 0) return Content(new { result = 2, page = Page, model = Model }.Ex_ToJson());

                var LayData = new LayUI.Data();
                LayData.AddSort(Code, IsAsc);

                #region Properties
                LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("EmpID", LayUI.E_Property_Type.@text);
                LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
                LayData.AddProperty("IsSexMan", LayUI.E_Property_Type.@bool);

                #endregion

                iList.ForEach(p => LayData.TableData.Add(new LayUI.Row
                {
                    Cells = new Dictionary<string, object>
                    {
                        {"ID",p.ID},
                        {"EmpID",p.EmpID},
                        {"Name",p.Name.Ex_ToString(15, "...")},
                        {"IsSexMan",p.IsSexMan??false},
                        }
                }));

                LayData.Translate(DIC_T_SYS_EMP_INFO);

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
        [NonAction]
        public ActionResult T_SYS_EMP_INFO_Insert(T_SYS_EMP_INFO Model)
        {
            if (Request.RequestType == "POST")
            {
                var R = A.Insert(Model);
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("EmpID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsSexMan", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("EmpID", Model.EmpID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("IsSexMan", Model.IsSexMan ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_EMP_INFO);
            return View("Save", LayData);
        }
        [NonAction]
        public ActionResult T_SYS_EMP_INFO_Update(T_SYS_EMP_INFO Model)
        {
            var M = A.Model<T_SYS_EMP_INFO>(Model.ID);
            var R = new M_Result { msg = "参数错误 对象为空", data = null, result = 0 };
            if (M == null)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {

                R = A.Update<T_SYS_EMP_INFO>(Model.ID, _M =>
                {
                    //_M.ID = Model.ID;
                    //_M.EmpID = Model.EmpID;
                    _M.Name = Model.Name;
                    _M.IsSexMan = Model.IsSexMan;
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }

            Model = M;

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("EmpID", LayUI.E_Property_Type.hidden);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsSexMan", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("EmpID", Model.EmpID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("IsSexMan", Model.IsSexMan ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_EMP_INFO);
            return View("Save", LayData);
        }

        public ActionResult T_SYS_EMP_INFO_Update_ByEmpID(T_SYS_EMP_INFO Model)
        {
            var M = A.Model<T_SYS_EMP_INFO>(p => p.EmpID == Model.EmpID);
            var R = new M_Result { msg = "参数错误 对象为空", data = null, result = 0 };
            if (M == null)
            {
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }
            if (Request.RequestType == "POST")
            {

                R = A.Update<T_SYS_EMP_INFO>(Model.ID, _M =>
                {
                    //_M.ID = Model.ID;
                    //_M.EmpID = Model.EmpID;
                    _M.Name = Model.Name;
                    _M.IsSexMan = Model.IsSexMan;
                });
                Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
                return Content("");
            }

            Model = M;

            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@hidden);
            LayData.AddProperty("EmpID", LayUI.E_Property_Type.hidden);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsSexMan", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("EmpID", Model.EmpID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("IsSexMan", Model.IsSexMan ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_EMP_INFO);
            return View("Save", LayData);
        }

        [NonAction]
        public ActionResult T_SYS_EMP_INFO_View(Int32 ID = 0)
        {
            var Model = A.Model<T_SYS_EMP_INFO>(ID);
            var LayData = new LayUI.Data();
            //LayData.AddSort(Code, IsAsc);

            #region Properties
            LayData.AddProperty("ID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("EmpID", LayUI.E_Property_Type.@text);
            LayData.AddProperty("Name", LayUI.E_Property_Type.@text);
            LayData.AddProperty("IsSexMan", LayUI.E_Property_Type.@bool);

            #endregion

            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("ID", Model.ID);
            LayData.AddModel("EmpID", Model.EmpID);
            LayData.AddModel("Name", Model.Name);
            LayData.AddModel("IsSexMan", Model.IsSexMan ?? false);

            #endregion
            LayData.Translate(DIC_T_SYS_EMP_INFO);
            return View("View", LayData);
        }
        [NonAction]
        public ActionResult T_SYS_EMP_INFO_Delete(Int32 ID = 0)
        {
            var R = A.Delete<T_SYS_EMP_INFO>(ID);
            Response.Write("<script type='text/javascript'>parent.LayUI_Layer_CloseiFrame(" + R.Ex_ToJson() + ")</script>");
            return Content("");
        }
        #endregion

    }
}