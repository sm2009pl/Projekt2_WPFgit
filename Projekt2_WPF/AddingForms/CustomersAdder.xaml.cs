using Castle.Core.Internal;
using Projekt2_WPF.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekt2_WPF.AddingForms
{
    /// <summary>
    /// Interaction logic for CustomersAdder.xaml
    /// </summary>
    public partial class CustomersAdder : Window
    {
        public Customers cust = new Customers();
        public CustomersAdder()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cust.CompanyName = companyName.Text.ToString();
            cust.ContactName = contactName.Text.ToString();
            cust.ContactTitle = contactTitle.Text.ToString();
            cust.Address = address.Text.ToString();
            cust.City = city.Text.ToString();
            cust.PostalCode = postalCode.Text.ToString();
            cust.Country = country.Text.ToString();
            cust.Phone = phone.Text.ToString();
            if (cust.CompanyName.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Nazwa firmy jest pusta.");
                return;
            }
            if (cust.Address.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Adres jest pusty.");
                return;
            }
            if (cust.City.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Nazwa miasta jest pusta.");
                return;
            }
            cust.Add();
            this.Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
