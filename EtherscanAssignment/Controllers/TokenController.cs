using EtherscanAssignment.Helpers;
using EtherscanAssignment.Infrastructure.Persistence;
using EtherscanAssignment.Infrastructure.Persistence.Entities;
using EtherscanAssignment.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace EtherscanAssignment.Controllers
{
    public class TokenController : ApiController
    {
        //readonly TokenModel[] DefaultTokens = new TokenModel[]
        //{
        //    new TokenModel { Id = 1, Name = "Vechain", Symbol = "VEN", ContractAddress = "0xd850942ef8811f2a866692a623011bde52a462c1", Price = 0, TotalSupply = 35987133, TotalHolders = 65 },
        //    new TokenModel { Id = 2, Name = "Zilliqa", Symbol = "ZIR", ContractAddress = "0x05f4a42e251f2d52b8ed15e9fedaacfcef1fad27", Price = 0, TotalSupply = 53272942, TotalHolders = 54 },
        //    new TokenModel { Id = 3, Name = "Maker", Symbol = "MKR", ContractAddress = "0x9f8f72aa9304c8b593d555f12ef6589cc3a579a2", Price = 0, TotalSupply = 45987133, TotalHolders = 567 },
        //    new TokenModel { Id = 4, Name = "Binance", Symbol = "BNB", ContractAddress = "0xB8c77482e45F1F44dE1745F52C74426C631bDD52", Price = 0, TotalSupply = 16579517, TotalHolders = 4234234 },
        //};

        // GET api/<controller>
        public PagedResult<TokenModel> Get(int pageNumber = 1, int pageSize = 10)
        {
            using (var db = new ApplicationDbContext())
            {
                PagedResult<Token> dbResult = db.Tokens
                    .OrderByDescending(x => x.TotalSupply)
                    .GetPaged(pageNumber, pageSize);

                List<TokenModel> tokens = dbResult.Results
                    .Select(x => new TokenModel(x))
                    .ToList();

                long sumTotalSupply = db.Tokens.Sum(x => x.TotalSupply);
                foreach (var token in tokens)
                {
                    token.TotalSupplyPercentage = (decimal)token.TotalSupply / sumTotalSupply;
                }
                tokens = tokens
                    .OrderByDescending(x => x.TotalSupplyPercentage)
                    .ToList();

                var result = new PagedResult<TokenModel>
                {
                    CurrentPage = dbResult.CurrentPage,
                    PageCount = dbResult.PageCount,
                    PageSize = dbResult.PageSize,
                    RowCount = dbResult.RowCount,
                    Results = tokens
                };

                return result;
            }
        }

        // GET api/<controller>/5
        public TokenModel Get(string id)
        {
            string symbol = id;
            TokenModel response = null;
            using (var db = new ApplicationDbContext())
            {
                Token foundToken = db.Tokens.FirstOrDefault(x => x.Symbol == symbol);
                if (foundToken == null)
                {
                    //throw new HttpResponseException(HttpStatusCode.NotFound);
                    return response;
                }
                response = new TokenModel(foundToken);
            }
            return response;
        }

        // POST api/<controller>
        public async Task<TokenModel> Post([FromBody] TokenModel token)
        {
            // create a token
            using (var db = new ApplicationDbContext())
            {
                Token foundToken = db.Tokens
                    .FirstOrDefault(x => x.Symbol == token.Symbol
                        || x.ContractAddress == token.ContractAddress
                        || x.Name == token.Name
                    );
                if (foundToken != null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                var newToken = new Token
                {
                    Name = token.Name,
                    Symbol = token.Symbol,
                    ContractAddress = token.ContractAddress,
                    Price = token.Price,
                    TotalSupply = token.TotalSupply,
                    TotalHolders = token.TotalHolders,
                };
                db.Tokens.Add(newToken);
                await db.SaveChangesAsync();
                var response = new TokenModel(newToken);
                return response;
            }
        }

        // PUT api/<controller>/5
        public async Task<TokenModel> Put(int id, [FromBody] TokenModel token)
        {
            using (var db = new ApplicationDbContext())
            {
                Token foundToken = db.Tokens.FirstOrDefault(x => x.Id == id);
                if (foundToken == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                bool hasChanged = false;
                if (foundToken.Name != token.Name)
                {
                    foundToken.Name = token.Name;
                    hasChanged = true;
                }

                if (foundToken.Symbol != token.Symbol)
                {
                    foundToken.Symbol = token.Symbol;
                    hasChanged = true;
                }

                if (foundToken.ContractAddress != token.ContractAddress)
                {
                    foundToken.ContractAddress = token.ContractAddress;
                    hasChanged = true;
                }

                if (foundToken.TotalSupply != token.TotalSupply)
                {
                    foundToken.TotalSupply = token.TotalSupply;
                    hasChanged = true;
                }

                if (foundToken.TotalHolders != token.TotalHolders)
                {
                    foundToken.TotalHolders = token.TotalHolders;
                    hasChanged = true;
                }

                if (hasChanged)
                {
                    await db.SaveChangesAsync();
                }
                var response = new TokenModel(foundToken);
                return response;
            }
        }

        // DELETE api/<controller>/5
        public async Task Delete(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                Token foundToken = db.Tokens.FirstOrDefault(x => x.Id == id);
                if (foundToken == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                db.Tokens.Remove(foundToken);
                await db.SaveChangesAsync();
            }
        }
    }
}