using IKUS.LIB.MODEL;
using IKUS.LIB.TOOL;
using SYS;
using SYS.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Functions
{
    public class fAccount
    {
        FSYS SYS = new FSYS();
        public T_SYS_EMP_INFO Emp_GetEmpInfo(Int32 EmpID)
        {
            return SYS.Emp_GetEmpInfo_ByEmpID(EmpID);
        }
        public M_Result Emp_Login(String AccountName, String AccountPsw)
        {
            AccountPsw = T_Crypt.MD5_String(AccountPsw);
            return SYS.Emp_Login(AccountName, AccountPsw);
        }
    }
}