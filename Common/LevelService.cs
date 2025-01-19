using k.LevelService.Common.Configs;
using k.LevelService.Common.Interfaces;
using k.Services;
using UnityEngine;

namespace k.LevelService.Common
{
    [CreateAssetMenu(menuName = "k/Services/Levels/" + nameof(LevelService), fileName = nameof(LevelService),
        order = 0)]
    public class LevelService : GenericScriptableService<LevelService>
    {
        [SerializeField] private LevelsConfig _levelsConfig;

        private LevelController _levelController;
        private LevelIndexStorage _levelIndexStorage;

        private int _activeLevelIndex = DEFAULT_LEVEL_INDEX;
        private int _totalLevelLength = DEFAULT_LEVEL_INDEX;
        private const int DEFAULT_LEVEL_INDEX = -1;

        public int ActiveLevelIndex => _activeLevelIndex;
        public int TotalLevelLength => _totalLevelLength;

        public override void Initialize()
        {
            base.Initialize();
            _levelController = new LevelController();
            _levelIndexStorage = new LevelIndexStorage();

            if (_levelsConfig == null)
            {
                Debug.LogWarning($"{nameof(_levelsConfig)} is not set in {nameof(LevelService)}!", this);
                return;
            }

            if (_levelsConfig.Levels == null || _levelsConfig.Levels.Length == 0)
            {
                Debug.LogWarning($"{nameof(_levelsConfig.Levels)} is NULL or EMPTY in {nameof(LevelService)}!",
                    _levelsConfig);
                return;
            }

            for (var index = 0; index < _levelsConfig.Levels.Length; index++)
            {
                var level = _levelsConfig.Levels[index];
                _levelController.RegisterLevel(level);
                _levelIndexStorage.RegisterLevel(index, level);
            }

            _totalLevelLength = _levelsConfig.Levels.Length;
        }

        public bool LoadLevel(int index)
        {
            if (!_levelIndexStorage.TryGetLevelByIndex(index, out var level)) return false;
            if (!_levelController.LoadLevel(level)) return false;
            _activeLevelIndex = index;
            return true;
        }

        public bool LoadLevel(ILevel level)
        {
            if (!_levelController.LoadLevel(level)) return false;
            if (!_levelIndexStorage.TryGetIndexByLevel(level, out var index)) return false;
            _activeLevelIndex = index;
            return true;
        }

        public void NextLevel()
        {
            var nextLevelIndex = _activeLevelIndex + 1;
            if (_levelsConfig.RepeatLevels) nextLevelIndex %= _totalLevelLength;
            LoadLevel(nextLevelIndex);
        }

        /// <summary>
        /// Ensure to call this method before unloading the level. Otherwise, set changeActiveIndex to false.
        /// </summary>
        public bool UnloadLevel(int index, bool changeActiveIndex = true)
        {
            if (!_levelIndexStorage.TryGetLevelByIndex(index, out var level)) return false;
            if (!_levelController.UnloadLevel(level)) return false;
            if (changeActiveIndex) _activeLevelIndex = DEFAULT_LEVEL_INDEX;
            return true;
        }

        public bool UnloadActiveLevel()
        {
            if (!_levelController.UnloadActiveLevel()) return false;
            _activeLevelIndex = DEFAULT_LEVEL_INDEX;
            return true;
        }

        public void ReloadActiveLevel()
        {
            if (_activeLevelIndex == DEFAULT_LEVEL_INDEX) return;
            var activeLevelIndex = _activeLevelIndex;
            UnloadActiveLevel();
            LoadLevel(activeLevelIndex);
        }
        
        public void ReloadLevel(int index)
        {
            if (index == DEFAULT_LEVEL_INDEX) return;
            UnloadLevel(index);
            LoadLevel(index);
        }
    }
}