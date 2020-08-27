using Projekt2_WPF.AddingForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    public class Customers : IModel
    {
        public Customers()
        {
            Orders = new HashSet<Orders>();
        }

        [Key]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(50)]
        public string CompanyName { get; set; }

        [StringLength(40)]
        public string ContactName { get; set; }

        [StringLength(30)]
        public string ContactTitle { get; set; }

        [StringLength(60)]
        public string Address { get; set; }

        [StringLength(15)]
        public string City { get; set; }

        [StringLength(15)]
        public string Region { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(15)]
        public string Country { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }

        [StringLength(24)]
        public string Fax { get; set; }
        [Browsable(false)]
        public virtual ICollection<Orders> Orders { get; set; }
        public void Add()
        {
            using (var db = new Context())
            {
                db.Customers.AddOrUpdate(this);
                db.SaveChanges();
            }
        }
        public void Load(DataGrid dataGrid)
        {
            using (var db = new Context())
            {
                var query = db.Customers.OrderBy(x => x.CustomerID);
                dataGrid.ItemsSource = query.ToList();
                dataGrid.Columns[11].Visibility = Visibility.Collapsed;
            }
        }
        public void OpenForm()
        {
            CustomersAdder form = new CustomersAdder();
            form.ShowDialog();
        }

        public void OpenFormToEdit(DataGrid dataGrid)
        {
            Customers cust = (Customers)dataGrid.SelectedItems[0];
            CustomersAdder form = new CustomersAdder();
            form.cust = cust;
            form.companyName.Text = cust.CompanyName;
            form.contactName.Text = cust.ContactName;
            form.contactTitle.Text = cust.ContactTitle;
            form.address.Text = cust.Address;
            form.country.Text = cust.Country;
            form.postalCode.Text = cust.PostalCode;
            form.city.Text = cust.City;
            form.phone.Text = cust.Phone;
            form.ShowDialog();
        }

        public void Delete(DataGrid dataGrid)
        {
            List<Customers> customers = new List<Customers>();
            foreach (var item in dataGrid.SelectedItems)
            {
                customers.Add((Customers)item);
            }
            using (var db = new Context())
            {
                foreach (var item in customers)
                {
                    var query = db.Orders.Where(x => x.CustomerID == item.CustomerID);
                    foreach (var item2 in query)
                    {
                        db.Orders.Remove(item2);
                    }
                    Customers delete = db.Customers.First(x => x.CustomerID == item.CustomerID);
                    db.Customers.Remove(delete);
                }
                db.SaveChanges();
            }
        }
    }
}
