using System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UploadData.RevenueFile
{
   static public class RevenueMySQL
    {
        public static DataTable Revenues;
        public static DataTable Products;
        public static DataTable Categories;
        static public void getRevenue()
        {
            string Query = @"select cast(month(OrderDate) as char(2))  as OrderMonth,cast(year(OrderDate) as char(4)) as orderYear, CustomerID,ProductID ,
sum(UnitPrice*Quantity-(Discount*UnitPrice*Quantity)) as ordersRevenue,
sum(case when ShippedDate is not null then UnitPrice*Quantity-(Discount*UnitPrice*Quantity) end) as shippedrevenue
from Orders o,`Order Details` d where o.OrderID=d.OrderID
 group by OrderMonth,orderYear ,CustomerID,ProductID;";

            try
            {
                Revenues = DatabaseMySQL.Sql.GetTable(Query);
            }
            catch (Exception ee)
            {
                Console.WriteLine("Get Data MySQL:\nfrom revenue \n Error :" + ee.Message);

            }

        }
        static public void getProducts()
        {
            string Query = @"select* from products; ";
            try
            {
                Products = DatabaseMySQL.Sql.GetTable(Query);
            }
            catch (Exception ee)
            {
                Console.WriteLine("Get Data MySQL:\nfrom Produscts \nError: " + ee.Message);

            }
        }
        static public void getCategories()
        {
            string Query = @"select * from Categories;";
            try
            {
                Categories = DatabaseMySQL.Sql.GetTable(Query);
            }
            catch (Exception ee)
            {
                Console.WriteLine("Get Data MySQL:\nfrom Categories \nError: " + ee.Message);

            }
        }
        
        static private void RVN(DataRow row)
        {
            Revenue revenue = new Revenue(row);
        }
        static public void CreateObjectRevenue()
        {
            getRevenue();
            getCategories();
            getProducts();
            OrdersFile.OrdersMySQL.GetCustomer();
            foreach(DataRow row in Revenues.Rows)
            {
                Thread t = new Thread(delegate () { RVN(row); });
                t.Start();
                t.Join();
              
             
            }
        }
    }
}
