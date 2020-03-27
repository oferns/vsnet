namespace VD.PayOn.Models {

    using System;
    using System.Text.Json.Serialization;

    public class DirectDebit {

        [JsonPropertyName("transactionDueDate")]
        public DateTimeOffset DueDate { get; set; }


        [JsonPropertyName("bankAccount")]
        public BankAccount BankAccount { get; set; }
    }
}
