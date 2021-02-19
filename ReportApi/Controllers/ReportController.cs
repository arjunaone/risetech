using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportApiDbContext db = new ReportApiDbContext();
        private readonly ProducerConfig pc;

        public ReportController(ProducerConfig pc)
        {
            this.pc = pc;
        }

        [HttpGet("request")]
        public async Task<IActionResult> CreateReport()
        {
            Report report = new Report();
            db.Reports.Add(report);
            db.SaveChanges();

            using (IProducer<Null, int> producer = new ProducerBuilder<Null, int>(pc).Build())
            {
                await producer.ProduceAsync("RequestTopic", new Message<Null, int> { Value = report.Id });
            }
            return Ok("Report was requested with the " + report.Id + " Id.");
        }

        [HttpGet("list")]
        public IActionResult ListReports()
        {
            return Ok(db.Reports.ToList().Select(x => new { x.Id, x.DateCreated, x.Status }));
        }

        [HttpGet("detail/{reportId:int}")]
        public IActionResult Detail(int reportId)
        {
            return Ok(db.Reports.Find(reportId));
        }
    }
}
