using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BMC.Security.CCTV
{
    public class AzureTableHelper
    {
        CloudTable cloudTable;

        public AzureTableHelper(string TableName)
        {
            CloudStorageAccount cloudStorageAccount =
    CloudStorageAccount.Parse
    (APPCONTANTS.BlobConnString);

            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();
            Console.WriteLine(TableName);
            //string tableName = Console.ReadLine();
            cloudTable = tableClient.GetTableReference(TableName);
            CreateNewTable(cloudTable);

        }

        //internal object InsertData(DataAudiensi data)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<bool> InsertData(CCTVData data)
        {
            try
            {
                if (data != null)
                {
                    TableOperation tableOperation = TableOperation.Insert(data);
                    await cloudTable.ExecuteAsync(tableOperation);
                    Console.WriteLine("Record inserted");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
       
        public async void CreateNewTable(CloudTable table)
        {
            if (!await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Table {0} already exists", table.Name);
                return;
            }
            Console.WriteLine("Table {0} created", table.Name);
        }

        public async Task<List<CCTVData>> GetCCTVData()
        {
         
            CloudStorageAccount cloudStorageAccount =
      CloudStorageAccount.Parse
      (APPCONTANTS.BlobConnString);

        
            // Create the table query.
            var datas = new List<CCTVData>();

            TableQuery<CCTVData> query = new TableQuery<CCTVData>().Where(TableQuery.GenerateFilterCondition("CCTVName", QueryComparisons.NotEqual, ""));

            // Print the fields for each customer.
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<CCTVData> resultSegment = await cloudTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (CCTVData entity in resultSegment.Results)
                {
                    datas.Add(entity);
                }
            } while (token != null);

            return datas;
        }

    }
}