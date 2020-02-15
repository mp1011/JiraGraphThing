using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
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
            var sprints = await _sprintService.GetSprints(_boardName);

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                Sprints.Clear();
                foreach (var sprint in sprints)
                    Sprints.Add(new SprintTile(sprint, _navigationService));
            });
        }

       
    }
}
