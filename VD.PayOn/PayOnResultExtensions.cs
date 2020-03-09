namespace VD.PayOn {
    using System.Text.RegularExpressions;

    public static class PayOnResultExtensions {


        public static bool DidntFail(this PayOnResult result) => result.IsSuccess() || result.IsSuccessButNeedsReview() || result.IsPending() || result.IsLongTermPending();

        public static bool IsSuccess(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(000 \.000\.|000\.100\.1|000\.[36])", RegexOptions.Compiled);

        public static bool IsSuccessButNeedsReview(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(000\.400\.0[^3]|000\.400\.100)", RegexOptions.Compiled);

        public static bool IsPending(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(000\.200)", RegexOptions.Compiled);

        public static bool IsLongTermPending(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(800\.400\.5|100\.400\.500)", RegexOptions.Compiled);

        public static bool IsRejectedThroughRisk(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(000\.400\.[1][0-9][1-9]|000\.400\.2)", RegexOptions.Compiled);

        public static bool IsRejectedByBank(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(800\.[17]00|800\.800\.[123])", RegexOptions.Compiled);

        public static bool IsRejectedThroughCommsError(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(900\.[1234]00|000\.400\.030)", RegexOptions.Compiled);

        public static bool IsRejectedThroughSystemError(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(800\.[56]|999\.|600\.1|800\.800\.[84])", RegexOptions.Compiled);

        public static bool IsRejectedThroughAsyncError(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(100\.39[765])", RegexOptions.Compiled);

        public static bool IsRejectedThroughSoftDecline(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(300\.100\.100)", RegexOptions.Compiled);

        public static bool IsRejectedThroughExternalSystem(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(100\.400\.[0-3]|100\.38|100\.370\.100|100\.370\.11)", RegexOptions.Compiled);

        public static bool IsRejectedThroughAddressValidation(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(800\.400\.1)", RegexOptions.Compiled);

        public static bool IsRejectedThrough3DSecure(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(800\.400\.2|100\.380\.4|100\.390)", RegexOptions.Compiled);

        public static bool IsRejectedThroughBlacklist(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(100\.100\.701|800\.[32])", RegexOptions.Compiled);

        public static bool IsRejectedThroughRiskValidation(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(800\.1[123456]0)", RegexOptions.Compiled);

        public static bool IsRejectedThroughConfigurationValidation(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(800\.1[123456]0)", RegexOptions.Compiled);

        public static bool IsRejectedThroughRegistrationValidation(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(100\.[13]50)", RegexOptions.Compiled);

        public static bool IsRejectedThroughJobValidation(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(100\.250|100\.360)", RegexOptions.Compiled);

        public static bool IsRejectedThroughReferenceValidation(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(700\.[1345][05]0)", RegexOptions.Compiled);

        public static bool IsRejectedThroughFormatValidation(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(200\.[123]|100\.[53] [07]|800\.900|100\.[69]00\.500)", RegexOptions.Compiled);

        public static bool IsRejectedThroughAddressValidation2(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(100\.800)", RegexOptions.Compiled);

        public static bool IsRejectedThroughContactValidation(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(100\.[97]00)", RegexOptions.Compiled);

        public static bool IsRejectedThroughAccountValidation(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(100\.100|100.2[01])", RegexOptions.Compiled);

        public static bool IsRejectedThroughAmountValidation(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(100\.55)", RegexOptions.Compiled);

        public static bool IsRejectedThroughRiskManagement(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(100\.380\.[23]|100\.380\.101)", RegexOptions.Compiled);

        public static bool IsRejectedThroughChargeBack(this PayOnResult result) => Regex.IsMatch(result.Code, @"^(000\.100\.2)", RegexOptions.Compiled);

    }
}