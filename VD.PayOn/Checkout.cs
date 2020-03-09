namespace VD.PayOn {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    [Description("Basic Payment")]
    public readonly struct Checkout {

        public Checkout(decimal amount,
                                string currency, 
                                PaymentType paymentType,
                                IEnumerable<string> registrationIds = default,
                                string paymentBrand = default,
                                IDictionary<string, PaymentType> paymentTypeOverrides = default, 
                                decimal taxAmount = default,
                                string descriptor = default, 
                                string merchantTransactionId = default, 
                                string merchantInvoiceId = default,
                                string merchantMemo = default,
                                bool createRegistration = true,
                                TransactionCategory transactionCategory = TransactionCategory.NONE) {
            Amount = amount;
            Currency = currency;
            PaymentType = paymentType;
            RegistrationIds = registrationIds;
            PaymentBrand = paymentBrand;
            PaymentTypeOverrides = paymentTypeOverrides;
            TaxAmount = taxAmount;
            Descriptor = descriptor;
            MerchantTransactionId = merchantTransactionId;
            MerchantInvoiceId = merchantInvoiceId;
            MerchantMemo = merchantMemo;
            CreateRegistration = createRegistration;
            TransactionCategory = transactionCategory;
        }

        [Required]
        [Description(@"Indicates the amount of the payment request. The dot is used as decimal separator.
            The amount is the only amount value which is processing relevant. 
            All other amount declarations like taxAmount or shipping.cost are already included.")]
        [Range(0, 9999999999.99, ErrorMessage = "Amount should be a positive decimal with 0-2 decimal places.")]
        [DisplayName("Amount")]
        public readonly decimal Amount { get; }
               
        [Required]     
        [Description("The currency code of the payment request's amount (ISO 4217).")]
        [DisplayName("Currency")]
        public readonly string Currency { get; }
                
        [Required]
        [Description("The payment type for the request.")]
        [DisplayName("Payment Type")]
        public readonly PaymentType PaymentType { get; }

        [Required]
        [Description("The Registration Ids of any cards saved by the User.")]
        [DisplayName("Registration Ids")]
        public IEnumerable<string> RegistrationIds { get; }

        [Description("The brand specifies the method of payment for the request. This is optional if you want to use brand detection for credit cards, if not then it is mandatory.")]
        [RegularExpression("[a-zA-Z0-9_]{1,32}")]
        [DisplayName("Payment Brand")]
        public readonly string PaymentBrand { get; }

        [Description(@"The payment type can be overriden for specific brands, for example:
                        BOLETO, PaymentType.PreAuthorization
                        KLARNA_INVOICE, PaymentType.PreAuthorization                        
                    In such cases, the default payment type will be the one defined in paymentType parameter 
                    and every brand defined in overridePaymentType will have its own payment type.
                    This parameter is only accepted during the checkout creation.")]
        [DisplayName("Override Payment Type")]
        public readonly IDictionary<string, PaymentType> PaymentTypeOverrides { get; }


        [Range(0, 9999999999.99, ErrorMessage = "Tax Amount should be a positive decimal with 0-2 decimal places.")]
        [Description("Indicates the tax amount of the payment request. The dot is used as decimal separator.")]
        [DisplayName("Tax Amount")]
        public readonly decimal TaxAmount { get; }

        [RegularExpression(@"[\s\S]{1,127}", ErrorMessage = "Descriptor should be a string between 1 and 127 characters")]
        [Description(@"Can be used to populate all or part of the Merchant Name descriptor, which often appears on the first line of the shopper's statement.
                        The full use of this field depends on the Merchant Account configuration. NOTE: merchant.name can override any data sent in this field.")]
        [DisplayName("Descriptor")]
        public readonly string Descriptor { get; }

        [RegularExpression(@"[\s\S]{8,255}", ErrorMessage = "Merchant Transaction Id should be a string between 8 and 255 characters")]
        [Description(@"Merchant-provided reference number, should be unique for your transactions. Some receivers require this ID. 
                        This identifier is often used for reconciliation.")]
        [DisplayName("Merchant Transaction Id")]
        public readonly string MerchantTransactionId { get; }

        [RegularExpression(@"[\s\S]{8,255}", ErrorMessage = "Merchant Invoice Id should be a string between 8 and 255 characters")]
        [Description(@"Merchant-provided invoice number, should be unique for your transactions. This identifier is not sent onwards.")]
        [DisplayName("Merchant Invoice Id")]
        public readonly string MerchantInvoiceId { get; }

        [RegularExpression(@"[\s\S]{8,255}", ErrorMessage = "Merchant Memo should be a string between 8 and 255 characters")]
        [Description(@"Merchant-provided additional information. The information provided is not transaction processing relevant. It will appear in reporting only.")]
        [DisplayName("Merchant Memo")]
        public readonly string MerchantMemo { get; }

        public bool CreateRegistration { get; }


        [Description(@"The category of the transaction.")]        
        [DisplayName("Transaction Category")]
        public readonly TransactionCategory TransactionCategory { get; }


        public override string ToString() {

            var sb = new StringBuilder().Append($"amount={Amount}&currency={Currency}&paymentType={PaymentCode(PaymentType)}");
            
            if (PaymentBrand is object) {
                sb.Append($"paymentBrand={PaymentBrand}");
            }

            if (PaymentTypeOverrides is IDictionary<string, PaymentType>) {
                foreach (var @override in PaymentTypeOverrides) {
                    sb.Append($"overridePaymentType[{@override.Key}]={PaymentCode(@override.Value)}");
                }
            }

            if (TaxAmount > 0) {
                sb.Append($"&taxAmount={TaxAmount}");
            }

            if (!string.IsNullOrEmpty(Descriptor)) {
                sb.Append($"&descriptor={Descriptor}");
            }

            if (!string.IsNullOrEmpty(MerchantTransactionId)) {
                sb.Append($"&merchantTransactionId={MerchantTransactionId}");
            }

            if (!string.IsNullOrEmpty(MerchantInvoiceId)) {
                sb.Append($"&merchantInvoiceId={MerchantInvoiceId}");
            }

            if (RegistrationIds is object) {
                var x = 0;
                foreach (var id in RegistrationIds) {
                    sb.Append($"&registrations[{x++}].id={id}");
                }
            }

            if (!string.IsNullOrEmpty(MerchantMemo)) {
                sb.Append($"&merchantMemo={MerchantMemo}");
            }

            if (CreateRegistration) {
                sb.Append("&createRegistration=true");
            }

            if (TransactionCategory != TransactionCategory.NONE) {
                sb.Append($"&transactionCategory={TransactionCategoryCode(TransactionCategory)}");
            }

            return sb.ToString();
        }        

        private string TransactionCategoryCode(TransactionCategory category) {
            return category switch
            {
                TransactionCategory.NONE => null,
                TransactionCategory.eCommerce => "EC",
                TransactionCategory.Installment => "IN",
                TransactionCategory.MailOrder => "MO",
                TransactionCategory.Recurring => "RC",
                TransactionCategory.PoS => "PO",
                TransactionCategory.mPoS => "PM",
                TransactionCategory.TelephoneOrder => "TO",
                _ => throw new NotSupportedException($"The transaction category {category} was not recognised"),
            };
        }


        private string PaymentCode(PaymentType type) {
            return type switch
            {
                PaymentType.PreAuthorization => "PA",
                PaymentType.Debit => "DB",
                PaymentType.Credit => "CD",
                PaymentType.Capture => "CP",
                PaymentType.Reversal => "RV",
                PaymentType.Refund => "RF",
                _ => throw new NotSupportedException($"The payment type {type} was not recognised"),
            };
        }

        public override bool Equals(object obj) {
            return obj is Checkout checkout && checkout.ToString().Equals(ToString());
        }

        public static bool operator ==(Checkout left, Checkout right) {
            return left.Equals(right);
        }

        public static bool operator !=(Checkout left, Checkout right) {
            return !(left == right);
        }

        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }
    }
}