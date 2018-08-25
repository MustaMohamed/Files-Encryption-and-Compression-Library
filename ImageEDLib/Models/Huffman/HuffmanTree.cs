using System;
using System.Collections.Generic;
using System.Text;
using ImageEDLib.Models.Huffman.Base;
using ImageEDLib.Models.Huffman.Exceptions;

namespace ImageEDLib.Models.Huffman
{
    internal class HuffmanTree : ITree
    {
        public HuffmanNode RootNode { get; private set; }

        public int NodesCounter { get; private set; }

        public bool IsBuilt { get; private set; }

        private PriorityQueue<HuffmanNode> PqNodeSelector { set; get; }

        private Dictionary<int, long> NodesFrequency { get; set; }

        public List<HuffmanNode> TreeNodes { get; set; }

        public Dictionary<int, string> TreePaths { get; private set; }

        public HuffmanTree()
        {
            RootNode = null;
            NodesCounter = 0;
            TreeNodes = new List<HuffmanNode>();
            NodesFrequency = new Dictionary<int, long>();
            TreePaths = new Dictionary<int, string>();
        }

        public HuffmanTree(List<HuffmanNode> treeNodes) : this()
        {
            TreeNodes = treeNodes;
            NodesCounter = treeNodes.Count;
        }

        public void AddNode(object node)
        {
            var tNode = node as HuffmanNode;
            if (null != tNode)
            {
                TreeNodes.Add(tNode);
                NodesFrequency.Add(tNode.NodeValue, tNode.Frequincy);
            }
            else
                throw new ArgumentNullException();
        }

        public object GetNode(object nodeKey)
        {
            return NodesFrequency[(int) nodeKey];
        }

        public object GetNodePath(object nodeKey)
        {
            if (!IsBuilt)
                throw new TreeNotBuiltException();
            return TreePaths[(int) nodeKey];
        }

        public bool BuildTree()
        {
            InitPq();

            // begin to generate the tree structure
            InitSelection();

            BuildPaths();

            return IsBuilt = true;
        }

        private void InitPq()
        {
            PqNodeSelector = new PriorityQueue<HuffmanNode>(TreeNodes);
        }

        /**
         * build the tree
         */
        private void InitSelection()
        {
            while (PqNodeSelector.Count > 1)
            {
                // get the minimum 2 node
                HuffmanNode rightNode = PqNodeSelector.Dequeue();
                HuffmanNode leftNode = PqNodeSelector.Dequeue();

                HuffmanNode parentNode = new HuffmanNode(-1,
                    rightNode.Frequincy + leftNode.Frequincy,
                    leftNode, rightNode);

                PqNodeSelector.Enqueue(parentNode);
                NodesCounter++;
            }

            // the final node is the root node
            RootNode = (HuffmanNode) PqNodeSelector.Dequeue();
        }

        /**
         * building the tree paths for each node
         */
        private void BuildPaths()
        {
            SearchPath(RootNode, new StringBuilder());
        }

        private void SearchPath(HuffmanNode node, StringBuilder path)
        {
            try
            {
                // the end of the path and returns
                if (node == null)
                {
                    return;
                }
                else if (node.IsLeaf)
                {
                    TreePaths.Add(node.NodeValue, path.ToString());
                    return;
                }

                // go to right node and add 1 to the path code
                path.Append('0');
                SearchPath(node.RightNode, path);
                path.Remove(path.Length - 1, 1);

                // go to left node and add 0 to the path code
                path.Append('1');
                SearchPath(node.LeftNode, path);
                path.Remove(path.Length - 1, 1);
            }
            catch (NullReferenceException)
            {
                // TODO
            }
        }

        public override string ToString()
        {
            return "HuffmanTree\t{" +
                   "\n\tmRootNode=" + RootNode +
                   "\n\t, mNodeCounter=" + NodesCounter +
                   "\n\t, mPQNodeSelector=" + PqNodeSelector.Count +
                   "\n\t, mNodeFrequency=" + NodesFrequency.Count +
                   "\n\t, mTreeNodes=" + TreeNodes.Count +
                   "\n\t, mTreePaths=" + TreePaths.Count +
                   "\n\t}";
        }

        public void Dispose()
        {
            RootNode.Dispose();
            NodesCounter = 0;
            PqNodeSelector.Clear();
            PqNodeSelector = null;
            NodesFrequency.Clear();
            NodesFrequency = null;
            TreeNodes.Clear();
            TreeNodes = null;
            TreePaths.Clear();
            TreePaths = null;
        }
    }
}