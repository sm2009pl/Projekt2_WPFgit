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
    /// Interaction logic for OrderDetailsAdder.xaml
    /// </summary>
    public partial class OrderDetailsAdder : Window
    {
        public OrderDetails ordDet = new OrderDetails();
        public List<int> foreignOrdID = new List<int>();
        public List<int> foreignProdID = new List<int>();
        public OrderDetailsAdder()
        {
            InitializeComponent();
            using (var db = new Context())
            {
                db.Orders.Load();
                var listOfEmployees = db.Orders.OrderBy(r => r.OrderID);
                foreach (var item in listOfEmployees)
                {
                    order.Items.Add($"{item.OrderID}- {item.CustomerID}, {item.OrderDate}");
                    foreignOrdID.Add(item.OrderID);
                }
                    
            }

            using (var db = new Context())
            {
                db.Products.Load();
                var listOfEmployees = db.Products.OrderBy(r => r.ProductID);
                foreach (var item in listOfEmployees)
                {
                    product.Items.Add($"{item.ProductID}- {item.ProductName}, {item.QuantityPerUnit}, {item.UnitPrice}");
                    foreignProdID.Add(item.ProductID);
                }
                    
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            decimal x;
            short y;
            float z;
            int r;
            string str;
            bool success;
            success = decimal.TryParse(unitPrice.Text.ToString(), out x);
            if (success)
                ordDet.UnitPrice = x;
            else
            {
                System.Windows.MessageBox.Show("Niepoprawna cena jednostkowa");
                return;
            }

            success = short.TryParse(quantity.Text.ToString(), out y);
            if (success)
                ordDet.Quantity = y;
            else
            {
                System.Windows.MessageBox.Show("Niepoprawna ilość porduktów");
                return;
            }

            success = float.TryParse(discount.Text.ToString(), out z);
            if (success)
                ordDet.Discount = z;
            else
            {
                ordDet.Discount = 0;
            }

            str = order.Text.ToString().Split('-')[0];
            success = Int32.TryParse(str, out r);
            if (success)
                ordDet.OrderID = r;
            else
            {
                System.Windows.MessageBox.Show("Zamówienie jest podane niepoprawnie.");
                return;
            }

            str = product.Text.ToString().Split('-')[0];
            success = Int32.TryParse(str, out r);
            if (success)
                ordDet.ProductID = r;
            else
            {
                System.Windows.MessageBox.Show("Produkt jest podany niepoprawnie.");
                return;
            }
            using (var db = new Context())
            {
                foreach (var item in db.OrderDetails)
                {
                    if(ordDet.ProductID == item.ProductID && ordDet.OrderID == item.OrderID)
                    {
                        System.Windows.MessageBox.Show("Detal o takich numerach ID już istnieje.");
                        return;
                    }
                }
                    
            }
            ordDet.Add();
            this.Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^1-9]+[0-9]");
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
