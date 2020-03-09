namespace VD.PayOn.BackOffice {

    public readonly struct Credit {

        public Credit(double amount, string currency) {
            Amount = amount;
            Currency = currency;
        }

        public readonly PaymentType PaymentType => PaymentType.Credit;

        public double Amount { get; }
        public string Currency { get; }
    }
}
