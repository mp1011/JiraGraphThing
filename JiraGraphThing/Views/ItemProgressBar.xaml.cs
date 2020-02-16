using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Models.GraphModels;
using JiraGraphThing.Models;
using JiraGraphThing.Services;
using JiraGraphThing.ViewModels;
using System.ComponentModel;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace JiraGraphThing.Views
{
    public sealed partial class ItemProgressBar : UserControl, INotifyPropertyChanged
    {
        public ItemProgressBar()
        {
            this.InitializeComponent();
            this.DataContextChanged += ItemProgressBar_DataContextChanged;
            ViewModel = new ItemProgressBarViewModel(SimpleIoc.Default.GetInstance<PageNavigationService>());
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            this.LayoutUpdated += ItemProgressBar_LayoutUpdated;
            ProgressBarHolder.PointerPressed += ProgressBarHolder_PointerPressed;
        }

        private void ProgressBarHolder_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ViewModel.NavigateToDetails();
        }

        public ItemProgressBarViewModel ViewModel { get; }

        public Color BarColor1
        {
            get
            {
                //should put in a resource
                if (ViewModel.IsItemCompleted)
                    return Color.FromArgb(255, 90,199,119);
                else
                    return Color.FromArgb(255, 0, 0x78, 0xD4);
            }
        }

        public Color BarColor2
        {
            get
            {
                //should put in a resource
                if (ViewModel.IsItemCompleted)
                    return Color.FromArgb(255, 107,219,137);
                else
                    return Color.FromArgb(255, 0, 0x88, 0xF4);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateProgressBarWidth();
        }

        private void ItemProgressBar_LayoutUpdated(object sender, object e)
        {
            UpdateProgressBarWidth();
        }

        private void UpdateProgressBarWidth()
        {
            if (ProgressBarHolder.ActualWidth <= 0)
                return;

            TimeEstimatedProgressBar.Width = ProgressBarHolder.ActualWidth * ((double)ViewModel.MinutesEstimated / ViewModel.MaxBarMinutes);
            TimeUsedProgressBar.Width = ProgressBarHolder.ActualWidth * ((double)ViewModel.MinutesLogged / ViewModel.MaxBarMinutes);  
        }

        private void ItemProgressBar_DataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
        {
            if (args.NewValue is UINode node)
            {
                ViewModel.Initialize(node);
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(BarColor1)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BarColor2)));

            }
        }
    }
}
