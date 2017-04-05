using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using keydiary.Models;

namespace keydiary.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public string Post([FromBody]List<Data> values)
        {
            try
            {
                // 構成ファイルから Azure Storage への接続文字列を取得
                string setting = CloudConfigurationManager.GetSetting("StorageConnectionString");
                CloudStorageAccount account = CloudStorageAccount.Parse(setting);

                // Create batch.
                TableBatchOperation batch = new TableBatchOperation();
                foreach (var value in values)
                {
                    // Add Insert operation to batch.
                    batch.Add(TableOperation.Insert(new EventEntity(value)));
                }

                // Create the table client.
                CloudTableClient client = account.CreateCloudTableClient();

                // Get the CloudTable object reference that represents the "event" table.
                CloudTable table = client.GetTableReference("event");
                table.CreateIfNotExists();

                // Execute batch operation.
                table.ExecuteBatch(batch);

                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
