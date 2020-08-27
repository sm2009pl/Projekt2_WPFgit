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
    public class OrderDetails : IModel
    {
        [Key]
        [Column(Order = 0)]
        public int OrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        public int ProductID { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }
        [Browsable(false)]
        public virtual Orders Orders { get; set; }
        [Browsable(false)]
        public virtual Products Products { get; set; }

        public void Add()
        {
            using (var db = new Context())
            {
                db.OrderDetails.AddOrUpdate(this);
                db.SaveChanges();
            }
        }
        public void Load(DataGrid dataGrid)
        {
            using (var db = new Context())
            {
                var query = db.OrderDetails.OrderBy(x => x.OrderID);
                dataGrid.ItemsSource = query.ToList();
                dataGrid.Columns[5].Visibility = Visibility.Collapsed;
                dataGrid.Columns[6].Visibility = Visibility.Collapsed;
            }
        }
        public void OpenForm()
        {
            OrderDetailsAdder form = new OrderDetailsAdder();
            form.ShowDialog();
        }

        public void OpenFormToEdit(DataGrid dataGrid)
        {
            OrderDetails ordDet = (OrderDetails)dataGrid.SelectedItems[0];
            OrderDetailsAdder form = new OrderDetailsAdder();
            form.order.IsEnabled = false;
            form.product.IsEnabled = false;
            form.ordDet = ordDet;
            form.order.SelectedIndex = form.foreignOrdID.IndexOf(ordDet.OrderID);
            form.product.SelectedIndex = form.foreignProdID.IndexOf(ordDet.ProductID);
            form.unitPrice.Text = ordDet.UnitPrice.ToString();
            form.quantity.Text = ordDet.Quantity.ToString();
            form.discount.Text = ordDet.Discount.ToString();
            form.ShowDialog();
        }

        public void Delete(DataGrid dataGrid)
        {
            using (var db = new Context())
            {
                List<OrderDetails> orderDetails = new List<OrderDetails>();
                foreach (var item in dataGrid.SelectedItems)
                {
                    orderDetails.Add((OrderDetails)item);
                }

                foreach (var item in orderDetails)
                {
                    OrderDetails delete = db.OrderDetails.First(x => x.OrderID == item.OrderID && x.ProductID == item.ProductID);
                    db.OrderDetails.Remove(delete);
                }
                db.SaveChanges();
            }
        }
    }
}
