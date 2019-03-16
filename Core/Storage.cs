using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroidLord.Core
{
    public class Storage : Savable
    {
        public Hashtable Global = new Hashtable();
        public Hashtable Temp = new Hashtable();

        public object Save()
        {
            return Global;
        }

        public void Load(object State)
        {
            Global = State as Hashtable;
        }
    }
}
