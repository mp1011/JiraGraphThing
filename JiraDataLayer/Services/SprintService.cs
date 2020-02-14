using JiraDataLayer.Models;
using JiraDataLayer.Models.JiraPocos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JiraDataLayer.Services
{
    public class SprintService
    {
        private readonly JiraRestClientProvider _jiraClientProvider;
        private readonly IJiraConfig _jiraConfig;
        internal SprintService(JiraRestClientProvider jiraClientProvider,IJiraConfig jiraConfig)
        {
            _jiraClientProvider = jiraClientProvider;
            _jiraConfig = jiraConfig;
        }

        public async Task<Sprint> GetSprint(string sprintName)
        {
            return await GetSprint(_jiraConfig.BoardName, sprintName);
        }
        
        public async Task<Sprint> GetSprint(string boardName, string sprintName)
        {
            var client = _jiraClientProvider.CreateClient();
            var boards = await client.RestClient.ExecuteRequestAsync<CollectionAPIModel<BoardAPIModel>>(
                method: RestSharp.Method.GET,
                resource: $@"/rest/agile/1.0/board?name={boardName}");

            if (boards == null || boards.values.Length == 0)
                throw new Exception($"Unable to find a board named {boardName}");

            int boardId = boards.values[0].id;

            var sprints = await client.RestClient.ExecuteRequestAsync<CollectionAPIModel<SprintAPIModel>>(
                method: RestSharp.Method.GET,
                resource: $@"/rest/agile/1.0/board/{boardId}/sprint");

            var sprint = sprints.values.FirstOrDefault(p => p.name == sprintName);
            if (sprint != null)
                return new Sprint(sprint,GetStoryPointsPerSprint(sprint));
            else
                throw new Exception($"Unable to find sprint {sprintName}. Pagination is not yet supported");
        }

        public async Task<Sprint[]> GetSprints(string boardName=null)
        {
            boardName = boardName ?? _jiraConfig.BoardName;
            var client = _jiraClientProvider.CreateClient();
            var boards = await client.RestClient.ExecuteRequestAsync<CollectionAPIModel<BoardAPIModel>>(
                method: RestSharp.Method.GET,
                resource: $@"/rest/agile/1.0/board?name={boardName}");

            if (boards == null || boards.values.Length == 0)
                throw new Exception($"Unable to find a board named {boardName}");

            int boardId = boards.values[0].id;

            var sprints = await client.RestClient.ExecuteRequestAsync<CollectionAPIModel<SprintAPIModel>>(
                method: RestSharp.Method.GET,
                resource: $@"/rest/agile/1.0/board/{boardId}/sprint");

            return sprints.values
                .Select(s => new Sprint(s, GetStoryPointsPerSprint(s)))
                .ToArray();
        }


        private TimeSpan GetStoryPointsPerSprint(SprintAPIModel model)
        {
            if (model.startDate <= new DateTime(2020, 2, 13))
                return TimeSpan.FromHours(6);
            else
                return TimeSpan.FromHours(3);
        }
    }

    public class Boards
    {
        public int total { get; set; }
    }
}
