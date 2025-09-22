using System.Configuration;

namespace PR.Persistence.EntityFrameworkCore.PostgreSQL
{
    public static class ConnectionStringProvider
    {
        private static string _schema;
        private static string _connectionString;

        static ConnectionStringProvider()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (env == "Production")
            {
                // Use connection string provided at runtime by Heroku.
                var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                // Parse connection URL to connection string for Npgsql
                connUrl = connUrl.Replace("postgres://", string.Empty);
                var pgUserPass = connUrl.Split("@")[0];
                var pgHostPortDb = connUrl.Split("@")[1];
                var pgHostPort = pgHostPortDb.Split("/")[0];
                var pgDb = pgHostPortDb.Split("/")[1];
                var pgUser = pgUserPass.Split(":")[0];
                var pgPass = pgUserPass.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];

                _connectionString = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}; SSL Mode=Require; Trust Server Certificate=true";
            }
            else
            {
                InitializeFromSettingsFile();
            }
        }

        public static void InitializeFromSettingsFile()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            var host = settings["Host"]?.Value;
            var port = settings["Port"]?.Value;
            var database = settings["Database"]?.Value;
            var schema = settings["Schema"]?.Value;
            var user = settings["User"]?.Value;
            var password = settings["Password"]?.Value;

            if (string.IsNullOrEmpty(host) ||
                string.IsNullOrEmpty(port) ||
                string.IsNullOrEmpty(database) ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(user) ||
                string.IsNullOrEmpty(password))
            {
                // If we are here, it may be because we're attempting to generate a migration
                host = "localhost";
                port = "5432";
                database = "PR";
                schema = "public";
                user = "postgres";
                password = "L1on8Zebra";
            }

            Initialize(host, string.IsNullOrEmpty(port) ? 5432 : int.Parse(port), database, schema, user, password);
        }

        private static void Initialize(
            string host,
            int port,
            string database,
            string schema,
            string user,
            string password)
        {
            _schema = schema;

            if (string.IsNullOrEmpty(host) ||
                string.IsNullOrEmpty(database) ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(user) ||
                string.IsNullOrEmpty(password))
            {
                return;
            }

            _connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={database}";
        }

        public static string GetConnectionString()
        {
            return _connectionString;
        }

        public static string GetPostgreSqlSchema()
        {
            return _schema;
        }
    }
}
