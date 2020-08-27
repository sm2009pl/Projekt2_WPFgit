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
    public class Categories : IModel
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [Browsable(false)]
        public virtual ICollection<Products> Products { get; set; }

        public void OpenForm()
        {
            CategoriesAdder form = new CategoriesAdder();
            form.ShowDialog();
        }

        public void OpenFormToEdit(DataGrid dataGrid)
        {
            Categories cat = (Categories)dataGrid.SelectedItems[0];
            CategoriesAdder form = new CategoriesAdder();
            form.cat = cat;
            form.categoryName.Text = cat.CategoryName;
            form.categoryDescription.Text = cat.Description;
            form.ShowDialog();
        }
        public void Add()
        {
            using (var db = new Context())
            {
                db.Categories.AddOrUpdate(this);
                db.SaveChanges();
            }
        }
        public void Load(DataGrid dataGrid)
        {
            using (var db = new Context())
            {
                var query = db.Categories.OrderBy(x => x.CategoryID);
                dataGrid.ItemsSource = query.ToList();
                
            }
            dataGrid.Columns[3].Visibility = Visibility.Collapsed;
        }

        public void Delete(DataGrid dataGrid)
        {
            List<Categories> categories = new List<Categories>();
            foreach (var item in dataGrid.SelectedItems)
            {
                categories.Add((Categories)item);
            }
            using (var db = new Context())
            {
                foreach(var item in categories)
                {
                    var query = db.Products.Where(x => x.CategoryID == item.CategoryID);
                    foreach (var item2 in query)
                    {
                        db.Products.Remove(item2);
                    }
                    Categories delete = db.Categories.First(x => x.CategoryID == item.CategoryID);
                    db.Categories.Remove(delete);
                }

                db.SaveChanges();
            }
        }
    }
}
