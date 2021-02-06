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
            FailIf(() => !ActiveCard, Resources.CARD_NOT_ACTIVE);
            FailIf(() => Amount - transaction.Amount < 0, Resources.INSUFFICIENT_LIMIT );

            var lastTwoMinutesTransactions = GetLastTwoMinutesTransactions(transaction);
            FailIf(() => lastTwoMinutesTransactions.Count() >= 3, Resources.HIGH_FREQUENCY_SMALL_INTERVAL);
            FailIf(() => lastTwoMinutesTransactions.Any(q => q.Equals(transaction)), Resources.DOUBLED_TRANSACTION);

            if (Valid)
                Transactions.Add(transaction);
        }

    }

}