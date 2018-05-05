using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models.Static
{
    public abstract class ConfigBase
    {

        public static String LocalHostAuthority
        {
            get
            {
                var S = System.Web.Configuration.WebConfigurationManager.AppSettings["LocalHostAuthority"];
                return String.IsNullOrEmpty(S) ? "http://" + System.Web.HttpContext.Current.Request.Url.Authority : S;
            }
        }
        public static String AppSettings(String Key)
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings[Key] ?? "";
        }

        public static readonly string DefaultMacByWeb = "This Is From Web";
        public static readonly string DefaultPhoneModelByWeb = "This Is From Web";
        public static readonly E_AppSource DefaultAppSourceByWeb = E_AppSource._Web;
        public static readonly E_AccountType DefaultAccountTypeByWeb = E_AccountType._WEB_网页未登录;


        public static Int32 APPLICATIONID { get { return 17062; } }
        public static String APPLICATIONNAME { get { return "CHIGUADASHU-DALI"; } }
        public static String MD5_ADDITIONKEY { get { return "XINSISHUANGSIWANGZHONGYOUQIANQIANJIE"; } }

        #region LSelectListItem
        public static List<System.Web.Mvc.SelectListItem> GetList<T>() where T : struct
        {
            var List = new List<System.Web.Mvc.SelectListItem>();
            foreach (var Item in Enum.GetValues(typeof(T)))
            {
                var Text = ((T)Item).ToString();
                var Value = (Int32)Item + "";
                List.Add(new System.Web.Mvc.SelectListItem
                {
                    Text = Text,
                    Value = Value,
                });
            }
            return List;
        }
        public static List<System.Web.Mvc.SelectListItem> GetList<T, V>()
            where T : struct
            where V : struct
        {
            var List = new List<System.Web.Mvc.SelectListItem>();
            foreach (var Item in Enum.GetValues(typeof(T)))
            {
                var Text = ((T)Item).ToString();
                var Value = (Int32)Item + "";
                List.Add(new System.Web.Mvc.SelectListItem
                {
                    Text = Text,
                    Value = Value,
                });
            }
            return List;
        }
        public static List<System.Web.Mvc.SelectListItem> LI_BOOLEAN
        {
            get
            {
                return new List<System.Web.Mvc.SelectListItem>{
                    new System.Web.Mvc.SelectListItem{Text="--  否  --",Value="FALSE"},
                    new System.Web.Mvc.SelectListItem{Text="--  是  --",Value="TRUE"},
                };
            }

        }
        #endregion

        #region Account
        //账号类型
        public static Int32 AccountType_User = 1;
        public static Int32 AccountType_Phone = 2;
        public static Int32 AccountType_Mac = 99;

        //应用类型 android？iPhone？
        public static Int32 AppSource_Android = 1;
        public static Int32 AppSource_iPhone = 2;
        public static Int32 AppSource_Web = 3;
        public enum E_AppSource
        {
            _Android = 1,
            _iPhone = 2,
            _Web = 3
        }
        public enum E_AccountType
        {
            _User_用户名 = 1,
            _Phone_手机 = 2,
            _Mac_设备码 = 99,

            _WEB_网页未登录 = 100,
            _YD_官方账户 = -99,
        }
        public enum E_AccountStatus
        {
            Success_正常 = 3,
            Warming_警告 = 4,
            Danger_危险 = 5,
            UnNormal_异常 = 6,
            Forbidden_禁言 = 7,//禁言
            Failure_封号 = 9,//封号
        }

        public enum E_Promoter_Identify
        {
            _User_用户 = 1,
            _Emp_管理员 = 2,
            _Promoter_推广员 = 3,
            _Null_自增用户 = 9,
        }

        #endregion

        #region SMS
        //验证码发送类型
        public static Int32 SMS_PhoneVerificationActionType_Register = 1;
        public static Int32 SMS_PhoneVerificationActionType_ResetPsw = 2;
        public enum E_SMS_PhoneVerification_ActionType
        {
            手机注册 = 1,
            找回密码 = 2,
        }
        #endregion

        public enum E_Trade_Action
        {
            文章_文章_举报 = 011107,
            文章_文章_审核 = 011108,
            文章_文章_分享 = 011109,
            文章_文章_打赏 = 011111,
            文章_文章_加精 = 011121,
            文章_文章_违规 = 111131,
            文章_文章_被评论 = 011192,
            文章_文章_被分享 = 011199,
            文章_评论_发布 = 011201,
            文章_评论_被点赞_赞 = 01129301,
            文章_评论_神评 = 011222,
            文章_文章_发布_段子 = 01110101,
            文章_文章_发布_音频 = 01110103,
            文章_文章_发布_视频 = 01110104,
            文章_文章_发布_图片 = 01110122,
            文章_文章_发布_文本图组 = 0111010122,
            文章_文章_点赞_赞 = 01110301,
            文章_文章_点赞_踩 = 01110302,
            文章_文章_被点赞_赞 = 01119301,
            图集_图集_购买 = 120101,
            吃瓜_文章_打赏 = 131111,
            吃瓜_评论_发布 = 031201,

            我的_日常_签到 = 050101,

            我的_关于_分享 = 050909,

            系统_奖励_发放 = 0901,
            系统_奖励_扣除 = 1901,
            //系统_补偿发放 = 0902,

        }
        public enum E_Trade_Type
        {
            在线支付 = 1,
            线下支付 = 2,
            其他 = 9,
            支付宝 = 101,
            微信支付 = 102,
            苹果支付 = 103,
            货到付款 = 201,
            CDKey兑换 = 901,
        }
        public enum E_Trade_Status
        {
            Creat_支付创建 = 1,
            Waitting_等待支付 = 2,
            Success_支付成功 = 3,
            Error_支付失败 = 4,
            Failure_支付出现错误 = 99,
        }

        public enum E_Logistics_Status
        {
            已提交 = 1,
            待付款 = 2,
            已付款 = 3,
            已取消 = 4,

            已确认 = 5,
            待发货 = 7,
            已发货 = 8,
            已失败 = 9,

            已完成 = 10,
        }

        public enum E_Content_Status
        {
            新创建 = 1,
            被举报 = 2,
            已公开 = 3,
            已屏蔽 = 4,
            待审核 = 6,
            审核通过 = 63,
            审核不通过 = 64,

            已删除 = 9,

        }

        public enum E_Sys_Priviege_Type
        {
            MENU = 101,
            BUTTON = 10101,
            //PROPERTY = 10102,
            //FILE = 3,
        }


        public enum E_ServerLocation
        {
            生产服务器 = 1,
            测试服务器 = 2,
        }
        public enum E_Channel
        {
            _Android = 1,


            _Android_应用宝 = 101,
            _Android_百度 = 102,
            _Android_360 = 103,
            _Android_安智 = 104,
            _Android_VIVO = 105,
            _Android_机锋网 = 106,
            _Android_华为 = 107,
            _Android_小米 = 108,
            _Android_联想 = 109,
            _Android_搜狗 = 110,
            _Android_淘宝 = 111,
            _Android_OPPO = 112,
            _Android_木蚂蚁 = 113,

            _iPhone = 2,
            //_Web = 3
        }
    }
}