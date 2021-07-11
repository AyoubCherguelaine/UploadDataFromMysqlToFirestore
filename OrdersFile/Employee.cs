using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadData.OrdersFile
{
    public class Employee
    {
        private string _Firstname, _lastname, _City, _Country;
        private string _ID;
        [FirestoreDocumentId]
        public string ID { get => _ID; set { _ID = value; } }
        [FirestoreProperty]
        public string Firstname { get => _Firstname; set { _Firstname = value; } }
        [FirestoreProperty]
        public string Lastname { get => _lastname; set { _lastname = value; } }
        [FirestoreProperty]
        public string City { get => _City; set { _City = value; } }
        [FirestoreProperty]
        public string Country { get => _Country; set { _Country = value; } }


        public void test()
        {
            Console.WriteLine($"id {ID} ,,Firstname : {Firstname}\nLAstname{Lastname}\nCity{City}\nCountry{Country}");
        }

        public bool Equals(Employee E)
        {
            return Firstname == E.Firstname && Lastname == E.Lastname && City == E.City && Country == E.Country;
        }

        public void getMyData(string id)
        {
            foreach(DataRow row in OrdersMySQL.Employees.Rows)
            {
                if(row["EmployeeID"].ToString() == id)
                {
                    this.ID = id;
                    this.Firstname = row["FirstName"].ToString();
                    this.Lastname = row["LastName"].ToString();
                    this.City = row["City"].ToString();
                    this.Country = row["Country"].ToString();
                    break;
                }
            }
                
        }

        public void UploadMyData(string IDORDERS)
        {
            DocumentReference Doc = DatabaseFirestore.DB.Collection("Orders").Document(IDORDERS).Collection("Employee").Document(ID);
            Dictionary<string, string> keys = new Dictionary<string, string>()
            {
                {"Firtname",Firstname },
                {"Lastname",Lastname },
                {"City",City },
                {"Country",Country }
            };

            Doc.SetAsync(keys);
        }
    }
}
