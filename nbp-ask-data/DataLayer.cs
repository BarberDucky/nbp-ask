using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_ask_data
{
    public class DataLayer
    {
        private static IMongoDatabase database = null;

        public static IMongoDatabase Database
        {
            get
            {
                if (database == null)
                    return Connect();
                return database;
            }
        }

        public static IMongoDatabase Connect()
        {
            try
            {
                MongoClient client = new MongoClient(new MongoUrl("mongodb://localhost/?safe=true"));
                database = client.GetDatabase("ask-db");
                return database;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
