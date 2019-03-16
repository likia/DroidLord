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
    public partial class SelectForm : MaterialForm
    {
        public SelectForm()
        {
            InitializeComponent();
        }

        public void SetSelection(string caption, string text, string[] list)
        {
            this.Text = String.Format(this.Text, caption);
            lbSelection.Text = String.Format(lbSelection.Text, text);
            combSelection.Items.Clear();
            combSelection.Items.AddRange(list); 
        }

        public int GetSelection()
        {
            return combSelection.SelectedIndex;
        }

        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            if (combSelection.SelectedIndex ==-1)
            {
                MessageBox.Show("请" + Text);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }      

        private void SelectForm_Load(object sender, EventArgs e)
        {

        }
    }
}
