using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace TEST
{
    class SpiderShadowsocks
    {
        public void Start()
        {

 
            // 设置配置以支持文档加载
            var config = Configuration.Default.WithDefaultLoader();
            // 豆瓣地址
            var address = "https://ssssssssjshhd.herokuapp.com";
            // 请求豆辨网
            var document = BrowsingContext.New(config).OpenAsync(address);
            var dom = document.Result;

            // 根据class获取html元素
            var cells = dom.QuerySelectorAll("ol li a").OfType<IHtmlAnchorElement>(); ;
         
            // We are only interested in the text - select it with LINQ
 
            StreamWriter sw = new StreamWriter(@"D:\a.txt");
            foreach (var item in cells)
            {
                 
                document = BrowsingContext.New(config).OpenAsync(item.Href);
                dom = document.Result;
                var cell = dom.QuerySelectorAll("input").OfType<IHtmlInputElement>(); ; ;
                var ssr = cell.AsEnumerable().FirstOrDefault().Value;
                sw.Write(ssr);
                sw.WriteLine();
            }
 
         sw.Close();


        }

    }

}
