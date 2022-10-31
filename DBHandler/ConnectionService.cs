
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace GeoLocationAPI.DBHandler
{
    
    public interface IConnectionService
    {
        string GetDBConnection(IConfiguration configuration, IHttpContextAccessor httpContext,
            string connectionValue = "DBSettings:ClientDB", string tokenHeader = "Authorization", string additionalInfo = "info");

        SQLConfig GetNoSQLConfig(IConfiguration configuration, string connectionValue = "ClientDB",
            string collectionName = "CollectionName", string dbName = "DataBaseName");
        //AuthUser GetAuthUser();
    }
    //Ref: Connection String
    //https://www.connectionstrings.com/sqlite/
    public class ConnectionService : IConnectionService
    {
        string _connectionString;
        //public AuthUser _AuthUser;

        public string GetDBConnection(IConfiguration configuration, IHttpContextAccessor httpContext,
            string connectionValue = "DBSettings:ClientDB", string tokenHeader = "Authorization", string additionalInfo = "info")
        {
            #region Required when we try to load the connection string/DB via JWT Token
            //if (httpContext != null)
            //{
            //    IHeaderDictionary headers = httpContext.HttpContext.Request.Headers;
            //    _connectionString = configuration.GetValue<string>(connectionValue);
            //    StringValues tokenString;
            //    headers.TryGetValue(tokenHeader, out tokenString);
            //    var jwt = tokenString.ToString();
            //    var jwtEncodedString = jwt.Substring(7);
            //}
            // trim 'Bearer ' from the start since its just a prefix for the 
            ////var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
            //var userInfo = token.Claims.First(c => c.Type == additionalInfo).Value;
            ////Console.WriteLine("email => " + token.Claims.First(c => c.Type == "email").Value);
            //AuthUser authUser = JsonConvert.DeserializeObject<AuthUser>(userInfo);
            //_connectionString = string.Format(_connectionString, authUser.ClientDBName);
            //_AuthUser = authUser;
            #endregion

            //_connectionString = "Data Source=GeoLocDB.db;Version=3;New=True;";
            _connectionString = string.Empty;
            return _connectionString;
        }

        public SQLConfig GetNoSQLConfig(IConfiguration configuration, string connectionValue = "DBSettings:ClientDB", string collectionName = "DBSettings:CollectionName", string dbName = "DBSettings:DataBaseName")
        {
            SQLConfig config = new SQLConfig()
            {
                ConnectionString = configuration.GetValue<string>(connectionValue),
                CollectionName = configuration.GetValue<string>(collectionName),
                DataBaseName = configuration.GetValue<string>(dbName)
            };
            return config;
        }
    }

    public class SQLConfig
    {
        public string ConnectionString { get; set; }
        public BaseTrace Trace { get; set; }
        public string DataBaseName { get; set; }
        /// <summary>
        /// Normally called as Collection in MongoDB, similar to Tables in SQL World.
        /// </summary>
        public string CollectionName { get; set; }
        public SQLCompiler Compiler { get; set; }

        /// <summary>
        /// Default DB type is Releational DB.
        /// </summary>
        public DBType DBType { get; set; } = DBType.SQL;
    }

    public enum DBType
    {
        SQL,
        NoSQL
    }
    public enum SQLCompiler
    {
        SQLServer,
        PostgreSQL,
        MySQL,
        SQLite,
        MongoDB
    }
}
