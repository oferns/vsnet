namespace VD.PayOn {
    using System;

    public static class Extensions {

        public static string ToPayOnString(this PaymentType paymentType) {
            return paymentType switch
            {
                PaymentType.PreAuthorization => "PA",
                PaymentType.Debit => "DB",
                PaymentType.Credit => "CD",
                PaymentType.Capture => "CP",
                PaymentType.Reversal => "RV",
                PaymentType.Refund => "RF",
                _ => throw new NotSupportedException($"The payment type {paymentType} was not recognised"),
            };
        }

        public static string ToPayOnString(this RecurringType recurringType) {
            return recurringType switch
            {
                RecurringType.Initial => "INITIAL",
                RecurringType.Repeated => "REPEATED",
                _ => throw new NotSupportedException($"The recurring type {recurringType} was not recognised"),
            };
        }

        public static string ToPayOnString(this TransactionCategory transactionCategory) {
            return transactionCategory switch
            {
                TransactionCategory.eCommerce => "EC",
                TransactionCategory.Installment => "IN",
                TransactionCategory.MailOrder => "MO",
                TransactionCategory.Recurring => "RC",
                TransactionCategory.PoS => "PO",
                TransactionCategory.mPoS => "PM",
                TransactionCategory.TelephoneOrder => "TO",
                _ => throw new NotSupportedException($"The transaction category {transactionCategory} was not recognised"),
            };
        }
    }
}
