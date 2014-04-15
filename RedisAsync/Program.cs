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
        private const int NumChars = 10000;
        static void Main(string[] args)
        {
            DoIt().Wait();
            Console.ReadLine();
        }
        private static async Task DoIt()
        {
            //using (var client = new RedisClient("cp-dev1.office.codeproject.com"))
            using (var client = new RedisClient())
            {
                try
                {
                    Console.Write("Selecting Database 4 - ");
                    await client.SendSelectCommandAsync(4);
                    Console.WriteLine(await client.ReadResponseAsync());

                    string data = new string('A', 10000);
                    List<object> results = new List<object>(NumIterations);
                    Stopwatch stopwatch = new Stopwatch();
                    Console.Write("Writing {0:N0} RedisStrings of {1:N0} chars - ", NumChars, NumIterations);
                    stopwatch.Start();
                    for (int i = 0; i < NumIterations; i++)
                    {
                        await client.SendSetCommandAsync("String" + i, data);
                        object obj = await client.ReadResponseAsync();
                        results.Add(obj);
                    }
                    stopwatch.Stop();
                    Console.WriteLine("with {0} errors", results.Count(rs => (string)rs != "OK"));
                    Console.WriteLine("{0:N0} Sets of {1:N0} chars took {2}.", NumIterations, NumChars, stopwatch.Elapsed);
                    results.Clear();
                    Console.WriteLine("Reading {0:N0} RedisStrings of {1:N0} chars - ", NumChars, NumIterations);

                    stopwatch.Start();
                    for (int i = 0; i < NumIterations; i++)
                    {
                        await client.SendGetCommandAsync("String" + i);
                        object obj = await client.ReadResponseAsync();

                        string resultString = "OK";
                        if (obj == null || !(obj is string) || ((string)obj).Length != NumChars)
                            resultString = obj.ToString();
                        Console.Write("\r{0} - {1}", i + 1, resultString);
                        results.Add(resultString);
                    }
                    stopwatch.Stop();
                    Console.WriteLine("with {0} errors", results.Count(rs => (string)rs != "OK"));
                    Console.WriteLine("{0:N0} Gets of {1:N0} chars took {2}.", NumIterations, NumChars, stopwatch.Elapsed);

                    await client.SendDbSizeCommandAsync();
                    WriteResult(await client.ReadResponseAsync());

                    await client.SendInfoCommandAsync(InfoSections.All);
                    WriteResult(await client.ReadResponseAsync());

                    await client.SendFlushDbCommandAsync();
                    WriteResult(await client.ReadResponseAsync());

                    await client.SendDbSizeCommandAsync();
                    WriteResult(await client.ReadResponseAsync());

                    await client.SendCommandAsync("Quit");
                    WriteResult(await client.ReadResponseAsync());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("*** EXCEPTION: {0}\n*** Message: {1}\n*** Stack Trace: {2}",
                                      ex.GetType().Name, ex.Message, ex.StackTrace);
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
