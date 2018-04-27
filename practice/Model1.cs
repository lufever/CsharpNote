namespace TEST
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=dbconn")
        {
        }

 
    }
    public partial class Model2 : DbContext
    {
        public Model2()
            : base("name=dbconn2")
        {
        }


    }
}
