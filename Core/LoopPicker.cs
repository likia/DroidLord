using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroidLord.Core
{
    public class LoopPicker<T> : Picker<T>
    {
        private object Lock = new object();
        public LoopPicker()
        {
            Index = 0;
        }
        protected int Index;

        public override T Pick()
        {
            T obj;
            lock (Lock)
            {
                if (Index >= List.Count)
                    Index = 0;

                obj = List[Index];
                ++Index;
            }
            return obj;
        }
    }
}
