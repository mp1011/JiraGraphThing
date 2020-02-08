using Atlassian.Jira;

namespace JiraDataLayer.Services
{
    internal class JiraRestClientProvider
    {
        private readonly IJiraConfig _config;

        public JiraRestClientProvider(IJiraConfig config)
        {
            _config = config;
        }

        public Jira CreateClient()
        {
            var jiraClient = Jira.CreateRestClient(
                url: _config.JiraURL,
                username: _config.JiraUsername,
                password: _config.JiraAccessToken);

            return jiraClient;
        }
    }
}
