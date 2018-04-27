using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using WM.Common;


namespace TEST
{
    public static class MyTypeBuilder
    {
 
        public static Type CompileType(Dictionary<string,Type> fieldDict)
        {
            TypeBuilder tb = GetTypeBuilder();
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
            foreach (var field in fieldDict.AsEnumerable())
            CreateProperty(tb, field.Key, field.Value);
            Type objectType = tb.CreateType();
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder()
        {
            var typeSignature = "MyDynamicType";
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,TypeAttributes.Public |TypeAttributes.Class |TypeAttributes.AutoClass |TypeAttributes.AnsiClass |TypeAttributes.BeforeFieldInit |TypeAttributes.AutoLayout,null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, System.Reflection.PropertyAttributes.None, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =tb.DefineMethod("set_" + propertyName,MethodAttributes.Public |MethodAttributes.SpecialName |MethodAttributes.HideBySig,null, new[] { propertyType });
            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }




    /// <summary>
    /// generate by GenerateEntityFromDataTable
    /// </summary>
    public class weather
    {

        public System.Int64 ts { get; set; }

        public System.Double? ws { get; set; }

        public System.Double? wd { get; set; }

    }
    public abstract class A //抽象类A 
    {
        private int num = 0;
       
        public abstract void E(); //类A中的抽象方法E 

    }



    class testC
    {
       
 

        private static Lazy<testC> lt = new Lazy<testC>(() => new testC());

        public static testC Instance
        {
            get { return lt.Value; }
        }
 
        public static testC InstanceF()
        {
            return new testC(); ; 
        }


        private testC()
        {
           
        }

        public void Start()
        {
            
        }
    }


        class Program
    {
        //常用文件名称



        public static void Write(string path,string text)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(text);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
        const string GateWay = "192.168.0.1";
        public static void Main(string[] args)
        {

            // test();
            //StreamReader sr = new StreamReader("c:\\users\\zyl\\desktop\\xv\\a.csv", Encoding.Default);
            //String line ="";
            //string allString = "";

            //while ((line = sr.ReadLine()) != "")
            //{
            //    allString += line+"\n";

            //}
            //Write("c:\\users\\zyl\\desktop\\xv\\" + 0.ToString()+ ".txt", allString);
            //for (int i = 1; i < 3000; i++)
            //{
            //    line = sr.ReadLine();
            //    allString = "";
            //    while ((line = sr.ReadLine()) != "")
            //    {
            //        allString += line + "\n";

            //    }

            //    Write("c:\\users\\zyl\\desktop\\xv\\" + i.ToString() + ".txt", allString);
            //}



           // Console.ReadKey();
            //Cryptography cryptography = new Cryptography();
            //var des=cryptography.DecryptDES("IyHrl0HP5iI=");
            //Db2Word.Instance("dbconn2").Start();
            //testC.Instance.Start();
            //testC.Instance.Start();
            //Db2Word db2Word= new Db2Word();
            // db2Word.Start();
            //     var y = test();


            //SpiderShadowsocks spider2 = new SpiderShadowsocks();
            //spider2.Start();

            SpiderChinaIp spider = new SpiderChinaIp();
            List<RouteModel> list = spider.Start();


            Console.WriteLine("开始创建路由..大概需要1分钟...");

            list.AsEnumerable().Select(route =>
            {
                 // RouteTableManager.DeleteIpForwardEntry(IPAddress.Parse(route.Ip));
                 // Console.WriteLine("创建路由" + route.Ip);
                 RouteTableManager.CreateIpForwardEntry(
                   IPAddress.Parse(route.Ip),
                   IPAddress.Parse(route.Mask),
                   IPAddress.Parse(GateWay), 50);
                return route;
            }).ToList();

            Console.WriteLine("创建完成..");
            Console.ReadKey();
        }
        public static object test()
        {

            var o = Enumerable.Range(1, 2).Select((x, i) =>
            {
                long ts = 14287000000 + x;
                double? windspeed = i;
                double? windirection = x + i;
                // return new ArrayList() {ts, windspeed};             //return a Multiple type array if you return new []{ts,windspeed}  you will lose the type of the value;
                //return new Tuple<long,double?> (ts, windspeed);   //return a Multiple type Tuple  with anonymous key like Item1,Item2...etc;
                var a= new { ts = ts, ws = windspeed, wd = windirection };  //return a anonomous object
                return a;
            });
            var oL=o.ToList();
            //typeof(Nullable<>).MakeGenericType(r.DataType);
            var anonymousObj = new {  ts=14287000001, ws=  0.3, wd=  3 };
            //oL.Add(anonymousObj);
            var T = oL.GetType().GetGenericArguments()[0];
            var T2 = anonymousObj.GetType();

            var cc=T ==T2;


            var emp = GetDataTable("select * from eh_employee");
            
             var   arrayList = o.Select(objectItem => objectItem.GetType().GetProperties().Select(prop => prop.GetValue(objectItem))).ToList();//o is a object  List here;convert   List<object> to   List<ArrayList>,get all property;
            // var arrayList = o.Select(x => { return new ArrayList() { x.ts, x.ws, x.wd }; });//o is a object  List here;convert   List<object> to   List<ArrayList>，get specific property;
            //var arraylistFromTupleList = o.Select(x => new ArrayList() { x.Item1, x.Item2 }); //o is a Tuple List here;convert   List<Tuple> to   List<ArrayList>
            var dictListFromObjList= o.ToDictionary(x => x.ts, x => new ArrayList(){ x.wd,x.ws});//o is a object  List here;convert   List<object> to   List<Dictionary>
            var dt = ConvertToDatatable(oL);
            var objectEnumerable = dt.AsEnumerable();
            var classCode = GenerateEntityFromDataTable(dt, "weather");
            dt.Rows.Add(new object[] { 1, 2 });

            var listAuto = ConvertToList(dt);
            var list = ConvertToList<weather>(dt);
            return JsonConvert.SerializeObject(listAuto);
        }



        public static DataTable GetDataTable(string sql)
        {
            var ctx = new Model1();
            SqlConnection connection = (SqlConnection)ctx.Database.Connection;
            DataTable dt = new DataTable();
            var da = new SqlDataAdapter(sql, connection);
            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                da.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return dt;
        }

      

        private static List<object> ConvertToList(DataTable dt)
        {
            var fieldDict = new Dictionary<string, Type>();
            foreach (DataColumn r in dt.Columns)
            {
                var colNullValue = dt.AsEnumerable().Where(x => x[r] == DBNull.Value);
                if( (colNullValue.Any() || r.AllowDBNull) &&  r.DataType!=typeof(string)  )
                {
                   var type = typeof(Nullable<>).MakeGenericType(r.DataType); ;
                    fieldDict.Add(r.ColumnName,  type);
                }
                else
                {
                    fieldDict.Add(r.ColumnName, r.DataType);
                }
            }
            var resultList=new List<object>();
            foreach (var row in dt.AsEnumerable())
            {
                Type myType = MyTypeBuilder.CompileType(fieldDict);
                object myObject = Activator.CreateInstance(myType);
                var i = 0;
                foreach (var val in row.ItemArray)
                {
                    string columnName = dt.Columns[i].ColumnName;
                    PropertyInfo p = myType.GetProperty(columnName);
                    if (val != DBNull.Value)
                        p.SetValue(myObject, val);
                    i++;
                }
                resultList.Add(myObject);
            }
    
            return resultList;
        }


        private static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
             .Select(c => c.ColumnName)
             .ToList();

            var properties = typeof(T).GetProperties();

            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();

                foreach (var pro in properties)
                {
                    if (row[pro.Name] != DBNull.Value && columnNames.Contains(pro.Name))
                        pro.SetValue(objT, row[pro.Name]);
                }
                return objT;
            }).ToList();
     
        }

 

        /// <summary>
        /// 根据DataTable获取实体类的字符串
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static string GenerateEntityFromDataTable(DataTable dt, string className)
        {
            StringBuilder reval = new StringBuilder();
            StringBuilder propertiesValue = new StringBuilder();
           // string replaceGuid = Guid.NewGuid().ToString();
            foreach (DataColumn r in dt.Columns)
            {
                propertiesValue.AppendLine();
                var colNullValue = dt.AsEnumerable().Where(x => x[r] == DBNull.Value);
                string typeName = colNullValue.Any()||r.AllowDBNull?r.DataType.FullName + "?" : r.DataType.FullName;
                propertiesValue.AppendFormat("public {0} {1} {2}", typeName, r.ColumnName, "{get;set;}");
                propertiesValue.AppendLine();
            }
            reval.AppendFormat(@"
                 public class {0}{{
                        {1}
                 }}
            ", className, propertiesValue);


            return reval.ToString();
        }




        /// <summary>
        /// T不可以为object类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static DataTable ConvertToDatatable<T>(List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.IsGenericType &&prop.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                else
                {
                    table.Columns.Add(prop.Name, prop.PropertyType);
                    table.Columns[prop.Name].AllowDBNull = false;
                }
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

   
    }
}
