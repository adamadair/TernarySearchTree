//-----------------------------------------------------------------------------
// <copyright file="TernarySearchTreeNode.cs">
//   Copyright (c) 2012 Adam W Adair. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace AWA.TernarySearchTree
{

    /// <summary>
    /// Represents a node in a TernarySearchTree based dictionary. A node in a 
    /// ternary search tree represents a subset of vectors with a partitioning 
    /// character value and three child nodes: one to lesser character elements, 
    /// one to greater elements and one to equal elements.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <remarks>
    /// Ternary search tree is initially proposed by Jon Bentley and Bob Sedgewick in
    /// "Fast Algorithms for Sorting and Searching Strings" 
    /// http://www.cs.princeton.edu/~rs/strings/paper.pdf 
    /// 
    /// Since a ternary search tree is text based, the ToString() function of the 
    /// inserted key values is used to determine the characters used to build the
    /// tree node structure and search the tree for those keys.
    /// </remarks>
    /// 
    internal class TernarySearchTreeNode<TKey, TValue> : ICloneable
    {
        # region Object Properties
        private char splitChar;
        public char SplitChar
        {
            get { return splitChar; }
            set { splitChar = value; }
        }

        private bool isKey;

        /// <summary>
        /// true if the node is a key node (key is a non null value that is not empty)
        /// </summary>
        public bool IsKey
        {
            get { return isKey; }            
        }

        private TKey key;
        /// <summary>
        /// The key associated with a node key.  Setting this to a value a non null
        /// value will set the node as a key node. The TST algorithm is based on strings, 
        /// and the toString() method of the key value will be used to place the node
        /// within the tree.  
        /// </summary>
        public TKey Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
                if (key == null || key.ToString() == string.Empty)
                {
                    isKey = false;
                    this.Value = default(TValue);
                    return;
                }
                isKey = true;
            }
        }
        
        private TValue value;

        /// <summary>
        /// Value of the node.  This is only useful in a TST if the node is a key node.
        /// </summary>
        public TValue Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        private TernarySearchTreeNode<TKey, TValue> parent;

        /// <summary>
        /// The parent node of the node.  Only the root node of the tree should contain 
        /// null for this member. Not implemented in the referenced code, but added for 
        /// completeness sake.
        /// </summary>
        public TernarySearchTreeNode<TKey, TValue> Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private TernarySearchTreeNode<TKey, TValue> lowChild;

        /// <summary>
        /// The low child node.
        /// </summary>
        public TernarySearchTreeNode<TKey, TValue> LowChild
        {
            get { return lowChild; }
            set { lowChild = value; }
        }

        private TernarySearchTreeNode<TKey, TValue> equalChild;

        /// <summary>
        /// equal child node
        /// </summary>
        public TernarySearchTreeNode<TKey, TValue> EqualChild
        {
            get { return equalChild; }
            set { equalChild = value; }
        }

        private TernarySearchTreeNode<TKey, TValue> highChild;

        /// <summary>
        /// high child node
        /// </summary>
        public TernarySearchTreeNode<TKey, TValue> HighChild
        {
            get { return highChild; }
            set { highChild = value; }
        }

        # endregion 

        # region Public Constructors

        public TernarySearchTreeNode() : this(default(char), null) { }

        public TernarySearchTreeNode(char splitChar) : this(splitChar, null) { }

        public TernarySearchTreeNode(char splitChar, TernarySearchTreeNode<TKey, TValue> parent)
        {
            this.isKey = false;
            this.key = default(TKey);
            this.value = default(TValue);
            this.parent = parent;
            this.splitChar = splitChar;
            this.lowChild = null;
            this.equalChild = null;
            this.highChild = null;
        }
        #endregion

        #region ICloneable Members

        public object Clone()
        {
            TernarySearchTreeNode<TKey, TValue> entry = new TernarySearchTreeNode<TKey, TValue>(SplitChar, Parent);
            entry.Key = this.Key;
            entry.Value = this.Value;
            if (LowChild != null)
            {
                entry.LowChild = LowChild.Clone() as TernarySearchTreeNode<TKey, TValue>;
                entry.LowChild.Parent = entry;
            }
            if (EqualChild != null)
            {
                entry.EqualChild = EqualChild.Clone() as TernarySearchTreeNode<TKey, TValue>;
                entry.EqualChild.Parent = entry;
            }
            if (highChild != null)
            {
                entry.HighChild = HighChild.Clone() as TernarySearchTreeNode<TKey, TValue>;
                entry.HighChild.Parent = entry;
            }
            return entry;
        }

        #endregion

        /// <summary>
        /// Returns a list of keys in the tree rooted at this node.
        /// </summary>
        public IList<TKey> TreeKeys
        {
            get
            {
                List<TKey> keys = new List<TKey>();
                getKeys(keys);
                return keys.AsReadOnly();
            }
        }

        //
        // The recursive getKeys function
        //
        private void getKeys(List<TKey> keys)
        {
            if(LowChild != null){
                LowChild.getKeys(keys);
            }
            if(IsKey){
                keys.Add(Key);
            }
            if(EqualChild!=null){
                EqualChild.getKeys(keys);
            }
            if(HighChild!=null){
                HighChild.getKeys(keys);
            }
        }
        
        /// <summary>
        /// Returns a list of values in the tree rooted at this node
        /// </summary>
        public IList<TValue> TreeValues
        {
            get
            {
                List<TValue> values = new List<TValue>();
                getValues(values);
                return values.AsReadOnly();
            }
        }

        //
        // the recursive get values function
        //
        private void getValues(List<TValue> values)
        {
            if (LowChild != null)
            {
                LowChild.getValues(values);
            }
            if (IsKey && Value!=null)
            {
                values.Add(Value);
            }
            if (EqualChild != null)
            {
                EqualChild.getValues(values);
            }
            if (HighChild != null)
            {
                HighChild.getValues(values);
            }
        }

        /// <summary>
        /// Clears all nodes from the tree.
        /// </summary>
        public void Clear()
        {
            if (LowChild != null)
            {
                LowChild.Clear();
                LowChild = null;
            }
            if (EqualChild != null)
            {
                EqualChild.Clear();
                LowChild = null;
            }
            if (HighChild != null)
            {
                HighChild.Clear();
                HighChild = null;
            }
            Parent = null;
            Key = default(TKey);
            Value = default(TValue);
        }

        /// <summary>
        /// Returns a list of key/value pairs rooted at this node
        /// </summary>
        public IList<KeyValuePair<TKey, TValue>> TreeKeyValuePairs
        {
            get
            {
                List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>();
                getKeyValuePairs(list);
                return list.AsReadOnly();
            }
        }

        // 
        // recursive function to retrieve all key value pairs
        private void getKeyValuePairs(IList<KeyValuePair<TKey, TValue>> list)
        {
            if (LowChild != null)
            {
                LowChild.getKeyValuePairs(list);
            }
            if (IsKey)
            {
                list.Add(new KeyValuePair<TKey, TValue>(Key, Value));
            }
            if (EqualChild != null)
            {
                EqualChild.getKeyValuePairs(list);
            }
            if (HighChild != null)
            {
                HighChild.getKeyValuePairs(list);
            }
        }

        /// <summary>
        /// Either finds or create a new node in the tree rooted at this node.
        /// </summary>
        /// <param name="key">The key of the node</param>
        /// <returns>
        /// The added node. If node already existed in the tree
        /// then that node is returned instead of created a new one.
        /// </returns>
        public TernarySearchTreeNode<TKey, TValue> Insert(string key)
        {
            return Insert(key, 0);
        }

        //
        // The recursive insert function.  If node with the provided key
        private TernarySearchTreeNode<TKey, TValue> Insert(string key, int index)
        {
            char s = key[index];
            if (s < SplitChar)
            {
                if (LowChild == null)
                {
                    LowChild = new TernarySearchTreeNode<TKey, TValue>(s, this);
                }
                return LowChild.Insert(key, index);
            }
            else if (s > SplitChar)
            {
                if (HighChild == null)
                {
                    HighChild = new TernarySearchTreeNode<TKey, TValue>(s, this);
                }
                return HighChild.Insert(key, index);

            }
            if (s == SplitChar && index == key.Length - 1)
            {
                return this;
            }
            if (EqualChild == null)
            {
                EqualChild = new TernarySearchTreeNode<TKey, TValue>(key[index + 1], this);
            }
            return EqualChild.Insert(key, index + 1);
        }

        /// <summary>
        /// Perform near search starting at this node.
        /// </summary>
        /// <param name="key">Starting key</param>
        /// <param name="distance">distance from key to search</param>
        /// <returns>List of key value pairs</returns>
        public IList<KeyValuePair<TKey, TValue>> NearSearch(string key, int distance)
        {
            List<KeyValuePair<TKey, TValue>> results = new List<KeyValuePair<TKey, TValue>>();
            nearSearch(results, key, 0, distance);
            return results.AsReadOnly();
        }

        //
        // recursive nearSearch
        //
        private void nearSearch(List<KeyValuePair<TKey, TValue>> results, string key, int keyIndex, int distance)
        {
            if (distance < 0) return;
            char c = key[keyIndex];
            if ((distance > 0 || c < this.SplitChar) && this.LowChild != null)
                this.LowChild.nearSearch(results, key, keyIndex, distance);
            if ((this.IsKey) && ((key.Length - keyIndex) <= distance))
            {
                results.Add(new KeyValuePair<TKey, TValue>(this.Key, this.Value));
            }
            else
            {
                if (this.EqualChild != null)
                {
                    this.EqualChild.nearSearch(results,
                        key,
                        (keyIndex != key.Length - 1) ? keyIndex + 1 : keyIndex,
                        (c == SplitChar) ? distance : distance - 1);
                }
            }
            if ((distance > 0 || c > this.SplitChar) && this.HighChild != null)
            {
                this.HighChild.nearSearch(results, key, keyIndex, distance);
            }
        }

        /// <summary>
        /// Partila match search starting at this node.
        /// </summary>
        /// <param name="keyPattern">The partial key pattern</param>
        /// <returns></returns>
        public IList<KeyValuePair<TKey, TValue>> PartialMatch(string keyPattern)
        {
            List<KeyValuePair<TKey, TValue>> results = new List<KeyValuePair<TKey, TValue>>();
            partialMatch(results, keyPattern, 0);
            return results.AsReadOnly();
        }

        //
        // recursive partialMatch function
        //
        private void partialMatch(List<KeyValuePair<TKey, TValue>> results, string keyPattern, int index)
        {
            char c = keyPattern[index];
            if (c == '*')
            {
                glob(results, keyPattern, index + 1);
                return;
            }
            if ((c == '.' || c < this.SplitChar) && this.LowChild!=null)
                LowChild.partialMatch(results, keyPattern, index);
            if (c == '.' || c == this.SplitChar)
            {
                if (index < keyPattern.Length - 1)
                {
                    if (this.EqualChild != null) 
                        EqualChild.partialMatch(results, keyPattern, index + 1);
                }
                else if (this.IsKey)
                    results.Add(new KeyValuePair<TKey, TValue>(this.Key, this.Value));
            }
            if ((c == '.' || c > this.SplitChar) && this.HighChild != null)
                HighChild.partialMatch(results, keyPattern, index);
        }

        //
        // match a wild card string.  Finds all remaining character nodes for the next
        // character in the key pattern, and continue the search. 
        //
        private void glob(List<KeyValuePair<TKey, TValue>> results, string keyPattern, int index)
        {
            if (index == keyPattern.Length)
            {
                // The wilds card char is last in pattern
                // return all remaining keys value pairs
                results.AddRange(this.TreeKeyValuePairs);
                return;
            }
            if (keyPattern[index] == '*')
                glob(results, keyPattern, index + 1);
            

            string newKeyPattern = keyPattern.Substring(index);
            char nextC = newKeyPattern[0];
            List<TernarySearchTreeNode<TKey, TValue>> nodes = new List<TernarySearchTreeNode<TKey, TValue>>();
            findCharNodes(nodes, nextC);

            foreach (TernarySearchTreeNode<TKey, TValue> node in nodes)
            {
                node.partialMatch(results, newKeyPattern, 0);
            }
        }

        //
        // recursive function to find all nodes with SplitChar equal to c
        //
        private void findCharNodes(List<TernarySearchTreeNode<TKey,TValue>> nodes, char c)
        {
            if (c == '.' || c == this.SplitChar)
            {
                nodes.Add(this);
            }
            if (this.LowChild != null)
                this.LowChild.findCharNodes(nodes, c);
            if (this.EqualChild != null)
                this.EqualChild.findCharNodes(nodes, c);
            if (this.HighChild != null)
                this.HighChild.findCharNodes(nodes, c);
        }
    }
}
