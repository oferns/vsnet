namespace VD.PayOn {
    using System.ComponentModel;

    public enum TransactionCategory  {
        
        [Description("eCommerce")]
        NONE,
        [Description("eCommerce")]
        eCommerce,
        [Description("Mail Order")]
        MailOrder,
        [Description("Telephone Order")]
        TelephoneOrder,
        [Description("Recurring")]
        Recurring,
        [Description("Installment")]
        Installment,
        [Description("POS")]
        PoS,
        [Description("MPOS")]
        mPoS
    }
}
