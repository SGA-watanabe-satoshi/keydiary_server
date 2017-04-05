using System;

namespace keydiary.Models
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
}
