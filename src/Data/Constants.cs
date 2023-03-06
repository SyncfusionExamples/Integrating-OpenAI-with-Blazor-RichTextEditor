namespace BlazorRTEWithOpenAI.Data
{
    internal static class Constants
    {
        // The OpenAI API model name.
        internal const string OPENAI_MODEL = "text-davinci-003";
        // The OpenAI API key.
        internal const string OPENAI_KEY = "";
        // The OpenAI API endpoint.
        internal const string API_ENDPOINT = "https://api.openai.com/v1/completions";
        internal const string STANDARD = "Rewrite the below text in a reliable manner to maintain meaning and provide the result as html content.\n\n";
        internal const string FORMAL = "Present the below text in a more sophisticated and professional way content and provide the result as html content.\n\n";
        internal const string EXPAND = "Adds more detail and depth to increase sentence length of the below html content and provide the result as html content.\n\n";
        internal const string SHORTEN = "Strips away extra words in the below html content and provide a clear message as html content.\n\n";
        internal const string PROFESSIONAL = "Change the below html content tone to professional content and provide the result as html content.\n\n";
        internal const string CASUAL = "Change the below html content tone to casual content and provide the result as html content.\n\n";
        internal const string STRAIGHTFORWARD = "Change the below html content tone to straightforward content and provide the result as html content.\n\n";
        internal const string CONFIDENT = "Change the below html content tone to confident content and provide the result as html content.\n\n";
        internal const string FRIENDLY = "Change the below html content tone to friendly content and provide the result as html content.\n\n";
        internal const string GRAMMARCHECK = "Check grammar and spelling in the below html content and provide the result as html content.\n\n";
        internal const string CONVERTTOARTICLE = "Convert to article using the below html content and provide the result as html content.\n\n";
    }
}
