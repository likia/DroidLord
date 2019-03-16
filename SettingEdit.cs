using DroidLord.Core;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DroidLord
{
    public partial class SettingEdit : MaterialForm
    {
        public SettingEdit()
        {
            InitializeComponent();
        }
        SettingType valueType =  SettingType.SETTING_STRING;
        public void SetRow(SettingItem row)
        {            
            lbType.Text = row.GetDisplayType();
            lbConfigName.Text = row.DisplayName;
            txbConfigValue.Text = row.Value.ToString();
            this.Tag = row.Name;
            valueType = row.Type;
            if (valueType == SettingType.SETTING_BOOLEAN)
            {
                combOption.Text = (bool)row.Value ? "是" : "否";
                txbConfigValue.Hide();
                combOption.Show();
            }
            else
            {
                combOption.Hide();
            }
        }

        private void SettingEdit_Load(object sender, EventArgs e)
        {

        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            if (valueType == SettingType.SETTING_BOOLEAN)
            {
                txbConfigValue.Text = combOption.Text.Replace("是", "true").Replace("否", "false");
            }
            var val = txbConfigValue.Text;
            var ret = false;
            object _out = null;
            switch (valueType)
            {
                case SettingType.SETTING_BOOLEAN:
                    bool _outBool = false;
                    ret = bool.TryParse(val, out _outBool);
                    _out = _outBool;
                    break;
                case SettingType.SETTING_FLOAT:
                    float _outFloat = 0.0f;
                    ret = float.TryParse(val, out _outFloat);
                    _out = _outFloat;
                    break;
                case SettingType.SETTING_INT:
                    int _outInt = 0;
                    ret = int.TryParse(val, out _outInt);
                    _out = _outInt;
                    break;
                case SettingType.SETTING_STRING:
                    ret = !string.IsNullOrEmpty(val) && !string.IsNullOrWhiteSpace(val);
                    _out = val.ToString();
                    break;
            }
            if (!ret)
            {
                MessageBox.Show("数据格式错误!");
                return;
            }
            Program.GlobalSetting.SetValue(Tag.ToString(), _out);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
