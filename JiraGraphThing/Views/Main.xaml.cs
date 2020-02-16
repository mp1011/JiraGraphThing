using JiraGraphThing.Models;
using JiraGraphThing.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace JiraGraphThing.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Main : Page
    {
        public Frame GetContentHolder()
        {
            return ContentHolder;
        }

        public SprintPickerViewModel ViewModel => DataContext as SprintPickerViewModel;

        public Main()
        {
            this.InitializeComponent();
            Navigation.ItemInvoked += Navigation_ItemInvoked;
            Navigation.BackRequested += Navigation_BackRequested;
        }

        private void Navigation_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if(ContentHolder.CanGoBack)
                ContentHolder.GoBack();
        }

        private void Navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var sprint = args.InvokedItemContainer.DataContext as SprintTile;
            sprint.GotoSprint.Execute(null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = ViewModel;
            Task.Run(async () => await vm.Initialize());
        }
    }
}
