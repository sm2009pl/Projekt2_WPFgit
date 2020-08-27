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
    public class Suppliers : IModel
    {
        public Suppliers()
        {
            Products = new HashSet<Products>();
        }

        [Key]
        public int SupplierID { get; set; }

        [Required]
        [StringLength(40)]
        public string CompanyName { get; set; }

        [StringLength(60)]
        public string Address { get; set; }

        [StringLength(15)]
        public string City { get; set; }

        [StringLength(60)]
        public string Email { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(15)]
        public string Country { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }


        [Column(TypeName = "ntext")]
        public string HomePage { get; set; }
        [Browsable(false)]
        public virtual ICollection<Products> Products { get; set; }

        public void Add()
        {
            using (var db = new Context())
            {
                db.Suppliers.AddOrUpdate(this);
                db.SaveChanges();
            }
        }
        public void Load(DataGrid dataGrid)
        {
            using (var db = new Context())
            {
                var query = db.Suppliers.OrderBy(x => x.SupplierID);
                dataGrid.ItemsSource = query.ToList();
                dataGrid.Columns[9].Visibility = Visibility.Collapsed;
            }
        }
        public void OpenForm()
        {
            SuppliersAdder form = new SuppliersAdder();
            form.ShowDialog();
        }

        public void OpenFormToEdit(DataGrid dataGrid)
        {
            Suppliers supp = (Suppliers)dataGrid.SelectedItems[0];
            SuppliersAdder form = new SuppliersAdder();
            form.supp = supp;
            form.companyName.Text = supp.CompanyName.ToString();
            form.address.Text = supp.Address.ToString();
            form.city.Text = supp.City;
            form.email.Text = supp.Email.ToString();
            form.postalCode.Text = supp.PostalCode.ToString();
            form.country.Text = supp.Country;
            form.phone.Text = supp.Phone.ToString();
            form.homePage.Text = supp.HomePage.ToString();
            form.ShowDialog();
        }

        public void Delete(DataGrid dataGrid)
        {
            List<Suppliers> suppliers = new List<Suppliers>();
            foreach (var item in dataGrid.SelectedItems)
            {
                suppliers.Add((Suppliers)item);
            }
            using (var db = new Context())
            {
                foreach (var item in suppliers)
                {
                    var query = db.Products.Where(x => x.SupplierID == item.SupplierID);
                    foreach (var item2 in query)
                    {
                        db.Products.Remove(item2);
                    }
                    Suppliers delete = db.Suppliers.First(x => x.SupplierID == item.SupplierID);
                    db.Suppliers.Remove(delete);
                }
                db.SaveChanges();
            }
        }
    }
}
