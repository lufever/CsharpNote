using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
 

namespace TEST
{
    class SpiderChinaIp
    {
        public IPAddress CidrToIpAddress(string cidrstr)
        {
            double cidr = Convert.ToDouble(cidrstr) ;
            var c = Math.Log(cidr, 2);
            var digit = 32 - (int)c;
            var m = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                if (digit >= 8)
                    m[i] = 0xff;
                else if (digit < 0)
                    m[i] = 0x00;
                else
                    m[i] = Convert.ToByte((int)256 - Math.Pow(2, 8 - digit));
                digit = digit - 8;
            }
   
            return new IPAddress(m);
        }
        public List<RouteModel> Start()
        {


            var strList = new List<string> {};
            StreamReader srReader = new StreamReader("delegated-apnic-latest.txt");
            var content = srReader.ReadToEnd();
               strList = content.Split('\n').ToList();

            //try
            //{     
            //Console.WriteLine("下载数据中...");
            // 设置配置以支持文档加载
            //var config = Configuration.Default.WithDefaultLoader();
            //var REMOTE_URL = "http://ftp.apnic.net/apnic/stats/apnic/delegated-apnic-latest";
            //    var document = BrowsingContext.New(config).OpenAsync(REMOTE_URL);
            //    var dom = document.Result;
            //    Console.WriteLine("下载数据完成...");
            //    // 根据class获取html元素
            //    IEnumerable<IElement>  cells = dom.QuerySelectorAll("pre").OfType<IElement>();
            //      strList = cells.AsEnumerable().FirstOrDefault().InnerHtml.Split('\n').ToList();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("下载数据失败,改从本目录下delegated-apnic-latest.txt文件读取");
           // StreamReader srReader = new StreamReader("delegated-apnic-latest.txt");
            //var content = srReader.ReadToEnd();
            //strList = content.Split('\n').ToList(); ;

            //}


            List<RouteModel> routes = new List<RouteModel>();
            Regex reg = new Regex(@"apnic\|CN\|ipv4\|([\d\.]+)\|(\d+)\|");

            foreach (var item in strList)
            {
             
                var result = reg.Match(item).Groups;
                if (result[0].Value != "")
                {
                    routes.Add(new RouteModel()
                    {
                        Ip = result[1].Value,
                        Mask = CidrToIpAddress( result[2].Value).ToString()
                    });
            };
            }

            return routes;


        }

    }

}
