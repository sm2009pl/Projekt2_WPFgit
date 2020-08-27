using Castle.Core.Internal;
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

namespace Projekt2_WPF.AddingForms
{
    /// <summary>
    /// Interaction logic for CategoriesAdder.xaml
    /// </summary>
    public partial class CategoriesAdder : Window
    {
        public Categories cat = new Categories();
        public CategoriesAdder()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            cat.CategoryName = categoryName.Text.ToString();
            cat.Description = categoryDescription.Text.ToString();
            if (cat.CategoryName.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Nazwa kategorii jest pusta.");
                return;
            }
            cat.Add();
            this.Close();
        }
    }


}
