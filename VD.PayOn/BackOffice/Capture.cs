namespace VD.PayOn.BackOffice {

    using System.ComponentModel;

    [Description(@"A capture is used to request clearing for previously authorized funds.
                    A capture request is performed against a previous preauthorization (PA) payment by referencing its payment.id
                    Captures can be for full or partial amounts and multiple capture requests against the same PA are allowed.")]
    public readonly struct Capture {

        public Capture(string referencedPaymentId, double amount, string currency) {
            ReferencedPaymentId = referencedPaymentId;
            Amount = amount;
            Currency = currency;
        }

        public readonly PaymentType PaymentType => PaymentType.Capture;

        public readonly string ReferencedPaymentId { get; }
        public readonly double Amount { get; }
        public readonly string Currency { get; }

        public override string ToString() {
            return $"referencedPaymentId={ReferencedPaymentId}&amount={Amount}&currency={Currency}&paymentType=CR";
        }

        public override bool Equals(object obj) {
            return obj is Capture capture && capture.Equals(ToString());
        }

        public static bool operator ==(Capture left, Capture right) {
            return left.Equals(right);
        }

        public static bool operator !=(Capture left, Capture right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }
    }
}
