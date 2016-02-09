using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace FunctionsList
{
    [Serializable]
    public class WrongArgumentCountException : Exception
    {
        public WrongArgumentCountException() { }
        public WrongArgumentCountException(string message) : base(message) { }
        public WrongArgumentCountException(string message, Exception inner) : base(message, inner) { }
        protected WrongArgumentCountException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class UndefinedVariableException : Exception
    {
        public UndefinedVariableException() { }
        public UndefinedVariableException(string message) : base(message) { }
        public UndefinedVariableException(string message, Exception inner) : base(message, inner) { }
        protected UndefinedVariableException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class WrongFormatArgumentException : Exception
    {
        public WrongFormatArgumentException() { }
        public WrongFormatArgumentException(string message) : base(message) { }
        public WrongFormatArgumentException(string message, Exception inner) : base(message, inner) { }
        protected WrongFormatArgumentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}