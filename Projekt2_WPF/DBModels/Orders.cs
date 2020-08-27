using Castle.MicroKernel.SubSystems.Conversion;
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
    public class Orders : IModel
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        [Key]
        public int OrderID { get; set; }

        public int CustomerID { get; set; }

        public int? EmployeeID { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? Freight { get; set; }

        [StringLength(40)]
        public string ShipName { get; set; }

        [StringLength(60)]
        public string ShipAddress { get; set; }

        [StringLength(15)]
        public string ShipCity { get; set; }

        [StringLength(10)]
        public string ShipPostalCode { get; set; }

        [StringLength(15)]
        public string ShipCountry { get; set; }
        [Browsable(false)]
        public virtual Customers Customers { get; set; }
        [Browsable(false)]
        public virtual Employees Employees { get; set; }
        [Browsable(false)]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        [Browsable(false)]
        public virtual Shippers Shippers { get; set; }
        public void Add()
        {
            using (var db = new Context())
            {
                db.Orders.AddOrUpdate(this);
                db.SaveChanges();
            }
        }
        public void Load(DataGrid dataGrid)
        {
            using (var db = new Context())
            {
                var query = db.Orders.OrderBy(x => x.OrderID);
                dataGrid.ItemsSource = query.ToList();
                dataGrid.Columns[12].Visibility = Visibility.Collapsed;
                dataGrid.Columns[13].Visibility = Visibility.Collapsed;
                dataGrid.Columns[14].Visibility = Visibility.Collapsed;
                dataGrid.Columns[15].Visibility = Visibility.Collapsed;
            }
        }
        public void OpenForm()
        {
            OrdersAdder form = new OrdersAdder();
            form.ShowDialog();
        }

        public void OpenFormToEdit(DataGrid dataGrid)
        {

            Orders ord = (Orders)dataGrid.SelectedItems[0];
            OrdersAdder form = new OrdersAdder();
            form.ord = ord;
            form.customer.SelectedIndex = form.foreignCustID.IndexOf(ord.CustomerID);
            if (ord.EmployeeID.HasValue)
            {
                form.employee.SelectedIndex = form.foreignEmplID.IndexOf((int)ord.EmployeeID);
            }
            form.orderDate.SelectedDate = ord.OrderDate;
            form.requiredDate.SelectedDate = ord.RequiredDate;
            form.shippedDate.SelectedDate = ord.ShippedDate;
            form.freight.Text = ord.Freight.ToString();
            form.shippedTo.Text = ord.ShipName;
            form.shipCity.Text = ord.ShipCity;
            form.shipCountry.Text = ord.ShipCountry;
            form.shipAddress.Text = ord.ShipAddress;
            form.shipPostalCode.Text = ord.ShipPostalCode;
            form.ShowDialog();
        }

        public void Delete(DataGrid dataGrid)
        {
            List<Orders> orders = new List<Orders>();
            foreach (var item in dataGrid.SelectedItems)
            {
                orders.Add((Orders)item);
            }
            using (var db = new Context())
            {
                foreach (var item in orders)
                {
                    var query = db.OrderDetails.Where(x => x.OrderID == item.OrderID);
                    foreach (var item2 in query)
                    {
                        db.OrderDetails.Remove(item2);
                    }
                    Orders delete = db.Orders.First(x => x.OrderID == item.OrderID);
                    db.Orders.Remove(delete);
                }
                db.SaveChanges();
            }
        }
    }
}
