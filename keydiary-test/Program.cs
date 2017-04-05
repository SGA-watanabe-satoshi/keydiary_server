using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types


namespace azureTableStrage
{
    class Program
    {
        static CloudStorageAccount storageAccount;
        static CloudTableClient tableClient;

        static void Main(string[] args)
        {
            // 構成ファイルから Azure Storage への接続文字列を取得
            storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();

            switch (args[0])
            {
                case "create":
                    Console.WriteLine(createTable(args[1]));
                    break;
                case "delete":
                    Console.WriteLine(deleteTable(args[1]));
                    break;
                case "add":
                    Console.WriteLine(addEntity(args[1], args[2]));
                    break;
                case "remove":
                    Console.WriteLine(deleteEntry(args[1], args[2]));
                    break;
                case "show":
                    string[] entityPropArray = showEntity(args[1], args[2]);
                    Console.WriteLine("FirstName : {0}", entityPropArray[0]);
                    Console.WriteLine("LastName : {0}", entityPropArray[1]);
                    Console.WriteLine("EMail Address : {0}", entityPropArray[2]);
                    Console.WriteLine("PhoneNumber : {0}", entityPropArray[3]);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }

        //テーブルの作成
        static string createTable(string tableName)
        {
            string returnString = "[done] Created table.";
            try
            {
                // Retrieve a reference to the table.
                CloudTable table = tableClient.GetTableReference(tableName);
                // Create the table if it doesn't exist.
                table.CreateIfNotExists();
            }
            catch (Exception err)
            {
                returnString = err.Message;
            }
            return returnString;
        }

        //テーブルの削除
        static string deleteTable(string tableName)
        {
            string returnString = "[done] Deleted table.";
            try
            {
                // Create the CloudTable that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);

                // Delete the table it if exists.
                table.DeleteIfExists();
            }
            catch (Exception err)
            {
                returnString = err.Message;
            }
            return returnString;
        }

        //エンティティの追加
        static string addEntity(string tableName, string csvArg)
        {
            string returnString = "[done] Added entity. ";
            try
            {
                string[] argArray = csvArg.Split(',');
                string firstName = argArray[0];
                string lastName = argArray[1];
                string mailAddress = argArray[2];
                string phoneNumber = argArray[3];

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);

                // Create a new customer entity.
                CustomerEntity customer1 = new CustomerEntity(firstName, lastName);
                customer1.Email = mailAddress;
                customer1.PhoneNumber = phoneNumber;

                // Create the TableOperation object that inserts the customer entity.
                TableOperation insertOperation = TableOperation.Insert(customer1);

                // Execute the insert operation.
                table.Execute(insertOperation);
            }
            catch (Exception err)
            {
                returnString = err.Message;
            }
            return returnString;
        }

        static string deleteEntry(string tableName, string csvArg)
        {
            string returnString = "[done] Added entity. ";
            try
            {
                string[] argArray = csvArg.Split(',');
                string firstName = argArray[0];
                string lastName = argArray[1];

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);

                // Create a retrieve operation that takes a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(firstName, lastName);

                // Execute the retrieve operation.
                TableResult retrievedResult = table.Execute(retrieveOperation);
                // Assign the result to a CustomerEntity.
                CustomerEntity deleteEntity = (CustomerEntity)retrievedResult.Result;

                // Create the Delete TableOperation.
                if (deleteEntity != null)
                {
                    TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                    // Execute the operation.
                    table.Execute(deleteOperation);
                }
                else
                {
                    returnString = "Could not retrieve the entity.";
                }
            }
            catch (Exception Err)
            {
                returnString = Err.Message;
            }
            return returnString;
        }

        //単一のエンティティ情報の表示
        static string[] showEntity(string tableName, string csvArg)
        {

            string[] argArray = csvArg.Split(',');
            string firstName = argArray[0];
            string lastName = argArray[1];

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference(tableName);

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(firstName, lastName);

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                CustomerEntity entity = (CustomerEntity)retrievedResult.Result;
                return new string[] { firstName, lastName, entity.Email, entity.PhoneNumber };
            }
            else
            {
                return new string[] { "The phone number could not be retrieved." };
            }
        }
    }

    //エンティティの構造の定義
    public class CustomerEntity : TableEntity
    {
        public CustomerEntity(string lastName, string firstName)
        {
            this.PartitionKey = lastName;
            this.RowKey = firstName;
        }

        public CustomerEntity() { }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
