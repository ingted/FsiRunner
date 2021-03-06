using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;

namespace ClearLines.FsiControl
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using FsiRunner;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class CodeBlock : ViewModelBase
    {
        private readonly FsiSession session;
        private RelayCommand run;
        private double fontSize;
        private IHighlightingDefinition syntaxHighlighting;

        public CodeBlock(FsiSession session, IHighlightingDefinition syntaxHighlighting)
        {
            this.Document = new TextDocument("");
            this.session = session;
            this.syntaxHighlighting = syntaxHighlighting;
            this.run = new RelayCommand(OnRun, CanRun);
        }

        public TextDocument Document { get; set; }

        public IHighlightingDefinition Highlighting
        {
            get { return this.syntaxHighlighting; }
            set
            {
                this.syntaxHighlighting = value;
                base.RaisePropertyChanged("Highlighting");
            }
        }

        public double FontSize
        {
            get { return this.fontSize; }
            set
            {
                this.fontSize = value;
                base.RaisePropertyChanged("FontSize");
            }
        }

        public string Code
        {
            get { return this.Document.Text; }
        }

        public ICommand Run
        {
            get { return this.run; }
        }

        private void OnRun()
        {
            var lines = this.BreakLines(this.Code);
            foreach (var line in lines)
            {
                this.Session.AddLine(line);
            }

            this.Session.Evaluate();
        }

        private bool CanRun()
        {
            return true;
        }

        private FsiSession Session
        {
            get { return this.session; }
        }

        private IEnumerable<string> BreakLines(string text)
        {
            return text.Split((char)10);
        }
    }
}