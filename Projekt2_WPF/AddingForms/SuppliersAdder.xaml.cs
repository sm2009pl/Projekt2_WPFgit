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
    /// Interaction logic for ShippersAdder.xaml
    /// </summary>
    public partial class SuppliersAdder : Window
    {
        public Suppliers supp = new Suppliers();
        public SuppliersAdder()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            supp.CompanyName = companyName.Text.ToString();
            supp.Address = address.Text.ToString();
            supp.City = city.Text.ToString();
            supp.Email = email.Text.ToString();
            supp.PostalCode = postalCode.Text.ToString();
            supp.Country = country.Text.ToString();
            supp.Phone = phone.Text.ToString();
            supp.HomePage = homePage.Text.ToString();


            if (supp.CompanyName.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Nazwa firmy jest pusta.");
                return;
            }
            if (supp.Address.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Adres jest pusty.");
                return;
            }
            if (supp.City.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Miasto jest puste.");
                return;
            }
            if(!IsValidEmail(supp.Email))
            {
                System.Windows.MessageBox.Show("Mail jest niepoprawny");
                return;
            }
            supp.Add();
            this.Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }



        public static bool IsValidEmail(string email)
        {
            if (!email.IsNullOrEmpty())
            {
                Regex rx = new Regex(
                @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
                return rx.IsMatch(email);
            }
            else
                return true;
        }
    }
}
