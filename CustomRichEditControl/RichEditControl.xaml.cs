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
using DataFormats = System.Windows.DataFormats;

namespace CustomRichEditControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : System.Windows.Controls.UserControl
    {
        public static readonly DependencyProperty TextProperty =
         DependencyProperty.Register("Text", typeof(string), typeof(UserControl1), new PropertyMetadata(new PropertyChangedCallback(OnTextChanged)));


        //public  string Text
        //{
        //    get
        //    {
        //        //return (string)GetValue(TextProperty);
        //        return RichTextBoxExtensions.GetText(rtbEditor.Document);
        //    }
        //    set
        //    {
        //        SetValue(TextProperty, value);
        //        if(!bDisableSet)
        //            RichTextBoxExtensions.SetText(rtbEditor.Document, value);
        //    }
        //}

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);

            }
        }

        public static readonly DependencyProperty RtfTextProperty =
         DependencyProperty.Register("RtfText", typeof(string), typeof(UserControl1), new PropertyMetadata(new PropertyChangedCallback(OnRTFTextChanged)));


        public string RtfText
        {
            get { return (string)GetValue(RtfTextProperty); }
            set { SetValue(RtfTextProperty, value); }
        }


        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UserControl1)d;

            if (!bDisableSet)
            {
                bDisableTextChange = true;
                RichTextBoxExtensions.SetText(control.rtbEditor.Document, e.NewValue?.ToString());
                bDisableTextChange = false;
            }
        }

        private static void OnRTFTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UserControl1)d;

            if (!bDisableSet)
            {
                bDisableTextChange = true;
                RichTextBoxExtensions.SetRtfText(control.rtbEditor.Document, e.NewValue.ToString());
                bDisableTextChange = false;
            }
        }

        //public string RtfText
        //{
        //    get
        //    {
        //        //return (string)GetValue(RtfTextProperty);
        //        return RichTextBoxExtensions.GetRtfText(rtbEditor.Document);
        //    }
        //    set
        //    {
        //        SetValue(RtfTextProperty, value);
        //        if(!bDisableSet)
        //            RichTextBoxExtensions.SetRtfText(rtbEditor.Document, value);
        //    }
        //}

        public static bool bDisableSet { get; private set; } = false;
        public static bool bDisableTextChange { get; private set; } = false;


        //public string GetRtfText()
        //{
        //    return RichTextBoxExtensions.GetRtf(rtbEditor);
        //}

        public UserControl1()
        {
            InitializeComponent();
            this.DataContext = this;
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            rtbEditor.FlowDirection = System.Windows.FlowDirection.RightToLeft;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            rtbEditor.Focus();
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

        private void RtbEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!bDisableTextChange)
            {
                bDisableSet = true;
                Text = RichTextBoxExtensions.GetText(rtbEditor.Document);
                RtfText = RichTextBoxExtensions.GetRtfText(rtbEditor.Document);
                bDisableSet = false;
            }
        }

        //private void RtbEditor_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    RtfText = RichTextBoxExtensions.GetRtf(rtbEditor.Document);
        //}

        //private void BtnDecIndent_Click(object sender, RoutedEventArgs e)
        //{
        //    rtbEditor.Selection.ApplyPropertyValue(, FlowDirection.LeftToRight);
        //}

    }

    public static class RichTextBoxExtensions
    {
        public static string GetRtfText(FlowDocument document)
        {
            TextRange tr = new TextRange(document.ContentStart, document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                tr.Save(ms, System.Windows.DataFormats.Rtf);
                return ASCIIEncoding.Default.GetString(ms.ToArray());
            }
        }

        public static void SetRtfText(FlowDocument document, string text)
        {
            try
            {
                if (String.IsNullOrEmpty(text))
                {
                    document.Blocks.Clear();
                }
                else
                {
                    TextRange tr = new TextRange(document.ContentStart, document.ContentEnd);
                    using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(text)))
                    {
                        tr.Load(ms, DataFormats.Rtf);
                    }
                }
            }
            catch
            {
                throw new InvalidDataException("Data provided is not in the correct RTF format.");
            }
        }

        public static string GetText(FlowDocument document)
        {
           return new TextRange(document.ContentStart, document.ContentEnd).Text;
        }

        public static void SetText(FlowDocument document, string text)
        {
            new TextRange(document.ContentStart, document.ContentEnd).Text = text;
        }

    }
}
