﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Management : UserControl
    {
        private ManagementViewModel _management_view_model;
        public Management(HomeView home_view, HomeViewModel home_view_model) {
            InitializeComponent();
            this._management_view_model = new ManagementViewModel(home_view, home_view_model);
            this.DataContext = _management_view_model;
        }
    }
}
