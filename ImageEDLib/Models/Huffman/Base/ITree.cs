
namespace ImageEDLib.Models.Huffman.Base
{
    interface ITree
    {
        void AddNode(object node);
        object GetNode(object nodeKey);
        object GetNodePath(object nodeKey);
        bool BuildTree();
        int NodesCounter { get; }
    }
}
