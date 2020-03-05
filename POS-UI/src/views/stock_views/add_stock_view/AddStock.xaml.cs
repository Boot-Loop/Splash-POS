﻿using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for AddUsers.xaml
    /// </summary>
    public partial class AddStock : Window
    {
        private AddStockViewModel _add_stock_view_model;
        public AddStock(StockModel model, HomeViewModel home_view_model) {
            InitializeComponent();
            this._add_stock_view_model = new AddStockViewModel(model, this, home_view_model);
            this.DataContext = _add_stock_view_model;
            this.Owner = Application.Current.MainWindow;
        }
    }
}
