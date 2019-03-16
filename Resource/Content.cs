using DroidLord.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroidLord.Resource
{
    [Serializable]
    public class Content : BaseResource
    {
        public Content()
        {
            Key = Guid.NewGuid().ToString();
        }
        public string Title
        {
            get { return GetValue("title") as string; }
            set { SetValue("title", value); }
        }
        public string Body
        {
            get { return GetValue("content") as string; }
            set { SetValue("content", value); }
        }
        public List<string> Album
        {
            get; set;
        } = new List<string>();
    }
}
