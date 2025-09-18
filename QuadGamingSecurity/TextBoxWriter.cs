using System.IO;
using System.Text;
using System.Windows.Forms;

namespace QuadGamingSecurity
{
    public class TextBoxWriter : TextWriter
    {
        public TextBoxWriter(Control textBox)
        {
            TextBox = textBox;
        }

        public override Encoding Encoding => Encoding.ASCII;

        private Control TextBox;
        private string CurrentLine;

        public override void Write(char value)
        {
            if (value != '\n')
            {
                CurrentLine += value;
            }
            else
            {
                AddLine(CurrentLine);
                CurrentLine = "";
            }
        }

        private void AddLine(string line)
        {
            TextBox.Text = "\n" + line + TextBox.Text;
        }
    }
}
