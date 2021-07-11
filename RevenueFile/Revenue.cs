using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Google.Cloud.Firestore;
using System.Threading;

namespace UploadData.RevenueFile
{
    public class Revenue
    {
        private string _ID;
        private double _OrderRevenue, _ShippedRevenue;

        public string ID { get => _ID; set { _ID = value; } }
        public double OrderRevenue { get => _OrderRevenue; set { _OrderRevenue = value; } }
        public double ShippedRevenue { get => _ShippedRevenue; set { _ShippedRevenue = value; } }
        public Time time = new Time();
        public Products products = new Products();
        public Customer customer = new Customer();

        
        public Revenue(DataRow row)
        {
            getMyData(row);
        }
        public void  getMyData(DataRow row)
        {
            ID = row["ProductID"].ToString() + "-" + row["CustomerID"].ToString() + "-"+row["orderYear"].ToString() + "-" + row["OrderMonth"].ToString();
          
            OrderRevenue = Convert.ToDouble(row["ordersRevenue"]);
            try
            {
                ShippedRevenue = Convert.ToDouble(row["shippedrevenue"]);
            }
            catch
            {

            }
            time.getMyData(row);
            customer.getMyData(row["CustomerID"].ToString());
            products.GetMyData(row["ProductID"].ToString());

            test();
            Thread thread = new Thread(delegate () { SaveData(); });
            thread.Start();
            thread.Join();
            Console.WriteLine(" \n\n\n\nFinish upload !");

        }

        private void SaveData()
        {
            Thread t1 = new Thread(delegate () { AddRevenueInFirestore(); });
            t1.Start();
            t1.Join();
         
            Thread t3 = new Thread(delegate () { customer.UploadMyData(ID); });
            t3.Start();
            t3.Join();
            Thread t4 = new Thread(delegate () { products.UploadMyData(ID); });
            t4.Start();
            t4.Join();
            Thread t2 = new Thread(delegate () { time.UploadMyData(ID); });
            t2.Start();
            t2.Join();
        }

        private void AddRevenueInFirestore()
        {
            DocumentReference Doc = DatabaseFirestore.DB.Collection("Revenue").Document(ID);
            Dictionary<string, double> keys = new Dictionary<string, double>()
            {
                {"OrderRevenue",OrderRevenue },
                { "ShippedRevenue",ShippedRevenue}
            };

            Doc.SetAsync(keys);
        }

        public void test()
        {
            Console.WriteLine($"renenue ID{ID} \n OrderRevune :{ OrderRevenue} \nShippedRevenue{ShippedRevenue}");

        }

    }
}
