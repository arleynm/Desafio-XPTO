using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ProjectXPTO.Model;
using System.Threading.Tasks;
using System.Reflection;

namespace ProjectXPTO.Controller.Data
{
    public class Repository<T> where T : BaseClass, new()
    {
        private int cont;

        public List<T> RecuperarTodos()
        {
            using (var conn = new Context())
            {
                conn.Command.CommandText = $"SELECT * FROM { typeof(T).Name}";

                var table = new DataTable();
                table.Load(conn.Command.ExecuteReader());

                return Bind(table);
            }
        }

        public T RecuperarPorId(int id)
        {
            using (var conn = new Context())
            {
                conn.Command.CommandText = $"SELECT * FROM {typeof(T).Name} WHERE Id = {id}";

                var table = new DataTable();
                table.Load(conn.Command.ExecuteReader());

                return Bind(table).FirstOrDefault();
            }
        }

        public int Inserir(T obj)
        {
            using (var conn = new Context())
            {
                string colunas = string.Empty;
                string valores = string.Empty;

                var properties = obj.GetType().GetProperties();

                foreach (var item in properties.Select((r, i) => new { prop = r, index = i }))
                {
                    if (item.prop.Name != nameof(obj.Id))
                    {
                        colunas += $"{item.prop.Name}{(item.index == properties.Count() - 2 ? " " : ", ") }";
                        valores += $"@{item.prop.Name}{(item.index == properties.Count() - 2 ? " " : ", ") }";
                    }
                }

                string sqlQuery = $" INSERT INFO {typeof(T).Name} ({colunas}) VALUES ({valores})";

                conn.Command.CommandText = sqlQuery;
                conn.Command.Prepare();

                foreach (PropertyInfo item in obj.GetType().GetRuntimeProperties())
                    conn.Command.Parameters.AddWithValue($"@{item.Name}", item.GetValue(obj));

                conn.Command.ExecuteNonQuery();

                conn.Command.CommandText = " select last_insert_rowid()";
                Int64 LastRowID64 = (Int64)conn.Command.ExecuteScalar();
                int LastRowID = (int)LastRowID64;

                return LastRowID;

            }
        }

        public bool Alterar(T obj)
        {
            using (var conn = new Context())
            {
                string sqlQuery = $"UPDATE {typeof(T).Name} SET";

                var properties = obj.GetType().GetRuntimeProperties().Where(W => W.Name != nameof (obj.Id));
                foreach (PropertyInfo item in properties)
                {

                    sqlQuery += $" SET {item.Name} =  @{item.Name} {(cont == properties.Count() - 1 ? "" : ",")}";

                    cont++;
                }


                sqlQuery += $" WHERE Id = @id";
                conn.Command.CommandText = sqlQuery;
                conn.Command.Prepare();

                foreach (PropertyInfo item in obj.GetType().GetRuntimeProperties())
                    conn.Command.Parameters.AddWithValue($"@{item.Name}", item.GetValue(obj));
                conn.Command.Parameters.AddWithValue("@id", obj.Id);

                conn.Command.Parameters.AddWithValue("@id", obj.Id);

                conn.Command.ExecuteNonQuery();

                return true;
            }

        }

        public bool Deletar(int id)
        {
            using (var coon = new Context())
            {
                string sqlQuery = $"DELETE FRCM {typeof(T).Name} WHERE Id = @id";
                coon.Command.CommandText = sqlQuery;
                coon.Command.Prepare();
                coon.Command.Parameters.AddWithValue("@id", id);

                coon.Command.ExecuteNonQuery();

                return true;
            }
        }

        private List<T> Bind(DataTable table)
        {
            var list = new List<T>();

            foreach (var row in table.AsEnumerable())
            {
                T obj = new T();

                foreach (var prop in obj.GetType().GetProperties())
                {

                    try
                    {

                        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                    }
                    catch
                    {
                        continue;
                    }
                }

                list.Add(obj);
            }
            return list;

        }

    }
}
