using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class ReportView : UserControl
    {
        ReportViewModel ReportViewModel { get; set; }

        public ReportView(HomeViewModel home_view_model) {
            InitializeComponent();
            ReportViewModel = new ReportViewModel(home_view_model, this);
            this.DataContext = ReportViewModel;
            startCalender.SelectedDate = DateTime.Today;
            endCalender.SelectedDate = DateTime.Today;
            ReportViewModel.DisplayDate = $"{startCalender.SelectedDate?.ToString("dd/MM/yyyy")} - {endCalender.SelectedDate?.ToString("dd/MM/yyyy")}";
        }

        private void btnClick(object sender, RoutedEventArgs e) {
            Button button = (Button)sender;
            string name = button.Name;
            if (name == "todayButton") {
                startCalender.SelectedDate = DateTime.Today;
                endCalender.SelectedDate = DateTime.Today;
                startCalender.DisplayDate = DateTime.Today;
                endCalender.DisplayDate = DateTime.Today;
            }
            else if (name == "yesterdayButton") {
                startCalender.SelectedDate = DateTime.Today.AddDays(-1);
                endCalender.SelectedDate = DateTime.Today.AddDays(-1);
                startCalender.DisplayDate = DateTime.Today.AddDays(-1);
                endCalender.DisplayDate = DateTime.Today.AddDays(-1);
            }
            else if (name == "thisWeekButton") {
                startCalender.SelectedDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
                endCalender.SelectedDate = DateTime.Today;
                startCalender.DisplayDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
                endCalender.DisplayDate = DateTime.Today;
            }
            else if (name == "lastWeekButton") {
                startCalender.SelectedDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 6);
                endCalender.SelectedDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                startCalender.DisplayDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 6);
                endCalender.DisplayDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            }
            else if (name == "thisMonthButton") {
                DateTime today = DateTime.Now;
                startCalender.SelectedDate = new DateTime(today.Year, today.Month, 1);
                endCalender.SelectedDate = DateTime.Today;
                startCalender.DisplayDate = new DateTime(today.Year, today.Month, 1);
                endCalender.DisplayDate = DateTime.Today;
            }
            else if (name == "lastMonthButton") {
                DateTime last_month = DateTime.Now.AddMonths(-1);
                startCalender.SelectedDate = new DateTime(last_month.Year, last_month.Month, 1);
                endCalender.SelectedDate = new DateTime(last_month.Year, last_month.Month, DateTime.DaysInMonth(last_month.Year, last_month.Month));
                startCalender.DisplayDate = new DateTime(last_month.Year, last_month.Month, 1);
                endCalender.DisplayDate = new DateTime(last_month.Year, last_month.Month, DateTime.DaysInMonth(last_month.Year, last_month.Month));
            }
            else if (name == "thisYearButton") {
                startCalender.SelectedDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfYear + 1);
                endCalender.SelectedDate = DateTime.Today;
                startCalender.DisplayDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfYear + 1);
                endCalender.DisplayDate = DateTime.Today;
            }
            else if (name == "lastYearButton") {
                int last_year = DateTime.Now.Year - 1;
                startCalender.SelectedDate = new DateTime(last_year, 1, 1);
                endCalender.SelectedDate = new DateTime(last_year, 12, 31);
                startCalender.DisplayDate = new DateTime(last_year, 1, 1);
                endCalender.DisplayDate = new DateTime(last_year, 12, 31);
            }
        }
        private void startCalenderDatesChanged(object sender, SelectionChangedEventArgs e) {
            ReportViewModel.DisplayDate = $"{startCalender.SelectedDate?.ToString("dd/MM/yyyy")} - {endCalender.SelectedDate?.ToString("dd/MM/yyyy")}";
        }
        private void endCalenderDatesChanged(object sender, SelectionChangedEventArgs e) {
            ReportViewModel.DisplayDate = $"{startCalender.SelectedDate?.ToString("dd/MM/yyyy")} - {endCalender.SelectedDate?.ToString("dd/MM/yyyy")}";
        }
    }
}
