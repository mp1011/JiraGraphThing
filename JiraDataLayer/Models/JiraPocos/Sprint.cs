using System;
using System.Collections.Generic;
using System.Text;

namespace JiraDataLayer.Models.JiraPocos
{
    class SprintAPIModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
