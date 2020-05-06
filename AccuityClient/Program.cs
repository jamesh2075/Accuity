using NetMQ;
using NetMQ.Sockets;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;

using CsvHelper;

namespace AccuityClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var reader = new StreamReader("People.csv"))                
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                using (var client = new RequestSocket())
                {
                    client.Connect("tcp://localhost:5555");

                    // I cannot get CsvHelper to skip the headers so I am forging a workaround.
                    csvReader.Configuration.HasHeaderRecord = false;
                    bool isFirstRecord = true;

                    var people = csvReader.GetRecords<Person>();
                    foreach (var person in people)
                    {
                        if (isFirstRecord)
                        {
                            isFirstRecord = false;
                            continue;
                        }

                        using (var stream = new MemoryStream())
                        using (var writer = new StreamWriter(stream))
                        using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csvWriter.NextRecord();
                            csvWriter.WriteRecord<Person>(person);
                            csvWriter.NextRecord();
                            writer.Flush();
                            stream.Seek(0, SeekOrigin.Begin);

                            using (StreamReader tmpReader = new StreamReader(stream))
                            {
                                string data = tmpReader.ReadToEnd();

                                Debug.WriteLine($"Sending {data}");

                                client.SendFrame(data);

                                var message = client.ReceiveFrameString();
                                Debug.WriteLine($"{0}");
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Console.WriteLine("An unexpected error occurred.");
            }

        }
    }
}
