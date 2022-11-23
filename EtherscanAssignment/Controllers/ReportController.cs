using EtherscanAssignment.Infrastructure.Persistence;
using EtherscanAssignment.Models;
using System.Linq;
using System.Web.Http;

namespace EtherscanAssignment.Controllers
{
    public class ReportController : ApiController
    {
        [HttpGet]
        [ActionName("total-supply-statistic")]
        public TotalSupplyStatisticModel GetTotalSupplyStatisticData()
        {
            using (var db = new ApplicationDbContext())
            {
                var tokens = db.Tokens.ToList();
                var response = new TotalSupplyStatisticModel
                {
                    Labels = tokens.Select(x => x.Name).ToArray(),
                    Data = tokens.Select(x => x.TotalSupply).ToArray(),
                };
                return response;
            }
        }
    }
}