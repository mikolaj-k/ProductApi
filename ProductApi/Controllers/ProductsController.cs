using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProductApi.Extensions;

namespace ProductApi.Controllers
{

    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFileManager _fileManager;
        private readonly IDataManager _dataManager;

        public ProductsController(IConfiguration configuration,
                                  IFileManager fileManager,
                                  IDataManager productsManager)
        {
            _configuration = configuration;
            _fileManager = fileManager;
            _dataManager = productsManager;
        }

        [HttpPost("import-data")]
        public async Task<IActionResult> ImportData()
        {
            var blobStorage = _configuration.GetValue(typeof(string), "BlobStorage");
            var localStorage = _configuration.GetValue(typeof(string), "LocalStorage");

            await _dataManager.CleanStagingTables();

            var productsUrl = blobStorage + "/Products.csv";
            var productsFilePath = localStorage + "\\products.csv";

            await _fileManager.DownloadFile(productsUrl, productsFilePath);
            await _dataManager.LoadStagingData(productsFilePath, ';', "StgProducts");
            await _dataManager.ExecuteNonQuery(Repository.ProductsMerge);

            var pricesUrl = blobStorage + "/Prices.csv";
            var pricesFilePath = localStorage + "\\prices.csv";

            await _fileManager.DownloadFile(pricesUrl, pricesFilePath);
            await _dataManager.LoadStagingData(pricesFilePath, ',', "StgPrices", false);
            await _dataManager.ExecuteNonQuery(Repository.PricesMerge);

            var invetoryUrl = blobStorage + "/Inventory.csv";
            var inventoryFilePath = localStorage + "\\inventory.csv";

            await _fileManager.DownloadFile(invetoryUrl, inventoryFilePath);
            await _dataManager.LoadStagingData(inventoryFilePath, ',', "StgInventories");
            await _dataManager.ExecuteNonQuery(Repository.InventoryMerge);

            return Ok("Imported data sucesfully");
        }

        [HttpGet("get-product/{sku}")]
        public async Task<IActionResult> GetProductBySku(string sku)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            var product = await connection.QueryAsync(Repository.SelectProduct(sku));

            if (product == null)
                return NotFound("Produkt nie został znaleziony");

            return Ok(product);
        }
    }
}
