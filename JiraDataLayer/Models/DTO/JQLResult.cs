using Dapper.Contrib.Extensions;

namespace JiraDataLayer.Models.DTO
{
    [Table("JQLSearch")]
    class JQLSearchDTO 
    {
        public string Key { get; set; }
        public int ROWID { get; set; }

        public string JQL { get; set; }
        public int Take { get; set; }
    }

    [Table("JQLSearchItem")]
    class JQLSearchItemDTO 
    {
        public string Key { get; set; }
        public int SearchID { get; set; }
        public string IssueKey { get; set; }
    }
}
