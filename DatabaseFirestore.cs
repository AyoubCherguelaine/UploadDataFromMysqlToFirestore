using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace UploadData
{
   public static class DatabaseFirestore
    {
        public static FirestoreDb DB;

        public static void Connect()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"olapnosql-firebase-adminsdk-z135x-55b7d93a98.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            DB = FirestoreDb.Create("olapnosql");

            Console.WriteLine("Connected!");
        }

    }
}
