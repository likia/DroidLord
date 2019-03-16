using DroidLord.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroidLord
{
    public class SlavedDevice : BaseResource
    {
        public string SerialNumber
        {
            get
            {
                return this.Key;
            }
            set
            {
                this.Key = value;
            }        
        }

        public Slavery.Slave Object
        {
            get {
                return (Slavery.Slave)this.GetValue("obj");                 
            }
            set {
                this.SetValue("obj", value);
            }
        }
    }
}
