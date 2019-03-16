using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroidLord.Core
{
    public interface ICipher
    {
        bool Init(string key);
        string Encrypt(string data);
        string Decrypt(string data);
    }
}
