using OneConfig;
using System;
using System.Collections.Generic;
using System.Text;

namespace JiraDataLayer
{
    public interface IJiraConfig
    {
        string JiraURL { get;  }
        string JiraUsername { get; } 
        string JiraAccessToken { get; }
    }

    public class JiraConfig : IJiraConfig
    {
        public string JiraURL => AppConfig.GetValue(nameof(JiraURL), required:true);

        public string JiraUsername => AppConfig.GetValue(nameof(JiraUsername), required: true);

        public string JiraAccessToken => AppConfig.GetValue(nameof(JiraAccessToken), required: true);
    }
}
