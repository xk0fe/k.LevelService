using UnityEngine;
using UnityEngine.SceneManagement;

namespace k.LevelService.Common.Implementations {
    [CreateAssetMenu(menuName = "k/Services/Levels/Implementations/" + nameof(SceneLevel),
        fileName = nameof(SceneLevel), order = 0)]
    public class SceneLevel : BaseScriptableLevel {
        [SerializeField] private int _sceneIndex;
        [SerializeField] private LoadSceneMode _loadSceneMode;
        [SerializeField] private bool _isAsync;

        public override void Load() {
            if (!Application.CanStreamedLevelBeLoaded(_sceneIndex)) {
                Debug.LogWarning($"Scene with index {_sceneIndex} cannot be loaded!", this);
                return;
            }

            if (_isAsync) {
                SceneManager.LoadSceneAsync(_sceneIndex, _loadSceneMode);
                return;
            }

            SceneManager.LoadScene(_sceneIndex, _loadSceneMode);
        }

        public override void Unload() {
            if (SceneManager.sceneCount <= 1) return;
            SceneManager.UnloadSceneAsync(_sceneIndex);
        }
    }
}