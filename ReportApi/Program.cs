using Confluent.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReportApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReportApi
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

            using (IConsumer<Null, string> consumer = new ConsumerBuilder<Null, string>(cc).Build())
            {
                consumer.Subscribe("ContactTopic");
                await Task.Run(() =>
                {
                    while (true)
                    {
                        string jsonString = consumer.Consume().Message.Value;
                        if (jsonString != null)
                        {
                            using (ReportApiDbContext db = new ReportApiDbContext())
                            {
                                ContactDto dto = JsonSerializer.Deserialize<ContactDto>(jsonString);
                                List<Contact> cl = dto.contactList;
                                int reportId = dto.ReportId;
                                var personCountByLocation = (from c in cl 
                                                           where c.Type == "location" 
                                                           group c.UserId by c.Content into g
                                                           select new { Location = g.Key, PersonCount = g.Count() }
                                                          ).ToList();

                                var telCountByLocation = cl.Where(x => x.Type != "email")
                                                         .Join(cl,
                                                         cl1 => cl1.UserId,
                                                         cl2 => cl2.UserId,
                                                         (cl1, cl2) => new { t1Type = cl1.Type, t1Content = cl1.Content, t2Type = cl2.Type, t2Content = cl2.Content }
                                                        ).Where(x => x.t1Type == "location" && x.t2Type == "tel")
                                                         .GroupBy(x => x.t1Content)
                                                         .Select(x => new { Location = x.Key, TelCount = x.Count() }).ToList();
                                Report report = db.Reports.Find(reportId);
                                for(int i = 0; i < personCountByLocation.Count(); i++)
                                {
                                    report.Content += $"{personCountByLocation[i].Location}: Person Count = {personCountByLocation[i].PersonCount} - ";
                                }
                                for (int i = 0; i < telCountByLocation.Count(); i++)
                                {
                                    report.Content += $"{telCountByLocation[i].Location}: Tel Count = {telCountByLocation[i].TelCount} - ";
                                }
                                report.Status = "Completed";
                                db.Reports.Update(report);
                                db.SaveChanges();
                            }
                        }
                    }
                });
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
