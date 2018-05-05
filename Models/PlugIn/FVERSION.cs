using IKUS.LIB;
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
    public class FVERSION:FactoryBase<EPlugIn>
    {
        //static Int32 CacheMaxLength = 120;
        //static Int32 CacheKeepSeconds = 600;
        private static List<T_Version_Update> F_Version_Update_Package()
        {

            return APlugIn.Exec(EF => EF.T_Version_Update
                .ToList(), false) ?? new List<T_Version_Update>();
        }
        public List<T_Version_Update> Cache_Version_Update_Package(Int32 CacheKeepSeconds=600)
        {
            var data = Cache.GetSlidingKeepCache("Version/Version_Update_Package", () => FVERSION.F_Version_Update_Package(), CacheKeepSeconds);
            return data;
        }

        private static List<T_Version_Server> F_Version_Server_Package()
        {
            return APlugIn.Exec(EF => EF.T_Version_Server
                .ToList(), false) ?? new List<T_Version_Server>();
        }
        public List<T_Version_Server> Cache_Version_Server_Package(Int32 CacheKeepSeconds = 600)
        {
            var data = Cache.GetSlidingKeepCache("Version/Version_Server_Package", () => FVERSION.F_Version_Server_Package(), CacheKeepSeconds);
            return data;
        }

        public M_Result Version_Current(Int32 VersionIndex, Int32 Channel)
        {
            var R = APlugIn.Exec(EF =>
            {
                var iList = EF.T_Version_Update
                      .Where(p => p.VersionIndex > VersionIndex && p.UpdateDateTime <= DateTime.Now)
                      .Select(p => new
                      {
                          p.VersionIndex,
                          p.IsMust,
                          p.DownloadLink,
                          p.UpdateDateTime,
                          p.VersionName,
                      })
                      .ToList();
                var IsMust = iList
                    .Select(p => (p.IsMust ?? false) ? 1 : 0)
                    .Sum() > 0;
                var model = iList.OrderByDescending(p => p.VersionIndex)
                    .FirstOrDefault();
                if (model != null)
                {
                    return new M_Result(E_ERRORCODE.操作成功, DATA: new
                    {
                        VersionIndex = model.VersionIndex,
                        DownloadLink = model.DownloadLink ?? "",
                        UpdateDateTime = model.UpdateDateTime.Ex_ToString(),
                        VersionName = model.VersionName ?? "",
                        IsMust
                    });
                }
                return new M_Result(E_ERRORCODE.空数据);
            }, false);
            return R;
        }
        public M_Result Server_Current(Int32 AppSource, Int32 VersionIndex, Int32 Channel)
        {
            var Model = APlugIn.Model<T_Version_Server>(p => p.VersionIndex == VersionIndex && p.AppSource == AppSource && p.Channel == Channel);
            if (Model == null) return new M_Result(E_ERRORCODE.空数据);
            return new M_Result(E_ERRORCODE.操作成功, DATA: new
            {
                ServerLocation = Config.LI_ServerLocation[Model.ServerLocation] ?? "",
                Model.VersionIndex,
                IsProduct = Model.ServerLocation == (Int32)Config.E_ServerLocation.生产服务器
            });
        }
    }
    public static partial class _F_SEARCH
    {
        #region T_Version_Server
        //F_Search 
        public static IQueryable<T_Version_Server> F_Search(this IQueryable<T_Version_Server> list, T_Version_Server Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
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
                if (Model.AppSource > 0) list = list.Where(p => p.AppSource == Model.AppSource);
                //int
                if (Model.Channel > 0) list = list.Where(p => p.Channel == Model.Channel);
                //int
                if (Model.VersionIndex > 0) list = list.Where(p => p.VersionIndex == Model.VersionIndex);
                //int
                if (Model.ServerLocation > 0) list = list.Where(p => p.ServerLocation == Model.ServerLocation);
            }
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
        #region T_Version_Update
        //F_Search 
        public static IQueryable<T_Version_Update> F_Search(this IQueryable<T_Version_Update> list, T_Version_Update Model = null, String KeyWord = "", DateTime? dt1 = null, DateTime? dt2 = null, List<Int32?> CreatorRange = null)
        {
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                list = list.Where(p => false
                           || p.VersionName.Contains(KeyWord)
                    );
            }
            if (Model != null)
            {
                //int
                if (Model.ID > 0) list = list.Where(p => p.ID == Model.ID);
                //int
                if (Model.Channel > 0) list = list.Where(p => p.Channel == Model.Channel);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.VersionName)) list = list.Where(p => p.VersionName.Contains(Model.VersionName));
                //int
                if (Model.VersionIndex > 0) list = list.Where(p => p.VersionIndex == Model.VersionIndex);
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.VersionDetails)) list = list.Where(p => p.VersionDetails.Contains(Model.VersionDetails));
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.FaceImage)) list = list.Where(p => p.FaceImage.Contains(Model.FaceImage));
                //real
                //bit
                if (Model.IsMust != null) list = list.Where(p => p.IsMust == Model.IsMust);
                //datetime
                //nvarchar
                if (!String.IsNullOrWhiteSpace(Model.DownloadLink)) list = list.Where(p => p.DownloadLink.Contains(Model.DownloadLink));
            }
            //if (dt1.HasValue) list = list.Where(p => p.DateTime >= dt1);
            //if (dt2.HasValue) list = list.Where(p => p.DateTime < dt2);

            //if (CreatorRange != null && CreatorRange.Count > 0) list = list.Where(p => CreatorRange.Contains(p.UserID));
            return list;
        }
        #endregion
    }
}