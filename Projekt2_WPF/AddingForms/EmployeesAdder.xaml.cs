using Castle.Core.Internal;
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
    /// Interaction logic for EmployeesAdder.xaml
    /// </summary>
    public partial class EmployeesAdder : Window
    {
        public Employees empl = new Employees();
        public List<int> foreignEmplID = new List<int>();
        public EmployeesAdder()
        {
            InitializeComponent();
            using (var db = new Context())
            {
                db.Employees.Load();
                var listOfEmployees = db.Employees.OrderBy(r => r.EmployeeID);
                foreach (var item in listOfEmployees)
                {
                    reportsTo.Items.Add($"{item.EmployeeID}- {item.FirstName}, {item.LastName}, {item.BirthDate}");
                    foreignEmplID.Add(item.EmployeeID);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int x;
            string str;
            bool success;
            if(!hireDate.SelectedDate.HasValue || !birthDate.SelectedDate.HasValue)
            {
                System.Windows.MessageBox.Show("Któraś z dat nie jest podana.");
                return;
            }
            empl.LastName = lastName.Text.ToString();
            empl.FirstName = firstName.Text.ToString();
            empl.Title = title.Text.ToString();
            empl.BirthDate = (DateTime)birthDate.SelectedDate;
            empl.HireDate = (DateTime)hireDate.SelectedDate;
            empl.Address = address.Text.ToString();
            empl.City = city.Text.ToString();
            empl.PostalCode = postalCode.Text.ToString();
            empl.Country = country.Text.ToString();
            empl.HomePhone = homePhone.Text.ToString();
            empl.Notes = notes.Text.ToString();
            str = reportsTo.Text.ToString().Split('-')[0];
            success = Int32.TryParse(str, out x);
            if (success)
                empl.ReportsTo = x;
            
            if (empl.LastName.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Nazwisko jest puste.");
                return;
            }
            if (empl.FirstName.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Imię jest puste.");
                return;
            }
            if (empl.Address.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Adres jest pusty.");
                return;
            }
            if (empl.City.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("Nazwa miasta jest pusta.");
                return;
            }
            if (empl.HireDate.Year <= 1900)
            {
                System.Windows.MessageBox.Show("Zbyt stara data.");
                return;
            }
            if (empl.HireDate.Year - empl.BirthDate.Year < 16)
            {
                System.Windows.MessageBox.Show("Zbyt niski wiek pracownika.");
                return;
            }
            if (empl.HireDate > DateTime.Now || empl.BirthDate > DateTime.Now)
            {
                System.Windows.MessageBox.Show("Któraś z dat jest późniejsza od daty dzisiejszej.");
                return;
            }
            empl.Add();
            this.Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
