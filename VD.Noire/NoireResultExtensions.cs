namespace VD.Noire {

    using System.Text.RegularExpressions;

    public static class NoireResultExtensions {

        public static bool IsSuccess(this NoireResult result) => Regex.IsMatch(result.Code, @"/^(000\.000\.|000\.100\.1|000\.[36])/");
        
    }
}
