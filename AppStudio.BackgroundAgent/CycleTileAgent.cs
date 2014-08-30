using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.Phone.Net.NetworkInformation;

using AppStudio.Data;

namespace AppStudio.BackgroundAgent
{
    public class CycleTileAgent
    {
        const string SHELLCONTENT_PATH = "/shared/shellcontent/";

        static public async Task<IEnumerable<Uri>> GetTileImages()
        {
            try
            {
                // Get the list of records from selected DataSource
                var records = (await LoadData(new TopNewsDataSource(), "TopNewsDataSource")) as IEnumerable<BindableSchemaBase>;

                if (records != null)
                {
                    using (var userStorage = new UserStorage())
                    {
                        var newImages = (from r in records
                                         where IsValidImageUrl(r.DefaultImageUrl)
                                         select new
                                         {
                                             ImageUrl = r.DefaultImageUrl,
                                             FileName = SHELLCONTENT_PATH + GetImageName(r.DefaultImageUrl)
                                         })
                                         .Where(r => r.FileName != null)
                                         .Take(8);

                        if (newImages.Count() > 0)
                        {
                            userStorage.EnsureDirectory(SHELLCONTENT_PATH);

                            // Download new images
                            foreach (var image in newImages)
                            {
                                if (!userStorage.FileExists(image.FileName))
                                {
                                    await DownloadImage(image.FileName, image.ImageUrl);
                                }
                            }

                            // Remove old stored images
                            foreach (var imageName in userStorage.GetFiles(SHELLCONTENT_PATH))
                            {
                                string fileName = SHELLCONTENT_PATH + imageName;
                                // If newImages doesn't contains this imageName, remove it.
                                if (!newImages.Any(r => r.FileName.EqualNoCase(fileName)))
                                {
                                    userStorage.DeleteFile(fileName);
                                }
                            }

                            IEnumerable<Uri> imageUrls = (from r in newImages select new Uri("isostore:" + r.FileName, UriKind.Absolute));
                            return imageUrls;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogs.WriteError("CycleTileAgent.GetTileImages", ex);
            }
            return null;
        }

        static private bool IsValidImageUrl(string url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                Uri uri;
                return Uri.TryCreate(url, UriKind.Absolute, out uri);
            }
            return false;
        }

        static private async Task<IEnumerable<T>> LoadData<T>(IDataSource<T> dataSource, string cacheKey) where T : BindableSchemaBase
        {
            // Read items from Cache
            var records = AppCache.GetItems<T>(cacheKey);

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                // Read items from DataSource
                var dsRecords = await dataSource.LoadData();
                if (dsRecords != null)
                {
                    AppCache.AddItems(cacheKey, dsRecords);
                    records = dsRecords;
                }
            }

            return records;
        }

        static private async Task DownloadImage(string fileName, string url)
        {
            try
            {
                var uri = new Uri(url);
                var request = WebRequest.CreateHttp(url);
                using (var response = await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null))
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var userStorage = new UserStorage())
                        {
                            using (var writer = userStorage.OpenFile(fileName, System.IO.FileMode.Create))
                            {
                                await stream.CopyToAsync(writer);
                                AppLogs.WriteInfo("DownloadImage", url);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogs.WriteError("CycleTileAgent.DownloadImage", ex);
            }
        }

        static private string GetImageName(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                string path = uri.AbsolutePath;
                string name = Path.GetFileName(path);
                return name;
            }
            return null;
        }
    }
}
