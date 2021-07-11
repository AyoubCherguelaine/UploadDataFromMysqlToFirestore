using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UploadData.OrdersFile
{
    static public class OrdersMySQL
    {

        static public DataTable Orders;
        static public DataTable Employees;
        static public DataTable Customers;
       

        static public void GetEmployee() {
            string Query= "select * from employees;"; // Query from wafa 

            try
            {
                Employees = DatabaseMySQL.Sql.GetTable(Query);
            }
            catch(Exception ee)
            {
                Console.WriteLine("Get Data MySQL:\nfrom Employee\n " + ee.Message);

            }
        
        }

        static public void GetCustomer()
        {
            string Query = "select * from customers;"; // Query from wafa 

            try
            {
                Customers = DatabaseMySQL.Sql.GetTable(Query);
            }
            catch (Exception ee)
            {
                Console.WriteLine("Get Data MySQL:\nfrom Customer\n " + ee.Message);

            }

        }
     
        static public void GetOrders()
        {
            string Query = @"select CustomerID, EmployeeID, (MONTH(OrderDate) ) orderMonth,(year(OrderDate)) as orderYear, count(case when shippeddate is not null then OrderID end) as NotShipped_orders
            ,count(case when shippeddate is null then OrderID end) as Shipped_orders  from Orders
            group by CustomerID,EmployeeID,cast(MONTH(OrderDate) as char(2)) + '/' + cast(year(OrderDate) as char(4)); "; // Query from wafa 

            try
            {
                Orders = DatabaseMySQL.Sql.GetTable(Query);
            }
            catch (Exception ee)
            {
                Console.WriteLine("Get Data MySQL:\nfrom Orders \nError: " + ee.Message);

            }

        }
        
        static private void ord(DataRow r)
        {
            Orders o = new Orders(r);
        } 
        
        static public void CreateObjectOrders()
        {
            GetEmployee();
            GetCustomer();
            GetOrders();
            
            foreach(DataRow r in Orders.Rows)
            {
                Thread thread = new Thread(delegate () { ord(r); });
                thread.Start();
                thread.Join();
                Console.WriteLine("Thread is down {ORD(R)}");

                // if you wanna to get the data 
                // remobe the break;
             
                //yes
            }
          

        }
    
    }
}
