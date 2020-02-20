using System;
using System.Collections.Generic;
using System.Text;

namespace JiraDataLayer.Models.JiraPocos
{
    class WorkLogV2
    {
        public Author Author { get; set; }
        public string comment { get; set; }
        public DateTime updated { get; set; }
        public DateTime started { get; set; }
        public int timeSpentSeconds { get; set; }
    }
}
