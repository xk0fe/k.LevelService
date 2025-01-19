using UnityEngine;
using UnityEngine.SceneManagement;

namespace k.LevelService.Common.Implementations
{
    [CreateAssetMenu(menuName = "k/Services/Levels/Implementations/" + nameof(SceneAssetLevel), fileName = nameof(SceneAssetLevel), order = 0)]
    public class SceneAssetLevel : BaseScriptableLevel
    {
        [SerializeField] private SceneReference _sceneAsset;
        [SerializeField] private LoadSceneMode _loadSceneMode;
        [SerializeField] private bool _isAsync;

        public override void Load()
        {
            base.Load();

            if (_isAsync)
            {
                SceneManager.LoadSceneAsync(_sceneAsset.SceneName, _loadSceneMode);
                return;
            }
            
            SceneManager.LoadScene(_sceneAsset.SceneName, _loadSceneMode);
        }
        
        public override void Unload()
        {
            base.Unload();
            SceneManager.UnloadSceneAsync(_sceneAsset.SceneName);
        }
    }
}