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
    /// Interaction logic for PieChartByCategories.xaml
    /// </summary>
    public partial class PieChartByCategories : Window
    {
        public PieChartByCategories()
        {
            InitializeComponent();
            Dictionary<int, decimal> listOfValues = new Dictionary<int, decimal>();
            Dictionary<int, string> listOfNames = new Dictionary<int, string>();
            using (var db = new Context())
            {
                var query = from cat in db.Categories
                            join prod in db.Products on cat.CategoryID equals prod.CategoryID
                            join ordDet in db.OrderDetails on prod.ProductID equals ordDet.ProductID
                            select new { cat.CategoryID, cat.CategoryName, ordDet.Quantity, ordDet.UnitPrice };
                foreach (var item in query)
                {
                    if (!listOfValues.ContainsKey(item.CategoryID))
                    {
                        listOfValues.Add(item.CategoryID, 0);
                        listOfNames.Add(item.CategoryID, item.CategoryName);
                    }
                    listOfValues[item.CategoryID] += item.Quantity * item.UnitPrice;
                }
            }
            foreach (var item in listOfNames)
            {
                chart.Series.Add(new PieSeries
                {
                    Title = item.Value,
                    Values = new ChartValues<decimal> { listOfValues[item.Key] }
                });
            }
        }
    }
}
