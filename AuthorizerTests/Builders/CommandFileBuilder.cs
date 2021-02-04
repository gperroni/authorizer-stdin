using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AuthorizerTests.Builders
{
    public class CommandFileBuilder
    {
        private string File { get; set; }

        public CommandFileBuilder()
        {
            File = "";
        }

        public CommandFileBuilder AddAccountCommand(bool activeCard, int availableLimit)
        {
            File += $@"{{""account"": {{""active-card"": {activeCard.ToString().ToLower()}, ""available-limit"": {availableLimit} }} }}";
            File += Environment.NewLine;

            return this;
        }

        public CommandFileBuilder AddTransactionCommand(string merchant, int amount, DateTime date)
        {
            var formattedDate = date.ToString();
            File += $@"{{""transaction"": {{""merchant"": ""{merchant}"", ""amount"": {amount}, ""time"":""{formattedDate}""}} }}";
            File += Environment.NewLine;

            return this;
        }

        public CommandFileBuilder AddEmptyLine()
        {
            File += Environment.NewLine;

            return this;
        }

        public CommandFileBuilder AddCustomLine(string value)
        {
            File += $@"{{ {value} }}";

            return this;
        }


        public StringReader Build()
        {
            return new StringReader(File);
        }
    }
}
