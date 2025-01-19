using UnityEngine;

namespace k.LevelService.Common.Implementations
{
    [CreateAssetMenu(menuName = "k/Services/Levels/Implementations/" + nameof(MonoLevel), fileName = nameof(MonoLevel), order = 0)]
    public class MonoLevel : BaseScriptableLevel
    {
        [SerializeField] private GameObject _levelPrefab;
        
        private GameObject _levelInstance;
        
        public override void Load()
        {
            _levelInstance = Instantiate(_levelPrefab);
        }
        
        public override void Unload()
        {
            if (_levelInstance == null) return;
            Destroy(_levelInstance);
        }
    }
}