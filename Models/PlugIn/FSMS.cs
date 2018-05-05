using IKUS.LIB.CACHE;
using IKUS.LIB.MODEL;
using IKUS.LIB.TOOL;
using IKUS.LIB.WEB.MVC;
using PlugIn.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models.PlugIn
{
    public class FSMS
    {
    }
    public static partial class _F_SEARCH
    {
        #region T_SMS_Verification
        //F_Search 
        public static IQueryable<T_SMS_Verification> F_Search(this IQueryable<T_SMS_Verification> list, T_SMS_Verification Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.PhoneNum.Contains(KeyWord)
                               || p.Code.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.PhoneNum)) list = list.Where(p => p.PhoneNum.Contains(Model.PhoneNum));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Code)) list = list.Where(p => p.Code.Contains(Model.Code));
                //int
                if (Model.ActionType > 0) list = list.Where(p => p.ActionType == Model.ActionType);
                //datetime
                //bit
                if (Model.HasUsed != null) list = list.Where(p => p.HasUsed == Model.HasUsed);
            }
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
    }
}