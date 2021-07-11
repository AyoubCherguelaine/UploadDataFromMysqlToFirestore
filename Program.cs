using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UploadData
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(delegate ()
            {
                DatabaseFirestore.Connect();
            });
            thread.Start();
            thread.Join();
            Console.WriteLine("download Data From  MySQL");

            OrdersFile.OrdersMySQL.CreateObjectOrders();
            RevenueFile.RevenueMySQL.CreateObjectRevenue();
            
            Console.ReadKey();

        }
    }
}
