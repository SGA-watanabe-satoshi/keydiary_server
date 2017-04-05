using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types

namespace keydiary.Controllers
{
    public class Data
    {
        public string LanguageID { get; set; }
        public string UserID { get; set; }
        public int WordCount { get; set; }
        public int CharCount { get; set; }
        public DateTime TimeStamp { get; set; }
        public string FilenameHash { get; set; }
    }

    public class EventEntity : TableEntity
    {
        public EventEntity(Data value)
        {
            this.PartitionKey = value.UserID;
            this.RowKey = Guid.NewGuid().ToString();
            this.LanguageID = value.LanguageID;
            this.WordCount = value.WordCount;
            this.CharCount = value.CharCount;
            this.Timestamp = value.TimeStamp;
            this.FilenameHash = value.FilenameHash;
        }
        public string LanguageID { get; set; }
        public string UserID { get; set; }
        public int WordCount { get; set; }
        public int CharCount { get; set; }
        public DateTime TimeStamp { get; set; }
        public string FilenameHash { get; set; }
    }

    public class ValuesController : ApiController
    {
        static CloudStorageAccount storageAccount;
        static CloudTableClient tableClient;

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
            // 構成ファイルから Azure Storage への接続文字列を取得
            storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();

            try
            {
                foreach (Data value in values)
                {
                    addEntity(value);
                }
            } catch (Exception e)
            {
                return e.Message;
            }
            return "OK";
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        //エンティティの追加
        static void addEntity(Data value)
        {
            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("event");

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(new EventEntity(value));

            // Execute the insert operation.
            table.Execute(insertOperation);
        }
    }
}
