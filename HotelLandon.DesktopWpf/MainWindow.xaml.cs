using HotelLandon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace HotelLandon.DesktopWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();
        public ObservableCollection<Customer> Customers
        {
            get
            {
                return _customers;
            }
            set
            {
                SetProperty(ref _customers, value);
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => Refresh();

        private void Refresh()
        {
            WebClient webClient = new WebClient
            {
                BaseAddress = "https://localhost:44339"
            };

            string json = webClient.DownloadString("Customers");
            var customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(json);
            Customers = new ObservableCollection<Customer>(customers);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool SetProperty<T>(ref T storage, T value, Action afterAction = null, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);
            afterAction?.Invoke();
            return true;
        }
    }
}
