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
    public partial class AddSupplier : Window
    {
        private AddSupplierViewModel _add_supplier_view_model;
        public AddSupplier(SupplierModel model) {
            InitializeComponent();
            this._add_supplier_view_model = new AddSupplierViewModel(model);
            this.DataContext = _add_supplier_view_model;
        }
    }
}