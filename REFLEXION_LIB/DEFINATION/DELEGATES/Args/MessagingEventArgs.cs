using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB
{
    public sealed class MessagingEventArgs : System.EventArgs
    {
        private readonly string _title;
        private readonly string _message;

        public MessagingEventArgs( string title, string message)
        {
            _title = title;
            _message = message;
        }
        
        public string Title { get { return _title; } }
        public string Message { get { return _message; } }

    };
}
