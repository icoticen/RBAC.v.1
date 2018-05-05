using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VEHICLEDETECTING.Models;
using IKUS.LIB.MODEL;
using VEHICLEDETECTING.Models.PlugIn;
using IKUS.LIB;

namespace VEHICLEDETECTING.Functions
{
    public class fVersion
    {
        static FVERSION VERSION = new FVERSION();


        public M_Result Version_Current(Int32 AppSource, String Mac, String PhoneModel, Int32 VersionIndex, Int32 Channel)
        {
            var L = VERSION.Cache_Version_Update_Package().Where(p => p.VersionIndex > VersionIndex && p.UpdateDateTime <= DateTime.Now && (Channel > 0 ? p.Channel == Channel : true));
            var IsMust = L.Select(p => (p.IsMust ?? false) ? 1 : 0).Sum() > 0;
            var model = L.OrderByDescending(p => p.VersionIndex).FirstOrDefault();

            if (model == null) return new M_Result(E_ERRORCODE.空数据);

            return new M_Result(E_ERRORCODE.操作成功, "操作成功", new
            {
                VersionIndex = model.VersionIndex,
                DownloadLink = model.DownloadLink ?? "",
                UpdateDateTime = model.UpdateDateTime.Ex_ToString(),
                VersionName = model.VersionName ?? "",
                IsMust
            });
        }
        public M_Result Server_Current(Int32 AppSource, String Mac, String PhoneModel, Int32 VersionIndex, Int32 Channel)
        {
            var L = VERSION.Cache_Version_Server_Package();

            var Model = L.FirstOrDefault(p => p.VersionIndex == VersionIndex && p.AppSource == AppSource && p.Channel == Channel);
            if (Model == null) return new M_Result(E_ERRORCODE.空数据);
            return new M_Result(E_ERRORCODE.操作成功, "操作成功", new
            {
                ServerLocation = Config.LI_ServerLocation[Model.ServerLocation] ?? "",
                Model.VersionIndex,
                IsProduct = Model.ServerLocation == (Int32)Config.E_ServerLocation.生产服务器
            });
        }
    }
}