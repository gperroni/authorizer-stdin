using Authorizer.Properties;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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

        public Account(bool activeCard, int startAmount, string error)
        {
            ActiveCard = activeCard;
            StartAmount = startAmount;
            FailIf(() => true, error);
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

        private IEnumerable<Transaction> GetLastTwoMinutesTransactions(Transaction transaction)
        {
            // If time as 10:00, the method will get transactions between 09:59:00.000 and 10:00:59.999
            var dateTimeCurrentTransaction = transaction.Time;
            var dateTimeMinutesAgo = dateTimeCurrentTransaction.AddMinutes(-1);

            // Set time as HH:MM:00.000
            dateTimeMinutesAgo = dateTimeMinutesAgo.AddSeconds(-dateTimeMinutesAgo.Second);
            dateTimeMinutesAgo = dateTimeMinutesAgo.AddMilliseconds(-dateTimeMinutesAgo.Millisecond);

            // Set time as HH:MM:59.999
            dateTimeCurrentTransaction = dateTimeCurrentTransaction.AddSeconds(59 - dateTimeCurrentTransaction.Second);
            dateTimeCurrentTransaction = dateTimeCurrentTransaction.AddMilliseconds(999 - dateTimeCurrentTransaction.Millisecond);

            return Transactions
                    .Where(q => q.Time.CompareTo(dateTimeMinutesAgo) >= 0 && q.Time.CompareTo(dateTimeCurrentTransaction) <= 0);
        }

        public void AddTransaction(Transaction transaction)
        {
            FailIf(() => !ActiveCard, Resources.CARD_NOT_ACTIVE);
            FailIf(() => Amount - transaction.Amount < 0, Resources.INSUFFICIENT_LIMIT);

            var lastTwoMinutesTransactions = GetLastTwoMinutesTransactions(transaction);
            FailIf(() => lastTwoMinutesTransactions.Count() >= 3, Resources.HIGH_FREQUENCY_SMALL_INTERVAL);
            FailIf(() => lastTwoMinutesTransactions.Any(q => q.Equals(transaction)), Resources.DOUBLED_TRANSACTION);

            if (Valid)
                Transactions.Add(transaction);
        }

    }

}