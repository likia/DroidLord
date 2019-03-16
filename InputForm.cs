
using System.Text.RegularExpressions;    
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DroidLord
{
    public partial class InputForm : MaterialForm
    {
        public InputForm()
        {
            InitializeComponent();
        }

        public string GetInput()
        {
            return textBox1.Text;
        }

        public void SetDefault(string def, string caption="", string title="")
        {
            textBox1.Text = def;
            this.Text = string.Format(Text, title);
            materialLabel1.Text = string.Format(materialLabel1.Text, caption);
        }

        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Regex.Replace(textBox1.Text, "\\s+","")))
            {
                MessageBox.Show("至少输入一些文字 ！");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void InputForm_Load(object sender, EventArgs e)
        {

        }
    }
}
