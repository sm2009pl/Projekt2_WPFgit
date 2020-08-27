using LiveCharts;
using LiveCharts.Wpf;
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
    /// Interaction logic for BarChartByMonths.xaml
    /// </summary>
    public partial class BarChartByMonths : Window
    {
        public BarChartByMonths()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            chart.Series.Clear();
            int baseYear = 1940;
            int yr, mn;
            int x;
            DateTime year;
            int yearInput;
            bool success = Int32.TryParse(yearText.Text.ToString(), out x);
            if (success && x > 1940 && x < 2060)
                yearInput = x;
            else
            {
                System.Windows.MessageBox.Show("Rok podany niepoprawnie.");
                return;
            }

            decimal[][] months = new decimal[12][];
            for (int i = 0; i < 12; i++)
            {
                months[i] = new decimal[120];
            }
            using (var db = new Context())
            {
                var query = from ord in db.Orders
                            join ordDet in db.OrderDetails on ord.OrderID equals ordDet.OrderID
                            select new { ord.ShippedDate, ordDet.Quantity, ordDet.UnitPrice };
                foreach (var item in query)
                {
                    year = (DateTime)item.ShippedDate;
                    yr = year.Year - baseYear;
                    mn = year.Month;
                    months[mn-1][yr] += item.Quantity * item.UnitPrice;
                }
            }
            int iter = 1;
            foreach (var item in months)
            {
                chart.Series.Add(new ColumnSeries
                {
                    Title = iter.ToString(),
                    Values = new ChartValues<decimal> { item[yearInput - baseYear] }
                });
                iter++;
            }
        }
    }
}
