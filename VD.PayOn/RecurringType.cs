
namespace VD.PayOn {
    using System.ComponentModel;

    public enum RecurringType {

        [Description("First payment.")]
        Initial,
        [Description("Subseque payment.")]
        Repeated
    }
}
