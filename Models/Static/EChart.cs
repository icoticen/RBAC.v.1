using K.Y.DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEHICLEDETECTING.Models.Static
{
    public class EChart
    {
        public class Data
        {
            public List<Series_item> TableItems { get; set; } = new List<Series_item>();
            public Option EChartOption { get; set; }
            public static Option GetOption(IEnumerable<Series_item> List, List<String> xAxisdata, Int32 MaxLegendCount = 8, Boolean SortTable = false)
            {
                xAxisdata = xAxisdata ?? List.Select(p => p.xAxis).Distinct().ToList();

                var series = List
                    .Select(p => new {
                        Name = p.yAxisPre1 + "." + p.Name,
                        p.Count,
                        p.xAxis,
                    })
                   .GroupBy(p => p.Name)
                   .Select(g => new
                   {
                       C = g.Sum(p => p.Count),
                       g
                   })
                   .OrderByDescending(p => p.C)
                   .Take(MaxLegendCount)

                   .Select(g => new Option.Series
                   {
                       name = g.g.Key,
                       type = "line",
                       data = xAxisdata.Select(p =>
                       {
                           var l = g.g.Where(i => i.xAxis == p);
                           if (l.Count() > 0) return l.Max(i => i.Count ?? 0f);
                           return 0f;
                       }),// [120, 132, 101, 134, 90, 230, 210]
                   }).ToList();
                var legend = new Option.Legend { data = series.Select(p => p.name) };
                var xAxis = new Option.Axis
                {
                    type = "category",
                    boundaryGap = false,
                    data = xAxisdata
                };
                return new Option()
                {
                    legend = legend,
                    series = series,
                    xAxis = xAxis,
                };
            }
        }
        public class Option
        {
            public IEnumerable<Series> series { get; set; }
            public Legend legend { get; set; }
            public Axis xAxis { get; set; }


            public class Legend
            {
                public IEnumerable<string> data { get; set; }
            }
            public class Series
            {
                public String name { get; set; }
                public String type { get; set; }
                public IEnumerable<double> data { get; set; }
            }
            public class Axis
            {
                public string type { get; set; }
                public bool boundaryGap { get; set; }
                public List<string> data { get; set; }
            }
        }



        public class Series_item
        {
            public double? Count { get; set; }
            public String Name { get; set; }
            public String yAxisPre1 { get; set; }
            public String xAxis { get; set; }
        }
        public class Series_Seed
        {
            public int? ID { get; set; }
            public Int32? AppSource { get; set; }
            public Int32? AccountType { get; set; }
            public string Mac { get; set; }
            public String IP { get; set; }
            public Int32? UserID { get; set; }
            public Int32? KeyID { get; set; }
            public DateTime? ActionDateTime { get; set; }
            public Dictionary<string, string> ExtraData { get; set; } = new Dictionary<string, string>();

            public String X { get; set; }
            public String Y1 { get; set; }
        }



        public static Func<String, String, IEnumerable<Series_Seed>, Series_item> F_SelectItem_ID = (X, Y1, G) => new Series_item { xAxis = X, yAxisPre1 = Y1, Name = "个数", Count = G.Count() };
        public static Func<String, String, IEnumerable<Series_Seed>, Series_item> F_SelectItem_User = (X, Y1, G) => new Series_item { xAxis = X, yAxisPre1 = Y1, Name = "用户数", Count = G.Select(p => p.UserID).Distinct().Count() };
        public static Func<String, String, IEnumerable<Series_Seed>, Series_item> F_SelectItem_Mac = (X, Y1, G) => new Series_item { xAxis = X, yAxisPre1 = Y1, Name = "MAC数", Count = G.Select(p => p.Mac).Distinct().Count() };
        public static Func<String, String, IEnumerable<Series_Seed>, Series_item> F_SelectItem_IP = (X, Y1, G) => new Series_item { xAxis = X, yAxisPre1 = Y1, Name = "IP数", Count = G.Select(p => p.IP).Distinct().Count() };
        public static Func<String, String, IEnumerable<Series_Seed>, Series_item> F_SelectItem_KeyID = (X, Y1, G) => new Series_item { xAxis = X, yAxisPre1 = Y1, Name = "对象数", Count = G.Select(p => p.KeyID).Distinct().Count() };
        /// <summary>
        /// 分析统计组合
        /// 初步获取数据
        /// 需要展现的元数据
        /// </summary>
        /// <param name="List">源种子数据 最小单位</param>
        /// <param name="FGroupByX">X轴第一顺位分组依据</param>
        /// <param name="FGroupByY1">Y轴第一顺位分组依据</param>
        /// <param name="FSelectItem">需要展现的元数据解析器 组合模式~</param>
        /// <returns></returns>
        public static List<Series_item> ToSeriesItem(IEnumerable<Series_Seed> List, Func<Series_Seed, String> FGroupByX, Func<Series_Seed, String> FGroupByY1, params Func<String, String, IEnumerable<Series_Seed>, Series_item>[] FSelectItem)
        {
            FGroupByX = FGroupByX ?? (p => p.ActionDateTime.Ex_ToString("yyyy-MM-dd"));
            FGroupByY1 = FGroupByY1 ?? (p => "-");
            FSelectItem = FSelectItem ?? new[] { F_SelectItem_ID, F_SelectItem_User, F_SelectItem_Mac, F_SelectItem_IP };
            var L = List.Select(p => new
            {
                Item = p,
                X = FGroupByX(p),
                Y1 = FGroupByY1(p),
            })
            .GroupBy(gx => gx.X)
            .SelectMany(gx => gx.GroupBy(gy1 => gy1.Y1)
            .SelectMany(gy1 => FSelectItem.Select(f => f(gx.Key, gy1.Key, gy1.Select(i => i.Item))))
            ).ToList();
            return L;
        }
        public static List<String> GetxAxisData(DateTime? dt1 = null, DateTime? dt2 = null, Int32 Seconds = 86400, String Format = "yyyy-MM-dd")
        {
            var dtmin = dt1 ?? DateTime.Now.Date.AddDays(-7);
            var dtmax = dt2 ?? DateTime.Now.Date.AddDays(1);
            var LDATA = new List<String>();
            for (var dt = dtmin.Date; dt < dtmax; dt = dt.AddSeconds(Seconds))
            {
                LDATA.Add(dt.Ex_ToString(Format));
            }
            return LDATA;
        }

    }
}