//-----------------------------------------------------------------------------
// <copyright file="TernarySearchTree.cs">
//   Copyright (c) 2012 Adam W Adair. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace AWA.TernarySearchTree
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Dictionary of key/values pairs stored as a Ternary Search Tree.
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

    public class TernarySearchTree<TKey, TValue> : ICloneable
    {
        private TernarySearchTreeNode<TKey, TValue> _root;  // root node of the search tree

        public TernarySearchTree()
        {
            _root = null;
        }

        /// <summary>
        /// Inserts a key/value pair into the search tree.  This is similar to 
        /// the insert function in the reference implementation that uses
        /// recursion to traverse the tree to the insert point. 
        /// 
        /// One thing to note, is that if a duplicate key is inserted the 
        /// value on the existing key node is overwritten.  However, the 
        /// toString() method of the key value is used to determine the 
        /// node position, if two unique keys return duplicate toString values 
        /// then an exception will be raised.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Raised if key value is null;
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Raised if a the key string value is empty, or exists as a key in 
        /// the tree but for a nonequal key object.
        /// </exception>
        /// <param name="key">The key to insert into the tree</param>
        /// <param name="value">value associated with the key, null is allowed</param>
        public void Insert(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            var keyValue = key.ToString();
            if (keyValue.Length == 0)
                throw new ArgumentException("The key string value is an invalid key value.");

            if (_root == null)
                _root = new TernarySearchTreeNode<TKey, TValue>(keyValue[0]);

            var p = _root.Insert(keyValue);

            if (p.Key != null)
            {
                if (!p.Key.Equals(key))
                {
                    throw new ArgumentException("Key collision. A different key with identical string value exists in the tree.");
                }
            }

            p.Key = key;        // Sets the current node as a key node.
            p.Value = value;    // Associate a value with the key.
        }

        /// <summary>
        /// Search the tree for node.
        /// </summary>
        /// <param name="key">Key to search for</param>
        /// <returns>
        /// Returns the node found traversing the tree based on the characters 
        /// in key.  The node that is returned may or may not be a key node.
        /// </returns>
        private TernarySearchTreeNode<TKey, TValue> Search(string key)
        {
            if (key == null)
                return null;
            var n = key.Length;
            if (n == 0)
                return null;

            var p = _root;
            var index = 0;
            while (index < n && p != null)
            {
                var c = key[index];
                if (c < p.SplitChar)
                    p = p.LowChild;
                else if (c > p.SplitChar)
                    p = p.HighChild;
                else
                {
                    if (index == n - 1)
                        return p;
                    ++index;
                    p = p.EqualChild;
                }
            }
            return p;
        }

        /// <summary>
        /// Clears all nodes from the tree. The entire tree structure is removed.
        /// </summary>
        public void Clear()
        {
            _root?.Clear();
            _root = null;
        }

        /// <summary>
        /// Determines if there is a node in the tree that contains the key
        /// </summary>
        /// <param name="key">The key to search the tree for</param>
        /// <returns>true if the key is found.</returns>
        public bool ContainsKey(TKey key)
        {
            if (key == null) return false;
            TernarySearchTreeNode<TKey, TValue> node = Search(key.ToString());
            return (node != null && node.IsKey && node.Key.Equals(key));            
        }

        /// <summary>
        /// Searches the tree for a node by key.  If the node is found the key 
        /// is removed from the node. The node still remains in the tree, but 
        /// is no longer a key node.
        /// </summary>
        /// <param name="key">node key to search tree for</param>
        /// <returns>true if the node found and key removed, else false</returns>
        public bool RemoveKeyNode(TKey key)
        {
            if(key == null) return false;
            TernarySearchTreeNode<TKey, TValue> node = Search(key.ToString());
            if(node != null && node.IsKey && node.Key.Equals(key)){
                node.Key = default(TKey);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Searches tree by key and returns the corresponding value.
        /// </summary>
        /// <param name="key">search key</param>
        /// <returns>value</returns>
        public TValue GetValue(TKey key)
        {
            if (key == null) return default(TValue);
            var node = Search(key.ToString());
            return (node != null && node.IsKey && node.Key.Equals(key)) ? node.Value : default(TValue);            

        }

        /// <summary>
        /// A sorted list of all keys in the tree.
        /// </summary>
        public IList<TKey> Keys => _root != null ? _root.TreeKeys : new List<TKey>().AsReadOnly();

        /// <summary>
        /// The list of all non null values in the tree.
        /// </summary>
        public IList<TValue> Values => _root != null ? _root.TreeValues : new List<TValue>().AsReadOnly();

        /// <summary>
        /// Returns all key value pairs contained in the search tree ordered by key.
        /// </summary>
        public IList<KeyValuePair<TKey,TValue>> TreeKeyValuePairs => _root == null ? new List<KeyValuePair<TKey, TValue>>().AsReadOnly() : _root.TreeKeyValuePairs;

        /// <summary>
        /// Searches tree for a node based on the characters in keyPart.  
        /// A true result would mean that a key exists in the tree that starts 
        /// with the characters contained in keyPart.
        /// </summary>
        /// <param name="keyPart">A partial or complete key</param>
        /// <returns>true if found.</returns>
        public bool ContainsNode(string keyPart)
        {
            if (string.IsNullOrEmpty(keyPart)) return false;
            var p = Search(keyPart);
            return (p != null);
        }

        /// <summary>
        /// Balances the search tree. This method should be called if keys were 
        /// added to the tree in accending or decending order. This 
        /// implementation uses a simple method that first retrieves the sorted 
        /// list of key/value pairs and then uses recursive build function to 
        /// insert the middle string of its subarray, then recursively builds 
        /// the left and right subarrays.
        /// </summary>
        public void BalanceTree()
        {
            if(_root==null) return;
            var list = _root.TreeKeyValuePairs;
            if (list.Count == 0) return;
            Clear();
            BuildBalancedTree(list, 0, list.Count - 1);
        }

        /// <summary>
        /// Adds the items contained in the list to the search tree. If loading 
        /// a large set or ordered data, this is perhaps that best method to 
        /// use because it attempts to build a balanced search tree which will 
        /// optimize search performance.
        /// 
        /// The cost of inserting all words in a dictionary is never more than 
        /// about 10 percent greater than searching for all words.
        /// </summary>
        /// <param name="pairs">
        /// The list of key/value pairs to add to the search tree.
        /// </param>
        public void Insert(IList<KeyValuePair<TKey, TValue>> pairs)
        {
            BuildBalancedTree(pairs, 0, pairs.Count - 1);
        }

        //
        // The recursive build tree function inserts the middle string of its 
        // subarray, then recursively builds the left and right subarrays. 
        //
        private void BuildBalancedTree(IList<KeyValuePair<TKey, TValue>> nodesValues, int start, int end)
        {
            while (true)
            {
                if (nodesValues == null) throw new ArgumentNullException(nameof(nodesValues));
                if (start > end || end < 0) return;
                var mid = (end - start + 1) / 2;
                var pair = nodesValues[start + mid];
                Insert(pair.Key, pair.Value);
                BuildBalancedTree(nodesValues, start, start + mid - 1);
                start = start + mid + 1;
            }
        }

        /// <summary>
        /// Finds all words in the dictionary that are within a given Hamming 
        /// distance of key.
        /// </summary>
        /// <param name="key">
        /// Uses the string value of the key for query string
        /// </param>
        /// <param name="distance">
        /// Hamming distance between two strings of equal length is the number 
        /// of positions at which the corresponding symbols are different.
        /// </param>
        /// <returns>
        /// List of Key/Value pairs found by search.
        /// </returns>
        public IList<KeyValuePair<TKey, TValue>> NearSearch(TKey key, int distance)
        {
            return NearSearch(key.ToString(), distance);
        }

        /// <summary>
        /// Finds all key/value paires in the dictionary with keys within a given 
        /// Hamming distance of a query string.
        /// </summary>
        /// <param name="queryString">
        /// 
        /// </param>
        /// <param name="distance">
        /// Hamming distance between two strings of equal length is the number 
        /// of positions at which the corresponding symbols are different.
        /// </param>
        /// <returns>
        /// List of Key/Value pairs found by search.
        /// </returns>
        public IList<KeyValuePair<TKey, TValue>> NearSearch(string queryString, int distance)
        {
            if (_root != null && queryString != null && queryString.Length > 0) 
                return _root.NearSearch(queryString, distance);
            return new List<KeyValuePair<TKey, TValue>>().AsReadOnly();            
        }

        /// <summary>
        /// Searches the tree for key/value pairs using a key pattern.  The key 
        /// pattern may contain both regular letters and wild card characters.
        /// The wild card character '.' will match any character.
        /// The wild card character '*' will match any string of characters.
        /// A key pattern of ".a.a.a’ matches the word banana.  
        /// A key pattern of "ban*" also matches the word banana.
        /// </summary>
        /// <param name="keyPattern">The key pattern query string</param>
        /// <returns>List of Key/Value pairs found by search.</returns>
        public IList<KeyValuePair<TKey, TValue>> PartialKeySearch(string keyPattern)
        {
            if (_root != null && keyPattern != null && keyPattern.Length > 0)
                return _root.PartialMatch(keyPattern);
            return new List<KeyValuePair<TKey, TValue>>().AsReadOnly();            
        }

        #region ICloneable Members

        public object Clone()
        {
            var newTree = new TernarySearchTree<TKey, TValue>();
            if (_root != null)
            {
                newTree._root = (TernarySearchTreeNode<TKey,TValue>)_root.Clone();
            }
            return newTree;
        }

        #endregion
    }
}
