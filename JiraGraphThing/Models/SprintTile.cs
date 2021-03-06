﻿using GalaSoft.MvvmLight.Command;
using JiraDataLayer.Models;
using JiraGraphThing.Services;
using System.Linq;
using System.Windows.Input;

namespace JiraGraphThing.Models
{
    public class SprintTile
    {
        private readonly Sprint _sprint;
        private readonly PageNavigationService _navigationService;

        public ICommand GotoSprint { get; }

        public string Name => _sprint.Name;

        public string NameShort 
        { 
            get
            {
                var parts = Name.Split(' ');
                return parts.Last();
            } 
        }

        public SprintTile(Sprint sprint, PageNavigationService navigationService)
        {
            _sprint = sprint;
            _navigationService = navigationService;
            GotoSprint = new RelayCommand(() =>
            {
                _navigationService.NavigateToSprint(_sprint.Name);
            });
        }

    }
}
