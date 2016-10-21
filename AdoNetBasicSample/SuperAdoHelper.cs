using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetBasicSample
{
    public partial class SuperAdoHelper
    {
        public string ConnectionString { get; private set; }

        public SuperAdoHelper(string connectionString)
        {
            if(connectionString == null || string.IsNullOrEmpty(connectionString.Trim()) == true)
            {
                throw new Exception("ConnectionString can not be empty.");
            }
            else
            {
                ConnectionString = connectionString;
            }
        }

        public object CreateAndRunQuery(QueryType queryType, string tableName, string[] columns, object[] parameters, params KeyValuePair<string, object>[] whereParameters)
        {
            SqlConnection baglanti = new SqlConnection(ConnectionString);
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;

            StringBuilder query = new StringBuilder(string.Empty);
            object result = null;

            Action<string[], object[]> parameterAddingAction =
                new Action<string[], object[]>((cols, pars) =>
                {
                    int counter = 0;

                    if (cols != null && cols.Length > 0 &&
                        pars != null && pars.Length > 0)
                    {
                        komut.Parameters.AddRange(
                           pars.Select(x =>
                           {
                               SqlParameter item = new SqlParameter($"@{cols[counter]}", x);
                               counter++;
                               return item;
                           }).ToArray());
                    }
                });


            switch (queryType)
            {
                case QueryType.Insert:
                    query.Append($"INSERT INTO {tableName}(");
                    query.Append(string.Join(",", columns));
                    query.Append(") VALUES (");
                    query.Append(string.Join(",", columns.Select(x => $"@{x} ")));
                    query.Append(")");

                    parameterAddingAction(columns, parameters);
                    break;
                case QueryType.Update:
                    query.Append($"UPDATE {tableName} SET ");
                    query.Append(string.Join(",", columns.Select(x => $"{x}=@{x} ")));

                    parameterAddingAction(columns, parameters);
                    break;
                case QueryType.Delete:
                    query.Append($"DELETE FROM {tableName} ");
                    break;
                case QueryType.Select:
                    query.Append($"SELECT ");
                    query.Append(string.Join(",", columns.Select(x => $"{x} ")));
                    query.Append($"FROM {tableName}");
                    break;
                default:
                    break;
            }

            if (whereParameters != null && whereParameters.Length > 0)
            {
                query.Append(" WHERE ");
                query.Append(string.Join(" AND ", whereParameters.Select(x => $"{x.Key}=@p_{x.Key}")));

                parameterAddingAction(
                    whereParameters.Select(x => $"p_{x.Key}").ToArray(),
                    whereParameters.Select(x => x.Value).ToArray());
            }

            komut.CommandText = query.ToString();

            switch (queryType)
            {
                case QueryType.Insert:
                case QueryType.Update:
                case QueryType.Delete:
                    baglanti.Open();
                    result = komut.ExecuteNonQuery();
                    baglanti.Close();
                    break;

                case QueryType.Select:
                    DataTable dt = new DataTable(tableName);
                    SqlDataAdapter adapter = new SqlDataAdapter(komut);
                    adapter.Fill(dt);
                    result = dt;
                    adapter.Dispose();
                    break;

                default:
                    break;
            }

            komut.Dispose();
            baglanti.Dispose();

            return result;
        }
    }

    public enum QueryType
    {
        Insert = 0,
        Update = 1,
        Delete = 2,
        Select = 3
    }
}
