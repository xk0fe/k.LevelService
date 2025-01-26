using k.LevelService.Common.Implementations;
using UnityEngine;

namespace k.LevelService.Common.Configs {
    [CreateAssetMenu(menuName = "k/Services/Levels/" + nameof(LevelsConfig), fileName = nameof(LevelsConfig),
        order = 0)]
    public class LevelsConfig : ScriptableObject {
        [SerializeField] private BaseScriptableLevel[] _levels;
        [SerializeField] private bool _repeatLevels;

        public BaseScriptableLevel[] Levels => _levels;
        public bool RepeatLevels => _repeatLevels;
    }
}