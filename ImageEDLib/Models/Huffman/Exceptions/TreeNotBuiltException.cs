using System;

namespace ImageEDLib.Models.Huffman.Exceptions
{
    class TreeNotBuiltException : Exception
    {
        public TreeNotBuiltException() : base("The Tree is Not built...!") { }

        public TreeNotBuiltException(string message) : base(message) { }

        public TreeNotBuiltException(string message, Exception cause) : base(message, cause) { }
    }
}
