namespace VD.PayOn.BackOffice {

    public readonly struct Reversal {

        public Reversal(string referencedPaymentId) {
            ReferencedPaymentId = referencedPaymentId;
        }

        public readonly PaymentType PaymentType => PaymentType.Reversal;

        public string ReferencedPaymentId { get; }

        public override string ToString() {
            return $"referencedPaymentId={ReferencedPaymentId}&paymentType=RV";
        }

        public override bool Equals(object obj) {
            return obj is Reversal refund && refund.Equals(ToString());
        }

        public static bool operator ==(Reversal left, Reversal right) {
            return left.Equals(right);
        }

        public static bool operator !=(Reversal left, Reversal right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }
    }
}