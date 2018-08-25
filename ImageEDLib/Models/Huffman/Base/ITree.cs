using System;

namespace ImageEDLib.Models.Huffman.Base
{
    interface ITree : IDisposable
    {
        void AddNode(object node);
        object GetNode(object nodeKey);
        object GetNodePath(object nodeKey);
        bool BuildTree();
        int NodesCounter { get; }
        bool IsBuilt { get; }
    }
}