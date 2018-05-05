using System.Collections.Generic;
using System.Web.Mvc;
using IKUS.LIB.MODEL;
using SYS.Table;

namespace VEHICLEDETECTING.Functions
{
    public interface IfSys
    {
        List<T_SYS_BUTTON> Button_GetAllButtons_InUser(int EmpID, int MenuID = 0);
        T_SYS_BUTTON Button_GetButton_ByRoute(string AreaName, string ControllerName, string ActionName, string BtnName, int MenuID = 0);
        T_SYS_BUTTON Button_GetButton_ByValue(string Value, int MenuID = 0);
        List<int> Emp_GetAllDepartmentIDs_ByEmpID(int EmpID);
        List<int> Emp_GetAllDepartmentPositionIDs_ByEmpID(int EmpID);
        List<int> Emp_GetAllLowerDepartmentIDs_ByEmpID(int EmpID);
        List<int> Emp_GetAllLowerDepartmentPositionIDs_ByEmpID(int EmpID);
        List<int> Emp_GetAllLowerEmpIDs_ByEmpID(int EmpID);
        List<T_SYS_PRIVIEGE> Emp_GetAllPrivieges_ByEmpID(int EmpID, int Type = 0);
        T_SYS_EMP_INFO Emp_GetEmpInfo(int EmpID);
        M_Result Emp_Login(string AccountName, string AccountPsw);
        M_Result Emp_ResetPsw(int EmpID, string AccountPsw, string OldAccountPsw);
        List<T_SYS_MENU> Menu_GetAllMenus_InUser(int EmpID, string Root = "Administrator");
        T_SYS_MENU Menu_GetMenu_ByRoute(string AreaName, string ControllerName, string ActionName, string Root = "");
        T_SYS_MENU Menu_GetMenu_ByValue(string Value, string Root = "");
        List<SelectListItem> Node_GetDirectChildrenToSelectList_ByCode(string Code);
        List<SelectListItem> Node_GetElementsToSelectList_ByCode(string Code);
        T_SYS_NODE Node_GetNode(int NodeID);
        T_SYS_NODE Node_GetNode_ByCode(string Code);
    }
}