using K.Y.DLL;
using K.Y.DLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models.Static
{
    public class Validate
    {
        public class RequestAPI
        {
            protected K.Y.DLL.Model.M_Validate.IConfig Cfg { get; set; }
            public String this[String _Key]
            {
                get
                {
                    return DIC == null ? "" : DIC.Keys.Contains(_Key) ? DIC[_Key] ?? "" : "";
                }
            }

            public String RequestContent { get; private set; }


            public void Log(String FunctionName)
            {
                var RequestInfo = RequestContent + Cfg.MD5_ADDITIONKEY;
                var MySign = T_Crypt.MD5_String(RequestInfo, System.Text.Encoding.UTF8);
                T_Log.LogRun(FunctionName, new { RequestInfo, URL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri, MySign }.Ex_ToJson());
            }
            public Boolean JsonValidate()
            {
                return DIC != null && DIC.Count > 0;
            }
            public Boolean SignValidate(string Sign)
            {
                var RequestInfo = RequestContent + Cfg.MD5_ADDITIONKEY;
                var MySign = T_Crypt.MD5_String(RequestInfo, System.Text.Encoding.UTF8).ToUpper();
                Sign = Sign.ToUpper();
                return MySign == Sign;
            }
            public Boolean RequestValidate(Int32 Seconds = 0)
            {
                var dt = this["TimeStamp"].Ex_ToDateTime();
                return Seconds > 0 ? (dt.AddSeconds(Seconds) > DateTime.Now && dt.AddSeconds(-Seconds) < DateTime.Now) : true;
            }
            public Boolean TokenValidate(Int32 Seconds = 0, params Int32[] AccountType)
            {
                return Token == null
                    ? false
                    : Seconds > 0
                        ? Token.DateTime.AddSeconds(Seconds) > DateTime.Now
                            ? AccountType.Length > 0
                                ? AccountType.Contains(Token.AccountType)
                                : true
                            : false
                        : true;
            }



            public Dictionary<String, String> DIC { get; protected set; }
            private K.Y.DLL.Model.M_Validate.Token _Token { get; set; }
            public K.Y.DLL.Model.M_Validate.Token Token
            {
                get
                {
                    if (_Token == null) _Token = new K.Y.DLL.Model.M_Validate.Token(Cfg);
                    return _Token;
                }
                protected set
                {
                    _Token = value;
                }
            }

            public Int32 UserID
            {
                get
                {
                    return Token == null ? 0 : Token.UserID;
                }
            }
            public RequestAPI(K.Y.DLL.Model.M_Validate.IConfig _Cfg)
            {
                Cfg = _Cfg;
                if (System.Web.HttpContext.Current.Request.InputStream != null)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(System.Web.HttpContext.Current.Request.InputStream, encoding: System.Text.Encoding.UTF8);
                    RequestContent = sr.ReadToEnd();
                    sr.Close();
                }
                try
                {
                    DIC = RequestContent.Ex_ToEntity<Dictionary<string, string>>() ?? new Dictionary<String, string>();

                    if (DIC.Keys.Contains("Token")) Token = new K.Y.DLL.Model.M_Validate.Token(Cfg, DIC["Token"]);

                }
                catch
                {
                    DIC = new Dictionary<string, string>();
                }
            }
        }

    }
}