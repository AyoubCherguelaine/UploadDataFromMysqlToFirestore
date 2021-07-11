using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadData.OrdersFile
{
   public class Time
    {
        private string _ID,_ID_DB;
        private bool done = false;
        public string ID { get => _ID; set { _ID = value; } }
        [FirestoreDocumentId]
        public string ID_DB { get => _ID_DB; set { _ID_DB = value; } }

        private int _month, _year;
        [FirestoreProperty]
        public int Month
        {
            get => _month;
            set { _month = value; if (!done) done = true; else createID(); }
        }
        [FirestoreProperty]
        public int Year
        {
            get => _year;
            set { _year = value; if (!done) done = true; else createID(); }
        }
        private void  createID()
        {
            ID = Year + "/" + Month;
        }

        public DateTime Date= new DateTime();
        
        public void test()
        {
            Console.WriteLine($"Month{Month},, year{Year}");
        }

        public bool Equals(Time T)
        {
            return Month == T.Month && Year == T.Year;
        }

       
        public void getMyData(DataRow row)
            {
                   
                    this.Month =Convert.ToInt32( row["orderMonth"]);
                    this.Year = Convert.ToInt32(row["orderYear"]);

                    this.ID_DB = row["orderYear"] + "-" + row["orderMonth"];


            }

        public void UploadMyData(string IDRevenue)
        {
            DocumentReference Doc = DatabaseFirestore.DB.Collection("Orders").Document(IDRevenue).Collection("Time").Document(ID_DB);
            Dictionary<string, int> keys = new Dictionary<string, int>()
            {
                {"Month",Month },
                {"Year",Year }
            };

            Doc.SetAsync(keys);
        }
    }
}
