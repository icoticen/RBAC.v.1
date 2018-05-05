using IKUS.LIB.MODEL;
using IKUS.LIB.WEB.MVC;
using SYS;
using SYS.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VEHICLEDETECTING.Models;

namespace VEHICLEDETECTING.Functions
{
    public class fSys : FunctionBase<FSYS>, IfSys
    {
        public T_SYS_EMP_INFO Emp_GetEmpInfo(Int32 EmpID)
        {
            return FACTHIS.Emp_GetEmpInfo_ByEmpID(EmpID);
        }
        public M_Result Emp_Login(String AccountName, String AccountPsw)
        {
            return FACTHIS.Emp_Login(AccountName, AccountPsw);
        }
        public M_Result Emp_ResetPsw(Int32 EmpID, String AccountPsw, String OldAccountPsw)
        {
            //AccountPsw = T_Crypt.MD5_String(AccountPsw).ToUpper();
            //OldAccountPsw = T_Crypt.MD5_String(OldAccountPsw).ToUpper();
            return FACTHIS.Emp_ResetPsw(EmpID, AccountPsw, OldAccountPsw);
        }


        public List<T_SYS_PRIVIEGE> Emp_GetAllPrivieges_ByEmpID(Int32 EmpID, Int32 Type = 0)
        {
            return FACTHIS.Emp_GetAllPrivieges_ByEmpID(EmpID, Type);
        }


        #region DEPARTMENT
        public List<Int32> Emp_GetAllDepartmentIDs_ByEmpID(Int32 EmpID)
        {
            return FACTHIS.Emp_GetAllDepartmentIDs_ByEmpID(EmpID);
        }
        public List<Int32> Emp_GetAllDepartmentPositionIDs_ByEmpID(Int32 EmpID)
        {
            return FACTHIS.Emp_GetAllDepartmentPositionIDs_ByEmpID(EmpID);
        }
        public List<Int32> Emp_GetAllLowerDepartmentIDs_ByEmpID(Int32 EmpID)
        {
            return FACTHIS.Emp_GetAllDepartmentIDs_ByEmpID(EmpID)
                .SelectMany(did => FACTHIS.Department_GetAllLowerDepartments_ByDepartmentID(did))
                .ToList();
        }
        public List<Int32> Emp_GetAllLowerDepartmentPositionIDs_ByEmpID(Int32 EmpID)
        {
            return FACTHIS.Emp_GetAllDepartmentPositionIDs_ByEmpID(EmpID)
                .SelectMany(dpid => FACTHIS.Department_GetAllLowerDepartmentPositions_ByDepartmentPositionID(dpid))
                .ToList();
        }
        public List<Int32> Emp_GetAllLowerEmpIDs_ByEmpID(Int32 EmpID)
        {
            return FACTHIS.Emp_GetAllLowerEmpIDs_ByEmpID(EmpID);
        } 
        #endregion





        public T_SYS_MENU Menu_GetMenu_ByValue(string Value, String Root = "")
        {
            Value = Value.ToUpper();
            return FACTHIS.Menu_GetAllMenus_ByRoot(Root)
                .FirstOrDefault(p => p.Value.ToUpper() == Value);
        }
        public T_SYS_MENU Menu_GetMenu_ByRoute(String AreaName, String ControllerName, String ActionName, String Root = "")
        {
            var Value = $"/{AreaName}/{ControllerName}/{ActionName}".ToUpper();
            return FACTHIS.Menu_GetAllMenus_ByRoot(Root)
                .FirstOrDefault(p => p.Value.ToUpper() == Value);
        }
        public T_SYS_BUTTON Button_GetButton_ByValue(String Value, Int32 MenuID = 0)
        {
            Value = Value.ToUpper();
            return FACTHIS.Button_GetAllButtons_ByRoot(MenuID)
                .FirstOrDefault(p => p.Value.ToUpper() == Value);
        }
        public T_SYS_BUTTON Button_GetButton_ByRoute(String AreaName, String ControllerName, String ActionName, String BtnName, Int32 MenuID = 0)
        {
            var Value = $"/{AreaName}/{ControllerName}/{ActionName}/{BtnName}".ToUpper();
            return FACTHIS.Button_GetAllButtons_ByRoot(MenuID).FirstOrDefault(p => p.Value.ToUpper() == Value);
        }
        public List<T_SYS_MENU> Menu_GetAllMenus_InUser(Int32 EmpID, String Root = "Administrator")
        {
            var lPriviegesMenu = FACTHIS.Emp_GetAllPrivieges_ByEmpID(EmpID)
                .Where(p => p.Type == (Int32)Config.E_Sys_Priviege_Type.MENU)
                .Select(p => p.KeyID).ToList();
            var L = FACTHIS.Menu_GetAllMenus_ByRoot(Root)
                .Where(p => lPriviegesMenu.Contains(p.ID))
                .ToList();
            return L;
        }
        public List<T_SYS_BUTTON> Button_GetAllButtons_InUser(Int32 EmpID, Int32 MenuID = 0)
        {
            var lPriviegesMenu = FACTHIS.Emp_GetAllPrivieges_ByEmpID(EmpID)
                .Where(p => p.Type == (Int32)Config.E_Sys_Priviege_Type.BUTTON)
                .Select(p => p.KeyID).ToList();
            var L = FACTHIS.Button_GetAllButtons_ByRoot(MenuID)
                .Where(p => lPriviegesMenu.Contains(p.ID))
                .ToList();
            return L;
        }

        public T_SYS_NODE Node_GetNode(Int32 NodeID)
        {
            return FACTHIS.ceInit<T_SYS_NODE>(NodeID).Model;
        }
        public T_SYS_NODE Node_GetNode_ByCode(String Code)
        {
            return FACTHIS.Node_GetNode_ByCode(Code);
        }

        public List<SelectListItem> Node_GetElementsToSelectList_ByCode(String Code)
        {
            var Node = FACTHIS.Node_GetNode_ByCode(Code);
            return FACTHIS.Node_GetElements(Node.ID)
                .OrderBy(p => p.SortIndex)
                .Select(p => new SelectListItem
                {
                    Text = p["Name"],
                    Value = p["Value"],
                }).ToList();
        }
        public List<SelectListItem> Node_GetDirectChildrenToSelectList_ByCode(String Code)
        {
            var Node = FACTHIS.Node_GetNode_ByCode(Code);
            return FACTHIS.Node_GetDirectChildren(Node.ID)
                .Select(p => FACTHIS.ceInit<T_SYS_NODE>(p.ID).Model)
                .Where(p => p.ID > 0)
                .OrderBy(p => p.SortNo)
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Code,
                }).ToList();
        }
    }
}