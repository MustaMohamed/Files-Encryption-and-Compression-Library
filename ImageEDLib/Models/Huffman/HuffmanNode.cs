using System;
using System.Text;
using ImageEDLib.Models.Huffman.Base;

namespace ImageEDLib.Models.Huffman
{
    public class HuffmanNode : INode<int>
    {

        public int NodeValue { get; set; }
        public long Frequincy { get; set; }
        public HuffmanNode RightNode { get; set; }
        public HuffmanNode LeftNode { get; set; }
        private bool Leaf
        {
            get { return RightNode == null && LeftNode == null; }
            // ReSharper disable once ValueParameterNotUsed
            set{ }
        }

        public HuffmanNode(int nodeValue = -1, long nodeFrequincy = -1, HuffmanNode rightNode = null, HuffmanNode leftNode = null)
        {
            NodeValue = nodeValue;
            Frequincy = nodeFrequincy;
            RightNode = rightNode;
            LeftNode = leftNode;
            Leaf = rightNode == null && leftNode == null;
        }

        public HuffmanNode(HuffmanNode thatNode) : this(thatNode.NodeValue, thatNode.Frequincy, thatNode.RightNode, thatNode.LeftNode) { }

        public bool IsLeaf()
        {
            return Leaf;
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var thatNode = obj as HuffmanNode;
            if (thatNode != null)
            {
                if (Frequincy > thatNode.Frequincy)
                    return 1;
                if (Frequincy < thatNode.Frequincy)
                    return -1;
                return Equals(thatNode) ? 0 : NodeValue.CompareTo(thatNode.NodeValue);
            }
            else
                throw new ArgumentException("Object is not a HuffmanNode");
        }

       

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            StringBuilder strB = new StringBuilder();
            strB.Append("Color :=> " + NodeValue + "\n");
            strB.Append("Frequincy :=> " + Frequincy + "\n");
            try
            {
                if (!IsLeaf())
                {
                    strB.Append("Left Node :=> " + LeftNode + "\n");
                    strB.Append("Righr Node :=> " + RightNode + "\n");
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return strB.ToString();
        }

        public override bool Equals(object obj)
        {
            var node = obj as HuffmanNode;
            return node != null && Equals(node);
        }

        private bool Equals(HuffmanNode other)
        {
            return NodeValue == other.NodeValue && Frequincy == other.Frequincy && Equals(RightNode, other.RightNode) && Equals(LeftNode, other.LeftNode);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = NodeValue;
                hashCode = (hashCode * 397) ^ Frequincy.GetHashCode();
                hashCode = (hashCode * 397) ^ (RightNode != null ? RightNode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LeftNode != null ? LeftNode.GetHashCode() : 0);
                return hashCode;
            }
        }
    }


}
