using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace DroidLord.Core
{
    public delegate void GlobalEventHandler(object sender, string Event, object Arg);

    public class EventManager
    {
        private Hashtable HandlerChain;

        public EventManager()
        {
            HandlerChain = new Hashtable();
        }

        public void Raise(object sender, string EventName, object Arg)
        {
            if (!HandlerChain.Contains(EventName)) return;
            List<GlobalEventHandler> Handlers = (List<GlobalEventHandler>)HandlerChain[EventName];
            for (int i = 0; i < Handlers.Count; i++)
            {
                Handlers[i].Invoke(sender, EventName, Arg);
            }
        }

        public void Register(string EventName, GlobalEventHandler Handler)
        {
            if (!HandlerChain.Contains(EventName))
            {
                HandlerChain.Add(EventName, new List<GlobalEventHandler>());
            }
            ((List<GlobalEventHandler>)HandlerChain[EventName]).Add(Handler);
        }

    }
}