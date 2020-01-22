namespace VS.PostGres.Tests {


    public class TestFunction {

        public TestFunction(string stringArg, int intArg, decimal decimalArg, bool boolArg) {
            StringArg = stringArg;
            IntArg = intArg;
            DecimalArg = decimalArg;
            BoolArg = boolArg;
        }

        public string StringArg { get; }

        public int IntArg { get; }

        public decimal DecimalArg { get; }

        public bool BoolArg { get; }
    }
}