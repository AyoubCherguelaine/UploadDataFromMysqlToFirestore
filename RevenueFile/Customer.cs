using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadData.RevenueFile
{
   public class Customer
    {
        private string _Name, _City, _Country;
        private string _ID;
        [FirestoreDocumentId]
        public string ID { get => _ID; set { _ID = value; } }
        [FirestoreProperty]
        public string Name
        {
            get => _Name; set { _Name = value; }
        }
        [FirestoreProperty]
        public string City { get => _City; set { _City = value; } }
        [FirestoreProperty]
        public string Country { get => _Country; set { _Country = value; } }

        public void Test()
        {
            Console.WriteLine($"CustomerID{ID}\n  name {Name}\n  City{City}\n  Country{Country}");
        }
        public bool Equals(Customer o)
        {
            return this.Name == o.Name && Country == o.Country && this.City == o.City;
        }

        public void getMyData(string id)
        {
            foreach (DataRow row in UploadData.OrdersFile.OrdersMySQL.Customers.Rows)
            {
                if (row["CustomerID"].ToString() == id)
                {
                    this.ID = id;
                    
                    this.Name = row["CompanyName"].ToString();
                    this.City = row["City"].ToString();
                    this.Country = row["Country"].ToString();
                    break;
                }
            }
            Test();
        }

        public void UploadMyData(string IDRevenue)
        {
            DocumentReference Doc = DatabaseFirestore.DB.Collection("Revenue").Document(IDRevenue).Collection("Customer").Document(ID);
            Dictionary<string, string> keys = new Dictionary<string, string>()
            {
                {"Name",Name },
                {"City",City },
                {"Country",Country }
            };

            Doc.SetAsync(keys);
        }
    }
}
