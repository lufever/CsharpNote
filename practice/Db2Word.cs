using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Tables;
 
using System.Configuration;
using System.Data.Common;

namespace TEST
{
 
    class Db2Word
    {
   

        private string conn ;
        private Db2Word(string dbconnname)
        {
            conn = ConfigurationManager.ConnectionStrings[dbconnname].ConnectionString;
        }


        public static Db2Word Instance(string dbconnname)
        {
            return  new Db2Word(dbconnname);
        }
       

        public static string key =
              "PExpY2Vuc2U+DQogIDxEYXRhPg0KICAgIDxMaWNlbnNlZFRvPkFzcG9zZSBTY290bGFuZCB" +
              "UZWFtPC9MaWNlbnNlZFRvPg0KICAgIDxFbWFpbFRvPmJpbGx5Lmx1bmRpZUBhc3Bvc2UuY2" +
              "9tPC9FbWFpbFRvPg0KICAgIDxMaWNlbnNlVHlwZT5EZXZlbG9wZXIgT0VNPC9MaWNlbnNlV" +
              "HlwZT4NCiAgICA8TGljZW5zZU5vdGU+TGltaXRlZCB0byAxIGRldmVsb3BlciwgdW5saW1p" +
              "dGVkIHBoeXNpY2FsIGxvY2F0aW9uczwvTGljZW5zZU5vdGU+DQogICAgPE9yZGVySUQ+MTQ" +
              "wNDA4MDUyMzI0PC9PcmRlcklEPg0KICAgIDxVc2VySUQ+OTQyMzY8L1VzZXJJRD4NCiAgIC" +
              "A8T0VNPlRoaXMgaXMgYSByZWRpc3RyaWJ1dGFibGUgbGljZW5zZTwvT0VNPg0KICAgIDxQc" +
              "m9kdWN0cz4NCiAgICAgIDxQcm9kdWN0PkFzcG9zZS5Ub3RhbCBmb3IgLk5FVDwvUHJvZHVj" +
              "dD4NCiAgICA8L1Byb2R1Y3RzPg0KICAgIDxFZGl0aW9uVHlwZT5FbnRlcnByaXNlPC9FZGl" +
              "0aW9uVHlwZT4NCiAgICA8U2VyaWFsTnVtYmVyPjlhNTk1NDdjLTQxZjAtNDI4Yi1iYTcyLT" +
              "djNDM2OGYxNTFkNzwvU2VyaWFsTnVtYmVyPg0KICAgIDxTdWJzY3JpcHRpb25FeHBpcnk+M" +
              "jAxNTEyMzE8L1N1YnNjcmlwdGlvbkV4cGlyeT4NCiAgICA8TGljZW5zZVZlcnNpb24+My4w" +
              "PC9MaWNlbnNlVmVyc2lvbj4NCiAgICA8TGljZW5zZUluc3RydWN0aW9ucz5odHRwOi8vd3d" +
              "3LmFzcG9zZS5jb20vY29ycG9yYXRlL3B1cmNoYXNlL2xpY2Vuc2UtaW5zdHJ1Y3Rpb25zLm" +
              "FzcHg8L0xpY2Vuc2VJbnN0cnVjdGlvbnM+DQogIDwvRGF0YT4NCiAgPFNpZ25hdHVyZT5GT" +
              "zNQSHNibGdEdDhGNTlzTVQxbDFhbXlpOXFrMlY2RThkUWtJUDdMZFRKU3hEaWJORUZ1MXpP" +
              "aW5RYnFGZkt2L3J1dHR2Y3hvUk9rYzF0VWUwRHRPNmNQMVpmNkowVmVtZ1NZOGkvTFpFQ1R" +
              "Hc3pScUpWUVJaME1vVm5CaHVQQUprNWVsaTdmaFZjRjhoV2QzRTRYUTNMemZtSkN1YWoyTk" +
              "V0ZVJpNUhyZmc9PC9TaWduYXR1cmU+DQo8L0xpY2Vuc2U+";



        private Stream GetLicense()
        {
            return new MemoryStream(Convert.FromBase64String(key));
        }



        /// <summary>
        ///     ASPOSEWORD
        /// </summary>
        private  void SetLicense()
        {
           
            var lic = GetLicense();
            new License().SetLicense(lic);

        }

        public   DataTable GetDataTable(string sql)
        {

            SqlConnection connection = new SqlConnection(conn);
           // Model2 model = new Model2();
            //SqlConnection connection = (SqlConnection)model.Database.Connection;
            DataTable dt = new DataTable();
            var da = new SqlDataAdapter(sql, connection);
            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                da.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return dt;
        }


        public DataTable GetTableNames()
        {
            string sql = "select id,name from sysobjects where xtype='U'and [name]<>'dtproperties' order by [name]";
            var tbs = this.GetDataTable(sql);
            return tbs;
        }


        public  DataTable GetTableInfo(string tbId)
        {
 
            StringBuilder builder = new StringBuilder();

            var sql2 = $@"SELECT
                        字段编号 = tbcolumn.colOrder,
                        英文字段名 = tbcolumn.name,
                     中文字段名 = ISNULL(tbprop.[value], ''),
                    字段类型 = CASE
                    WHEN tbtype.name = 'nvarchar' THEN

                        tbtype.name + '(' + CAST(tbcolumn.length / 2 AS varchar) + ')'
                    WHEN tbtype.name = 'varchar' THEN

                        tbtype.name + '(' + CAST(tbcolumn.length  AS varchar) + ')'
                    ELSE

                        tbtype.name
                    END,

                    备注 = CASE
                    WHEN EXISTS(
                        SELECT

                            1

                        FROM

                            dbo.sysindexes si

                        INNER JOIN dbo.sysindexkeys sik ON si.id = sik.id

                        AND si.indid = sik.indid

                        INNER JOIN dbo.syscolumns sc ON sc.id = sik.id

                        AND sc.colid = sik.colid

                        INNER JOIN dbo.sysobjects so ON so.name = so.name

                        AND so.xtype = 'PK'

                        WHERE

                            sc.id = tbcolumn.id

                        AND sc.colid = tbcolumn.colid
                    ) THEN
                    '主键'
                    ELSE

                        ''
                    END

                      FROM

                        dbo.syscolumns tbcolumn
                    LEFT JOIN dbo.systypes tbtype ON tbcolumn.xtype = tbtype.xusertype
                    LEFT JOIN   sysobjects  tbo on  tbcolumn.id = tbo.id   AND(tbo.xtype = 'U' OR tbo.xtype = 'V') AND tbo.status >= 0
                    LEFT JOIN sys.extended_properties tbprop ON tbcolumn.id = tbprop.major_id  AND tbcolumn.colid = tbprop.minor_id where tbo.id = {tbId} ";

            var tbinfo = GetDataTable(sql2);
            return tbinfo;
        }

    
        public static void WriteTable(DocumentBuilder builder, DataTable dt)
        {
            builder.MoveToDocumentEnd();
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                builder.InsertCell();
                builder.Write(dt.Columns[i].ColumnName);
            }

            builder.EndRow();

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                for (var j = 0; j < dt.Columns.Count; j++)
                {

                    builder.InsertCell();
                    //  builder.Bold = bold != null && (_builder.Bold = bold.Item2 <= j && j <= bold.Item3);
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.Write(dt.Rows[i][j].ToString());

                }
                builder.EndRow();
            }
            builder.EndTable();
            builder.Write(Environment.NewLine);
        }
        public   Document OpenWord(string docx)
        {
            if (!File.Exists(docx)) CreateWord(docx);
            var doc = new Document(docx);
            return doc;
        }

        public  void CreateWord(string docx)
        {
            var doc = new Document();
            doc.Save(docx, Aspose.Words.SaveFormat.Docx);
        }

        public  void Start()
        {
          SetLicense();
          Document doc =  OpenWord("c:\\abc.docx"); 
            var builder = new DocumentBuilder(doc);
            int i = 0;
            var tableRows = GetTableNames().Rows;
            foreach (DataRow dataRow in tableRows)
            {

                var tbId = dataRow["id"].ToString();
                var tbName = dataRow["name"].ToString();
                

                  var tbInfo = GetTableInfo(tbId);
                var tbl = builder.StartTable();


                for (var n = 0; n < tbInfo.Columns.Count; n++)
                {
                    builder.InsertCell();
                    if (n == 0)
                    {
                        builder.Write("表编号：" + (i+1).ToString().PadLeft(tableRows.Count.ToString().Length, '0'));
                    }
                }

                builder.EndRow();
                for (var n = 0; n < tbInfo.Columns.Count; n++)
                {
                    builder.InsertCell();
                    if (n == 0)
                    {
                        builder.Write("表英文名称：" + tbName);
                    }
                }

                builder.EndRow();
                for (var n = 0; n < tbInfo.Columns.Count; n++)
                {
                    builder.InsertCell();
                    if (n == 0)
                    {
                        builder.Write("表中文名称：");
                    }
                }
                builder.EndRow();



                for (var j = 0; j < 3; j++)
                {
                    tbl.Rows[j].Cells[0].CellFormat.HorizontalMerge = CellMerge.First;
                    for (var k = 1; k < tbInfo.Columns.Count; k++)
                        tbl.Rows[j].Cells[k].CellFormat.HorizontalMerge = CellMerge.Previous;
                }





                WriteTable(builder, tbInfo);
                i++;
            }
            doc.Save("c:\\abc.docx");

   
        }

 




    }



}
