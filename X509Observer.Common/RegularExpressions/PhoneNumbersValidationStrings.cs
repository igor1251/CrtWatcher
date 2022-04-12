namespace X509Observer.Common.RegularExpressions
{
    public static class PhoneNumbersValidationStrings
    {
        public static readonly string PHONE_TEMPLATE_REGULAR_EXPRESSION = @"^(\s*)?(\+)?([- _():=+]?\d[- _():=+]?){10,14}(\s*)?$";
    }
}
