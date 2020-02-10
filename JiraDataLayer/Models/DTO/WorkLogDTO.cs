using Dapper.Contrib.Extensions;
using System;

namespace JiraDataLayer.Models.DTO
{
    [Table("WorkItem")]
    public class WorkLogDTO : IWithKey
    {
        public string Key { get; set; }
        public string Author { get; set; }
        public DateTime Start { get; set; }
        public int TimeSpentInSeconds { get; set; }
    }
}
