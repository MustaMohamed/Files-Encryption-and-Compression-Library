using System;

namespace ImageEDLib.Models.Huffman.Base
{
    public interface INode<T> : IComparable, ICloneable
    {
        bool IsLeaf();
        T NodeValue { get; set; }
    }
}