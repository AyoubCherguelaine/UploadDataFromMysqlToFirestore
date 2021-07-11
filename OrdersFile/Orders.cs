 using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace UploadData.OrdersFile
{
    [FirestoreData]
  public class Orders
    {
        private int _NB_No_Ship, _NB_Ship;
        private string _ID;
        [FirestoreDocumentId]
        public string ID { get => _ID; set { _ID = value; } }
        [FirestoreProperty]
        public int NB_NO_Ship { get => _NB_No_Ship; set { _NB_No_Ship = value; } }
        [FirestoreProperty]
        public int NB_Ship { get => _NB_Ship; set { _NB_Ship = value; } }

        public Customer customer = new Customer();
        public Employee employee = new Employee();
        public Time time = new Time();
        private List<bool> Check=new List<bool>();
        public Orders(DataRow Row)
        {
            Mydata(Row);
        }
     
       public void Mydata(DataRow row)
       {
            ID = row["EmployeeID"].ToString() + "-"+ row["CustomerID"]+"-"+ row["orderMonth"]+"-"+row["orderYear"];
            _NB_Ship =Convert.ToInt32( row["Shipped_orders"]);
          
            _NB_No_Ship = Convert.ToInt32(row["NotShipped_orders"]);
      
            time.getMyData(row);
            employee.getMyData(row["EmployeeID"].ToString());
            customer.getMyData(row["CustomerID"].ToString());
            SaveData();

        }
        private void AddOrderInFirestore()
        {
            DocumentReference Doc = DatabaseFirestore.DB.Collection("Orders").Document(ID);
            Dictionary<string, int> keys = new Dictionary<string, int>()
            {
                {"NB_Ship",NB_Ship },
                { "NB_NO_Ship",NB_NO_Ship}
            };

            Doc.SetAsync(keys);
        }
        private void SaveData()
        {
            Thread AddOrder = new Thread(delegate() { AddOrderInFirestore(); });
            AddOrder.Start();
            AddOrder.Join();
            Thread AddEmployee = new Thread(delegate () { employee.UploadMyData(this.ID); });
            AddEmployee.Start();
            AddEmployee.Join();
            Thread AddCustomer = new Thread(delegate () { customer.UploadMyData(this.ID); });
            AddCustomer.Start();
            AddCustomer.Join();

            Thread AddTime = new Thread(delegate () { time.UploadMyData(this.ID); });
         AddTime.Start();
           AddTime.Join();
            Console.WriteLine("UploadFirst");

        }
        public void test()
        {
            Console.WriteLine("Order n:" + ID);
            employee.test();
            customer.Test();
            time.test();
            Console.WriteLine("\n\n");
        }


}
}
