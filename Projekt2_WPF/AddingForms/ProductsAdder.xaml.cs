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
    /// Interaction logic for ProductsAdder.xaml
    /// </summary>
    public partial class ProductsAdder : Window
    {
        public Products prod = new Products();
        public List<int> foreignSuplID = new List<int>();
        public List<int> foreignCatID = new List<int>();
        public ProductsAdder()
        {
            InitializeComponent();
            using (var db = new Context())
            {
                db.Categories.Load();
                var listOfEmployees = db.Categories.OrderBy(r => r.CategoryID);
                foreach (var item in listOfEmployees)
                { 
                    category.Items.Add($"{item.CategoryID}- {item.CategoryName}");
                    foreignCatID.Add(item.CategoryID);
                }
        }

            using (var db = new Context())
            {
                db.Suppliers.Load();
                var listOfEmployees = db.Suppliers.OrderBy(r => r.SupplierID);
                foreach (var item in listOfEmployees)
                {
                    supplier.Items.Add($"{item.SupplierID}- {item.CompanyName}");
                    foreignSuplID.Add(item.SupplierID);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            int x;
            decimal y;
            short z;
            string str;
            bool success;
            prod.ProductName = productName.Text.ToString();
            prod.QuantityPerUnit = quantityPerUnit.Text.ToString();
            if (disconinued.IsChecked == true)
                prod.Discontinued = true;
            else
                prod.Discontinued = false;

            str = supplier.Text.ToString().Split('-')[0];
            success = Int32.TryParse(str, out x);
            if (success)
                prod.SupplierID = x;
            else
            {
                System.Windows.MessageBox.Show("Dostawca jest podany niepoprawnie.");
                return;
            }

            str = category.Text.ToString().Split('-')[0];
            success = Int32.TryParse(str, out x);
            if (success)
                prod.CategoryID = x;
            else
            {
                System.Windows.MessageBox.Show("Kategoria jest podana niepoprawnie.");
                return;
            }

            str = unitPrice.Text.ToString().Split('-')[0];
            success = decimal.TryParse(str, out y);
            if (success)
                prod.UnitPrice = y;
            else
            {
                System.Windows.MessageBox.Show("Cena jednostkowa jest podana niepoprawnie.");
                return;
            }

            str = unitsInStock.Text.ToString().Split('-')[0];
            success = short.TryParse(str, out z);
            if (success)
                prod.UnitsInStock = z;
            else
            {
                System.Windows.MessageBox.Show("Jednostki na stanie są podane niepoprawnie.");
                return;
            }

            prod.Add();
            this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
