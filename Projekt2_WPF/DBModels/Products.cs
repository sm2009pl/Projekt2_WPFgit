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
    public class Products : IModel
    {
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public bool Discontinued { get; set; }
        [Browsable(false)]
        public virtual Categories Categories { get; set; }
        [Browsable(false)]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        [Browsable(false)]
        public virtual Suppliers Suppliers { get; set; }
        public void Add()
        {
            using (var db = new Context())
            {
                db.Products.AddOrUpdate(this);
                db.SaveChanges();
            }
        }
        public void Load(DataGrid dataGrid)
        {
            using (var db = new Context())
            {
                var query = db.Products.OrderBy(x => x.ProductID);
                dataGrid.ItemsSource = query.ToList();
                dataGrid.Columns[8].Visibility = Visibility.Collapsed;
                dataGrid.Columns[9].Visibility = Visibility.Collapsed;
                dataGrid.Columns[10].Visibility = Visibility.Collapsed;
            }
        }
        public void OpenForm()
        {
            ProductsAdder form = new ProductsAdder();
            form.ShowDialog();
        }

        public void OpenFormToEdit(DataGrid dataGrid)
        {
            Products prod = (Products)dataGrid.SelectedItems[0];
            ProductsAdder form = new ProductsAdder();
            form.prod = prod;
            form.productName.Text = prod.ProductName;
            if (prod.SupplierID.HasValue)
            {
                form.supplier.SelectedIndex = form.foreignSuplID.IndexOf((int)prod.SupplierID);
            }
            if (prod.CategoryID.HasValue)
            {
                form.category.SelectedIndex = form.foreignCatID.IndexOf((int)prod.CategoryID);
            }
            form.unitPrice.Text = prod.UnitPrice.ToString();
            form.unitsInStock.Text = prod.UnitsInStock.ToString();
            form.quantityPerUnit.Text = prod.QuantityPerUnit;
            form.disconinued.IsChecked = prod.Discontinued;
            form.ShowDialog();
        }

        public void Delete(DataGrid dataGrid)
        {
            List<Products> products = new List<Products>();
            foreach (var item in dataGrid.SelectedItems)
            {
                products.Add((Products)item);
            }
            using (var db = new Context())
            {
                foreach (var item in products)
                {
                    var query = db.OrderDetails.Where(x => x.ProductID == item.ProductID);
                    foreach (var item2 in query)
                    {
                        db.OrderDetails.Remove(item2);
                    }
                    Products delete = db.Products.First(x => x.ProductID == item.ProductID);
                    db.Products.Remove(delete);
                }
                db.SaveChanges();
            }
        }
    }
}
