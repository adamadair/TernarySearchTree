//-----------------------------------------------------------------------------
// <copyright file="TernarySearchTreeNode.cs">
//   Copyright (c) 2012 Adam W Adair. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace AWA.TernarySearchTree
{

    /// <inheritdoc />
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
    /// Since a ternary search tree is text based, the ToString() function of the 
    /// inserted key values is used to determine the characters used to build the
    /// tree node structure and search the tree for those keys.
    /// </remarks>
    internal class TernarySearchTreeNode<TKey, TValue> : ICloneable
    {
        # region Object Properties

        public char SplitChar { get; set; }

        /// <summary>
        /// true if the node is a key node (key is a non null value that is not empty)
        /// </summary>
        public bool IsKey { get; private set; }

        private TKey _key;
        /// <summary>
        /// The key associated with a node key.  Setting this to a value a non null
        /// value will set the node as a key node. The TST algorithm is based on strings, 
        /// and the toString() method of the key value will be used to place the node
        /// within the tree.  
        /// </summary>
        public TKey Key
        {
            get => _key;
            set
            {
                _key = value;
                if (_key == null || _key.ToString() == string.Empty)
                {
                    IsKey = false;
                    Value = default(TValue);
                    return;
                }
                IsKey = true;
            }
        }

        /// <summary>
        /// Value of the node.  This is only useful in a TST if the node is a key node.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// The parent node of the node.  Only the root node of the tree should contain 
        /// null for this member. Not implemented in the referenced code, but added for 
        /// completeness sake.
        /// </summary>
        public TernarySearchTreeNode<TKey, TValue> Parent { get; set; }

        /// <summary>
        /// The low child node.
        /// </summary>
        public TernarySearchTreeNode<TKey, TValue> LowChild { get; set; }

        /// <summary>
        /// equal child node
        /// </summary>
        public TernarySearchTreeNode<TKey, TValue> EqualChild { get; set; }

        /// <summary>
        /// high child node
        /// </summary>
        public TernarySearchTreeNode<TKey, TValue> HighChild { get; set; }

        # endregion 

        # region Public Constructors

        public TernarySearchTreeNode() : this(default(char), null) { }

        public TernarySearchTreeNode(char splitChar) : this(splitChar, null) { }

        public TernarySearchTreeNode(char splitChar, TernarySearchTreeNode<TKey, TValue> parent)
        {
            IsKey = false;
            _key = default(TKey);
            Value = default(TValue);
            Parent = parent;
            SplitChar = splitChar;
            LowChild = null;
            EqualChild = null;
            HighChild = null;
        }
        #endregion

        #region ICloneable Members

        public object Clone()
        {
            var entry = new TernarySearchTreeNode<TKey, TValue>(SplitChar, Parent)
            {
                Key = Key,
                Value = Value
            };
            if (LowChild != null)
            {
                entry.LowChild = LowChild.Clone() as TernarySearchTreeNode<TKey, TValue>;
                if (entry.LowChild != null) entry.LowChild.Parent = entry;
            }
            if (EqualChild != null)
            {
                entry.EqualChild = EqualChild.Clone() as TernarySearchTreeNode<TKey, TValue>;
                if (entry.EqualChild != null) entry.EqualChild.Parent = entry;
            }
            if (HighChild == null) return entry;
            entry.HighChild = HighChild.Clone() as TernarySearchTreeNode<TKey, TValue>;
            if (entry.HighChild != null) entry.HighChild.Parent = entry;
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
                var keys = new List<TKey>();
                GetKeys(keys);
                return keys.AsReadOnly();
            }
        }

        //
        // The recursive getKeys function
        //
        private void GetKeys(ICollection<TKey> keys)
        {
            LowChild?.GetKeys(keys);
            if(IsKey){
                keys.Add(Key);
            }
            EqualChild?.GetKeys(keys);
            HighChild?.GetKeys(keys);
        }
        
        /// <summary>
        /// Returns a list of values in the tree rooted at this node
        /// </summary>
        public IList<TValue> TreeValues
        {
            get
            {
                var values = new List<TValue>();
                GetValues(values);
                return values.AsReadOnly();
            }
        }

        //
        // the recursive get values function
        //
        private void GetValues(ICollection<TValue> values)
        {
            LowChild?.GetValues(values);
            if (IsKey && Value!=null)
            {
                values.Add(Value);
            }
            EqualChild?.GetValues(values);
            HighChild?.GetValues(values);
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
                var list = new List<KeyValuePair<TKey, TValue>>();
                GetKeyValuePairs(list);
                return list.AsReadOnly();
            }
        }

        // 
        // recursive function to retrieve all key value pairs
        private void GetKeyValuePairs(ICollection<KeyValuePair<TKey, TValue>> list)
        {
            LowChild?.GetKeyValuePairs(list);
            if (IsKey)
            {
                list.Add(new KeyValuePair<TKey, TValue>(Key, Value));
            }
            EqualChild?.GetKeyValuePairs(list);
            HighChild?.GetKeyValuePairs(list);
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
            var s = key[index];
            if (s < SplitChar)
            {
                if (LowChild == null)
                {
                    LowChild = new TernarySearchTreeNode<TKey, TValue>(s, this);
                }
                return LowChild.Insert(key, index);
            }
            if (s > SplitChar)
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
            var results = new List<KeyValuePair<TKey, TValue>>();
            NearSearch(results, key, 0, distance);
            return results.AsReadOnly();
        }

        //
        // recursive nearSearch
        //
        private void NearSearch(ICollection<KeyValuePair<TKey, TValue>> results, string key, int keyIndex, int distance)
        {
            if (distance < 0) return;
            var c = key[keyIndex];
            if ((distance > 0 || c < SplitChar))
                LowChild?.NearSearch(results, key, keyIndex, distance);
            if ((IsKey) && ((key.Length - keyIndex) <= distance))
            {
                results.Add(new KeyValuePair<TKey, TValue>(Key, Value));
            }
            else
            {
                EqualChild?.NearSearch(results,
                    key,
                    (keyIndex != key.Length - 1) ? keyIndex + 1 : keyIndex,
                    (c == SplitChar) ? distance : distance - 1);
            }
            if ((distance > 0 || c > SplitChar))
            {
                HighChild?.NearSearch(results, key, keyIndex, distance);
            }
        }

        /// <summary>
        /// Partila match search starting at this node.
        /// </summary>
        /// <param name="keyPattern">The partial key pattern</param>
        /// <returns></returns>
        public IList<KeyValuePair<TKey, TValue>> PartialMatch(string keyPattern)
        {
            var results = new List<KeyValuePair<TKey, TValue>>();
            PartialMatch(results, keyPattern, 0);
            return results.AsReadOnly();
        }

        //
        // recursive partialMatch function
        //
        private void PartialMatch(List<KeyValuePair<TKey, TValue>> results, string keyPattern, int index)
        {
            var c = keyPattern[index];
            if (c == '*')
            {
                Glob(results, keyPattern, index + 1);
                return;
            }
            if ((c == '.' || c < SplitChar))
                LowChild?.PartialMatch(results, keyPattern, index);
            if (c == '.' || c == SplitChar)
            {
                if (index < keyPattern.Length - 1)
                {
                    EqualChild?.PartialMatch(results, keyPattern, index + 1);
                }
                else if (IsKey)
                    results.Add(new KeyValuePair<TKey, TValue>(Key, Value));
            }
            if ((c == '.' || c > SplitChar))
                HighChild?.PartialMatch(results, keyPattern, index);
        }

        //
        // match a wild card string.  Finds all remaining character nodes for the next
        // character in the key pattern, and continue the search. 
        //
        private void Glob(List<KeyValuePair<TKey, TValue>> results, string keyPattern, int index)
        {
            if (index == keyPattern.Length)
            {
                // The wilds card char is last in pattern
                // return all remaining keys value pairs
                results.AddRange(TreeKeyValuePairs);
                return;
            }
            if (keyPattern[index] == '*')
                Glob(results, keyPattern, index + 1);
            

            string newKeyPattern = keyPattern.Substring(index);
            char nextC = newKeyPattern[0];
            List<TernarySearchTreeNode<TKey, TValue>> nodes = new List<TernarySearchTreeNode<TKey, TValue>>();
            FindCharNodes(nodes, nextC);

            foreach (TernarySearchTreeNode<TKey, TValue> node in nodes)
            {
                node.PartialMatch(results, newKeyPattern, 0);
            }
        }

        //
        // recursive function to find all nodes with SplitChar equal to c
        //
        private void FindCharNodes(ICollection<TernarySearchTreeNode<TKey, TValue>> nodes, char c)
        {
            if (c == '.' || c == SplitChar)
            {
                nodes.Add(this);
            }
            LowChild?.FindCharNodes(nodes, c);
            EqualChild?.FindCharNodes(nodes, c);
            HighChild?.FindCharNodes(nodes, c);
        }
    }
}
