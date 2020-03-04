﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Suppliers : UserControl
    {
        private SupplierViewModel _supplier_view_model;
        public Suppliers()
        {
            InitializeComponent();
            this._supplier_view_model = new SupplierViewModel();
            this.DataContext = _supplier_view_model;
        }
    }
}
