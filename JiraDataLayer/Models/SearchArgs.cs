using System;
using System.Collections.Generic;
using System.Text;

namespace JiraDataLayer.Models
{
    public class SearchArgs
    {
        public string Project { get; }
        public int Take { get; }

        public SearchArgs(string project, int take)
        {
            Project = project;
            Take = take;
        }
    }
}
