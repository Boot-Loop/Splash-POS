using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Windows.Forms;

using CoreApp = Core.Application;

using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _printer_names;
        private string _selected_printer;
        private string _selected_doc_printer;
        private string _document_save_path;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand BrowseCommand { get; private set; }
        public HomeViewModel HomeViewModel { get; set; }
        public SettingsView SettingsView { get; set; }

        public ObservableCollection<string> PrinterNames {
            get { return _printer_names; }
            set { _printer_names = value; onPropertyRaised("PrinterNames"); }
        }
        public string SelectedPrinter {
            get { return _selected_printer; }
            set { _selected_printer = value; onPropertyRaised("SelectedPrinter"); }
        }
        public string SelectedDocPrinter {
            get { return _selected_doc_printer; }
            set { _selected_doc_printer = value; onPropertyRaised("SelectedDocPrinter"); }
        }
        public string DocumentSavePath {
            get { return _document_save_path; }
            set { _document_save_path = value; onPropertyRaised("DocumentSavePath"); CoreApp.singleton.updateDocumentSavePath(DocumentSavePath); }
        }

        public SettingsViewModel(HomeViewModel home_view_model, SettingsView settings_view) {
            this.HomeViewModel = home_view_model;
            this.SettingsView = settings_view;
            this.SaveCommand = new RelayCommand(savePrinter);
            this.BrowseCommand = new RelayCommand(browseSavePath);
            DocumentSavePath = CoreApp.singleton.readDocumentSavePath();
            home_view_model.Title = "Settings";
            loadPrinters();
            setPrinters();
        }

        public void loadPrinters() {
            ObservableCollection<string> printer_names = new ObservableCollection<string>();
            foreach (string printer in PrinterSettings.InstalledPrinters) {
                printer_names.Add(printer);
            }
            this.PrinterNames = printer_names;
        }

        private void setPrinters() {
            try { this.SelectedPrinter = CoreApp.singleton.readReciptPrinterName(); }
            catch (Exception) { this.SelectedPrinter = null; }
            try { this.SelectedDocPrinter = CoreApp.singleton.readDocumentPrinterName(); }
            catch (Exception) { this.SelectedDocPrinter = null; }
        }

        private void savePrinter(object parameter) {
            if (SettingsView.setting_tab_control.SelectedIndex == 0) { //Recipt Printer
                if (SelectedPrinter != null) {
                    try {
                        CoreApp.singleton.updateReciptPrinterName(this.SelectedPrinter);
                        this.HomeViewModel.setNotification("Recipt printer successfully configured!", true);
                    }
                    catch (Exception) {
                        this.HomeViewModel.setNotification("Recipt printer configuration Failed!", false);
                    }
                }
            }
            else if (SettingsView.setting_tab_control.SelectedIndex == 1) { //Document Printer
                if (SelectedDocPrinter != null) {
                    try {
                        CoreApp.singleton.updateDocumentPrinterName(this.SelectedDocPrinter);
                        this.HomeViewModel.setNotification("Document printer successfully configured!", true);
                    }
                    catch (Exception) {
                        this.HomeViewModel.setNotification("Document printer configuration Failed!", false);
                    }
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
