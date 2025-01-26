using System.Collections.Generic;
using k.LevelService.Common.Interfaces;
using UnityEngine;

namespace k.LevelService.Common {
    public class LevelIndexStorage {
        private readonly Dictionary<int, ILevel> _levelsByIndex = new();

        public bool RegisterLevel(int index, ILevel level) {
            if (_levelsByIndex.TryAdd(index, level)) return true;
            Debug.LogWarning($"Index {index} is already assigned to a level.");
            return false;
        }

        public bool UnregisterLevelByIndex(int index) {
            return _levelsByIndex.Remove(index, out var level);
        }

        public bool TryGetLevelByIndex(int index, out ILevel level) {
            level = null;
            _levelsByIndex.TryGetValue(index, out level);
            return level != null;
        }

        /// <summary>
        /// Slow method, prefer to use TryGetLevelByIndex
        /// </summary>
        public bool TryGetIndexByLevel(ILevel level, out int index) {
            index = -1;
            foreach (var kvp in _levelsByIndex)
                if (kvp.Value == level) {
                    index = kvp.Key;
                    return true;
                }

            return false;
        }

        public bool ContainsLevelIndex(int index) {
            return _levelsByIndex.ContainsKey(index);
        }

        public void Clear() {
            _levelsByIndex.Clear();
        }
    }
}