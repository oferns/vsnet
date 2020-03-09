
namespace VD.PayOn.BackOffice {
    public readonly struct Refund {

        public Refund(string referencedPaymentId, double amount, string currency) {
            ReferencedPaymentId = referencedPaymentId;
            Amount = amount;
            Currency = currency;
        }

        public readonly PaymentType PaymentType => PaymentType.Refund;

        public readonly string ReferencedPaymentId { get; }
        public readonly double Amount { get; }
        public readonly string Currency { get; }

        public override string ToString() {
            return $"referencedPaymentId={ReferencedPaymentId}&amount={Amount}&currency={Currency}&paymentType=RF";
        }

        public override bool Equals(object obj) {
            return obj is Refund refund && refund.Equals(ToString());
        }

        public static bool operator ==(Refund left, Refund right) {
            return left.Equals(right);
        }

        public static bool operator !=(Refund left, Refund right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }
    }
}
