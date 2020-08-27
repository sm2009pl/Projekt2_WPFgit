using Projekt2_WPF.AddingForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Projekt2_WPF.DBModels
{
    public class Employees : IModel
    {
        public Employees()
        {
            Employees1 = new HashSet<Employees>();
            Orders = new HashSet<Orders>();
        }

        [Key]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string Title { get; set; }


        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }

        [StringLength(60)]
        public string Address { get; set; }

        [StringLength(15)]
        public string City { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(15)]
        public string Country { get; set; }

        [StringLength(24)]
        public string HomePhone { get; set; }


        [Column(TypeName = "ntext")]
        public string Notes { get; set; }

        public int? ReportsTo { get; set; }
        [Browsable(false)]
        public virtual ICollection<Employees> Employees1 { get; set; }
        [Browsable(false)]
        public virtual Employees Employees2 { get; set; }
        [Browsable(false)]
        public virtual ICollection<Orders> Orders { get; set; }

        public void Add()
        {
            using (var db = new Context())
            {
                db.Employees.AddOrUpdate(this);
                db.SaveChanges();
            }
        }
        public void Load(DataGrid dataGrid)
        {
            using (var db = new Context())
            {
                var query = db.Employees.OrderBy(x => x.EmployeeID);
                dataGrid.ItemsSource = query.ToList();
                dataGrid.Columns[13].Visibility = Visibility.Collapsed;
                dataGrid.Columns[14].Visibility = Visibility.Collapsed;
                dataGrid.Columns[15].Visibility = Visibility.Collapsed;

            }
        }
        public void OpenForm()
        {
            EmployeesAdder form = new EmployeesAdder();
            form.ShowDialog();
        }

        public void OpenFormToEdit(DataGrid dataGrid)
        {
            Employees empl = (Employees)dataGrid.SelectedItems[0];
            EmployeesAdder form = new EmployeesAdder();
            form.empl = empl;
            form.firstName.Text = empl.FirstName;
            form.lastName.Text = empl.LastName;
            form.title.Text = empl.Title;
            form.hireDate.SelectedDate = empl.HireDate;
            form.birthDate.SelectedDate = empl.BirthDate;
            form.address.Text = empl.Address;
            form.country.Text = empl.Country;
            form.postalCode.Text = empl.PostalCode;
            form.city.Text = empl.City;
            form.homePhone.Text = empl.HomePhone;
            form.notes.Text = empl.Notes;
            if(empl.ReportsTo.HasValue)
            {
                form.reportsTo.SelectedIndex = form.foreignEmplID.IndexOf((int)empl.ReportsTo);
            }
            form.ShowDialog();
        }


        public void Delete(DataGrid dataGrid)
        {
            List<Employees> employees = new List<Employees>();
            foreach (var item in dataGrid.SelectedItems)
            {
                employees.Add((Employees)item);
            }
            using (var db = new Context())
            {
                foreach (var item in employees)
                {
                    var query = db.Orders.Where(x => x.EmployeeID == item.EmployeeID);
                    foreach (var item2 in query)    
                    {
                        db.Orders.Remove(item2);
                    }
                    var query2 = db.Employees.Where(x => x.ReportsTo == item.EmployeeID);
                    foreach (var item2 in query2)
                    {
                        item2.ReportsTo = null;
                        db.Employees.AddOrUpdate(item2);
                    }
                    Employees delete = db.Employees.First(x => x.EmployeeID == item.EmployeeID);
                    db.Employees.Remove(delete);
                }
                db.SaveChanges();
            }
        }
    }
}
