namespace VS.PostGres.Tests {

    public class TestFunctionResult {

        public TestFunctionResult(int intProp, string stringProp, decimal decimalProp, bool boolProp) {
            IntProp = intProp;
            StringProp = stringProp;
            DecimalProp = decimalProp;
            BoolProp = boolProp;
        }

        public string StringProp { get; private set; }

        public int IntProp { get; private set; }

        public decimal DecimalProp { get; private set; }

        public bool BoolProp { get; private set; }
    }
}
