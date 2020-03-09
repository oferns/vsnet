using System;

namespace VD.PayOn {

    public readonly struct DeRegisterCard {

        public DeRegisterCard(string registrationId) {
            if (string.IsNullOrWhiteSpace(registrationId)) {
                throw new ArgumentException("You must supply a registration id", nameof(registrationId));
            }

            RegistrationId = registrationId;
        }

        public readonly string RegistrationId { get; }

        public override string ToString() {
            return RegistrationId;
        }

        public override bool Equals(object obj) {
            return obj is DeRegisterCard capture && capture.Equals(ToString());
        }

        public static bool operator ==(DeRegisterCard left, DeRegisterCard right) {
            return left.Equals(right);
        }

        public static bool operator !=(DeRegisterCard left, DeRegisterCard right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }
    }
}
