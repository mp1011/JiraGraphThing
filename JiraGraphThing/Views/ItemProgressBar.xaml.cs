﻿using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Models.GraphModels;
using JiraGraphThing.Models;
using JiraGraphThing.ViewModels;
using System.Numerics;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace JiraGraphThing.Views
{
    public sealed partial class ItemProgressBar : UserControl
    {
        public ItemProgressBarViewModel ViewModel { get; }

        public ItemProgressBar()
        {
            this.InitializeComponent();
            this.DataContextChanged += ItemProgressBar_DataContextChanged;
            ViewModel = new ItemProgressBarViewModel();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            this.LayoutUpdated += ItemProgressBar_LayoutUpdated;
        }

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
            TimeUsedProgressBar.Width = (ProgressBarHolder.ActualWidth-10) * ((double)ViewModel.MinutesLogged / ViewModel.MaxBarMinutes);

            TimeUsedProgressBar.Translation = new Vector3(5, 
                (float)(TimeEstimatedProgressBar.ActualHeight - TimeUsedProgressBar.ActualHeight)/2.0f 
                , 0);
        }

        private void ItemProgressBar_DataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
        {
            if (args.NewValue is NodeWithSprint node)
                ViewModel.Initialize(node.Node, node.Sprint);
        }
    }
}
