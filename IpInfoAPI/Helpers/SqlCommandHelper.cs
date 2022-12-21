using IpInfoAPI.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IpInfoAPI.Helpers
{
    public class SqlCommandHelper
    {
        private const string ConnectionString = "Server=localhost; Database=db; Trusted_Connection = True; Encrypt=False;";

        /// <summary>
        /// Runs a SQL query using the given string
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static async Task<List<Dictionary<string, object>>> RunQueryStringSelect(string query)
        {
            SqlConnection sqlConnection = new(ConnectionString);
            SqlCommand command = new(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = await command.ExecuteReaderAsync();
            List<Dictionary<string, object>> rows = new();
            int i = 0;
            while (sqlDataReader.Read())
            {
                Dictionary<string, object> columns = new();
                for (int j = 0; j < sqlDataReader.FieldCount; j++)
                {
                    string ColumnName = sqlDataReader.GetName(j);
                    columns.Add(ColumnName, sqlDataReader.GetValue(j));
                }
                rows.Add(columns);
                i++;
            }
            return rows;
        }
        /// <summary>
        /// Queries for country response (raw SQL)
        /// </summary>
        /// <param name="TwoLetterList"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<CountryRes>?> QueryCountryRes(string[]? TwoLetterList)
        {
            string query = @"SELECT 
                            DISTINCT (c.Name) as CountryName,
                            (SELECT COUNT(*) FROM IPAddresses i2 WHERE i2.CountryId = i.CountryId) as AddressesCount,
                            (SELECT MAX(i3.UpdatedAt) FROM IPAddresses i3 WHERE i3.CountryId = c.Id  ) as LastAddressUpdated
                            FROM Countries c 
                            LEFT JOIN IPAddresses i on c.Id = i.CountryId 
                            WHERE c.TwoLetterCode ";
            if (TwoLetterList == null || TwoLetterList.Length <= 0)
            {
                query += "IS NOT NULL ;";
            }
            else
            {
                string CountryCodes = "'" + String.Join("','", TwoLetterList) + "'";
                query += $"IN({CountryCodes}) ;";
            }
            List<Dictionary<string, object>> response = await RunQueryStringSelect(query);
            return response.Select(r => new CountryRes()
            {
                CountryName = r["CountryName"].ToString(),
                AddressessCount = (int)r["AddressesCount"],
                LastAddressUpdated = DateTime.TryParse(r["LastAddressUpdated"].ToString(), out DateTime t) ? t : null
            });
        }
    }
}
