using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadData.RevenueFile
{
    public class Products
    {
        private string _ID, _ProductName, _CategoryName;
        private int _CategoryID;




        public string ID{ get => _ID; set { _ID = value; } }
        public string ProductName { get => _ProductName; set { _ProductName = value; } }
        public int CategoryID { get => _CategoryID; set { _CategoryID = value; } }
        public string CategoryName { get => _CategoryName; set { _CategoryName = value; } }

        public Products()
        {

        }

        public void GetMyData(string id)
        {
           foreach(DataRow row in RevenueMySQL.Products.Rows)
            {
                if (row["ProductID"].ToString() == id)
                {
                    this.ID = id;

                    this.CategoryID =Convert.ToInt32( row["CategoryID"].ToString());
                   foreach( DataRow r in RevenueMySQL.Categories.Rows)
                    {
                        if(Convert.ToInt32(r["CategoryID"]) == _CategoryID)
                        {
                            this.CategoryName = r["CategoryName"].ToString();
                            break;
                        }

                    }
                    this.ProductName = row["ProductName"].ToString();
                    break;
                }
            }
            test();

        }

       public  void UploadMyData(string IDRevenue)
        {
            DocumentReference Doc = DatabaseFirestore.DB.Collection("Revenue").Document(IDRevenue).Collection("Product").Document(ID);
            Dictionary<string, object> keys = new Dictionary<string, object>()
            {
                {"ProductName",ProductName },
                {"CategoryName",CategoryName },
                 {"CategoryID",CategoryID }
            };
           

            Doc.SetAsync(keys);
           
        }

        public void test()
        {
            Console.WriteLine("Product  num :" + ID);
            Console.WriteLine("Products name :" + ProductName);
            Console.WriteLine("Categoryname :" + CategoryName);
            Console.WriteLine("categoryID :" + CategoryID);
        }
    }
}
