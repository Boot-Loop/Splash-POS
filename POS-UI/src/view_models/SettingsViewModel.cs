using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Threading;

using UI.ViewModels.Commands;

using CoreApp = Core.Application;

namespace UI.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _printer_names;
        private string _selected_printer;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand SaveCommand { get; private set; }
        public HomeViewModel HomeViewModel { get; set; }

        public ObservableCollection<string> PrinterNames {
            get { return _printer_names; }
            set { _printer_names = value; onPropertyRaised("PrinterNames"); }
        }
        public string SelectedPrinter {
            get { return _selected_printer; }
            set { _selected_printer = value; onPropertyRaised("SelectedPrinter"); }
        }

        public SettingsViewModel(HomeViewModel home_view_model)
        {
            this.HomeViewModel = home_view_model;
            this.SaveCommand = new RelayCommand(savePrinter);
            home_view_model.Title = "Settings";
            loadPrinters();
            this.SelectedPrinter = loadPrinter();
        }

        public void loadPrinters() {
            ObservableCollection<string> printer_names = new ObservableCollection<string>();
            foreach (string printer in PrinterSettings.InstalledPrinters) {
                printer_names.Add(printer);
            }
            this.PrinterNames = printer_names;
        }

        private string loadPrinter() {
            try { return CoreApp.singleton.readReciptPrinterName(); }
            catch (Exception) { return null; }
        }

        private void savePrinter(object parameter) {
            if (SelectedPrinter != null) {
                try {
                    CoreApp.singleton.updateReciptPrinterName(this.SelectedPrinter);
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Printer Successfully Configured!"));
                    thread.Start();
                }
                catch (Exception) {
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Printer Configuration Failed!"));
                    thread.Start();
                }
            }
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
