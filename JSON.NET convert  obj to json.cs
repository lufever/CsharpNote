 using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WM.Common;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ///数据初始化部分
            string aaa = "abc";
            int bbb = 1;
            bool ccc = true;
            int[] ddd = {1, 2, 3};
            ///混合类型List
            List<object> eee = new List<object>() {1, 2.0, "3"};
            ///混合类型Array
            Object[] fff = {aaa, bbb, ccc, ddd, eee};

            DataTable dt = new DataTable();
            dt.Columns.Add("ts", typeof (long));
            dt.Columns.Add("windspeed", typeof (double));
            dt.Columns.Add("dt", typeof (DateTime));
            //dt.Rows.Add("1444400000", "05");
            //dt.Rows.Add("1444400000", "05.2");
            //dt.Rows.Add("1449400000", DBNull.Value);
            dt.Rows.Add("1444400000", "05", "2013-1-12");
            dt.Rows.Add("1445400000", "2.156", new DateTime());
            dt.Rows.Add("1446400000", DBNull.Value, DateTime.Now);
            dt.Rows.Add("1447500000", 3, DBNull.Value);
            dt.Rows.Add("1448400000", "05", "2013-1-12 13:35:34");

            ///A 带键名
            ///A.1 包含表格所有字段
            var enumRowCollectionDictAll = dt;
            ///A.2 带键名，更改默认键名，不处理数据
            var enumRowCollectionDict = from r in dt.AsEnumerable() select new {t = r["ts"], ws = r["windspeed"]};
            ///A.3 带键名，并处理数据
            var enumRowCollectionDictWithType = dt.AsEnumerable().Select(r =>
            {
                var ws = r["windspeed"];
                object result = null;
                if (ws != DBNull.Value)
                {
                    if (r["windspeed"].ToString().IndexOf(".") == -1)
                    {
                        result = r["windspeed"].ToInt();
                    }
                    else
                    {
                        ws = r.Field<double>("windspeed");
                        result = Math.Round(ws.ToDouble(), 2);
                        //result= Math.Round(ws.ToDouble(), 2).ToString("G");
                    }
                }
                return new {ts = r["ts"], windspeed = result};
            }
            );

            /// B 不带键名
      
            ///B.1不带键名，包含表格所有字段
           var enumRowCollectionArrayAll= dt.AsEnumerable().Select(r => r.ItemArray);
            ///B.2不带键名,包含指定字段，三种写法
            var enumRowCollectionArray1 = from r in dt.AsEnumerable() select new []{r["ts"],r["windspeed"]};
            var enumRowCollectionArray2 = dt.AsEnumerable().Select(r => new[] { r["ts"], r["windspeed"]});
            var enumRowCollectionArray3 = new DataView(dt).ToTable(false, new[] {"ts", "windspeed"});
            
            Dictionary<string,object> dict = new Dictionary<string, object>();
            dict.Add("a",aaa);
            dict.Add("b", bbb);
            dict.Add("c", ccc);
            dict.Add("d", ddd);
            dict.Add("e", eee);
            dict.Add("complex", fff);
            dict.Add("rows", enumRowCollectionDictWithType);
            
           
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            var keyContainedJsonArray = JsonConvert.SerializeObject(dict, Formatting.Indented, timeFormat);
            var noKeyContainedJsonArrayFromObjectArray = JsonConvert.SerializeObject(fff, Formatting.Indented, timeFormat);
            var KeyContainedJsonArrayFromDataTable = JsonConvert.SerializeObject(enumRowCollectionDictAll, Formatting.Indented, timeFormat);
  

        }
    }
}
