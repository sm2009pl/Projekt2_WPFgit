using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Projekt2_WPF.DBModels
{
    public interface IModel
    {
        void Add();
        void Load(DataGrid dataGrid);
        void OpenForm();
        void Delete(DataGrid dataGrid);
        void OpenFormToEdit(DataGrid dataGrid);
    }
}
