using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace ProductApi.Extensions
{
    public interface IDataManager
    {
        Task CleanStagingTables();
        Task LoadStagingData(string filePath, char delimiter, string tableName, bool hasHeader = true);
        Task ExecuteNonQuery(string sql);
    }

    public class DataManager : IDataManager
    {
        private readonly IConfiguration _configuration;

        public DataManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task CleanStagingTables()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            await connection.ExecuteAsync("TRUNCATE TABLE StgProducts");
            await connection.ExecuteAsync("TRUNCATE TABLE StgPrices");
            await connection.ExecuteAsync("TRUNCATE TABLE StgInventories");
        }

        public async Task ExecuteNonQuery(string sql)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync(sql);
        }

        public async Task LoadStagingData(string filePath, char delimiter, string tableName, bool hasHeader = true)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeader,
                Delimiter = delimiter.ToString(),
                MissingFieldFound = null,
                BadDataFound = null
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, csvConfig);

            using var dr = new CsvDataReader(csv);

            using var bulk = new SqlBulkCopy(connectionString)
            {
                DestinationTableName = tableName,
                BatchSize = 1000,
                BulkCopyTimeout = 0
            };

            if (hasHeader)
            {
                foreach (var header in csv.HeaderRecord)
                {
                    if (!string.IsNullOrEmpty(header))
                        bulk.ColumnMappings.Add(header, header);
                }
            }

            await bulk.WriteToServerAsync(dr);
        }
    }
}
