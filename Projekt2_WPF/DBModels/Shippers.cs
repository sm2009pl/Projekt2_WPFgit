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
    public class Shippers : IModel
    {
        public Shippers()
        {
            Orders = new HashSet<Orders>();
        }

        [Key]
        public int ShipperID { get; set; }

        [Required]
        [StringLength(40)]
        public string CompanyName { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }
        [Browsable(false)]
        public virtual ICollection<Orders> Orders { get; set; }

        public void Add()
        {
            using (var db = new Context())
            {
                db.Shippers.AddOrUpdate(this);
                db.SaveChanges();
            }
        }
        public void Load(DataGrid dataGrid)
        {
            using (var db = new Context())
            {
                var query = db.Shippers.OrderBy(x => x.ShipperID);
                dataGrid.ItemsSource = query.ToList();
                dataGrid.Columns[3].Visibility = Visibility.Collapsed;
            }
        }
        public void OpenForm()
        {
            ShippersAdder form = new ShippersAdder();
            form.ShowDialog();
        }

        public void OpenFormToEdit(DataGrid dataGrid)
        {
            Shippers ship = (Shippers)dataGrid.SelectedItems[0];
            ShippersAdder form = new ShippersAdder();
            form.ship = ship;
            form.companyName.Text = ship.CompanyName;
            form.phone.Text = ship.Phone;
            form.ShowDialog();
        }

        public void Delete(DataGrid dataGrid)
        {
            List<Shippers> shippers = new List<Shippers>();
            foreach (var item in dataGrid.SelectedItems)
            {
                shippers.Add((Shippers)item);
            }
            using (var db = new Context())
            {
                foreach (var item in shippers)
                {
                    Shippers delete = db.Shippers.First(x => x.ShipperID == item.ShipperID);
                    db.Shippers.Remove(delete);
                }
                db.SaveChanges();
            }
        }
    }
}
