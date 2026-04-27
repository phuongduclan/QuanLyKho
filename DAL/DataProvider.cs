using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace QuanLyKho.DAL
{
    public class DataProvider
    {
        /// <summary>Lấy tên tham số @xxx theo thứ tự xuất hiện (tránh lỗi @NgayBatDau, khi tách theo khoảng trắng).</summary>
        private static List<string> ExtractParameterNames(string query)
        {
            var list = new List<string>();
            foreach (Match m in Regex.Matches(query, @"@\w+"))
                list.Add(m.Value);
            return list;
        }

        private static DataProvider? _instance;

        public static DataProvider Instance
        {
            get
            {
                _instance ??= new DataProvider();
                return _instance;
            }
        }

        private DataProvider() { }

        private readonly string connectionSTR =
            "Data Source=localhost\\SQLEXPRESS;" +
            "Initial Catalog=warehouse_management;" +
            "Integrated Security=True;" +
            "Encrypt=True;" +
            "TrustServerCertificate=True;";

        public DataTable ExecuteQuery(string query, object[]? parameter = null)
        {
            var data = new DataTable();
            using (var connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                using var command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    var names = ExtractParameterNames(query);
                    for (int i = 0; i < names.Count && i < parameter.Length; i++)
                        command.Parameters.AddWithValue(names[i], parameter[i] ?? DBNull.Value);
                }

                using var adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
            }

            return data;
        }

        public int ExecuteNonQuery(string query, object[]? parameter = null)
        {
            using var connection = new SqlConnection(connectionSTR);
            connection.Open();
            using var command = new SqlCommand(query, connection);
            if (parameter != null)
            {
                var names = ExtractParameterNames(query);
                for (int i = 0; i < names.Count && i < parameter.Length; i++)
                    command.Parameters.AddWithValue(names[i], parameter[i] ?? DBNull.Value);
            }

            return command.ExecuteNonQuery();
        }

        public object? ExecuteScalar(string query, object[]? parameter = null)
        {
            using var connection = new SqlConnection(connectionSTR);
            connection.Open();
            using var command = new SqlCommand(query, connection);
            if (parameter != null)
            {
                var names = ExtractParameterNames(query);
                for (int i = 0; i < names.Count && i < parameter.Length; i++)
                    command.Parameters.AddWithValue(names[i], parameter[i] ?? DBNull.Value);
            }

            return command.ExecuteScalar();
        }

        /// <summary>
        /// Thực thi lệnh SQL/thủ tục có tham số rõ ràng (không dùng split chuỗi).
        /// </summary>
        public int ExecuteNonQueryTyped(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using var connection = new SqlConnection(connectionSTR);
            connection.Open();
            using var command = new SqlCommand(commandText, connection) { CommandType = commandType };
            if (parameters is { Length: > 0 })
                command.Parameters.AddRange(parameters);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Gọi thủ tục rồi lấy IDENT_CURRENT cho bảng identity (script không trả về SCOPE_IDENTITY).
        /// </summary>
        public int ExecuteStoredProcedureThenIdentCurrent(string procedureName, SqlParameter[]? procedureParameters, string identTableName)
        {
            if (string.IsNullOrWhiteSpace(identTableName) || identTableName.Contains('\'') || identTableName.Contains(';'))
                throw new ArgumentException(nameof(identTableName));

            using var connection = new SqlConnection(connectionSTR);
            connection.Open();
            using (var cmd = new SqlCommand(procedureName, connection) { CommandType = CommandType.StoredProcedure })
            {
                if (procedureParameters is { Length: > 0 })
                    cmd.Parameters.AddRange(procedureParameters);
                cmd.ExecuteNonQuery();
            }

            using var cmd2 = new SqlCommand($"SELECT CAST(IDENT_CURRENT(N'{identTableName}') AS INT)", connection);
            return Convert.ToInt32(cmd2.ExecuteScalar());
        }
    }
}
