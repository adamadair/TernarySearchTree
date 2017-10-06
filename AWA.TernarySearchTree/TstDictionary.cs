//-----------------------------------------------------------------------------
// <copyright file="TstDictionary.cs">
//   Copyright (c) 2012 Adam W Adair. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace AWA.TernarySearchTree
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Represents a collection of keys and values.
    /// </summary>
    public class TstDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICloneable
    {
        private TernarySearchTree<TKey, TValue> _tree;

        public TstDictionary()
        {           
            _tree = new TernarySearchTree<TKey,TValue>();
        }

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            _tree.Insert(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return _tree.ContainsKey(key);
        }

        public ICollection<TKey> Keys => _tree.Keys;

        public bool Remove(TKey key)
        {
            return _tree.RemoveKeyNode(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            try
            {
                value = _tree.GetValue(key);
                return true;
            }
            catch
            {
                // ignored
            }
            return false;
        }

        public ICollection<TValue> Values => _tree.Values;

        public IList<KeyValuePair<TKey, TValue>> KeyValuePairs => _tree.TreeKeyValuePairs;

        public TValue this[TKey key]
        {
            get => _tree.GetValue(key);
            set => _tree.Insert(key, value);
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _tree.Insert(item.Key, item.Value);
        }

        public void Clear()
        {
            _tree.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var value = _tree.GetValue(item.Key);
            return value != null && value.Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var item in _tree.TreeKeyValuePairs)
            {
                array[arrayIndex++] = item;
            }
        }

        public int Count => _tree.TreeKeyValuePairs.Count;

        public bool IsReadOnly => false;

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!Contains(item)) return false;
            _tree.RemoveKeyNode(item.Key);
            return true;
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _tree.TreeKeyValuePairs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            var newDictionary =
                new TstDictionary<TKey, TValue> {_tree = (TernarySearchTree<TKey, TValue>) _tree.Clone()};
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
            return _tree.ContainsNode(keyPart);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BalanceSearchTree()
        {
            _tree.BalanceTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyPattern"></param>
        /// <returns></returns>
        public IList<KeyValuePair<TKey, TValue>> PartialKeyMatch(string keyPattern)
        {
            return _tree.PartialKeySearch(keyPattern);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public IList<KeyValuePair<TKey, TValue>> NearSearch(TKey key, int distance)
        {
            return _tree.NearSearch(key, distance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public IList<KeyValuePair<TKey, TValue>> NearSearch(string key, int distance)
        {
            return _tree.NearSearch(key, distance);
        }

        #endregion

    }
}
