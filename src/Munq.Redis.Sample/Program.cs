﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Munq.Redis;
using Munq.Redis.Requests;
using Munq.Redis.Responses;

namespace RedisAsync
{
    class Program
    {
        const int NumIterations = 10000;
        const int NumChars = 10000;
        static void Main(string[] args)
        {
            DoIt().Wait();
            Console.ReadLine();
        }
        static async Task DoIt()
        {
            var config = new RedisClientConfig();
            using (var client = RedisClientFactory.Create(config))
            {
                var rnd = new Random();
                try
                {
                    Console.Write("Sending Ping - ");
                    await client.SendPingAsync();
                    if (await client.ExpectConstStringAsync("PONG"))
                        Console.WriteLine("Pong response received.");
                    else
                        Console.WriteLine("Unexpected string returned.");

                    Console.Write("Selecting Database 4 - ");
                    await client.SendSelectAsync(4);
                    Console.WriteLine((await client.ExpectOkAsync()) ? "Success" : "Failed");

                    var sampleData = new byte[NumChars];
                    for (int i = 0; i < NumChars; i++)
                        sampleData[i] = (byte)(rnd.Next(64) + 32);
                    var data = Encoding.UTF8.GetString(sampleData);

                    var results = new List<object>(NumIterations);
                    var stopwatch = new Stopwatch();
                    Console.Write("Writing {0:N0} RedisStrings of {1:N0} chars - ", NumChars, NumIterations);
                    stopwatch.Start();
                    int errorCount = 0;
                    for (int i = 0; i < NumIterations; i++)
                    {
                        await client.SendSetAsync("String" + i, data);
                        if (!await client.ExpectOkAsync())
                            errorCount++;
                    }
                    stopwatch.Stop();
                    Console.WriteLine("with {0} errors", errorCount);
                    Console.WriteLine("{0:N0} Sets of {1:N0} chars took {2}.", NumIterations, NumChars, stopwatch.Elapsed);
                    results.Clear();
                    Console.Write("Reading {0:N0} RedisStrings of {1:N0} chars - ", NumChars, NumIterations);

                    stopwatch.Start();
                    for (int i = 0; i < NumIterations; i++)
                    {
                        await client.SendGetAsync("String" + i);
                        string bulkString = await client.ExpectBulkStringAsync();

                        string resultString = "OK";
                        if (bulkString == null || bulkString.Length != NumChars)
                            resultString = bulkString;
                        // Console.Write("\r{0} - {1}", i + 1, resultString);
                        results.Add(resultString);
                    }
                    stopwatch.Stop();
                    Console.WriteLine("with {0} errors", results.Count(rs => (string)rs != "OK"));
                    Console.WriteLine("{0:N0} Gets of {1:N0} chars took {2}.", NumIterations, NumChars, stopwatch.Elapsed);

                    Console.WriteLine("Get Db Size");
                    await client.SendDbSizeAsync();
                    WriteResult(await client.ReadResponseAsync());

                    Console.WriteLine("Get Server Info");
                    await client.SendInfoAsync(InfoSections.All);
                    WriteResult(await client.ReadResponseAsync());

                    Console.WriteLine("Delete 3 keys");
                    await client.SendDeleteAsync("String0", "String1", "String2");
                    WriteResult(await client.ReadResponseAsync());

                    Console.WriteLine("Get Db Size");
                    await client.SendDbSizeAsync();
                    WriteResult(await client.ReadResponseAsync());

                    Console.WriteLine("Empty the database");
                    await client.SendFlushDbAsync();
                    WriteResult(await client.ReadResponseAsync());

                    Console.WriteLine("Get Db Size");
                    await client.SendDbSizeAsync();
                    WriteResult(await client.ReadResponseAsync());

                    Console.WriteLine("Quit");
                    await client.SendAsync("Quit");
                    WriteResult(await client.ReadResponseAsync());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("*** EXCEPTION: {0}\n*** Message: {1}\n*** Stack Trace: {2}",
                                      ex.GetType().Name, ex.Message, ex.StackTrace);
                }
            }
        }
        static void WriteResult(object result)
        {
            if (result is Array)
            {
                var objArray = (object[])result;
                Console.WriteLine("An Array of {0} objects:", objArray.Length);
                foreach (object o in objArray)
                    Console.WriteLine("({0}) {1}", o.GetType().Name, o);
            }
            else
                Console.WriteLine("({0}) {1}", result.GetType().Name, result);
        }
    }
}
