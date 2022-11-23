using EtherscanAssignment.Infrastructure.Persistence.Entities;

namespace EtherscanAssignment.Models
{
    public class TokenModel
    {
        public TokenModel()
        {
        }

        public TokenModel(Token token)
        {
            Id = token.Id;
            Symbol = token.Symbol;
            Name = token.Name;
            TotalSupply = token.TotalSupply;
            ContractAddress = token.ContractAddress;
            TotalHolders = token.TotalHolders;
            Price = token.Price;
        }

        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public long TotalSupply { get; set; }
        public string ContractAddress { get; set; }
        public int TotalHolders { get; set; }
        public decimal Price { get; set; }
        public int Rank { get; set; }
        public decimal TotalSupplyPercentage { get; set; }
    }
}