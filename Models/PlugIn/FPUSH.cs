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
    public class FPUSH
    {
    }
    public static partial class _F_SEARCH
    {
        #region T_Push
        //F_Search 
        public static IQueryable<T_Push> F_Search(this IQueryable<T_Push> list, T_Push Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Title.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Title)) list = list.Where(p => p.Title.Contains(Model.Title));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.FaceImage)) list = list.Where(p => p.FaceImage.Contains(Model.FaceImage));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.FaceVideo)) list = list.Where(p => p.FaceVideo.Contains(Model.FaceVideo));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Link)) list = list.Where(p => p.Link.Contains(Model.Link));
                //int
                if (Model.AgreementType > 0) list = list.Where(p => p.AgreementType == Model.AgreementType);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Describe)) list = list.Where(p => p.Describe.Contains(Model.Describe));
                //datetime
                //datetime
                //int
                if (Model.PushTimes > 0) list = list.Where(p => p.PushTimes == Model.PushTimes);
                //int
                if (Model.RemainTimes > 0) list = list.Where(p => p.RemainTimes == Model.RemainTimes);
                //int
                if (Model.FrequencySeconds > 0) list = list.Where(p => p.FrequencySeconds == Model.FrequencySeconds);
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //datetime
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
                //int
                if (Model.ShowCount > 0) list = list.Where(p => p.ShowCount == Model.ShowCount);
                //int
                if (Model.ClickCount > 0) list = list.Where(p => p.ClickCount == Model.ClickCount);
            }
            if (dt1.HasValue) list = list.Where(p => p.CreateDateTime >= dt1);
            if (dt2.HasValue) list = list.Where(p => p.CreateDateTime >= dt2);
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_Push_ActClick
        //F_Search 
        public static IQueryable<T_Push_ActClick> F_Search(this IQueryable<T_Push_ActClick> list, T_Push_ActClick Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Mac.Contains(KeyWord)
                               || p.IP.Contains(KeyWord)
                               || p.PhoneModel.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.AppSource > 0) list = list.Where(p => p.AppSource == Model.AppSource);
                //int
                if (Model.AccountType > 0) list = list.Where(p => p.AccountType == Model.AccountType);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Mac)) list = list.Where(p => p.Mac.Contains(Model.Mac));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.IP)) list = list.Where(p => p.IP.Contains(Model.IP));
                //int
                if (Model.KeyID > 0) list = list.Where(p => p.KeyID == Model.KeyID);
                //int
                if (Model.UserID > 0) list = list.Where(p => p.UserID == Model.UserID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.PhoneModel)) list = list.Where(p => p.PhoneModel.Contains(Model.PhoneModel));
                //datetime
                //bit
                if (Model.HasCancle != null) list = list.Where(p => p.HasCancle == Model.HasCancle);
                //datetime
            }
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_Push_ActShow
        //F_Search 
        public static IQueryable<T_Push_ActShow> F_Search(this IQueryable<T_Push_ActShow> list, T_Push_ActShow Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.Mac.Contains(KeyWord)
                               || p.IP.Contains(KeyWord)
                               || p.PhoneModel.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.AppSource > 0) list = list.Where(p => p.AppSource == Model.AppSource);
                //int
                if (Model.AccountType > 0) list = list.Where(p => p.AccountType == Model.AccountType);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Mac)) list = list.Where(p => p.Mac.Contains(Model.Mac));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.IP)) list = list.Where(p => p.IP.Contains(Model.IP));
                //int
                if (Model.KeyID > 0) list = list.Where(p => p.KeyID == Model.KeyID);
                //int
                if (Model.UserID > 0) list = list.Where(p => p.UserID == Model.UserID);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.PhoneModel)) list = list.Where(p => p.PhoneModel.Contains(Model.PhoneModel));
                //datetime
                //bit
                if (Model.HasCancle != null) list = list.Where(p => p.HasCancle == Model.HasCancle);
                //datetime
            }
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
    }
}