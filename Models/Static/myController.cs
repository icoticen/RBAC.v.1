using K.Y.DLL;
using K.Y.DLL.Model;
using K.Y.DLL.PlugIn;
using K.Y.DLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Table;
using VEHICLEDETECTING.Functions;

namespace VEHICLEDETECTING.Models.Static
{
    public class myController
    {
        public class Admin : Controller
        {

            private Int32 __AdminID = 0;
            protected Int32 _AdminID
            {
                get
                {
                    if (__AdminID > 0)
                        return __AdminID;
                    var M = M_Identify.Get("_Admin");
                    if (M == null) __AdminID = 0;
                    else __AdminID = M.ID;
                    return __AdminID;
                }
                set
                {
                    if (value <= 0)
                    {
                        __AdminID = 0;
                        M_Identify.Clear("_Admin");
                    }
                    else
                    {
                        M_Identify.Set(new M_Identify { ID = value, Identify = 1, IdentifyCode = "_Admin" }, "_Admin");
                        __AdminID = value;
                    }
                }
            }

            protected static fSys FunctionSYS = new fSys();

            protected static Config_Validate CFG_VALIDATE = new Config_Validate();

            #region FilePost
            public ActionResult Base64_FilePost()
            {
                #region VALIDATE
                var Req = new Validate.RequestAPI(CFG_VALIDATE);
                Req.Log(ControllerContext.RouteData.Values["action"].ToString());
                if (!Req.JsonValidate()) return Content(new M_Result(E_ERRORCODE.客户端_API请求_JSON无效, DATA: Req.RequestContent).Ex_ToJson());
                if (!Req.RequestValidate(300)) return Content(new M_Result(E_ERRORCODE.客户端_API请求_请求过期, DATA: Req["_"]).Ex_ToJson());
                //if (!Req.SignValidate(Sign)) return Content(new M_Result(E_ERRORCODE.客户端_API请求_SIGN验证失败, DATA: Sign).Ex_ToJson());
                //if (!Req.TokenValidate()) return Content(new M_Result(E_ERRORCODE.客户端_API请求_TOKEN无效, DATA: Req.Token).Ex_ToJson());
                #endregion

                #region PROPERTIES
                var FileElementName = Req["FileElementName"];
                var FileType = Req["FileType"];
                var ExtensionName = Req["ExtensionName"];
                var Base64String = Req["Base64"];
                //TimeStamp

                var bitmap = T_Image.ToBitmap(Base64String);
                var R = PostFile_SaveToServer(bitmap, FileElementName, FileType, ExtensionName);
                return Content(R.Ex_ToJson());
                #endregion
            }
            public virtual ActionResult LayUI_FilePost(String FileElementName = "file", string FileType = "image")
            {
                T_Log.LogUpload("Admin/LayUI_FilePost");
                var R = PostFile_SaveToFTP(FileElementName, PreStr: FileElementName, FileType: FileType);
                if (R.result == 1)
                {
                    return Content(new
                    {
                        code = 0, //0表示成功，其它失败
                        msg = R.msg, //提示信息 //一般上传失败后返回
                        data = new
                        {
                            src = R.data,
                            title = "恩恩 测试的" //可选
                        }
                    }.Ex_ToJson());
                }
                return Content(new
                {
                    code = 1, //0表示成功，其它失败
                    msg = R.msg, //提示信息 //一般上传失败后返回
                    data = new
                    {
                        src = "http://pic31.nipic.com/20130715/3566232_110502405145_2.gif",
                        title = "出错了" //可选
                    }
                }.Ex_ToJson());
            }
            public virtual ActionResult WangEditor_FilePost(String FileElementName = "file", string FileType = "image")
            {
                T_Log.LogUpload("Admin/WangEditor_FilePost");
                var R = PostFile_SaveToFTP(FileElementName, PreStr: FileElementName, FileType: FileType);
                if (R.result == 1)
                {
                    return Content(R.data);
                }
                return Content("error|" + R.msg);
            }


            protected M_Result PostFile_SaveToServer(String FileElementName, String Dirpath = "Image", string PreStr = "_admin_", string FileType = "image", string FileExtension = "")
            {
                switch (FileType.ToLower())
                {
                    //其他定制吧  ->_->
                    case "image": FileExtension += ".jpg.bmp.png.jpeg.gif"; break;
                    case "video": FileExtension += ".mp4.flv.swf.avi.3gp.f4v"; break;
                    case "zip": FileExtension += ".zip.rar.7z."; break;
                    case "application": FileExtension += ".apk."; break;
                    case "document": FileExtension += ".xls.xlsx.doc.docx.ppt.pptx.wps.txt"; break;


                    case "datafile": FileExtension += ".jpg.bmp.png.jpeg.gif .xls.xlsx.doc.docx.ppt.pptx.wps.txt .pdf .zip.rar.7z .apk .mp4.flv.swf.avi.3gp.f4v"; break;
                }
                var URL = "";
                var pFile = Request.Files[FileElementName];
                URL = T_File.File_Save(pFile, Dirpath, PreStr, FileExtension);
                if (URL == "0") return new M_Result { result = 0, msg = "文件不能为空~" };
                else if (URL == "-1") return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };

                return new M_Result { result = 1, data = Config.LocalHostAuthority + URL, msg = "上传成功~" };
            }
            protected M_Result PostFile_SaveToServer(System.Drawing.Bitmap BitMap, String FileElementName, String Dirpath = "Image", string PreStr = "_admin_", string FileType = "image", string ExtensionName = ".png", string IncludeExtension = "")
            {
                switch (FileType.ToLower())
                {
                    //其他定制吧  ->_->
                    case "image": IncludeExtension += ".jpg.bmp.png.jpeg.gif"; break;
                    case "video": IncludeExtension += ".mp4.flv.swf.avi.3gp.f4v"; break;
                    case "zip": IncludeExtension += ".zip.rar.7z."; break;
                    case "application": IncludeExtension += ".apk."; break;
                    case "document": IncludeExtension += ".xls.xlsx.doc.docx.ppt.pptx.wps.txt"; break;


                    case "datafile": IncludeExtension += ".jpg.bmp.png.jpeg.gif .xls.xlsx.doc.docx.ppt.pptx.wps.txt .pdf .zip.rar.7z .apk .mp4.flv.swf.avi.3gp.f4v"; break;
                }
                var URL = "";
                URL = T_File.File_Save(BitMap, Dirpath, PreStr, true, ExtensionName, IncludeExtension);
                if (URL == "0") return new M_Result { result = 0, msg = "文件不能为空~" };
                else if (URL == "-1") return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };

                return new M_Result { result = 1, data = Config.LocalHostAuthority + URL, msg = "上传成功~" };
            }
            protected M_Result PostFile_SaveToFTP(String FileElementName, String Dirpath = "Image", string PreStr = "_admin_", string FileType = "image", string FileExtension = "")
            {
                try
                {
                    //UserID = UserID > 0 ? UserID : _UserID;
                    //_Log_FileUpload_URLMsg("Ajax_Web_FileUpLoad_Image");

                    String IncludeExtension = ".jpg.bmp.png.jpeg.gif";
                    var URL = "http://download.95dao.com/app/oxtt/pic/201612/16/f_HeadImage_9c01aac924aef469.jpeg";
                    var File = Request.Files[FileElementName];

                    if (File != null && File.FileName.LastIndexOf('.') > 0)
                    {
                        var ExtensionName = File.FileName.Substring(File.FileName.LastIndexOf('.')).ToLower();
                        if (String.IsNullOrEmpty(IncludeExtension) || IncludeExtension.Contains(ExtensionName))
                        {
                            String targetDir = @"app/cgds/pic/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Day.ToString("00");
                            String filename = FileElementName + "_" + T_Crypt.Guid16() + ExtensionName;

                            String Path = AppDomain.CurrentDomain.BaseDirectory + "/PostFile/" + targetDir + "/";
                            if (!System.IO.Directory.Exists(Path)) System.IO.Directory.CreateDirectory(Path);
                            File.SaveAs(Path + filename);

                            URL = T_File.FTP_UploadFile(File.InputStream, targetDir, filename, "newupload.dnion.com", "95dao_download", "95dao_download@83241");


                            if (URL.Length > 10)
                            {
                                URL = "http://download.95dao.com/" + targetDir + "/" + filename;
                                return new M_Result { result = 1, data = URL, msg = "上传成功~" };
                            }
                        }
                    }
                    //var x = new
                    //{
                    //    code = 0, //0表示成功，其它失败
                    //    msg = "//提示信息 //一般上传失败后返回", //提示信息 //一般上传失败后返回
                    //    data = new
                    //    {
                    //        src = URL,
                    //        title = "恩恩 测试的" //可选
                    //    }
                    //};
                    //return Content(x.Ex_ToJson());
                }
                catch
                {
                    return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };
                    //var x = new
                    //{
                    //    code = 1, //0表示成功，其它失败
                    //    msg = "一般上传失败后返回", //提示信息 //一般上传失败后返回
                    //    data = new
                    //    {
                    //        src = "http://pic31.nipic.com/20130715/3566232_110502405145_2.gif",
                    //        title = "出错了" //可选
                    //    }
                    //};
                    //return Content(x.Ex_ToJson());
                }
                return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };
            }
            protected M_Result PostFile_SaveToFTP(System.Drawing.Bitmap BitMap, String FileElementName, String Dirpath = "Image", string PreStr = "_admin_", string FileType = "image", string ExtensionName = ".png", string IncludeExtension = ".jpg.bmp.png.jpeg.gif")
            {
                try
                {
                    IncludeExtension = ".jpg.bmp.png.jpeg.gif";
                    var URL = "http://download.95dao.com/app/oxtt/pic/201612/16/f_HeadImage_9c01aac924aef469.jpeg";


                    if (String.IsNullOrEmpty(IncludeExtension) || IncludeExtension.Contains(ExtensionName))
                    {
                        String targetDir = @"app/cgds/pic/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Day.ToString("00");
                        String filename = FileElementName + "_" + T_Crypt.Guid16() + ExtensionName;

                        String Path = AppDomain.CurrentDomain.BaseDirectory + "/PostFile/" + targetDir + "/";
                        if (!System.IO.Directory.Exists(Path)) System.IO.Directory.CreateDirectory(Path);
                        BitMap.Save(Path + filename);

                        var ms = new System.IO.MemoryStream();
                        BitMap.Save(ms, BitMap.RawFormat);
                        URL = T_File.FTP_UploadFile(ms, targetDir, filename, "newupload.dnion.com", "95dao_download", "95dao_download@83241");


                        if (URL.Length > 10)
                        {
                            URL = "http://download.95dao.com/" + targetDir + "/" + filename;
                            return new M_Result { result = 1, data = URL, msg = "上传成功~" };
                        }
                    }
                }
                catch
                {
                    return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };
                    //var x = new
                    //{
                    //    code = 1, //0表示成功，其它失败
                    //    msg = "一般上传失败后返回", //提示信息 //一般上传失败后返回
                    //    data = new
                    //    {
                    //        src = "http://pic31.nipic.com/20130715/3566232_110502405145_2.gif",
                    //        title = "出错了" //可选
                    //    }
                    //};
                    //return Content(x.Ex_ToJson());
                }
                return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };
            }

            #endregion


            #region AdminEmpAccount
            public virtual ActionResult _LogOut(String BackURL = null)
            {
                _AdminID = 0;
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
                    var R = FunctionSYS.Emp_Login(AccountName, AccountPsw);
                    if (R.result != (Int32)E_ERRORCODE.操作成功)
                    {
                        Response.Write("<script>alert('" + R.msg + "~~~~~！');</script>");
                        return View();
                    }
                    else
                    {
                        _AdminID = (Int32)R.data;
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
                LayData.AddModel("NPSW", OPSW);
                LayData.AddModel("NPSWR", OPSW);


                #endregion


                if (Request.RequestType == "POST")
                {
                    if (NPSW != NPSWR)
                    {
                        Response.Write("<script>alert('重复密码不一致~~~~~！');</script>");
                        return View("Save", LayData);
                    }


                    NPSW = T_Crypt.MD5_String(NPSW).ToUpper();
                    OPSW = T_Crypt.MD5_String(OPSW).ToUpper();
                    var R = FunctionSYS.Emp_ResetPsw(_AdminID, NPSW, OPSW);

                    Response.Write("<script>alert('" + R.msg + "~~~~~！');</script>");
                    return _LogOut();
                }
                return View("Save", LayData);
            }
            #endregion







            public void refUpdate<T>(System.Data.Entity.DbContext EF, String refIDs, Int32 KeyID) where T : class, Table.IRefBase, new()
            {
                var ExistItems = EF.Set<T>().Where(p => p.KeyID == KeyID && !(p.HasCancle ?? false)).Select(p => p.refKeyID).Where(p => p > 0).Distinct().ToList();
                var CurrentItems = refIDs.Ex_ToList().Ex_ToInt32().Where(p => p > 0).Distinct().ToList();
                var ToInsertItems = CurrentItems.Where(p => !ExistItems.Contains(p)).ToList();
                var ToRemoveItems = ExistItems.Where(p => !CurrentItems.Contains(p)).ToList();
                foreach (var m in EF.Set<T>().Where(p => !(p.HasCancle ?? false) && p.KeyID == KeyID && ToRemoveItems.Contains(p.refKeyID)))
                {
                    m.HasCancle = true;
                    m.CancleDateTime = DateTime.Now;
                }
                foreach (var m in ToInsertItems)
                {
                    EF.Set<T>()
                        .Add(new T()
                        {
                            CreateAdminID = _AdminID,
                            KeyID = KeyID,
                            CreateDateTime = DateTime.Now,
                            refKeyID = m,
                            HasCancle = false,
                        });
                }
            }
            public void refInsert<T>(System.Data.Entity.DbContext EF, String refIDs, Int32 KeyID) where T : class, Table.IRefBase, new()
            {
                refIDs.Ex_ToList()
                    .Ex_ToInt32()
                    .Where(p => p > 0)
                    .Distinct()
                    .ToList()
                    .ForEach(p => EF.Set<T>().Add(new T()
                    {
                        CreateAdminID = _AdminID,
                        KeyID = KeyID,
                        CreateDateTime = DateTime.Now,
                        refKeyID = p,
                        HasCancle = false,
                    }));
            }

            //public void refEnumGet<T,R>(System.Data.Entity.DbContext EF,) where R : class, Table.IRefBase, new()
            //{
            //    EF.Set<T>().Where(p=>p.is)

            //}

            public IEnumerable<zTree.ITreeNodeBase> TreeList<T>(System.Data.Entity.DbContext EF, Func<T, zTree.ITreeNodeBase> FSwitch = null) where T : class, Table.ITreeBase, new()
            {
                FSwitch = FSwitch ?? (p => new zTree.TreeNodeBase
                {
                    id = p.ID,
                    pId = p.ParentID,
                    name = p.Name,
                    open = true,
                    chkDisabled = false,
                    @checked = true,
                   // color = (p.IsOpen==true) ? null : "#A9A9A9",
                });
                var L = EF.Set<T>()
                       .OrderByDescending(p => p.SortNo)
                       .ToList()
                       .Select(p => FSwitch(p));
                return L;
            }
        }
        public class API : Controller
        {

            protected static fSys FunctionSYS = new fSys();

            protected virtual  K.Y.DLL.Model.M_Validate.IConfig CFG_VALIDATE { get; set; } = new Config_Validate();
            protected Int32 ValidateSeconds { get; set; } = 300;
            #region FilePost
            //public ActionResult Base64_FilePost()
            //{
            //    #region VALIDATE
            //    var Req = new Validate.RequestAPI(CFG_VALIDATE);
            //    Req.Log(ControllerContext.RouteData.Values["action"].ToString());
            //    if (!Req.JsonValidate()) return Content(new M_Result(E_ERRORCODE.客户端_API请求_JSON无效, DATA: Req.RequestContent).Ex_ToJson());
            //    if (!Req.RequestValidate(300)) return Content(new M_Result(E_ERRORCODE.客户端_API请求_请求过期, DATA: Req["_"]).Ex_ToJson());
            //    //if (!Req.SignValidate(Sign)) return Content(new M_Result(E_ERRORCODE.客户端_API请求_SIGN验证失败, DATA: Sign).Ex_ToJson());
            //    //if (!Req.TokenValidate()) return Content(new M_Result(E_ERRORCODE.客户端_API请求_TOKEN无效, DATA: Req.Token).Ex_ToJson());
            //    #endregion

            //    #region PROPERTIES
            //    var FileElementName = Req["FileElementName"];
            //    var FileType = Req["FileType"];
            //    var ExtensionName = Req["ExtensionName"];
            //    var Base64String = Req["Base64"];
            //    //TimeStamp

            //    var bitmap = T_Image.ToBitmap(Base64String);
            //    var R = PostFile_SaveToServer(bitmap, FileElementName, FileType, ExtensionName);
            //    return Content(R.Ex_ToJson());
            //    #endregion
            //}
            //public ActionResult LayUI_FilePost(String FileElementName = "file", string FileType = "image")
            //{
            //    T_Log.LogUpload("Admin/LayUI_FilePost");
            //    var R = PostFile_SaveToFTP(FileElementName, PreStr: FileElementName, FileType: FileType);
            //    if (R.result == 1)
            //    {
            //        return Content(new
            //        {
            //            code = 0, //0表示成功，其它失败
            //            msg = R.msg, //提示信息 //一般上传失败后返回
            //            data = new
            //            {
            //                src = R.data,
            //                title = "恩恩 测试的" //可选
            //            }
            //        }.Ex_ToJson());
            //    }
            //    return Content(new
            //    {
            //        code = 1, //0表示成功，其它失败
            //        msg = R.msg, //提示信息 //一般上传失败后返回
            //        data = new
            //        {
            //            src = "http://pic31.nipic.com/20130715/3566232_110502405145_2.gif",
            //            title = "出错了" //可选
            //        }
            //    }.Ex_ToJson());
            //}
            //public ActionResult WangEditor_FilePost(String FileElementName = "file", string FileType = "image")
            //{
            //    T_Log.LogUpload("Admin/WangEditor_FilePost");
            //    var R = PostFile_SaveToFTP(FileElementName, PreStr: FileElementName, FileType: FileType);
            //    if (R.result == 1)
            //    {
            //        return Content(R.data);
            //    }
            //    return Content("error|" + R.msg);
            //}


            private M_Result PostFile_SaveToServer(String FileElementName, String Dirpath = "Image", string PreStr = "_admin_", string FileType = "image", string FileExtension = "")
            {
                switch (FileType.ToLower())
                {
                    //其他定制吧  ->_->
                    case "image": FileExtension += ".jpg.bmp.png.jpeg.gif"; break;
                    case "video": FileExtension += ".mp4.flv.swf.avi.3gp.f4v"; break;
                    case "zip": FileExtension += ".zip.rar.7z."; break;
                    case "application": FileExtension += ".apk."; break;
                    case "document": FileExtension += ".xls.xlsx.doc.docx.ppt.pptx.wps.txt"; break;


                    case "datafile": FileExtension += ".jpg.bmp.png.jpeg.gif .xls.xlsx.doc.docx.ppt.pptx.wps.txt .pdf .zip.rar.7z .apk .mp4.flv.swf.avi.3gp.f4v"; break;
                }
                var URL = "";
                var pFile = Request.Files[FileElementName];
                URL = T_File.File_Save(pFile, Dirpath, PreStr, FileExtension);
                if (URL == "0") return new M_Result { result = 0, msg = "文件不能为空~" };
                else if (URL == "-1") return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };

                return new M_Result { result = 1, data = Config.LocalHostAuthority + URL, msg = "上传成功~" };
            }
            private M_Result PostFile_SaveToServer(System.Drawing.Bitmap BitMap, String FileElementName, String Dirpath = "Image", string PreStr = "_admin_", string FileType = "image", string ExtensionName = ".png", string IncludeExtension = "")
            {
                switch (FileType.ToLower())
                {
                    //其他定制吧  ->_->
                    case "image": IncludeExtension += ".jpg.bmp.png.jpeg.gif"; break;
                    case "video": IncludeExtension += ".mp4.flv.swf.avi.3gp.f4v"; break;
                    case "zip": IncludeExtension += ".zip.rar.7z."; break;
                    case "application": IncludeExtension += ".apk."; break;
                    case "document": IncludeExtension += ".xls.xlsx.doc.docx.ppt.pptx.wps.txt"; break;


                    case "datafile": IncludeExtension += ".jpg.bmp.png.jpeg.gif .xls.xlsx.doc.docx.ppt.pptx.wps.txt .pdf .zip.rar.7z .apk .mp4.flv.swf.avi.3gp.f4v"; break;
                }
                var URL = "";
                URL = T_File.File_Save(BitMap, Dirpath, PreStr, true, ExtensionName, IncludeExtension);
                if (URL == "0") return new M_Result { result = 0, msg = "文件不能为空~" };
                else if (URL == "-1") return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };

                return new M_Result { result = 1, data = Config.LocalHostAuthority + URL, msg = "上传成功~" };
            }
            private M_Result PostFile_SaveToFTP(String FileElementName, String Dirpath = "Image", string PreStr = "_admin_", string FileType = "image", string FileExtension = "")
            {
                try
                {
                    //UserID = UserID > 0 ? UserID : _UserID;
                    //_Log_FileUpload_URLMsg("Ajax_Web_FileUpLoad_Image");

                    String IncludeExtension = ".jpg.bmp.png.jpeg.gif";
                    var URL = "http://download.95dao.com/app/oxtt/pic/201612/16/f_HeadImage_9c01aac924aef469.jpeg";
                    var File = Request.Files[FileElementName];

                    if (File != null && File.FileName.LastIndexOf('.') > 0)
                    {
                        var ExtensionName = File.FileName.Substring(File.FileName.LastIndexOf('.')).ToLower();
                        if (String.IsNullOrEmpty(IncludeExtension) || IncludeExtension.Contains(ExtensionName))
                        {
                            String targetDir = @"app/cgds/pic/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Day.ToString("00");
                            String filename = FileElementName + "_" + T_Crypt.Guid16() + ExtensionName;

                            String Path = AppDomain.CurrentDomain.BaseDirectory + "/PostFile/" + targetDir + "/";
                            if (!System.IO.Directory.Exists(Path)) System.IO.Directory.CreateDirectory(Path);
                            File.SaveAs(Path + filename);

                            URL = T_File.FTP_UploadFile(File.InputStream, targetDir, filename, "newupload.dnion.com", "95dao_download", "95dao_download@83241");


                            if (URL.Length > 10)
                            {
                                URL = "http://download.95dao.com/" + targetDir + "/" + filename;
                                return new M_Result { result = 1, data = URL, msg = "上传成功~" };
                            }
                        }
                    }
                    //var x = new
                    //{
                    //    code = 0, //0表示成功，其它失败
                    //    msg = "//提示信息 //一般上传失败后返回", //提示信息 //一般上传失败后返回
                    //    data = new
                    //    {
                    //        src = URL,
                    //        title = "恩恩 测试的" //可选
                    //    }
                    //};
                    //return Content(x.Ex_ToJson());
                }
                catch
                {
                    return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };
                    //var x = new
                    //{
                    //    code = 1, //0表示成功，其它失败
                    //    msg = "一般上传失败后返回", //提示信息 //一般上传失败后返回
                    //    data = new
                    //    {
                    //        src = "http://pic31.nipic.com/20130715/3566232_110502405145_2.gif",
                    //        title = "出错了" //可选
                    //    }
                    //};
                    //return Content(x.Ex_ToJson());
                }
                return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };
            }
            private M_Result PostFile_SaveToFTP(System.Drawing.Bitmap BitMap, String FileElementName, String Dirpath = "Image", string PreStr = "_admin_", string FileType = "image", string ExtensionName = ".png", string IncludeExtension = ".jpg.bmp.png.jpeg.gif")
            {
                try
                {
                    IncludeExtension = ".jpg.bmp.png.jpeg.gif";
                    var URL = "http://download.95dao.com/app/oxtt/pic/201612/16/f_HeadImage_9c01aac924aef469.jpeg";


                    if (String.IsNullOrEmpty(IncludeExtension) || IncludeExtension.Contains(ExtensionName))
                    {
                        String targetDir = @"app/cgds/pic/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Day.ToString("00");
                        String filename = FileElementName + "_" + T_Crypt.Guid16() + ExtensionName;

                        String Path = AppDomain.CurrentDomain.BaseDirectory + "/PostFile/" + targetDir + "/";
                        if (!System.IO.Directory.Exists(Path)) System.IO.Directory.CreateDirectory(Path);
                        BitMap.Save(Path + filename);

                        var ms = new System.IO.MemoryStream();
                        BitMap.Save(ms, BitMap.RawFormat);
                        URL = T_File.FTP_UploadFile(ms, targetDir, filename, "newupload.dnion.com", "95dao_download", "95dao_download@83241");


                        if (URL.Length > 10)
                        {
                            URL = "http://download.95dao.com/" + targetDir + "/" + filename;
                            return new M_Result { result = 1, data = URL, msg = "上传成功~" };
                        }
                    }
                }
                catch
                {
                    return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };
                    //var x = new
                    //{
                    //    code = 1, //0表示成功，其它失败
                    //    msg = "一般上传失败后返回", //提示信息 //一般上传失败后返回
                    //    data = new
                    //    {
                    //        src = "http://pic31.nipic.com/20130715/3566232_110502405145_2.gif",
                    //        title = "出错了" //可选
                    //    }
                    //};
                    //return Content(x.Ex_ToJson());
                }
                return new M_Result { result = -1, msg = "文件上传失败~请选择正确的文件或稍后再试~" };
            }

            #endregion
        }
    }
}