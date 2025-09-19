using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;

namespace CommonOperations.CommonMethods
{
    public static class CommonMethod
    {


        public static string GetConnectionString()
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
            .Build();
            return configuration.GetConnectionString("ConnectionString");
        }

        public static List<dynamic> ExecuteStoredProcedure(DynamicParameters parameters, string storedProcedureName)
        {
            string connectionString = GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = connection.Query<dynamic>(
                    storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return data;
            }
        }

        public static void UploadBase64Data(string base64Data, string filePath)
        {
            if (string.IsNullOrEmpty(base64Data))
            {
                throw new ArgumentException("Base64 data is empty or null.");
            }
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            System.IO.File.WriteAllTextAsync(filePath, base64Data);
        }

        public static string RetrieveBase64Data(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return "no file found";
            }
            string base64Data = System.IO.File.ReadAllText(filePath);

            return base64Data;
        }

        public static String ConvertStringToShah256(string value)
        {
            StringBuilder Sb = new StringBuilder();
            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }
        public static string EncrypthePassword(string Password)
        {
            string password = Base64Encode(Password);
            return ConvertStringToShah256(password);
        }
        public static string Base64Encode(string password)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static List<TModel> ExecuteStoredProcedureAndMaptoModel<TModel>(string storedProcedureName, object parameters = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(GetConnectionString()))
            {
                dbConnection.Open();
                var result = dbConnection.QueryMultiple(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
                var models = result.Read<TModel>();
                return models.ToList();
            }
        }


        public static IList<T> JsonToListClass<T>(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                return new List<T>();
            var data = JsonConvert.DeserializeObject<List<T>>(jsonString);
            return data;
        }

        public static string GenerateJwtToken(string UserEmail, long UserID, string UserName, IConfiguration config)
        {

            var authClaims = new List<Claim>
            {
                new Claim("Email", UserEmail),
                new Claim("ID", UserID.ToString()),
                new Claim("UserName", UserName)
            };
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: config["JWT:ValidIssuer"],
                audience: config["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddDays(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public static bool SoftDeleteRecord(long Id, string TableName)
        {
            string connectionString = GetConnectionString();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string updateQuery = $@"
                UPDATE {TableName} 
                SET 
                IsDeleted = 1,
                UpdatedBy = @UpdatedBy,
                UpdatedDate = @UpdateDate
                WHERE Id = @Id";
                var parameters = new
                {
                    Id = Id,
                    //UpdatedBy = TokenVm.UserEmail,
                    UpdateDate = DateTime.Now
                };

                var affectedRows = dbConnection.Execute(updateQuery, parameters);
                return affectedRows > 0;
            }
        }


    }
}
