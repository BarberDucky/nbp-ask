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
        private static MongoClient client = null;

        public static MongoClient Client
        {
            get
            {
                if (client == null)
                    return Connect();
                return client;
            }
        }

        public static MongoClient Connect()
        {
            try
            {
                client = new MongoClient(new MongoUrl("mongodb://localhost/?safe=true"));
                return client;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
