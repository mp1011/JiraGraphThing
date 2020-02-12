using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Models.GraphModels;
using JiraGraphThing.ViewModels;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace JiraGraphThing.Views
{
    public sealed partial class ItemProgressBar : UserControl
    {
        public ItemProgressBarViewModel ViewModel { get; }

        public double ProgressBarHolderWidth => ProgressBarHolder.ActualWidth;

        public ItemProgressBar()
        {
            this.InitializeComponent();
            this.DataContextChanged += ItemProgressBar_DataContextChanged;
            ViewModel = new ItemProgressBarViewModel();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            this.LayoutUpdated += ItemProgressBar_LayoutUpdated;
        }

        private void ItemProgressBar_LayoutUpdated(object sender, object e)
        {
            UpdateProgressBarWidth();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ItemProgressBarViewModel.PercentTimeUsed))
                UpdateProgressBarWidth();
        }

        private void UpdateProgressBarWidth()
        {
            TimeUsedProgressBar.Width = ProgressBarHolder.ActualWidth * (double)ViewModel.PercentTimeUsed;
        }

        private void ItemProgressBar_DataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
        {
            if (args.NewValue is IssueNode node)
                ViewModel.Initialize(node);
        }
    }
}
