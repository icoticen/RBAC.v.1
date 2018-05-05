using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IKUS.LIB.MODEL;
using IKUS.LIB.WEB.MVC;
using System.Web.Mvc;
using IKUS.LIB.TOOL;
using IKUS.LIB.WEB.PLUGIN;
using IKUS.LIB;
using System.Data.Entity;
using IKUS.LIB.TABLE;
using VEHICLEDETECTING.Functions;

namespace VEHICLEDETECTING.Models
{
    //public abstract class myControllerAdmin<FUNCSYS> : myController.WEB where FUNCSYS:IfSys,new()
    //{
    //    protected FUNCSYS FunctionSys { get; set; } = new FUNCSYS();
    //    protected override M_Validate.IConfigWEB VALIDATECFG { get; set; } = new DefaultValidateConfigAdmin();
    //    #region AdminEmpAccount
    //    public virtual ActionResult _LogOut(String BackURL = null)
    //    {
    //        REQ.UserID = 0;
    //        return Redirect($"/Admin/Main/_LogIn?BackURL={Url.Encode(BackURL ?? "/Admin/Main/Index")}");
    //    }
    //    [HttpGet]
    //    public ActionResult _LogIn(String BackURL)
    //    {
    //        return View();
    //    }
    //    [HttpPost]
    //    public virtual ActionResult _LogIn(string AccountName, string AccountPsw, String BackURL)
    //    {
    //        BackURL = BackURL ?? "/Admin/Main/Index";
    //        if (Request.RequestType == "POST")
    //        {
    //            if (string.IsNullOrWhiteSpace(AccountName) || string.IsNullOrWhiteSpace(AccountPsw))
    //            {
    //                Response.Write("<script>alert('用户名密码不能为空~~~~~！');</script>");
    //                return View();
    //            }
    //            AccountPsw = T_Crypt.MD5_String(AccountPsw).ToUpper();
    //            var R = FunctionSYS?.Emp_Login(AccountName, AccountPsw) ?? new M_Result(E_ERRORCODE.操作成功, DATA: 1);
    //            if (R.result != (Int32)E_ERRORCODE.操作成功)
    //            {
    //                Response.Write("<script>alert('" + R.msg + "~~~~~！');</script>");
    //                return View();
    //            }
    //            else
    //            {
    //                REQ.UserID = (Int32)R.data;
    //                return Redirect(BackURL);
    //            }
    //        }
    //        return View();
    //    }
    //    public virtual ActionResult _ResetPsw(String OPSW = "", string NPSW = "", string NPSWR = "")
    //    {
    //        var LayData = new LayUI.Data();
    //        #region Properties
    //        LayData.AddProperty("OPSW", LayUI.E_Property_Type.@text, "原密码");
    //        LayData.AddProperty("NPSW", LayUI.E_Property_Type.@text, "新密码");
    //        LayData.AddProperty("NPSWR", LayUI.E_Property_Type.@text, "重复密码");

    //        #endregion
    //        #region Nodes

    //        #endregion
    //        #region Model
    //        LayData.AddModel("OPSW", OPSW);
    //        LayData.AddModel("NPSW", NPSW);
    //        LayData.AddModel("NPSWR", NPSWR);


    //        #endregion


    //        if (Request.RequestType == "POST")
    //        {
    //            if (String.IsNullOrWhiteSpace(OPSW) || String.IsNullOrWhiteSpace(NPSW))
    //            {
    //                Response.Write("<script>alert('密码不能为空~~~~~！');</script>");
    //                return View("Save", LayData);
    //            }
    //            if (NPSW != NPSWR)
    //            {
    //                Response.Write("<script>alert('重复密码不一致~~~~~！');</script>");
    //                return View("Save", LayData);
    //            }


    //            NPSW = T_Crypt.MD5_String(NPSW).ToUpper();
    //            OPSW = T_Crypt.MD5_String(OPSW).ToUpper();
    //            var R = FunctionSYS?.Emp_ResetPsw(REQ.UserID, NPSW, OPSW) ?? new M_Result(E_ERRORCODE.其他错误, MSG: "配置参数未初始化");
    //            Response.Write("<script>alert('" + R.msg + "~~~~~！');</script>");
    //            if (R.result == 1) REQ.UserID = 0;
    //            //return _LogOut();
    //        }
    //        return View("Save", LayData);
    //    }

    //    #endregion
    //}
    public abstract class myControllerAdmin : myController.WEB
    {
        protected static IfSys FunctionSYS { get; set; } = new fSys() ;
        protected override M_Validate.IConfigWEB VALIDATECFG { get; set; } = new DefaultValidateConfigAdmin();
        #region AdminEmpAccount
        public virtual ActionResult _LogOut(String BackURL = null)
        {
            REQ.UserID = 0;
            return Redirect($"/Admin/Main/_LogIn?BackURL={Url.Encode(BackURL ?? "/Admin/Main/Index")}");
        }
        [HttpGet]
        public ActionResult _LogIn(String BackURL)
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult _LogIn(string AccountName, string AccountPsw, String BackURL)
        {
            BackURL = BackURL ?? "/Admin/Main/Index";
            if (Request.RequestType == "POST")
            {
                if (string.IsNullOrWhiteSpace(AccountName) || string.IsNullOrWhiteSpace(AccountPsw))
                {
                    Response.Write("<script>alert('用户名密码不能为空~~~~~！');</script>");
                    return View();
                }
                AccountPsw = T_Crypt.MD5_String(AccountPsw).ToUpper();
                var R = FunctionSYS?.Emp_Login(AccountName, AccountPsw) ?? new M_Result(E_ERRORCODE.操作成功, DATA: 1);
                if (R.result != (Int32)E_ERRORCODE.操作成功)
                {
                    Response.Write("<script>alert('" + R.msg + "~~~~~！');</script>");
                    return View();
                }
                else
                {
                    REQ.UserID = (Int32)R.data;
                    return Redirect(BackURL);
                }
            }
            return View();
        }
        public virtual ActionResult _ResetPsw(String OPSW = "", string NPSW = "", string NPSWR = "")
        {
            var LayData = new LayUI.Data();
            #region Properties
            LayData.AddProperty("OPSW", LayUI.E_Property_Type.@text, "原密码");
            LayData.AddProperty("NPSW", LayUI.E_Property_Type.@text, "新密码");
            LayData.AddProperty("NPSWR", LayUI.E_Property_Type.@text, "重复密码");

            #endregion
            #region Nodes

            #endregion
            #region Model
            LayData.AddModel("OPSW", OPSW);
            LayData.AddModel("NPSW", NPSW);
            LayData.AddModel("NPSWR", NPSWR);


            #endregion


            if (Request.RequestType == "POST")
            {
                if (String.IsNullOrWhiteSpace(OPSW)|| String.IsNullOrWhiteSpace(NPSW))
                {
                    Response.Write("<script>alert('密码不能为空~~~~~！');</script>");
                    return View("Save", LayData);
                }
                if (NPSW != NPSWR)
                {
                    Response.Write("<script>alert('重复密码不一致~~~~~！');</script>");
                    return View("Save", LayData);
                }


                NPSW = T_Crypt.MD5_String(NPSW).ToUpper();
                OPSW = T_Crypt.MD5_String(OPSW).ToUpper();
                var R = FunctionSYS?.Emp_ResetPsw(REQ.UserID, NPSW, OPSW) ?? new M_Result(E_ERRORCODE.其他错误, MSG: "配置参数未初始化");
                Response.Write("<script>alert('" + R.msg + "~~~~~！');</script>");
                if (R.result == 1) REQ.UserID = 0;
                //return _LogOut();
            }
            return View("Save", LayData);
        }

        #endregion
    }
    public abstract class myControllerAuth : myController.WEB
    {
        protected IfSys FunctionSYS { get; set; } 
        protected override M_Validate.IConfigWEB VALIDATECFG { get; set; } = new DefaultValidateConfigAuth();

        #region AdminEmpAccount
        public virtual ActionResult _LogOut(String BackURL = null)
        {
            REQ.UserID = 0;
            return Redirect($"/Admin/Main/_LogIn?BackURL={Url.Encode(BackURL ?? "/Admin/Main/Index")}");
        }
        //[HttpGet]
        //public ActionResult _LogIn(String BackURL)
        //{
        //    return View();
        //}
        //[HttpPost]
        //public virtual ActionResult _LogIn(string AccountName, string AccountPsw, String BackURL)
        //{
        //    BackURL = BackURL ?? "/Admin/Main/Index";
        //    if (Request.RequestType == "POST")
        //    {
        //        if (string.IsNullOrWhiteSpace(AccountName) || string.IsNullOrWhiteSpace(AccountPsw))
        //        {
        //            Response.Write("<script>alert('用户名密码不能为空~~~~~！');</script>");
        //            return View();
        //        }
        //        AccountPsw = T_Crypt.MD5_String(AccountPsw).ToUpper();
        //        TODO TO FILL FSYS
        //        var R = new M_Result(E_ERRORCODE.操作成功, DATA: 1);// FunctionSYS.Emp_Login(AccountName, AccountPsw);
        //        if (R.result != (Int32)E_ERRORCODE.操作成功)
        //        {
        //            Response.Write("<script>alert('" + R.msg + "~~~~~！');</script>");
        //            return View();
        //        }
        //        else
        //        {
        //            REQ.UserID = (Int32)R.data;
        //            return Redirect(BackURL);
        //        }
        //    }
        //    return View();
        //}
        //public virtual ActionResult _ResetPsw(String OPSW = "", string NPSW = "", string NPSWR = "")
        //{
        //    var LayData = new LayUI.Data();
        //    #region Properties
        //    LayData.AddProperty("OPSW", LayUI.E_Property_Type.@text, "原密码");
        //    LayData.AddProperty("NPSW", LayUI.E_Property_Type.@text, "新密码");
        //    LayData.AddProperty("NPSWR", LayUI.E_Property_Type.@text, "重复密码");

        //    #endregion
        //    #region Nodes

        //    #endregion
        //    #region Model
        //    LayData.AddModel("OPSW", OPSW);
        //    LayData.AddModel("NPSW", OPSW);
        //    LayData.AddModel("NPSWR", OPSW);


        //    #endregion


        //    if (Request.RequestType == "POST")
        //    {
        //        if (NPSW != NPSWR)
        //        {
        //            Response.Write("<script>alert('重复密码不一致~~~~~！');</script>");
        //            return View("Save", LayData);
        //        }


        //        NPSW = T_Crypt.MD5_String(NPSW).ToUpper();
        //        OPSW = T_Crypt.MD5_String(OPSW).ToUpper();
        //        var R = FunctionSYS.Emp_ResetPsw(REQ.UserID, NPSW, OPSW);

        //        Response.Write("<script>alert('" + R.msg + "~~~~~！');</script>");
        //        return _LogOut();
        //    }
        //    return View("Save", LayData);
        //}
        #endregion

    }
    public abstract class myControllerAnalysis : myController.WEB
    {
        protected IfSys FunctionSYS { get; set; }
        protected override M_Validate.IConfigWEB VALIDATECFG { get; set; } = new DefaultValidateConfigAnalysis();

        #region AdminEmpAccount
        public virtual ActionResult _LogOut(String BackURL = null)
        {
            REQ.UserID = 0;
            return Redirect($"/Admin/Main/_LogIn?BackURL={Url.Encode(BackURL ?? "/Admin/Main/Index")}");
        }
        #endregion
        public LayUI.Data Analysis_LayData_Default<T>(T Model, String KeyWord, DateTime? dt1 = null, DateTime? dt2 = null, String DateTimeFormat = "yyyy-MM-dd",
            String SAppSource = null,
            String SChannel = null,
            String SAccountType = null,
            String SApplicationID = null,
            String SUserID = null,
            String SKeyID = null)
            where T : class, IActBase, new()
        {
            var RAppSource = SAppSource.Ex_ToInt32List();
            var RAccountType = SAccountType.Ex_ToInt32List();
            var RChannel = SChannel.Ex_ToInt32List();
            var RApplicationID = SApplicationID.Ex_ToInt32List();
            var RUserID = SUserID.Ex_ToInt32List();
            var RKeyID = SKeyID.Ex_ToInt32List();

            dt1 = dt1 ?? DateTime.Now.Date.AddDays(-7);
            dt2 = dt2 ?? DateTime.Now.AddDays(1);

            var LayData = new LayUI.Data();

            LayData.AddProperty("SApplicationID", LayUI.E_Property_Type.text, "ApplicationIDRange");
            LayData.AddProperty("SUserID", LayUI.E_Property_Type.text, "UserIDRange");
            LayData.AddProperty("SKeyID", LayUI.E_Property_Type.text, "KeyIDRange");
            LayData.AddProperty("IP", LayUI.E_Property_Type.text, "IP");
            LayData.AddProperty("Mac", LayUI.E_Property_Type.text, "Mac");
            LayData.AddProperty("PhoneModel", LayUI.E_Property_Type.text, "PhoneModel");
            LayData.AddProperty("VersionCode", LayUI.E_Property_Type.text, "VersionCode");

            LayData.AddProperty("SAppSource", LayUI.E_Property_Type.echartoption, "AppSource");
            LayData.AddProperty("SChannel", LayUI.E_Property_Type.echartoption, "Channel");
            LayData.AddProperty("SAccountType", LayUI.E_Property_Type.echartoption, "AccountType");

            LayData.AddNode("SAccountType", Config.GetList<Config.E_AccountType>());
            LayData.AddNode("SAppSource", Config.GetList<Config.E_AppSource>());
            LayData.AddNode("SChannel", Config.GetList<Config.E_Channel>());

            LayData.AddModel("SApplicationID", string.Join(",", RApplicationID ?? new List<int>()));
            LayData.AddModel("SUserID", string.Join(",", RUserID ?? new List<int>()));
            LayData.AddModel("SKeyID", string.Join(",", RKeyID ?? new List<int>()));
            LayData.AddModel("IP", Model.IP);
            LayData.AddModel("Mac", Model.Mac);
            LayData.AddModel("PhoneModel", Model.PhoneModel);
            LayData.AddModel("VersionCode", Model.VersionCode);
            LayData.AddModel("dt1", dt1.Ex_ToString("yyyy-MM-dd"));
            LayData.AddModel("dt2", dt2.Ex_ToString("yyyy-MM-dd"));
            LayData.AddModel("KeyWord", KeyWord);

            return LayData;
        }
        public LayUI.Data Analysis_LayData_NonEChartOption<T>(T Model, String KeyWord, DateTime? dt1 = null, DateTime? dt2 = null, String DateTimeFormat = "yyyy-MM-dd",
            String SAppSource = null,
            String SChannel = null,
            String SAccountType = null,
            String SApplicationID = null,
            String SUserID = null,
            String SKeyID = null)
        where T : class, IActBase, new()
        {
            var RAppSource = SAppSource.Ex_ToInt32List();
            var RAccountType = SAccountType.Ex_ToInt32List();
            var RChannel = SChannel.Ex_ToInt32List();
            var RApplicationID = SApplicationID.Ex_ToInt32List();
            var RUserID = SUserID.Ex_ToInt32List();
            var RKeyID = SKeyID.Ex_ToInt32List();

            dt1 = dt1 ?? DateTime.Now.Date.AddDays(-7);
            dt2 = dt2 ?? DateTime.Now.AddDays(1);

            var LayData = new LayUI.Data();

            LayData.AddProperty("SApplicationID", LayUI.E_Property_Type.text, "ApplicationIDRange");
            LayData.AddProperty("SUserID", LayUI.E_Property_Type.text, "UserIDRange");
            LayData.AddProperty("SKeyID", LayUI.E_Property_Type.text, "KeyIDRange");
            LayData.AddProperty("IP", LayUI.E_Property_Type.text, "IP");
            LayData.AddProperty("Mac", LayUI.E_Property_Type.text, "Mac");
            LayData.AddProperty("PhoneModel", LayUI.E_Property_Type.text, "PhoneModel");
            LayData.AddProperty("VersionCode", LayUI.E_Property_Type.text, "VersionCode");

            //LayData.AddProperty("SAppSource", LayUI.E_Property_Type.echartoption, "AppSource");
            //LayData.AddProperty("SChannel", LayUI.E_Property_Type.echartoption, "Channel");
            //LayData.AddProperty("SAccountType", LayUI.E_Property_Type.echartoption, "AccountType");

            //LayData.AddNode("SAccountType", Config.GetList<Config.E_AccountType>());
            //LayData.AddNode("SAppSource", Config.GetList<Config.E_AppSource>());
            //LayData.AddNode("SChannel", Config.GetList<Config.E_Channel>());

            LayData.AddModel("SApplicationID", string.Join(",", RApplicationID ?? new List<int>()));
            LayData.AddModel("SUserID", string.Join(",", RUserID ?? new List<int>()));
            LayData.AddModel("SKeyID", string.Join(",", RKeyID ?? new List<int>()));
            LayData.AddModel("IP", Model.IP);
            LayData.AddModel("Mac", Model.Mac);
            LayData.AddModel("PhoneModel", Model.PhoneModel);
            LayData.AddModel("VersionCode", Model.VersionCode);
            LayData.AddModel("dt1", dt1.Ex_ToString("yyyy-MM-dd"));
            LayData.AddModel("dt2", dt2.Ex_ToString("yyyy-MM-dd"));
            LayData.AddModel("KeyWord", KeyWord);

            return LayData;
        }
    }
    public abstract class myControllerAPI : myController.API
    {
        protected override M_Validate.IConfigAPI VALIDATECFG { get; set; } = new DefaultValidateConfigAPI();

        public override ActionResult Image_FilePost(string FileElementName = "file", string FileType = "image")
        {
            T_Log.LogUpload("API/Image_FilePost");
            var R = PostFiles_SaveToServer(FileElementName, PreStr: FileElementName, FileType: FileType);
            return Content(R.Ex_ToJson());
        }
    }
    public abstract class myControllerWeb : myController.WEB
    {
        protected override M_Validate.IConfigWEB VALIDATECFG { get; set; } = new DefaultValidateConfigWeb();

    }
    public class DefaultValidateConfigAdmin : M_Validate.DefaultConfigWEB
    {
        public DefaultValidateConfigAdmin()
        {
            this.CookieName = "_Admin";
            this.CookieTimeOutSeconds = 0;
            this.TokenTimeOutSeconds = 0;

            this.CfgToken = new DefaultValidateConfigToken();
        }
    }
    public class DefaultValidateConfigAuth : M_Validate.DefaultConfigWEB
    {
        public DefaultValidateConfigAuth()
        {
            this.CookieName = "_Admin";
            this.CookieTimeOutSeconds = 0;
            this.TokenTimeOutSeconds = 0;

            this.CfgToken = new DefaultValidateConfigToken();
        }
    }
    public class DefaultValidateConfigAnalysis : M_Validate.DefaultConfigWEB
    {
        public DefaultValidateConfigAnalysis()
        {
            this.CookieName = "_Admin";
            this.CookieTimeOutSeconds = 0;
            this.TokenTimeOutSeconds = 0;

            this.CfgToken = new DefaultValidateConfigToken();
        }
    }
    public class DefaultValidateConfigAPI : M_Validate.DefaultConfigAPI
    {
        public DefaultValidateConfigAPI()
        {
            this.MD5_ADDITIONKEY = "RENMIANBUZHIHECHUQUTAOHUAYIJIUXIAOCHUNFENG";
            this.RequestTimeOutSeconds = 0;
            this.TokenTimeOutSeconds = 0;

            this.CfgToken = new DefaultValidateConfigToken();
        }
    }
    public class DefaultValidateConfigWeb : M_Validate.DefaultConfigWEB
    {
        public DefaultValidateConfigWeb()
        {
            this.CookieName = "_Admin";
            this.CookieTimeOutSeconds = 0;
            this.TokenTimeOutSeconds = 0;

            this.CfgToken = new DefaultValidateConfigToken();
        }
    }
    public class DefaultValidateConfigToken : M_Validate.DefaultConfigToken
    {
        public DefaultValidateConfigToken()
        {
            this.BaseStr = "qwertyuioplkjhgfdsazxcvbnm-01234|56789.QAZXSWEDCVFRTGBNHYUJMKIOLP";
            this.KeyStr = "CHIGUADASHU-DALI";
            this.RadomIndex = 20170301;
            this.SplitSeparator = "|";
            this.EqualsSeparator = ".";
        }
    }
}