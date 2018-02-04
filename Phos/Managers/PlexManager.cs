using System;
namespace Phos.Managers
{
    public static class PlexManager
    {
        public static string ParseJsonFromWebhook(string content)
        {
            var start = content.IndexOf('{');
            var end = content.LastIndexOf('}');

            return content.Substring(start, (end - start) + 1);
        }
    }
}
