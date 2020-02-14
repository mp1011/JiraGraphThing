using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using JiraDataLayer;
using JiraDataLayer.Models;
using JiraDataLayer.Services;
using JiraGraphThing.Models;
using JiraGraphThing.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JiraGraphThing.ViewModels
{
    public class SprintPickerViewModel : ViewModelBase
    {
        private readonly SprintService _sprintService;
        private readonly PageNavigationService _navigationService;

        private readonly string _boardName;

        public ObservableCollection<SprintTile> Sprints { get; } = new ObservableCollection<SprintTile>();

        public SprintPickerViewModel(SprintService sprintService, PageNavigationService navigationService, IJiraConfig config)
        {
            _sprintService = sprintService;
            _navigationService = navigationService;
            _boardName = config.BoardName;
        }

        public async Task Initialize()
        {
            Sprints.Clear();
            foreach (var sprint in await _sprintService.GetSprints(_boardName))
                Sprints.Add(new SprintTile(sprint, _navigationService));
        }

       
    }
}
