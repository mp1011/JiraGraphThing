using JiraDataLayer.Models.GraphModels;
using JiraGraphThing.Models;
using JiraGraphThing.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ItemWorkHistory : Page
    {
        public ItemWorkHistoryViewModel ViewModel => DataContext as ItemWorkHistoryViewModel;
        public ItemWorkHistory()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = ViewModel;
            if(e.Parameter is UINode node)
            {
                vm.Initialize(node);
            }
        }
    }
}
