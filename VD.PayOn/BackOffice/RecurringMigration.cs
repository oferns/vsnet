
namespace VD.PayOn.BackOffice {

    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RecurringMigration {

        public RecurringMigration(PaymentType paymentType,
                                   decimal? amount = default,
                                   DateTimeOffset? requestTimestamp = default,
                                   string connectorTxId1 = default,
                                   string connectorTxId2 = default,
                                   string connectorTxId3 = default
                                   ) {
            PaymentType = paymentType;
            Amount = amount;
            RequestTimestamp = requestTimestamp;
            ConnectorTxId1 = connectorTxId1;
            ConnectorTxId2 = connectorTxId2;
            ConnectorTxId3 = connectorTxId3;
        }

        public PaymentType PaymentType { get; private set; }
        public decimal? Amount { get; private set; }
        public DateTimeOffset? RequestTimestamp { get; private set; }
        public string ConnectorTxId1 { get; private set; }
        public string ConnectorTxId2 { get; private set; }
        public string ConnectorTxId3 { get; private set; }

        public override string ToString() {

            return $"payment.id={PaymentType.ToPayOnString()}"; // TODO: Finsih this
        }

        public override bool Equals(object obj) {
            return obj is RecurringMigration recurringMigration && recurringMigration.Equals(ToString());
        }

        public static bool operator ==(RecurringMigration left, RecurringMigration right) {
            return left.Equals(right);
        }

        public static bool operator !=(RecurringMigration left, RecurringMigration right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }
    }

}
