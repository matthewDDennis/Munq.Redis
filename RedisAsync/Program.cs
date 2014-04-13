using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Munq.Redis;

namespace RedisAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            DoIt();
            Console.ReadLine();
        }
        private static async Task DoIt()
        {
            using (var client = new RedisClient())
            {
                Console.WriteLine("--- Sending Client List:");
                await client.SendCommand("Client", "List");

                Console.WriteLine("--- Time:");
                await client.SendCommand("Time");

                Console.WriteLine("--- Keys *");
                await client.SendKeys("*");

                Console.WriteLine("--- Del, Exists, Append, Exists");
                await client.SendDelete("myKey");
                await client.SendExists("myKey");
                await client.SendAppend("myKey", "Some data");
                await client.SendExists("myKey");

                await client.SendSet("aKey", 1000);
                await client.SendGet("aKey");

                using (var reader = await client.GetReader())
                {
                    object response;
                    while ((response = reader.Read()) != null)
                        WriteResult(response);
                }
            }
        }
        private static void WriteResult(object result)
        {
            if (result is Array)
            {
                object[] objArray = (object[])result;
                Console.WriteLine("An Array of {0} objects:", objArray.Length);
                foreach (object o in objArray)
                    Console.WriteLine("({0}) {1}", o.GetType().Name, o);
            }
            else
                Console.WriteLine("({0}) {1}", result.GetType().Name, result);
        }
    }
}
