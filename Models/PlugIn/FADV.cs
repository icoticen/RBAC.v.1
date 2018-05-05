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
    public class FADV : FactoryBase<EAuth>
    {
        //static Int32 CacheMaxLength = 120;
        //static Int32 CacheKeepSeconds = 600;




        private static List<T_Adv_Site> F_Adv_Site_Package()
        {
            return APlugIn.Exec(EF => EF.T_Adv_Site
               .ToList(), false) ?? new List<T_Adv_Site>();
        }
        private static List<pItem> F_Adv_Package_refSite(Int32 SiteID)
        {
            var SortIndex = 0;
            return APlugIn.Exec(EF =>
            {
                return EF.T_Adv_refSite
                .Where(p => !(p.HasCancle ?? false))
                .Where(p => p.refKeyID == SiteID)
                .Join(EF.T_Adv
                .Where(p => p.ShelveDateTime == null || p.ShelveDateTime <= DateTime.Now)
                .Where(p => p.UnShelveDateTime == null || p.UnShelveDateTime > DateTime.Now)
                .Where(p => p.IsOpen ?? false)
                , p => p.KeyID, q => q.ID, (p, q) => new
                {
                    p.KeyID,
                    p.ID
                })
                .ToList();
            }, false)//.OrderBy(p => p.ID)
            .Select(p => new pItem
            {
                ID = p.KeyID,
                SortIndex = SortIndex++,
            }).ToList() ?? new List<pItem>();
        }


        public List<T_Adv_Site> Cache_Adv_Site_Package(Int32 CacheKeepSeconds =600)
        {
            var data = Cache.GetAbsoluteKeepCache("Adv/Cache_Adv_Site_Package", () => FADV.F_Adv_Site_Package(), CacheKeepSeconds);
            return data;
        }

        public List<pItem> Static_Items_Adv_Package_refSite(Int32 SiteID, Int32 CacheKeepSeconds = 600)
        {
            var data = Cache.Items.Get("Adv/Static_Items_Adv_Package_refSite/" + SiteID, () => FADV.F_Adv_Package_refSite(SiteID), CacheKeepSeconds);
            return data;
        }
        public List<pItem> Static_Items_Adv_Package_refSite(String SiteName)
        {
            var SiteID = (Cache_Adv_Site_Package().FirstOrDefault(p => p.Name == SiteName) ?? new T_Adv_Site()).ID;
            return Static_Items_Adv_Package_refSite(SiteID);
        }


        public M_Result Adv_ActShow_Action(Int32 AppSource, Int32 AccountType, String Mac, String PhoneModel, Int32 UserID, Int32 AdvID, Int32 AdditionCount = 1)
        {
            var R = APlugIn.Exec(EF =>
            {
                var M = EF.T_Adv.FirstOrDefault(p => p.ID == AdvID);
                if (M == null) return new M_Result(E_ERRORCODE.服务端_操作_资源不存在, DATA: AdvID);
                M.ShowCount += AdditionCount;
                var LOG = EF.T_Adv_ActShow.Add(new T_Adv_ActShow
                {
                    AppSource = AppSource,
                    AccountType = AccountType,
                    KeyID = AdvID,
                    ActionDateTime = DateTime.Now,
                    PhoneModel = PhoneModel,
                    IP = T_Web.GetWebClientIp(),
                    Mac = Mac,
                    UserID = UserID,
                });
                EF.SaveChanges();
                //==========
                ceRefresh(M);

                return new M_Result(E_ERRORCODE.操作成功, DATA: LOG);
            }, false);
            return R;
        }
        public M_Result Adv_ActClick_Action(Int32 AppSource, Int32 AccountType, String Mac, String PhoneModel, Int32 UserID, Int32 AdvID, Int32 AdditionCount = 1)
        {
            var R = APlugIn.Exec(EF =>
            {
                var M = EF.T_Adv.FirstOrDefault(p => p.ID == AdvID);
                if (M == null) return new M_Result(E_ERRORCODE.服务端_操作_资源不存在, DATA: AdvID);

                M.ClickCount += AdditionCount;
                var LOG = EF.T_Adv_ActClick.Add(new T_Adv_ActClick
                {
                    AppSource = AppSource,
                    AccountType = AccountType,
                    KeyID = AdvID,
                    ActionDateTime = DateTime.Now,
                    PhoneModel = PhoneModel,
                    IP = T_Web.GetWebClientIp(),
                    Mac = Mac,
                    UserID = UserID,
                });
                EF.SaveChanges();

                //==========
                ceRefresh(M);

                return new M_Result(E_ERRORCODE.操作成功, DATA: LOG);
            }, false);
            return R;
        }
    }
    public static partial class _F_SEARCH
    {
        #region T_Adv
        //F_Search 
        public static IQueryable<T_Adv> F_Search(this IQueryable<T_Adv> list, T_Adv Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
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
        #region T_Adv_ActClick
        //F_Search 
        public static IQueryable<T_Adv_ActClick> F_Search(this IQueryable<T_Adv_ActClick> list, T_Adv_ActClick Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
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
        #region T_Adv_ActShow
        //F_Search 
        public static IQueryable<T_Adv_ActShow> F_Search(this IQueryable<T_Adv_ActShow> list, T_Adv_ActShow Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
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
        #region T_Adv_refSite
        //F_Search 
        public static IQueryable<T_Adv_refSite> F_Search(this IQueryable<T_Adv_refSite> list, T_Adv_refSite Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
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
        #region T_Adv_Site
        //F_Search 
        public static IQueryable<T_Adv_Site> F_Search(this IQueryable<T_Adv_Site> list, T_Adv_Site Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
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
                //datetime
                //int
                if (Model.CreateAdminID > 0) list = list.Where(p => p.CreateAdminID == Model.CreateAdminID);
                //bit
                if (Model.IsOpen != null) list = list.Where(p => p.IsOpen == Model.IsOpen);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.Note)) list = list.Where(p => p.Note.Contains(Model.Note));
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