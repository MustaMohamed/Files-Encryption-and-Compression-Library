using System;
using System.Collections.Generic;
using System.Text;
using ImageEDLib.Models.Huffman.Base;
using ImageEDLib.Models.Huffman.Exceptions;

namespace ImageEDLib.Models.Huffman
{
    public class HuffmanTree : ITree
    {

        // the root node that holds the tree
        public HuffmanNode RootNode { get; private set; }

        // the number of nodes in tree
        public int NodesCounter { get; private set; }

        // the priority queue data structure to get the minimum node
        private PriorityQueue<HuffmanNode> PqNodeSelector { set; get; }

        // the nodes frequencies
        private Dictionary<int, long> NodesFrequency { get; set; }

        // the tree nodes members
        public List<HuffmanNode> TreeNodes { get; set; }

        // the path of each leaf node in tree
        public Dictionary<int, String> TreePaths { get; private set; }

        // the final tree code in binary string
        public String TreeCode { get; set; }

        // the flag of tree building
        public bool IsBuilt { get; set; }

        /**
          * Empty Constructor initialize the data members
          */
        public HuffmanTree()
        {
            RootNode = null;
            NodesCounter = 0;
            TreeNodes = new List<HuffmanNode>();
            NodesFrequency = new Dictionary<int, long>();
            PqNodeSelector = new PriorityQueue<HuffmanNode>();
            TreePaths = new Dictionary<int, string>();
        }


        public HuffmanTree(List<HuffmanNode> treeNodes) : this()
        {
            TreeNodes = treeNodes;
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
            return NodesFrequency[(int)nodeKey];
        }


        public object GetNodePath(object nodeKey)
        {
            if (!IsBuilt)
                throw new TreeNotBuiltException();
            return TreePaths[(int)nodeKey];
        }

        public bool BuildTree()
        {
            // initialize the priority queue frequencies
            InitPq();

            // begin to generate the tree structure
            InitSelection();

            // build nodes paths
            BuildPaths();

            // set the flag to be true
            return IsBuilt = true;
            
//            throw new NotImplementedException();
        }


        /**
         * copies all node in nodes list to the priority queue
         */
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

                // merge the 2 nodes in parent node and set them to right and left nodes
                HuffmanNode parentNode = new HuffmanNode(-1,
                    rightNode.Frequincy + leftNode.Frequincy,
                    leftNode, rightNode);
                // add the parent node to PQ
                PqNodeSelector.Enqueue(parentNode);
                // increment the nodes counter
                NodesCounter++;
            }

            NodesCounter += TreeNodes.Count;
            // the final node is the root node
            RootNode = (HuffmanNode) PqNodeSelector.Dequeue();
        }

        /**
         * building the tree paths for each node
         */
        private void BuildPaths()
        {
            // generate node paths
            SearchPath(RootNode, new StringBuilder());
            // convert paths to char
            ConvertPaths();
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
                else if (node.IsLeaf())
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
            }
        }

        private void ConvertPaths()
        {
            StringBuilder pathCode = new StringBuilder();
            foreach (String tempstr in TreePaths.Values)
                pathCode.Append(tempstr);
            TreeCode = pathCode.ToString();
        }

        public override string ToString()
        {
            return "HuffmanTree\t{" +
                   "\nmRootNode=" + RootNode +
                   "\n, mNodeCounter=" + NodesCounter +
                   "\n, mPQNodeSelector=" + PqNodeSelector.Count +
                   "\n, mNodeFrequency=" + NodesFrequency.Count +
                   "\n, mTreeNodes=" + TreeNodes.Count +
                   "\n, mTreePaths=" + TreePaths.Count +
                   "\n, mTreeCode='" + TreeCode + '\'' +
                   "\n\t}";
        }
    }
}