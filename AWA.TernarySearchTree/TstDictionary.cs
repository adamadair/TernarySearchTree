//-----------------------------------------------------------------------------
// <copyright file="TstDictionary.cs">
//   Copyright (c) 2012 Adam W Adair. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace AWA.TernarySearchTree
{
    /// <summary>
    /// Represents a collection of keys and values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <remarks>
    /// The TstDictionary<TKey, TValue> generic class provides a mapping from 
    /// a set of keys to a set of values. The class is a wrapper of the 
    /// TernarySearchTree class and implements the IDictionary interface.
    /// </remarks>    
    public class TstDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICloneable
    {
        private TernarySearchTree<TKey, TValue> tree;

        public TstDictionary()
        {           
            tree = new TernarySearchTree<TKey,TValue>();
        }

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            tree.Insert(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return tree.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return tree.Keys; }
        }

        public bool Remove(TKey key)
        {
            return tree.RemoveKeyNode(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            try
            {
                value = tree.GetValue(key);
                return true;
            }
            catch { }
            return false;
        }

        public ICollection<TValue> Values
        {
            get { return tree.Values; }
        }

        public IList<KeyValuePair<TKey, TValue>> KeyValuePairs
        {
            get
            {
                return tree.TreeKeyValuePairs;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return tree.GetValue(key);
            }
            set
            {
                tree.Insert(key, value);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            tree.Insert(item.Key, item.Value);
        }

        public void Clear()
        {
            tree.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value = tree.GetValue(item.Key);
            if (value == null) return false;
            return value.Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<TKey, TValue> item in tree.TreeKeyValuePairs)
            {
                array[arrayIndex++] = item;
            }
        }

        public int Count
        {
            get { return tree.TreeKeyValuePairs.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Contains(item))
            {
                tree.RemoveKeyNode(item.Key);
                return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return tree.TreeKeyValuePairs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            TstDictionary<TKey,TValue> newDictionary = new TstDictionary<TKey, TValue>();
            newDictionary.tree = (TernarySearchTree<TKey, TValue>)this.tree.Clone();
            return newDictionary;
        }

        #endregion

        #region TST Search Functions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyPart"></param>
        /// <returns></returns>
        public bool PartialKeyExists(string keyPart)
        {
            return tree.ContainsNode(keyPart);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BalanceSearchTree()
        {
            tree.BalanceTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyPattern"></param>
        /// <returns></returns>
        public IList<KeyValuePair<TKey, TValue>> PartialKeyMatch(string keyPattern)
        {
            return tree.PartialKeySearch(keyPattern);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public IList<KeyValuePair<TKey, TValue>> NearSearch(TKey key, int distance)
        {
            return tree.NearSearch(key, distance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public IList<KeyValuePair<TKey, TValue>> NearSearch(string key, int distance)
        {
            return tree.NearSearch(key, distance);
        }

        #endregion

    }
}
