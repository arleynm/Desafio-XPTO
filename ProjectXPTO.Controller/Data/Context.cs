using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectXPTO.Controller.Data
{
    public class Context : IDisposable
    {
        private string _databasePach = $"{AppDomain.CurrentDomain.BaseDirectory}project-xpto.s3db";
        private SQLiteConnection Connection { get; set; }

        public SQLiteCommand Command { get; private set; }

        public Context()
        {
            CreateDatabase();

            Connection = new SQLiteConnection();
            Command = Connection.CreateCommand();
            Command.Connection.ConnectionString = $"Data Source=(_databasePach)";
            Command.Connection.Open();

            CreateTable();
        }

        private void CreateDatabase()
        {
            if (System.IO.File.Exists(_databasePach))
                System.IO.File.Create(_databasePach);

        }
        private void CreateTable()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS Cliente(
                         Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                         Nome NVARCHAR (100) NULL,
                         Cpf NVACHAR (11) NULL,
                         DataNascimento DATE NULL,
                         DataRegistro DATATIME NULL,
                        );";
            Command.CommandText = sql;
            Command.ExecuteNonQuery();
        }

        public void DropDatabase()
        {
            if (System.IO.File.Exists(_databasePach))
                System.IO.File.Delete(_databasePach);
        }
        public void Dispose()
        {
            if (Command.Connection.State == System.Data.ConnectionState.Open)
                Command.Connection.Close();
        }

    }
}
