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
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public virtual FormattedTextEntity Entity1 { get; set; }
        public virtual FormattedTextEntity Entity2 { get; set; }
        //private string _TestText;
        public virtual string TestText { get; set; }

        public virtual string RtfTestText { get; set; }
        //{
        //    get
        //    {
        //        return _TestText;
        //    }
        //    set
        //    {
        //        _TestText = value;
        //        RaisePropertyChanged("TestText");
        //    }
        //}        

        public MainWindow()
        {
            InitializeComponent();
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(String propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //public void TestTextChanged()
        //{
        //    MessageBox.Show(TestText);
        //}
         
       private void Btn1Save_Click(object sender, RoutedEventArgs e)
       {            
            Entity1 = new FormattedTextEntity();
            Entity1.PlainText = TestText;
            Entity1.RtfText = RtfTestText;
        }
        private void Btn2Save_Click(object sender, RoutedEventArgs e)
        {            
            Entity2 = new FormattedTextEntity();
            Entity2.PlainText = TestText;
            Entity2.RtfText = RtfTestText;
        }

        private void Btn1Load_Click(object sender, RoutedEventArgs e)
        {
            //TestText = Entity1.PlainText;
            RtfTestText = Entity1.RtfText;
            RaisePropertyChanged("TestText");
            RaisePropertyChanged("RtfTestText");

        }

        private void Btn2Load_Click(object sender, RoutedEventArgs e)
        {
            //TestText = Entity2.PlainText;
            RtfTestText = Entity2.RtfText;
            RaisePropertyChanged("TestText");
            RaisePropertyChanged("RtfTestText");
        }
    }


    //public class TestConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {

    //        return value;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return value;
    //    }
    //}

    public class FormattedTextEntity
    {
        public string PlainText { get; set; }
        public string RtfText { get; set; }
    }

}
