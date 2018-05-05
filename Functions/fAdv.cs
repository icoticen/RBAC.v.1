using IKUS.LIB.MODEL;
using PlugIn.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VEHICLEDETECTING.Models.PlugIn;

namespace VEHICLEDETECTING.Functions
{
    public class fAdv
    {
        static FADV ADV = new FADV();

        //广告-列表-所有
        public M_Result Adv_Package(Int32 AppSource, String Mac, String PhoneModel, String SiteName)
        {
            var data = ADV.Static_Items_Adv_Package_refSite(SiteName);
            var L = ADV.ceInit<T_Adv>(data.Select(p => p.ID));
            if (L == null) return new M_Result(E_ERRORCODE.数据库错误);
            if (L.Count() == 0) return new M_Result(E_ERRORCODE.空数据);
            var d = L.Select(p => p.Model).Where(p => p.ID > 0).Select(p => new
            {
                p.ID,
                FaceImage = p.FaceImage ?? "",
                Link = p.Link ?? "",
                Title = p.Title ?? "",
                Describe = p.Describe ?? "",
                AgreementType = p.AgreementType,
            }).ToList();
            return new M_Result(E_ERRORCODE.操作成功, DATA: d);
        }
        //广告-列表-所有
        public M_Result Adv_Package(Int32 AppSource, String Mac, String PhoneModel, Int32 SiteID)
        {
            var data = ADV.Static_Items_Adv_Package_refSite(SiteID);
            var L = ADV.ceInit<T_Adv>(data.Select(p => p.ID));
            if (L == null) return new M_Result(E_ERRORCODE.数据库错误);
            if (L.Count() == 0) return new M_Result(E_ERRORCODE.空数据);
            var d = L.Select(p => p.Model).Where(p => p.ID > 0).Select(p => new
            {
                p.ID,
                FaceImage = p.FaceImage ?? "",
                Link = p.Link ?? "",
                Title = p.Title ?? "",
                Describe = p.Describe ?? "",
                AgreementType = p.AgreementType,
            }).ToList();
            return new M_Result(E_ERRORCODE.操作成功, DATA: d);
        }
        //广告位-列表-所有
        public M_Result Site_Package(Int32 AppSource, String Mac, String PhoneModel)
        {
            var L = ADV.Cache_Adv_Site_Package();// A.Exec(EF => EF.T_Adv_Site.ToList());
            if (L == null) return new M_Result(E_ERRORCODE.数据库错误);
            if (L.Count() == 0) return new M_Result(E_ERRORCODE.空数据);
            var d = L.Select(p => new
            {
                p.ID,
                Name = p.Name ?? "",
            }).ToList();
            return new M_Result(E_ERRORCODE.操作成功, DATA: d);
        }

        public M_Result ActShow_Action(Int32 AppSource, Int32 AccountType, String Mac, String PhoneModel, Int32 UserID, Int32 AdvID)
        {
            var R = ADV.Adv_ActShow_Action(AppSource, AccountType, Mac, PhoneModel, UserID, AdvID, 1);
            return R;
        }
        public M_Result ActClick_Action(Int32 AppSource, Int32 AccountType, String Mac, String PhoneModel, Int32 UserID, Int32 AdvID)
        {
            var R = ADV.Adv_ActClick_Action(AppSource, AccountType, Mac, PhoneModel, UserID, AdvID, 1);
            return R;
        }

    }
}