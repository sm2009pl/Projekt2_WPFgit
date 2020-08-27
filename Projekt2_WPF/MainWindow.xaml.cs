using Projekt2_WPF.DBModels;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt2_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, IModel> tableList = new Dictionary<string, IModel>();
        IModel usedModel;
        public MainWindow()
        {
            InitializeComponent();
            using (var db = new Context())
            {
                tableList.Add("Categories", new Categories());
                tableList.Add("Customers", new Customers());
                tableList.Add("Employees", new Employees());
                tableList.Add("Order Details", new OrderDetails());
                tableList.Add("Orders", new Orders());
                tableList.Add("Products", new Products());
                tableList.Add("Shippers", new Shippers());
                tableList.Add("Suppliers", new Suppliers());
                foreach (var item in tableList)
                {
                    comboBox.Items.Add(item.Key);
                }
                comboBox.SelectedIndex = 0;
                usedModel = tableList[comboBox.Text];
                var query = db.Categories.OrderBy(x => x.CategoryID);
                dataGrid.ItemsSource = query.ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            usedModel = tableList[comboBox.Text];
            usedModel.Load(dataGrid);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            usedModel.OpenForm();
            usedModel.Load(dataGrid);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("Nie zaznaczono żadnego elementu.");
                return;
            }
            usedModel.Delete(dataGrid);
            usedModel.Load(dataGrid);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("Nie zaznaczono żadnego elementu.");
                return;
            }
            usedModel.OpenFormToEdit(dataGrid);
            usedModel.Load(dataGrid);
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            PieChartByCategories chart = new PieChartByCategories();
            chart.ShowDialog();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            BarChartByMonths chart = new BarChartByMonths();
            chart.ShowDialog();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            OrdersByCustomers raport = new OrdersByCustomers();
            raport.ShowDialog();
        }
    }
}
