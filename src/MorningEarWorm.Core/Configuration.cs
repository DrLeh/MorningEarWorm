using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorningEarWorm.Core
{
    public interface IConfiguration
    {
        string GetAppSetting(string key);
    }

    public class StaticConfiguration : IConfiguration
    {
        private static Dictionary<string, string> AppSettings = new Dictionary<string, string>
        {
            ["LastFMSecret"] = "ce4adfb8675204ef59b78f0517d730d8",
            ["LastFMApiKey"] = "4b7f1d0e206b9cf8ee0e57feb0ce90e4",
            ["TwitterConsumerKey"] = "v198p3mdOPWpBdJINXfiyb4Or",
            ["TwitterSecret"] = "MTIBiDDjqKZowz2zQf7YPdQbIa6cVl0i03vxqXPdAEsJompNEG",
            ["TwitterAccessToken"] = "774956676-mia9cCbHuIU20lNia3IUoZ4ftnf51pPD51YKnbfJ",
            ["TwitterAccessTokenSecret"] = "O7bSgomlBNjF3bzhpoCGEe0G7WSD0cSSWsxQNDw7Wp8U2",
        };

        public string GetAppSetting(string key)
        {
            return AppSettings[key];
        }
    }
}
