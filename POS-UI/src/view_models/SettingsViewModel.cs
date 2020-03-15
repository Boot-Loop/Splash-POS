using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Windows.Forms;

using CoreApp = Core.Application;

using UI.ViewModels.Commands;

namespace UI.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _printer_names;
        private string _selected_printer;
        private string _document_save_path;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand BrowseCommand { get; private set; }
        public HomeViewModel HomeViewModel { get; set; }

        public ObservableCollection<string> PrinterNames {
            get { return _printer_names; }
            set { _printer_names = value; onPropertyRaised("PrinterNames"); }
        }
        public string SelectedPrinter {
            get { return _selected_printer; }
            set { _selected_printer = value; onPropertyRaised("SelectedPrinter"); }
        }
        public string DocumentSavePath {
            get { return _document_save_path; }
            set { _document_save_path = value; onPropertyRaised("DocumentSavePath"); CoreApp.singleton.updateDocumentSavePath(DocumentSavePath); }
        }

        public SettingsViewModel(HomeViewModel home_view_model) {
            this.HomeViewModel = home_view_model;
            this.SaveCommand = new RelayCommand(savePrinter);
            this.BrowseCommand = new RelayCommand(browseSavePath);
            DocumentSavePath = CoreApp.singleton.readDocumentSavePath();
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
                    this.HomeViewModel.setNotification("Printer Successfully Configured!", true);
                }
                catch (Exception) {
                    this.HomeViewModel.setNotification("Printer Configuration Failed!", false);
                }
            }
        }
        private void browseSavePath(object parameter) {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK) DocumentSavePath = folderBrowserDialog.SelectedPath;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
