using System;

namespace ImageEDLib.Models.Huffman.Base
{
    public interface INode<T> : IComparable, ICloneable, IDisposable
    {
        bool IsLeaf();
        T NodeValue { get; set; }
    }
}