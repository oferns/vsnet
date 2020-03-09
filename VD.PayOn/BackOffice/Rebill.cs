namespace VD.PayOn.BackOffice {

    public readonly struct Rebill {

        public Rebill(string referencedPaymentId) {
            ReferencedPaymentId = referencedPaymentId;
        }

        public string ReferencedPaymentId { get; }

        public readonly PaymentType PaymentType => PaymentType.Rebill;

        public override string ToString() {
            return $"payment.id={ReferencedPaymentId}&paymentType=RB";
        }

        public override bool Equals(object obj) {
            return obj is Rebill rebill && rebill.Equals(ToString());
        }

        public static bool operator ==(Rebill left, Rebill right) {
            return left.Equals(right);
        }

        public static bool operator !=(Rebill left, Rebill right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }
    }
}