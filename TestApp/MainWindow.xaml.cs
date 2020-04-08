using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public virtual string TestText { get; set; }
        

        public MainWindow()
        {
            InitializeComponent();
            
        }

       

        public void TestTextChanged()
        {
            MessageBox.Show(TestText);
        }
         
       private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(TestText);
        }
    }

    //public class TestEntity
    //{
    //    public string Remarks { get; set; }
    //}

    public class TestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
