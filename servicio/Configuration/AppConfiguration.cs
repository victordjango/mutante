using Microsoft.Extensions.Configuration;
using System.IO;


namespace myMicroservice.Configuration
{
    public class AppConfiguration
    {
        private readonly string _sqlConnection;

        private static AppConfiguration _instance;

        public static AppConfiguration Instance
        {
            get {

                if (_instance == null)
                {
                    _instance = new AppConfiguration();
                }
                return _instance;
            }
        }

        private AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            _sqlConnection = root.GetConnectionString("SQLConnectionString");
            
        }

        public string SqlDataConnection
        {
            get => _sqlConnection;
        }
        
    }
}
