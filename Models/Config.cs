//using IKUS.LIB.MODEL;
using IKUS.LIB.WEB.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models
{
    public class Config : ConfigBase
    {
        public enum E_Project_Type
        {
            /// <summary>
            /// 单表单类型
            /// </summary>
            single = 01,
            /// <summary>
            /// 多表单类型
            /// </summary>
            multiple = 11,
        }
        public enum E_Project_Form_Type
        {
            /// <summary>
            /// 传统表单
            /// </summary>
            general = 1,
            /// <summary>
            /// Map热点图
            /// </summary>
            map = 2,
            /// <summary>
            /// 客户自定义
            /// </summary>
            customize = 99,
        }
        public enum E_Project_Form_Option_Type
        {
            /// <summary>
            /// 文本
            /// </summary>
            text = 1,
            /// <summary>
            /// 文本域
            /// </summary>
            textarea = 2,
            //html =3,
            //date=4,
            //datetime=5,
            /// <summary>
            /// 下拉枚举 单选
            /// </summary>
            @enum = 6,
            /// <summary>
            /// 多选
            /// </summary>
            list = 7,
            //listac=8,
            /// <summary>
            /// checkbox 
            /// </summary>
            @bool = 9,
            //video=10,
            //image=11,
            //imagelist=12,

            //hidden,
        }
        public enum E_Project_Pack_Type
        {
            @default = 1,
        }
        public enum E_Report_Status
        {
            /// <summary>
            /// 新创建
            /// </summary>
            新创建 = 1,
            修改中 = 2,
            /// <summary>
            /// 所以子级项目全部为已完成状态且子级没有未填的
            /// </summary>
            已完成 = 3,
            /// <summary>
            /// 已取消报告
            /// </summary>
            已取消 = 4,
            /// <summary>
            /// 提交到店长审核
            /// </summary>
            已提交 = 5,
            /// <summary>
            /// 店长确认通过
            /// </summary>
            已确认 = 6,
        }
        public enum E_Report_CustomerType
        {
            个人用户 = 1,
            签约用户 = 3,
            非签约用户 = 4,
        }
        public enum E_Report_VehiclePowerType {
            前驱=1,
            后驱=2,
            四驱=3,
        }
        public enum E_Report_VehicleFuelType
        {
            汽油 = 1,
            柴油 = 2,
            其他 = 3,
        }
        public enum E_Report_VehicleGearboxType
        {
            手动 = 1,
            自动 = 2,
            手自一体 = 3,
            其他=4,
        }
        public enum E_Report_VehicleOwnerNatureType
        {
            个人 = 1,
            公司 = 2,
        }
        public enum E_Report_VehicleUseNatureType
        {
            非营运 = 1,
            营运 = 2,
            营转非 = 3,
            租赁 = 4,
        }

        public enum E_AgreementType
        {
            HTTP = 1,
        }
        public enum E_Sex
        {
            男 = 1,
            女 = 2,
            保密 = 3,
        }
        public enum E_IDType
        {
            身份证 = 1,
            驾驶证 = 2,
            军官证 = 3,
            护照 = 4,
        }

        public enum E_INDEX_FORMNODE
        {
            E_PayType,
        }
        /// <summary>
        /// FROMNODE = [
        /// E_IDType
        /// E_PayType
        /// ]
        /// </summary>

        public static readonly Dictionary<Int32, string> LI_ServerLocation = new Dictionary<int, string> {
            {(Int32)Config.E_ServerLocation.生产服务器,"" },
            {(Int32)Config.E_ServerLocation.测试服务器,"" },
        };
        public enum E_Channel {

        }
    }
    public class Config_FilterAuthorityCheckAdmin
    {
        public static readonly Func<String, String, Int32, Boolean> FAuthorityCheck = (ControllerName, ActionName, AdminID) =>
        {
            return true;
        };
    }



}