using Projekt2_WPF.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekt2_WPF
{
    /// <summary>
    /// Interaction logic for OrdersByCustomers.xaml
    /// </summary>
    public partial class OrdersByCustomers : Window
    {
        public OrdersByCustomers()
        {
            InitializeComponent();
            using (var db = new Context())
            {
                var query = db.Customers.OrderBy(x => x.CustomerID);
                foreach (var item in query)
                    comboBoxCustomers.Items.Add($"{item.CustomerID}- {item.CompanyName}, {item.Address}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int x;
            int custID;
            decimal sum = 0;
            Dictionary<int, string> listOfNames = new Dictionary<int, string>();
            Dictionary<int, decimal> listOfSumOfPrices = new Dictionary<int, decimal>();
            string str = comboBoxCustomers.Text.ToString().Split('-')[0];
            bool success = Int32.TryParse(str, out x);
            if (success)
                custID = x;
            else
                custID = 1;
            using (var db = new Context())
            {
                var query = from cust in db.Customers
                            join ord in db.Orders on cust.CustomerID equals ord.CustomerID
                            join ordDet in db.OrderDetails on ord.OrderID equals ordDet.OrderID
                            join prod in db.Products on ordDet.ProductID equals prod.ProductID
                            join cat in db.Categories on prod.CategoryID equals cat.CategoryID
                            where cust.CustomerID == custID
                            orderby cat.CategoryID
                            select new { cust.CustomerID, prod.ProductID, cat.CategoryID, cat.CategoryName, prod.ProductName, prod.QuantityPerUnit, ordDet.Quantity, ordDet.UnitPrice };
                foreach(var item in query)
                {
                    if (!listOfNames.ContainsKey(item.ProductID))
                    {
                        listOfSumOfPrices.Add(item.ProductID, 0);
                        listOfNames.Add(item.ProductID, $"{item.ProductName} - {item.QuantityPerUnit} - {item.Quantity} - {item.CategoryName}");
                    }
                    listOfSumOfPrices[item.ProductID] += item.Quantity * item.UnitPrice;
                    sum += item.Quantity * item.UnitPrice;
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Nazwa produktu - Liczba na jednostkę - Liczba jednostek zakupiona - Nazwa kategorii : Kwota zakupu");
            sb.AppendLine();
            foreach(var item in listOfNames)
            {
                sb.Append(item.Value);
                sb.Append(" : ");
                sb.Append(listOfSumOfPrices[item.Key]);
                sb.AppendLine();

            }
            blockText.Text = sb.ToString();
            blockEndText.Text = $"Suma kupionych towarów wynosi: {sum}.";
        }
    }
}
