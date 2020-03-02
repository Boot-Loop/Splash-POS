using POS_UI.src.views.customers_views;
using POS_UI.src.views.login_views;
using POS_UI.src.views.reports_views;
using POS_UI.src.views.sales;
using POS_UI.src.views.sales.new_sale_views;
using POS_UI.src.views.setting_views;
using POS_UI.src.views.suppliers_views;
using POS_UI.src.views.users_views;
using POS_UI.src.views.users_views.add_users_views;
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

namespace POS_UI.src.views.home_views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            main_content_control.Content = new Login();
        }
    }
}
