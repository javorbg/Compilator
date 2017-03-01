using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Compiler
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            BuildCommand = new DelegateCommand(OnBuildCommand);
            ChangeFontCommand = new DelegateCommand(OnChangeFontCommand);
            ChangeColorCommand = new DelegateCommand(OnChangeColorCommand);
            ViewTablesCommand = new DelegateCommand(OnViewTablesCommand);
            DebugCommand = new DelegateCommand(OnDebugCommand);
            NewFileCommand = new DelegateCommand(OnNewFileCommand);
            SaveFileCommand = new DelegateCommand(OnSaveFileCommand);
            SaveAsFileCommand = new DelegateCommand(OnSaveAsFileCommand);
            ExiteCommand = new DelegateCommand(OnExiteCommand);
            SourceCode = new FlowDocument();
            SourceCode.LineHeight = 1d;

        }

        public FlowDocument SourceCode { get; set; }

        public DelegateCommand NewFileCommand { get; set; }

        public DelegateCommand SaveFileCommand { get; set; }

        public DelegateCommand SaveAsFileCommand { get; set; }

        public DelegateCommand ExiteCommand { get; set; }

        public DelegateCommand BuildCommand { get; set; }

        public DelegateCommand ChangeFontCommand { get; set; }

        public DelegateCommand ChangeColorCommand { get; set; }

        public DelegateCommand ViewTablesCommand { get; set; }

        public DelegateCommand DebugCommand { get; set; }

        private void OnDebugCommand()
        {
        }

        private void OnViewTablesCommand()
        {
        }

        private void OnChangeColorCommand()
        {
        }

        private void OnChangeFontCommand()
        {
        }

        private void OnBuildCommand()
        {
        }


        private void OnExiteCommand()
        {
            Application.Current.Shutdown();
        }

        private void OnSaveAsFileCommand()
        {
        }

        private void OnSaveFileCommand()
        {

        }

        private void OnNewFileCommand()
        {
        }

        
    }
}
