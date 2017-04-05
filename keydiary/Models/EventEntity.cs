using System;
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types

namespace keydiary.Models
{
    public class EventEntity : TableEntity
    {
        public EventEntity(Data value)
        {
            this.PartitionKey = value.UserID;
            this.RowKey = Guid.NewGuid().ToString();
            this.LanguageID = value.LanguageID;
            this.WordCount = value.WordCount;
            this.CharCount = value.CharCount;
            this.TimeStamp = value.TimeStamp;
            this.FilenameHash = value.FilenameHash;
        }
        public EventEntity() { }

        public string LanguageID { get; set; }
        public string UserID { get; set; }
        public int WordCount { get; set; }
        public int CharCount { get; set; }
        public DateTime TimeStamp { get; set; }
        public string FilenameHash { get; set; }
    }
}
