using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Поломка.ViewModels;

namespace Поломка.Views
{
    /// <summary>
    /// Interaction logic for ListClientsView.xaml
    /// </summary>
    public partial class ListClientsView : Page
    {
        public ListClientsView()
        {
            InitializeComponent();
            DataContext = new ListClientsViewModel();
        }

        private void ClickColumn(object sender, MouseButtonEventArgs e)
        {
            string p = ((Control)sender).Tag as string;
            ((ListClientsViewModel)DataContext).Sort(p);
        }
    }
}
