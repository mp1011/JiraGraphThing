using Dapper.Contrib.Extensions;

namespace JiraDataLayer.Models.DTO
{
    [Table("JQLSearch")]
    public class JQLSearchDTO : IWithKey
    {
        public string Key { get; set; }
        public int ROWID { get; set; }

        public string JQL { get; set; }
        public int Take { get; set; }
    }

    [Table("JQLSearchItem")]
    class JQLSearchItemDTO : IWithKey
    {
        public string Key { get; set; }
        public int SearchID { get; set; }
        public string IssueKey { get; set; }
    }
}
