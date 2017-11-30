 using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
			
			String json="{\"row\":[{\"ts\":1444000,\"ws\":3.3},{\"ts\":1454000,\"ws\":null},{\"ts\":1464000,\"ws\":5.3}]}";
			Dictionary<long,float?> dict= new Dictionary<long,float?> ();
            ///数据初始化部分
			dynamic newValue = JsonConvert.DeserializeObject<dynamic>(json);
            for (int i = 0; i < newValue.row.Count; i++)
            {
         
                long ts = newValue.row[i].ts;
                float? ws = newValue.row[i].ws;          
                dict.Add(ts, ws);
        
            }


        }
    }
}
