using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomRichEditControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : System.Windows.Controls.UserControl
    {
        public static readonly DependencyProperty TextProperty =
         DependencyProperty.Register("Text", typeof(string), typeof(UserControl1), new UIPropertyMetadata(null));

        public  string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public UserControl1()
        {
            InitializeComponent();
            this.DataContext = this;
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            rtbEditor.FlowDirection = System.Windows.FlowDirection.RightToLeft;
        }
        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog OpenFiledlg = new Microsoft.Win32.OpenFileDialog();
            OpenFiledlg.DefaultExt = "All files (*.*)|*.*";
            //OpenFiledlg.Filter = "RTF Files(*.rtf)|*.rtf |All files (*.*)|*.*";
            OpenFiledlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (OpenFiledlg.ShowDialog() ==true & OpenFiledlg.FileName.Length > 0)
            {
                FileStream fileStream = new FileStream(OpenFiledlg.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, System.Windows.DataFormats.Rtf);

                
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, System.Windows.Forms.DataFormats.Rtf);
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);

        }

        private void FontColor(System.Windows.Controls.RichTextBox rc)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                var wpfcolor = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                TextRange range = new TextRange(rc.Selection.Start, rc.Selection.End);
                range.ApplyPropertyValue(FlowDocument.ForegroundProperty, new SolidColorBrush(wpfcolor));
            }
        }
        private void HighlightColor(System.Windows.Controls.RichTextBox rc)
        {
            var colorDialog = new System.Windows.Forms.ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var wpfcolor = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                TextRange range = new TextRange(rc.Selection.Start, rc.Selection.End);
                range.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(wpfcolor));
            }
        }
        private void BtnRtl_Click(object sender, RoutedEventArgs e)
        {
            //rtbEditor.Selection.ApplyPropertyValue(Inline.FlowDirectionProperty, FlowDirection.RightToLeft);
            rtbEditor.FlowDirection = System.Windows.FlowDirection.RightToLeft;
        }
        private void BtnLtr_Click(object sender, RoutedEventArgs e)
        {
            //rtbEditor.Selection.ApplyPropertyValue(Inline.FlowDirectionProperty, 0);
            rtbEditor.FlowDirection = System.Windows.FlowDirection.LeftToRight;
        }

        private void SettingsFontColor_Click(object sender, RoutedEventArgs e)
        {
            FontColor(rtbEditor);
        }
        private void SettingsHighlightColor_Click(object sender, RoutedEventArgs e)
        {
            HighlightColor(rtbEditor);
        }

        private void BtnInsertTable_Click(object sender, RoutedEventArgs e)
        {
            //var InserttableDlg = new 
            //Document doc = rtbEditor.Document;
            //doc.InsertTable

        }

        //private void BtnDecIndent_Click(object sender, RoutedEventArgs e)
        //{
        //    rtbEditor.Selection.ApplyPropertyValue(, FlowDirection.LeftToRight);
        //}
    
}
}
