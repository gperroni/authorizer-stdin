using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorizer.Models
{
    [JsonObject(Title = "transaction", Id = "transaction")]
    public class Transaction : IEquatable<Transaction>
    {
        public Transaction(int amount, string merchant, DateTime time)
        {
            Amount = amount;
            Merchant = merchant;
            Time = time;
        }


        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("merchant")]
        public string Merchant { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        public bool Equals(Transaction newTransaction)
        {
            return newTransaction.Amount.Equals(Amount)
                && newTransaction.Merchant.Equals(Merchant);
        }
    }
}
