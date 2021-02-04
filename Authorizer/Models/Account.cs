using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Authorizer.Models
{
    [JsonObject("account")]
    public class Account : Entity
    {

        public Account(bool activeCard, int startAmount)
        {
            ActiveCard = activeCard;
            StartAmount = startAmount;
        }

        public Account() { }

        [JsonProperty("active-card")]
        public bool ActiveCard { get; set; }

        [JsonIgnore]
        private int StartAmount { get; set; }

        [JsonIgnore]
        public IList<Transaction> Transactions { get; set; } = new List<Transaction>();

        [JsonProperty("available-limit")]
        public int Amount => StartAmount - Transactions.Sum(q => q.Amount);

        [JsonIgnore]
        public bool IsInitialized => ActiveCard && StartAmount > 0;

        [JsonIgnore]
        public override bool Valid => IsInitialized && base.Valid;

        private IEnumerable<Transaction> GetLastTwoMinutesTransactions(Transaction transaction)
        {
            var dateTimeCurrentTransaction = transaction.Time;
            var dateTimeTwoMinutesAgo = dateTimeCurrentTransaction.AddMinutes(-2);

            // Set time as HH:MM:00.000
            dateTimeTwoMinutesAgo = dateTimeTwoMinutesAgo.AddSeconds(60 - dateTimeTwoMinutesAgo.Second);

            // Set time as HH:MM:59.999
            dateTimeCurrentTransaction = dateTimeCurrentTransaction.AddSeconds(59 - dateTimeCurrentTransaction.Second);
            dateTimeCurrentTransaction = dateTimeCurrentTransaction.AddMilliseconds(999 - dateTimeCurrentTransaction.Millisecond);

            // If date = 12, get 11:59:00.000000 and 12:00
            return Transactions
                    .Where(q => q.Time.CompareTo(dateTimeTwoMinutesAgo) >= 0 && q.Time.CompareTo(dateTimeCurrentTransaction) <= 0);
        }

        public void AddTransaction(Transaction transaction)
        {
            FailIf(() => !ActiveCard, "card-not-active");
            FailIf(() => Amount - transaction.Amount < 0, "insufficient-limit");

            var lastTwoMinutesTransactions = GetLastTwoMinutesTransactions(transaction);
            FailIf(() => lastTwoMinutesTransactions.Count() >= 3, "high-frequency-small-interval");
            FailIf(() => lastTwoMinutesTransactions.Any(q => q.Equals(transaction)), "doubled-transaction");

            if (Valid)
                Transactions.Add(transaction);
        }
    }
}