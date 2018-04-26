
using Dapper.BLL;
using System;
namespace Dapper.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            UsersBLL bll = new UsersBLL();
            try
            {
                if(bll.Insert(new Model.Users { Number = "sa", Name = "管理员", Password = "sa" }))
                    System.Console.WriteLine("新增成功");
            }
            catch(Exception exp)
            {
                System.Console.WriteLine(exp.Message);
            }
            //TestDB();
        }

        private static void TestDB()
        {
            var projLoc = System.Reflection.Assembly.GetAssembly(typeof(Program)).Location;
            var projFolder = System.IO.Path.GetDirectoryName(projLoc);

            if (System.IO.File.Exists(projFolder + "\\kinfar.sdf"))
                System.IO.File.Delete(projFolder + "\\kinfar.sdf");
            var connectionString = "Data Source = " + projFolder + "\\kinfar.sdf;";
            var engine = new System.Data.SqlServerCe.SqlCeEngine(connectionString);
            engine.CreateDatabase();
            using (var connection = new System.Data.SqlServerCe.SqlCeConnection(connectionString))
            {
                connection.Open();
                connection.Execute(@" create table Users (Id int IDENTITY(1,1) not null, Name nvarchar(100) not null, Age int not null) ");
                connection.Execute(@" create table Automobiles (Id int IDENTITY(1,1) not null, Name nvarchar(100) not null) ");
                connection.Execute(@" create table Results (Id int IDENTITY(1,1) not null, Name nvarchar(100) not null, [Order] int not null) ");
            }
            System.Console.WriteLine("Created database");
        }
    }
}