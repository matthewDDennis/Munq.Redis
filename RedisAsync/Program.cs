using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Munq.Redis;
using System.Diagnostics;

namespace RedisAsync
{
    class Program
    {
        private const int NumIterations = 10000;
        static void Main(string[] args)
        {
            DoIt();
            Console.ReadLine();
        }
        private static async Task DoIt()
        {
            using (var client = new RedisClient())
            {
                string data = new string('A', 10000);
                List<object> result = new List<object>(NumIterations);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < NumIterations; i++)
                {
                    await client.SendSet("String" + i, data);
                    object obj = await client.ReadResponse();
                    result.Add(obj);
                }
                stopwatch.Stop();
                Console.WriteLine("{0} Sets of 10,000 chars took {1}.", NumIterations, stopwatch.Elapsed);
                stopwatch.Start();
                for (int i = 0; i < NumIterations; i++)
                {
                    await client.SendGet("String" + i);
                    object obj = await client.ReadResponse();
                }
                stopwatch.Stop();
                Console.WriteLine("{0} Gets of 10,000 chars took {1}.", NumIterations, stopwatch.Elapsed);

                await client.SendCommand("DBSIZE");
                WriteResult(await client.ReadResponse());

                //Console.WriteLine("--- Sending Client List:");
                //await client.SendCommand("Client", "List");

                //Console.WriteLine("--- Time:");
                //await client.SendCommand("Time");

                //Console.WriteLine("--- Keys *");
                //await client.SendKeys("*");

                //Console.WriteLine("--- Del, Exists, Append, Exists");
                //await client.SendDelete("myKey");
                //await client.SendExists("myKey");
                //await client.SendAppend("myKey", "Some data");
                //await client.SendExists("myKey");

                //await client.SendSet("aKey", 1000);
                //await client.SendGet("aKey");
                //await client.SendIncr("aKey");
                //await client.SendIncrBy("aKey", 42);
                //await client.SendDecr("aKey");
                //await client.SendDecrBy("aKey", 42);
                //await client.SendSet("aKey", "ABCDEFG");
                //await client.SendDelete("aKey");

                //await client.SendSetBit("aKey", 1, true);
                //await client.SendSetBit("aKey", 1, false);


                //await client.SendSet("aKey", 123.456d);
                //await client.SendGet("aKey");
                //await client.SendIncrByFloat("aKey", 1.23);
                //await client.SendGet("aKey");


                //using (var reader = await client.GetReader())
                //{
                //    object response;
                //    while ((response = reader.Read()) != null)
                //        WriteResult(response);
                //}
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
