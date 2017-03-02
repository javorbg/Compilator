using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Compiler
{
    public class RichTextBoxHelper : DependencyObject
    {
        private static HashSet<Thread> _recursionProtection = new HashSet<Thread>();

        private static SyntaxHighlighter highlighter = new SyntaxHighlighter();
        public static FlowDocument GetDocumentXaml(DependencyObject obj)
        {
            return (FlowDocument)obj.GetValue(DocumentXamlProperty);
        }

        public static void SetDocumentXaml(DependencyObject obj, FlowDocument value)
        {
            _recursionProtection.Add(Thread.CurrentThread);
            highlighter.ColorKeyWords(value);
            obj.SetValue(DocumentXamlProperty, value);
            
            _recursionProtection.Remove(Thread.CurrentThread);
        }

        public static readonly DependencyProperty DocumentXamlProperty = DependencyProperty.RegisterAttached(
            "DocumentXaml",
            typeof(FlowDocument),
            typeof(RichTextBoxHelper),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_recursionProtection.Contains(Thread.CurrentThread))
                return;

            var richTextBox = (RichTextBox)d;

            try
            {

                FlowDocument doc = GetDocumentXaml(richTextBox);
                richTextBox.Document = doc;
            }
            catch (Exception)
            {
                richTextBox.Document = new FlowDocument();
            }

            richTextBox.TextChanged += RichTextBox_TextChanged;

        }

        private static void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;
            if (richTextBox != null)
            {
                SetDocumentXaml(richTextBox, richTextBox.Document);
            }
        }
    }
}
