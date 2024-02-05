namespace ProductApi.Extensions
{
    public interface IFileManager
    {
        Task DownloadFile(string url, string localPath);
    }

    public class FileManager : IFileManager
    {
        public async Task DownloadFile(string url, string localPath)
        {
            using HttpClient client = new();

            try
            {
                using HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                using Stream fileStream = File.Create(localPath);
                await (await response.Content.ReadAsStreamAsync()).CopyToAsync(fileStream);
                fileStream.Close();
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }
    }
}
