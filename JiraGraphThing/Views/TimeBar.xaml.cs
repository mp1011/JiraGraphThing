using JiraGraphThing.ViewModels;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace JiraGraphThing.Views
{
    public sealed partial class TimeBar : UserControl
    {
        public int SpentMinutes
        {
            get => (int)GetValue(SpentMinutesProperty);
            set => SetValue(SpentMinutesProperty, value);
        }

        public int EstimatedMinutes
        {
            get => (int)GetValue(EstimatedMinutesProperty);
            set => SetValue(EstimatedMinutesProperty, value);
        }

        public int MaxPossibleMinutes
        {
            get => (int)GetValue(MaxPossibleMinutesProperty);
            set => SetValue(MaxPossibleMinutesProperty, value);
        }

        public bool IsItemCompleted
        {
            get => (bool)GetValue(IsItemCompletedProperty);
            set => SetValue(IsItemCompletedProperty, value);

        }

        public static readonly DependencyProperty SpentMinutesProperty = DependencyProperty.Register(
            nameof(SpentMinutes),
            typeof(int),
            typeof(TimeBar),
            new PropertyMetadata(0));

        public static readonly DependencyProperty EstimatedMinutesProperty = DependencyProperty.Register(
            nameof(EstimatedMinutes),
            typeof(int),
            typeof(TimeBar),
            new PropertyMetadata(1));

        public static readonly DependencyProperty MaxPossibleMinutesProperty = DependencyProperty.Register(
            nameof(MaxPossibleMinutes),
            typeof(int),
            typeof(TimeBar),
            new PropertyMetadata(1));

        public static readonly DependencyProperty IsItemCompletedProperty = DependencyProperty.Register(
           nameof(IsItemCompleted),
           typeof(bool),
           typeof(TimeBar),
           new PropertyMetadata(false));

        public TimeBar()
        {
            this.InitializeComponent();
            this.LayoutUpdated += TimeBar_LayoutUpdated;
        }

        private void TimeBar_LayoutUpdated(object sender, object e)
        {
            UpdateProgressBarWidth();
        }

        public Color BarColor1
        {
            get
            {
                //should put in a resource
                if (IsItemCompleted)
                    return Color.FromArgb(255, 90, 199, 119);
                else
                    return Color.FromArgb(255, 0, 0x78, 0xD4);
            }
        }

        public Color BarColor2
        {
            get
            {
                //should put in a resource
                if (IsItemCompleted)
                    return Color.FromArgb(255, 107, 219, 137);
                else
                    return Color.FromArgb(255, 0, 0x88, 0xF4);
            }
        }

        private void UpdateProgressBarWidth()
        {
            if (ProgressBarHolder.ActualWidth <= 0)
                return;

            TimeEstimatedProgressBar.Width = ProgressBarHolder.ActualWidth * ((double)EstimatedMinutes / MaxPossibleMinutes);
            TimeUsedProgressBar.Width = ProgressBarHolder.ActualWidth * ((double)SpentMinutes / MaxPossibleMinutes);
        }
    }
}
