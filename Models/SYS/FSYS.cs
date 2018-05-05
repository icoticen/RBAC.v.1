using IKUS.LIB.CACHE;
using IKUS.LIB.MODEL;
using IKUS.LIB.TOOL;
using IKUS.LIB.WEB.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SYS.Table;
using VEHICLEDETECTING.Models;

namespace SYS
{
    public class FSYS : FactoryBase<EAuth>
    {

        public M_Result Emp_Login(String AccountName, string AccountPsw)
        {
            var EMP = Model<T_SYS_EMP>(p => p.AccountName == AccountName);
            if (EMP == null)
            {
                if (AccountName == "admin")
                {
                    var Psw = T_Crypt.MD5_String("admin").ToUpper();
                    var R = Emp_Register("admin", Psw);
                    if (R.result == 1)
                    {
                        if (Psw == AccountPsw) return new M_Result(E_ERRORCODE.操作成功, DATA: EMP.ID);
                    }
                    return new M_Result(E_ERRORCODE.服务端_账号_密码不正确);
                }
                return new M_Result(E_ERRORCODE.服务端_账号_不存在);
            }
            if (EMP.HasFailure ?? false)
                return new M_Result(E_ERRORCODE.服务端_账号_无权操作, DATA: EMP.ID);
            if (!(EMP.IsOpen ?? false))
                return new M_Result(E_ERRORCODE.服务端_账号_不可用, DATA: EMP.ID);
            if (EMP.AccountPsw.ToUpper() == AccountPsw.ToUpper())
                return new M_Result(E_ERRORCODE.操作成功, DATA: EMP.ID);
            return new M_Result(E_ERRORCODE.服务端_账号_密码不正确);
        }
        public M_Result Emp_Register(String AccountName, string AccountPsw, bool HasFailure = false)
        {
            var R = Insert(new T_SYS_EMP
            {
                AccountName = AccountName,
                AccountPsw = AccountPsw,
                HasFailure = HasFailure,
            }, EF =>
            {
                var C = EF.T_SYS_EMP.Count(p => p.AccountName == AccountName);
                if (C > 0) return new M_Result(E_ERRORCODE.服务端_账号_已存在);
                return null;
            }, CallBack: (EF, _M) =>
            {
                EF.T_SYS_EMP_INFO.Add(new T_SYS_EMP_INFO
                {
                    EmpID = _M.ID,
                    IsSexMan = false,
                    Name = _M.AccountName,
                });
                EF.SaveChanges();
            });
            if (R.result == 1) R.data = ((T_SYS_EMP)R.data).ID;
            return R;
        }
        public M_Result Emp_ResetPsw(Int32 EmpID, String AccountPsw, String OldAccountPsw)
        {
            return Exec(EF =>
            {
                var M = EF.T_SYS_EMP.FirstOrDefault(p => p.ID == EmpID);
                if (M == null) return new M_Result(E_ERRORCODE.服务端_账号_不存在);
                if (M.AccountPsw != OldAccountPsw) return new M_Result(E_ERRORCODE.服务端_账号_密码不正确);

                M.AccountPsw = AccountPsw;
                EF.SaveChanges();
                return new M_Result(E_ERRORCODE.操作成功, DATA: M.ID);
            });
        }


        public T_SYS_EMP_INFO Emp_GetEmpInfo_ByEmpID(Int32 EmpID)
        {
            return ceInit<T_SYS_EMP_INFO>(p => p.EmpID == EmpID, $"p => p.EmpID == {EmpID}").Model;
        }
        public T_SYS_NODE Node_GetNode_ByCode(String Code)
        {
            return ceInit<T_SYS_NODE>(p => p.Code == Code, $"p => p.Code == {Code}").Model;
        }
        public T_SYS_MENU Menu_GetMenu_ByValue(String Value)
        {
            Value = Value.ToUpper();
            var lMenus = Menu_GetAllMenus_ByRoot();
            return lMenus.FirstOrDefault(p => p.Value.ToUpper() == Value);
        }
        //public T_SYS_MENU Button_GetMenu_ByValue(String Value)
        //{
        //    Value = Value.ToUpper();
        //    var lMenus = Menu_GetAllMenus_ByRoot();
        //    return lMenus.FirstOrDefault(p => p.Value.ToUpper() == Value);
        //}



        /// <summary>
        /// 获取用户的所以权限
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public List<T_SYS_PRIVIEGE> Emp_GetAllPrivieges_ByEmpID(Int32 EmpID, Int32 Type = 0)
        {
            var L = Cache.GetSlidingKeepCache(CacheKey: $"FACTORY/SYS/Emp_GetAllPrivieges_ByEmpID/{EmpID}",
                FDataSource: () => Exec(EF =>
                {
                    return EF.T_SYS_EMP_refGROUP
                  .Where(p => !(p.HasCancle ?? false))
                  .Where(p => p.KeyID == EmpID)
                  .Select(p => p.refKeyID)
                  .GroupJoin(EF.T_SYS_GROUP_refROLE
                  .Where(p => !(p.HasCancle ?? false)), p => p, q => q.KeyID, (p, q) => new
                  {
                      Roles = q.Select(r => r.refKeyID)
                  })
                  .SelectMany(p => p.Roles).Concat(
                      EF.T_SYS_EMP_refROLE
                      .Where(p => !(p.HasCancle ?? false)).Where(p => p.KeyID == EmpID)
                      .Select(p => p.refKeyID)
                  ).Distinct()
                  .GroupJoin(EF.T_SYS_ROLE_refPRIVIEGE.Where(p => !(p.HasCancle ?? false)),
                  p => p, q => q.KeyID, (p, q) => new
                  {

                      Privieges = q.Select(r => r.refKeyID)
                  }).SelectMany(p => p.Privieges)
                  .Distinct()
                  .Join(EF.T_SYS_PRIVIEGE, p => p, q => q.ID, (p, q) => q)
                  .ToList();

                }));
            return Type <= 0 ? L : L.Where(p => p.Type == Type).ToList();
        }
        public List<Int32> Emp_GetAllDepartmentIDs_ByEmpID(Int32 EmpID)
        {
            var lDepartmentPositions = Department_GetAllDepartmentPositions();
            var L = Cache.GetAbsoluteKeepCache(CacheKey: $"FACTORY/SYS/Emp_GetAllDepartmentIDs_ByEmpID/{EmpID}",
                FDataSource: () => Exec(EF =>
                {
                    return EF.T_SYS_DEPARTMENT_POSITION_refEMP
                  .Where(p => !(p.HasCancle ?? false))
                  .Where(p => p.refKeyID == EmpID)
                  .Select(p => p.KeyID)
                  .Join(EF.T_SYS_DEPARTMENT_POSITION, p => p, q => q.ID, (p, q) => q)
                  .Select(p => p.DepartmentID)
                  .Distinct()
                  .ToList();
                }));
            return L;
        }
        public List<Int32> Emp_GetAllDepartmentPositionIDs_ByEmpID(Int32 EmpID)
        {
            var lDepartmentPositions = Department_GetAllDepartmentPositions();
            var L = Cache.GetAbsoluteKeepCache(CacheKey: $"FACTORY/SYS/Emp_GetAllDepartmentPositionIDs_ByEmpID/{EmpID}",
                FDataSource: () => Exec(EF =>
                {
                    return EF.T_SYS_DEPARTMENT_POSITION_refEMP
                  .Where(p => !(p.HasCancle ?? false))
                  .Where(p => p.refKeyID == EmpID)
                  .Select(p => p.KeyID)
                  .Distinct()
                  .ToList();
                }));
            return L;
        }
        ///// <summary>
        ///// 不包括自身所在
        ///// </summary>
        ///// <param name="EmpID"></param>
        ///// <returns></returns>
        //public List<Int32> Emp_GetAllLowerDepartmentIDs_ByEmpID(Int32 EmpID)
        //{
        //    var L = Cache.GetAbsoluteKeepCache(CacheKey: $"FACTORY/SYS/Emp_GetAllLowerDepartmentIDs_ByEmpID/{EmpID}",
        //        FDataSource: () =>
        //        {
        //            var lDepartments = Department_GetAllDepartments();
        //            var lDepartmentPositions = Department_GetAllDepartmentPositions();

        //            var lEmpDepartmentIDs = Emp_GetAllDepartmentIDs_ByEmpID(EmpID);
        //            var lEmpDepartmentPositionIDs = Emp_GetAllDepartmentPositionIDs_ByEmpID(EmpID);

        //            //var lEmpDepartmentPositions = lDepartmentPositions.Where(p => lEmpDepartmentPositionIDs.Contains(p.ID)).ToList();

        //            var lParentDepartmentIDs = lEmpDepartmentIDs.Select(p => p).ToList();
        //            var lChildDepartmentIDs = new List<Int32>();

        //            do
        //            {
        //                var lChildren = new List<Int32>();
        //                foreach (var m in lParentDepartmentIDs)
        //                {
        //                    lChildDepartmentIDs.Add(m);
        //                    var chileren = lDepartments.Where(p => p.ParentID == m).Select(p => p.ID).ToList();
        //                    lChildren.AddRange(chileren);
        //                }
        //                lParentDepartmentIDs = lChildren;
        //            } while (lParentDepartmentIDs.Count > 0);
        //            //移除自己所在的
        //            lEmpDepartmentIDs.ForEach(p => lChildDepartmentIDs.Remove(p));
        //            return lChildDepartmentIDs;
        //        });

        //    return L;
        //}
        ///// <summary>
        ///// 不包括自身所在
        ///// </summary>
        ///// <param name="EmpID"></param>
        ///// <returns></returns>
        //public List<Int32> Emp_GetAllLowerDepartmentPositionIDs_ByEmpID(Int32 EmpID)
        //{
        //    var L = Cache.GetAbsoluteKeepCache(CacheKey: $"FACTORY/SYS/Emp_GetAllLowerDepartmentPositionIDs_ByEmpID/{EmpID}",
        //        FDataSource: () =>
        //        {
        //            var lDepartments = Department_GetAllDepartments();
        //            var lDepartmentPositions = Department_GetAllDepartmentPositions();

        //            var lEmpDepartmentIDs = Emp_GetAllDepartmentIDs_ByEmpID(EmpID);
        //            var lEmpDepartmentPositionIDs = Emp_GetAllDepartmentPositionIDs_ByEmpID(EmpID);

        //            var lEmpDepartmentPositions = lDepartmentPositions.Where(p => lEmpDepartmentPositionIDs.Contains(p.ID)).ToList();

        //            var lParentDepartmentPositions = lEmpDepartmentPositions;
        //            var lLowerDepartmentPositionIDs = lEmpDepartmentPositions.SelectMany(p =>
        //            {
        //                var ldpids = Department_GetAllLowerDepartments_ByDepartmentID(p.DepartmentID);
        //                return lDepartmentPositions.Where(m => ldpids.Contains(m.DepartmentID) && m.Level <= p.Level)
        //                .Concat(lDepartmentPositions.Where(m => m.DepartmentID == p.DepartmentID && m.Level < p.Level))
        //                .Select(m => m.ID);
        //            }).Distinct().ToList();
        //            return lLowerDepartmentPositionIDs;
        //        });
        //    return L;
        //}
        /// <summary>
        /// 不包括自身
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public List<Int32> Emp_GetAllLowerEmpIDs_ByEmpID(Int32 EmpID)
        {
            var L = Cache.GetAbsoluteKeepCache(CacheKey: $"FACTORY/SYS/Emp_GetAllLowerEmpIDs_ByEmpID/{EmpID}",
                FDataSource: () =>
                {
                    var lEmpDepartmentPositionIDs = Emp_GetAllDepartmentPositionIDs_ByEmpID(EmpID);

                    var lEmpLowerDepartmentPositionIDs = lEmpDepartmentPositionIDs
                    .SelectMany(m => Department_GetAllLowerDepartmentPositions_ByDepartmentPositionID(m))
                    .ToList();
                    if (lEmpLowerDepartmentPositionIDs.Count <= 0) return new List<Int32>();

                    return Exec(EF => EF.T_SYS_DEPARTMENT_POSITION_refEMP
                   .Where(p => lEmpLowerDepartmentPositionIDs.Contains(p.KeyID))
                   .Select(p => p.refKeyID).ToList()
                    );
                });

            return L;
        }

        public List<T_SYS_MENU> Menu_GetAllMenus_ByRoot(String Root = "")
        {
            var L = Cache.GetSlidingKeepCache(CacheKey: $"FACTORY/SYS/Menu_GetAllMenus_ByRoot",
                FDataSource: () => Exec(EF => EF.T_SYS_MENU.ToList())
            );
            return string.IsNullOrWhiteSpace(Root) ? L : L.Where(p => p.Root == Root).ToList();
        }
        public List<T_SYS_BUTTON> Button_GetAllButtons_ByRoot(Int32 MenuID = 0)
        {
            var L = Cache.GetSlidingKeepCache(CacheKey: $"FACTORY/SYS/Button_GetAllButton_ByRoot",
                FDataSource: () => Exec(EF => EF.T_SYS_BUTTON.ToList())
            );
            return MenuID <= 0 ? L : L.Where(p => p.MenuID == MenuID).ToList();
        }

        public List<T_SYS_DEPARTMENT> Department_GetAllDepartments()
        {
            var L = Cache.GetSlidingKeepCache(CacheKey: $"FACTORY/SYS/Department_GetAllDepartments",
                FDataSource: () => Exec(EF => EF.T_SYS_DEPARTMENT.ToList())
            );
            return L;
        }
        public List<Int32> Department_GetAllLowerDepartments_ByDepartmentID(Int32 DepartmentID)
        {
            var L = Cache.GetSlidingKeepCache(CacheKey: $"FACTORY/SYS/Department_GetAllLowerDepartments_ByDepartmentID/{DepartmentID}",
                FDataSource: () =>
                {
                    var lDepartments = Department_GetAllDepartments();

                    var lParentDepartmentIDs = new List<Int32> { DepartmentID, };
                    var lChildDepartmentIDs = new List<Int32>();
                    do
                    {
                        var lChildren = new List<Int32>();
                        foreach (var m in lParentDepartmentIDs)
                        {
                            lChildDepartmentIDs.Add(m);
                            var chileren = lDepartments.Where(p => p.ParentID == m).Select(p => p.ID).ToList();
                            lChildren.AddRange(chileren);
                        }
                        lParentDepartmentIDs = lChildren;
                    } while (lParentDepartmentIDs.Count > 0);
                    lChildDepartmentIDs.Remove(DepartmentID);
                    return lChildDepartmentIDs;
                }
            );
            return L;
        }
        public List<Int32> Department_GetAllLowerDepartmentPositions_ByDepartmentPositionID(Int32 DepartmentPositionID)
        {
            var L = Cache.GetSlidingKeepCache(CacheKey: $"FACTORY/SYS/Department_GetAllLowerDepartmentPositions_ByDepartmentPositionID/{DepartmentPositionID}",
                FDataSource: () =>
                {
                    //var lDepartments = Department_GetAllDepartments();
                    var lDepartmentPositions = Department_GetAllDepartmentPositions();

                    var lParentDepartmentPositions = lDepartmentPositions
                    .Where(p => p.ID == DepartmentPositionID)
                    .ToList();
                    var lLowerDepartmentPositionIDs = lParentDepartmentPositions
                    .SelectMany(p =>
                    {
                        var ldpids = Department_GetAllLowerDepartments_ByDepartmentID(p.DepartmentID);
                        return lDepartmentPositions.Where(m => ldpids.Contains(m.DepartmentID) && m.Level <= p.Level)
                        .Concat(lDepartmentPositions.Where(m => m.DepartmentID == p.DepartmentID && m.Level < p.Level))
                        .Select(m => m.ID);
                    }).Distinct().ToList();
                    return lLowerDepartmentPositionIDs;
                }
            );
            return L;
        }
        public List<T_SYS_DEPARTMENT_POSITION> Department_GetAllDepartmentPositions()
        {
            var L = Cache.GetSlidingKeepCache(CacheKey: $"FACTORY/SYS/Department_GetAllDepartmentPositions",
                FDataSource: () => Exec(EF => EF.T_SYS_DEPARTMENT_POSITION.ToList())
            );
            return L;
        }        

        //public List<T_SYS_PRIVIEGE> Emp_GetAllPrivieges_ByEmpID(Int32 EmpID)
        //{
        //    var L = Cache.Cache.GetSlidingKeepCache(CacheKey: $"FACTORY/SYS/Emp_GetAllPriviegeByEmpID/{EmpID}", FDataSource: () => Exec(EF =>
        //     {
        //         return EF.T_SYS_EMP_refGROUP
        //       .Where(p => !(p.HasCancle ?? false))
        //       .Where(p => p.KeyID == EmpID)
        //       .Select(p => p.refKeyID)
        //       .GroupJoin(EF.T_SYS_GROUP_refROLE
        //       .Where(p => !(p.HasCancle ?? false)), p => p, q => q.KeyID, (p, q) => new
        //       {
        //           Roles = q.Select(r => r.refKeyID)
        //       })
        //       .SelectMany(p => p.Roles).Concat(
        //           EF.T_SYS_EMP_refROLE
        //           .Where(p => !(p.HasCancle ?? false)).Where(p => p.KeyID == EmpID)
        //           .Select(p => p.refKeyID)
        //       ).Distinct()
        //       .GroupJoin(EF.T_SYS_ROLE_refPRIVIEGE.Where(p => !(p.HasCancle ?? false)),
        //       p => p, q => q.KeyID, (p, q) => new
        //       {

        //           Privieges = q.Select(r => r.refKeyID)
        //       }).SelectMany(p => p.Privieges)
        //       .Distinct()
        //       .Join(EF.T_SYS_PRIVIEGE, p => p, q => q.ID, (p, q) => q)
        //       .ToList();

        //     }));
        //    return L;
        //}

        //public List<Int32> Node_GetElements_ByCode(String Code)
        //{
        //    var Node = Node_GetNode_ByCode(Code);
        //    if (Node.ID <= 0) return new List<Int32>();
        //    return Cache.GetAbsoluteKeepCache($"SYS/Node_GetElements_ByCode/{Code}", () => Exec(EF =>
        //        EF.T_SYS_NODE_ELEMENT.Where(p => p.NodeID == Node.ID)
        //        .Where(p => p.IsOpen ?? false)
        //        .Select(p => p.ID)
        //        .ToList()), CacheKeepSeconds: 600);
        //}

        public List<pItem> Node_GetElements(Int32 NodeID)
        {
            var ceModel = ceInit<T_SYS_NODE>(NodeID);
            if (ceModel.Model.ID <= 0) return new List<pItem>();
            return ceModel.Items.Get("Node/Node_GetElements", () => Exec(EF =>
                EF.T_SYS_NODE_ELEMENT.Where(p => p.NodeID == ceModel.ID)
                .Where(p => p.IsOpen ?? false)
                .ToList())
                .Select(p => new pItem
                {
                    ID = p.ID,
                    SortIndex = p.SortNo,
                    ExtraData = new Dictionary<string, string> {
                        { "NodeID",p.NodeID.ToString()},
                        { "Name",p.Name},
                        { "Value",p.Value},
                    }
                }).ToList(), Seconds: 600);
        }
        public List<pItem> Node_GetDirectChildren(Int32 NodeID)
        {
            var ceModel = ceInit<T_SYS_NODE>(NodeID);
            if (ceModel.Model.ID <= 0) return new List<pItem>();
            return ceModel.Items.Get("Node/Node_GetDirectChildren", () => Exec(EF =>
                EF.T_SYS_NODE.Where(p => p.ParentID == ceModel.ID)
                .Where(p => p.IsOpen ?? false)
                .ToList())
                .Select(p => new pItem
                {
                    ID = p.ID,
                    SortIndex = p.SortNo,
                }).ToList(), Seconds: 600);
        }
    }
    public static partial class _F_SEARCH
    {
        #region T_SYS_BUTTON
        //F_Search 
        public static IQueryable<T_SYS_BUTTON> F_Search(this IQueryable<T_SYS_BUTTON> list, T_SYS_BUTTON Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Name.Contains(KeyWord)
                               || p.Value.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.MenuID > 0) list = list.Where(p => p.MenuID == Model.MenuID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Name)) list = list.Where(p => p.Name.Contains(Model.Name));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Value)) list = list.Where(p => p.Value.Contains(Model.Value));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Image)) list = list.Where(p => p.Image.Contains(Model.Image));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Link)) list = list.Where(p => p.Link.Contains(Model.Link));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Describe)) list = list.Where(p => p.Describe.Contains(Model.Describe));
                //int
                if (Model.SortNo > 0) list = list.Where(p => p.SortNo == Model.SortNo);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_DEPARTMENT
        //F_Search 
        public static IQueryable<T_SYS_DEPARTMENT> F_Search(this IQueryable<T_SYS_DEPARTMENT> list, T_SYS_DEPARTMENT Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Name.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.ParentID > 0) list = list.Where(p => p.ParentID == Model.ParentID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Name)) list = list.Where(p => p.Name.Contains(Model.Name));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Describe)) list = list.Where(p => p.Describe.Contains(Model.Describe));
                //int
                if (Model.SortNo > 0) list = list.Where(p => p.SortNo == Model.SortNo);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_DEPARTMENT_POSITION
        //F_Search 
        public static IQueryable<T_SYS_DEPARTMENT_POSITION> F_Search(this IQueryable<T_SYS_DEPARTMENT_POSITION> list, T_SYS_DEPARTMENT_POSITION Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Name.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.DepartmentID > 0) list = list.Where(p => p.DepartmentID == Model.DepartmentID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Name)) list = list.Where(p => p.Name.Contains(Model.Name));
                //int
                if (Model.Level > 0) list = list.Where(p => p.Level == Model.Level);
                //int
                if (Model.SortNo > 0) list = list.Where(p => p.SortNo == Model.SortNo);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
                //int
                if (Model.EmpID > 0) list = list.Where(p => p.EmpID == Model.EmpID);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_EMP
        //F_Search 
        public static IQueryable<T_SYS_EMP> F_Search(this IQueryable<T_SYS_EMP> list, T_SYS_EMP Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.AccountName.Contains(KeyWord)
                               || p.AccountPsw.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.AccountName)) list = list.Where(p => p.AccountName.Contains(Model.AccountName));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.AccountPsw)) list = list.Where(p => p.AccountPsw.Contains(Model.AccountPsw));
                //bit
                if (Model.HasFailure != null) list = list.Where(p => p.HasFailure == Model.HasFailure);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_EMP_INFO
        //F_Search 
        public static IQueryable<T_SYS_EMP_INFO> F_Search(this IQueryable<T_SYS_EMP_INFO> list, T_SYS_EMP_INFO Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Name.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.EmpID > 0) list = list.Where(p => p.EmpID == Model.EmpID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Name)) list = list.Where(p => p.Name.Contains(Model.Name));
                //bit
                if (Model.IsSexMan != null) list = list.Where(p => p.IsSexMan == Model.IsSexMan);
            }
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_EMP_refGROUP
        //F_Search 
        public static IQueryable<T_SYS_EMP_refGROUP> F_Search(this IQueryable<T_SYS_EMP_refGROUP> list, T_SYS_EMP_refGROUP Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.KeyID > 0) list = list.Where(p => p.KeyID == Model.KeyID);
                //int
                if (Model.refKeyID > 0) list = list.Where(p => p.refKeyID == Model.refKeyID);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.HasCancle != null) list = list.Where(p => p.HasCancle == Model.HasCancle);
                //datetime
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_EMP_refROLE
        //F_Search 
        public static IQueryable<T_SYS_EMP_refROLE> F_Search(this IQueryable<T_SYS_EMP_refROLE> list, T_SYS_EMP_refROLE Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.KeyID > 0) list = list.Where(p => p.KeyID == Model.KeyID);
                //int
                if (Model.refKeyID > 0) list = list.Where(p => p.refKeyID == Model.refKeyID);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.HasCancle != null) list = list.Where(p => p.HasCancle == Model.HasCancle);
                //datetime
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_GROUP
        //F_Search 
        public static IQueryable<T_SYS_GROUP> F_Search(this IQueryable<T_SYS_GROUP> list, T_SYS_GROUP Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Name.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.ParentID > 0) list = list.Where(p => p.ParentID == Model.ParentID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Name)) list = list.Where(p => p.Name.Contains(Model.Name));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Describe)) list = list.Where(p => p.Describe.Contains(Model.Describe));
                //int
                if (Model.SortNo > 0) list = list.Where(p => p.SortNo == Model.SortNo);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_GROUP_refROLE
        //F_Search 
        public static IQueryable<T_SYS_GROUP_refROLE> F_Search(this IQueryable<T_SYS_GROUP_refROLE> list, T_SYS_GROUP_refROLE Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.KeyID > 0) list = list.Where(p => p.KeyID == Model.KeyID);
                //int
                if (Model.refKeyID > 0) list = list.Where(p => p.refKeyID == Model.refKeyID);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.HasCancle != null) list = list.Where(p => p.HasCancle == Model.HasCancle);
                //datetime
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_MENU
        //F_Search 
        public static IQueryable<T_SYS_MENU> F_Search(this IQueryable<T_SYS_MENU> list, T_SYS_MENU Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Value.Contains(KeyWord)
                               || p.Root.Contains(KeyWord)
                               || p.Name.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Value)) list = list.Where(p => p.Value.Contains(Model.Value));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Image)) list = list.Where(p => p.Image.Contains(Model.Image));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Link)) list = list.Where(p => p.Link.Contains(Model.Link));
                //int
                if (Model.Level > 0) list = list.Where(p => p.Level == Model.Level);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Root)) list = list.Where(p => p.Root.Contains(Model.Root));
                //int
                if (Model.ParentID > 0) list = list.Where(p => p.ParentID == Model.ParentID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Name)) list = list.Where(p => p.Name.Contains(Model.Name));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Describe)) list = list.Where(p => p.Describe.Contains(Model.Describe));
                //int
                if (Model.SortNo > 0) list = list.Where(p => p.SortNo == Model.SortNo);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_NODE
        //F_Search 
        public static IQueryable<T_SYS_NODE> F_Search(this IQueryable<T_SYS_NODE> list, T_SYS_NODE Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Code.Contains(KeyWord)
                               || p.Name.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Code)) list = list.Where(p => p.Code.Contains(Model.Code));
                //int
                if (Model.ParentID > 0) list = list.Where(p => p.ParentID == Model.ParentID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Name)) list = list.Where(p => p.Name.Contains(Model.Name));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Describe)) list = list.Where(p => p.Describe.Contains(Model.Describe));
                //int
                if (Model.SortNo > 0) list = list.Where(p => p.SortNo == Model.SortNo);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_NODE_ELEMENT
        //F_Search 
        public static IQueryable<T_SYS_NODE_ELEMENT> F_Search(this IQueryable<T_SYS_NODE_ELEMENT> list, T_SYS_NODE_ELEMENT Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Name.Contains(KeyWord)
                               || p.Value.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Name)) list = list.Where(p => p.Name.Contains(Model.Name));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Value)) list = list.Where(p => p.Value.Contains(Model.Value));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Describe)) list = list.Where(p => p.Describe.Contains(Model.Describe));
                //int
                if (Model.SortNo > 0) list = list.Where(p => p.SortNo == Model.SortNo);
                //int
                if (Model.NodeID > 0) list = list.Where(p => p.NodeID == Model.NodeID);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_PRIVIEGE
        //F_Search 
        public static IQueryable<T_SYS_PRIVIEGE> F_Search(this IQueryable<T_SYS_PRIVIEGE> list, T_SYS_PRIVIEGE Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            //if (!String.IsNullOrWhiteSpace(KeyWord))
            //{
            //    list = list.Where(p => false
            //               || p.TypeText.Contains(KeyWord)
            //        );
            //}
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.Type > 0) list = list.Where(p => p.Type == Model.Type);
                //nvarchar
                //if (!String.IsNullOrWhiteSpace(Model.TypeText)) list = list.Where(p => p.TypeText.Contains(Model.TypeText));
                //int
                if (Model.KeyID > 0) list = list.Where(p => p.KeyID == Model.KeyID);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_PRIVIEGE_ACTION
        //F_Search 
        public static IQueryable<T_SYS_PRIVIEGE_ACTION> F_Search(this IQueryable<T_SYS_PRIVIEGE_ACTION> list, T_SYS_PRIVIEGE_ACTION Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Name.Contains(KeyWord)
                               || p.Code.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Name)) list = list.Where(p => p.Name.Contains(Model.Name));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Code)) list = list.Where(p => p.Code.Contains(Model.Code));
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_ROLE
        //F_Search 
        public static IQueryable<T_SYS_ROLE> F_Search(this IQueryable<T_SYS_ROLE> list, T_SYS_ROLE Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Name.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Name)) list = list.Where(p => p.Name.Contains(Model.Name));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Describe)) list = list.Where(p => p.Describe.Contains(Model.Describe));
                //int
                if (Model.SortNo > 0) list = list.Where(p => p.SortNo == Model.SortNo);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_SYS_ROLE_refPRIVIEGE
        //F_Search 
        public static IQueryable<T_SYS_ROLE_refPRIVIEGE> F_Search(this IQueryable<T_SYS_ROLE_refPRIVIEGE> list, T_SYS_ROLE_refPRIVIEGE Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.KeyID > 0) list = list.Where(p => p.KeyID == Model.KeyID);
                //int
                if (Model.refKeyID > 0) list = list.Where(p => p.refKeyID == Model.refKeyID);
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.HasCancle != null) list = list.Where(p => p.HasCancle == Model.HasCancle);
                //datetime
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion


    }
}