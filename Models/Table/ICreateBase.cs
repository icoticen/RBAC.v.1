using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Table
{
    public interface ICreateBase : IAdminCreateBase, IUserCreateBase
    {
        //int ID { get; set; }
        //int CreateUserID { get; set; }
        //DateTime? CreateDateTime { get; set; }
        //int CreateAdminID { get; set; }
        //int Status { get; set; }
        //bool? IsOpen { get; set; }
    }
    public interface IAdminCreateBase : IBase
    {
        //int ID { get; set; }
        int CreateAdminID { get; set; }
        DateTime? CreateDateTime { get; set; }
        bool? IsOpen { get; set; }
    }
    public interface IUserCreateBase : IBase
    {
        //int ID { get; set; }
        int CreateUserID { get; set; }
        DateTime? CreateDateTime { get; set; }
        int Status { get; set; }
    }
}