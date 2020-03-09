namespace VD.PayOn {

    using System.ComponentModel;

    [Description("Register Card")]
    public readonly struct RegisterCard {

        public readonly bool CreateRegistration => true;


        public override string ToString() {
            return "createRegistration=true";
        }

        public override bool Equals(object obj) {
            return obj is RegisterCard checkout && checkout.ToString().Equals(ToString());
        }

        public static bool operator ==(RegisterCard left, RegisterCard right) {
            return left.Equals(right);
        }

        public static bool operator !=(RegisterCard left, RegisterCard right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }
    }
}
