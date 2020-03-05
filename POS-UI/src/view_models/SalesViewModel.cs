using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Views;

namespace UI.ViewModels
{
    public class SalesViewModel
    {
        public HomeViewModel HomeViewModel { get; set; }

        public SalesViewModel(HomeViewModel home_view_model) {
            this.HomeViewModel = home_view_model;
            home_view_model.Title = "Sales";
        }
    }
}
