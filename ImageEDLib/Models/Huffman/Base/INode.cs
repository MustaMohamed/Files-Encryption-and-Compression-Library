using System;

namespace ImageEDLib.Models.Huffman.Base
{
    public interface INode<T> : IComparable, ICloneable, IDisposable
    {
        bool IsLeaf { get; }
        T NodeValue { get; set; }
    }
}