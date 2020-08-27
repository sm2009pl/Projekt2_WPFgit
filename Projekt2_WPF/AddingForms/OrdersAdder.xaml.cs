using Projekt2_WPF.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for OrdersAdder.xaml
    /// </summary>
    public partial class OrdersAdder : Window
    {
        public Orders ord = new Orders();
        public List<int> foreignCustID = new List<int>();
        public List<int> foreignEmplID = new List<int>();
        public OrdersAdder()
        {
            InitializeComponent();
            using (var db = new Context())
            {
                db.Customers.Load();
                var listOfEmployees = db.Customers.OrderBy(r => r.CustomerID);
                foreach (var item in listOfEmployees)
                {
                    customer.Items.Add($"{item.CustomerID}- {item.CompanyName}, {item.City}");
                    foreignCustID.Add(item.CustomerID);
                }

            }

            using (var db = new Context())
            {
                db.Employees.Load();
                var listOfEmployees = db.Employees.OrderBy(r => r.EmployeeID);
                foreach (var item in listOfEmployees)
                { 
                    employee.Items.Add($"{item.EmployeeID}- {item.FirstName}, {item.LastName}, {item.BirthDate}");
                    foreignEmplID.Add(item.EmployeeID);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int x;
            string str;
            bool success;
            if (!orderDate.SelectedDate.HasValue || !requiredDate.SelectedDate.HasValue)
            {
                System.Windows.MessageBox.Show("Któraś z dat nie jest podana.");
                return;
            }
            if(!shippedDate.SelectedDate.HasValue)
            {
                ord.ShippedDate = null;
            }
            ord.OrderDate = (DateTime)orderDate.SelectedDate;
            ord.RequiredDate = (DateTime)requiredDate.SelectedDate;
            ord.ShipName = shippedTo.Text.ToString();
            ord.ShipAddress = shipAddress.Text.ToString();
            ord.ShipCity = shipCity.Text.ToString();
            ord.ShipPostalCode = shipPostalCode.Text.ToString();
            ord.ShipCountry = shipCountry.Text.ToString();


            success = Int32.TryParse(freight.Text.ToString(), out x);
            if (success)
                ord.Freight = x;
            else
            {
                ord.Freight = null;
            }

            str = customer.Text.ToString().Split('-')[0];
            success = Int32.TryParse(str, out x);
            if (success)
                ord.CustomerID = x;
            else
            {
                System.Windows.MessageBox.Show("Klient jest podany niepoprawnie.");
                return;
            }
            str = employee.Text.ToString().Split('-')[0];
            success = Int32.TryParse(str, out x);
            if (success)
                ord.EmployeeID = x;
            else
            {
                System.Windows.MessageBox.Show("Pracownik jest podany niepoprawnie.");
                return;
            }

            if (ord.OrderDate > DateTime.Now || ord.RequiredDate > DateTime.Now || ord.ShippedDate > DateTime.Now)
            {
                System.Windows.MessageBox.Show("Któraś z dat jest późniejsza od daty dzisiejszej.");
                return;
            }

            if (ord.OrderDate > ord.RequiredDate || ord.RequiredDate > ord.ShippedDate || ord.OrderDate > ord.ShippedDate)
            {
                System.Windows.MessageBox.Show("Któraś z dat jest późniejsza od dat pozostałych.");
                return;
            }
            ord.Add();
            this.Close();
        }

        private void NumberValidationTextBoxDecimal(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }
    }
}
