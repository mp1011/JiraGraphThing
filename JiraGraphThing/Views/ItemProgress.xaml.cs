using GalaSoft.MvvmLight.Ioc;
using JiraGraphThing.Models;
using JiraGraphThing.Services;
using JiraGraphThing.ViewModels;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace JiraGraphThing.Views
{
    public sealed partial class ItemProgress : UserControl
    {
        public ItemProgress()
        {
            this.InitializeComponent();
            ViewModel = new ItemProgressBarViewModel(SimpleIoc.Default.GetInstance<PageNavigationService>());
            ProgressBar.PointerPressed += ProgressBarHolder_PointerPressed;
            DataContextChanged += ItemProgress_DataContextChanged;
        }

        private void ItemProgress_DataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
        {
            if (args.NewValue is UINode node)
            {
                ViewModel.Initialize(node);
            }
        }

        private void ProgressBarHolder_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ViewModel.NavigateToDetails();
        }

        public ItemProgressBarViewModel ViewModel { get; }
 
    }
}
