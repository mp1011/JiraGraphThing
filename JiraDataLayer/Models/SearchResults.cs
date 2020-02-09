namespace JiraDataLayer.Models
{
    public class SearchResults
    {
        public string JQL { get; }
        public int MaxRecords { get; }
        public JiraIssue[] Items { get; }

        public SearchResults(string jQL, int maxRecords, JiraIssue[] results)
        {
            JQL = jQL;
            MaxRecords = maxRecords;
            Items = results;
        }
    }
}
