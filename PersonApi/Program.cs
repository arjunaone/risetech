using Confluent.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PersonApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Kafka();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            CreateHostBuilder(args).Build().Run();
        }

        public static async Task Kafka()
        {
            ConsumerConfig cc = new ConsumerConfig { BootstrapServers = "localhost:9092", GroupId = "Group1" };
            ProducerConfig pc = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using (IConsumer<Null, int> consumer = new ConsumerBuilder<Null, int>(cc).Build())
            {
                consumer.Subscribe("RequestTopic");
                using (IProducer<Null, string> producer = new ProducerBuilder<Null, string>(pc).Build())
                {
                    await Task.Run(() =>
                    {
                        while (true)
                        {
                            int reportId = consumer.Consume().Message.Value;
                            if (reportId > 0)
                            {
                                using (PersonApiDbContext db = new PersonApiDbContext())
                                {
                                    List<Contact> cl = db.Contacts.ToList();
                                    ContactDto dto = new ContactDto() { ReportId = reportId, contactList = cl };
                                    string jsonString = JsonSerializer.Serialize(dto);
                                    producer.ProduceAsync("ContactTopic", new Message<Null, string> { Value = jsonString });
                                }
                            }
                        }
                    });
                }

            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
