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
    /// Interaction logic for SuppliersAdder.xaml
    /// </summary>
    public partial class ShippersAdder : Window
    {
        public Shippers ship = new Shippers();
        public ShippersAdder()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ship.CompanyName = companyName.Text.ToString();
            ship.Phone = phone.Text.ToString();
            
            if (ship.CompanyName.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Nazwa firmy jest pusta.");
                return;
            }

            ship.Add();
            this.Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
