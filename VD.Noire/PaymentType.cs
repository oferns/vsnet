namespace VD.Noire {

    using System.ComponentModel;

    public enum PaymentType {

        [Description("Preauthorization: A stand-alone authorisation that will also trigger optional risk management and validation. A Capture (CP) with reference to the Preauthorisation (PA) will confirm the payment.")]
        PreAuthorization,

        [Description("Debit: Debits the account of the end customer and credits the merchant account.")]
        Debit,

        [Description("Credit: Credits the account of the end customer and debits the merchant account")]
        Credit,

        [Description("Capture: Captures a preauthorized (PA) amount.")]
        Capture,

        [Description("Reversal: Reverses an already processed Preauthorization (PA), Debit (DB) or Credit (CD) transaction. As a consequence, the end customer will never see any booking on his statement. A Reversal is only possible until a connector.")]
        Reversal,

        [Description(" Refund: Credits the account of the end customer with a reference to a prior Debit (DB) or Credit (CD) transaction. The end customer will always see two bookings on his statement. Some connectors do not support Refunds.")]
        Refund
    }
}
