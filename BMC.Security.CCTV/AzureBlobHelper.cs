using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System.IO;

namespace BMC.Security.CCTV
{
    public class AzureBlobHelper
    {
        public CloudBlobContainer container { get; set; }
        public AzureBlobHelper()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                APPCONTANTS.BlobConnString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            container = blobClient.GetContainerReference("cctv");

            // Create the container if it doesn't already exist.
            Task<bool> a = container.CreateIfNotExistsAsync();

            a.Wait();
        }

        public async Task<(bool result,string url)> UploadFile(string imagePath,string cctvname)
        {
            try
            {
                var blobName = $"{cctvname}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.jpg";
                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
                var datas = File.ReadAllBytes(imagePath);
                
                using (MemoryStream ms = new MemoryStream(datas))
                {
                    // Create or overwrite the "myblob" blob with contents from a local file.
                    await blockBlob.UploadFromStreamAsync(ms);
                }
                var urls = $"https://bmspace.blob.core.windows.net/cctv/{blobName}";
                return (true,urls);
            }
            catch
            {
                return (false,"");
            }
        }
    }
}
