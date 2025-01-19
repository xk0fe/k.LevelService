using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace k.LevelService.Common
{
    [Serializable]
    public class SceneReference
    {
        [SerializeField] private Object _sceneAsset;
        [SerializeField] private string _sceneName;

#if UNITY_EDITOR
        private bool IsValidSceneAsset
        {
            get
            {
                if (_sceneAsset == null) return false;
                var assetPath = UnityEditor.AssetDatabase.GetAssetPath(_sceneAsset);
                return assetPath.EndsWith(".unity", StringComparison.OrdinalIgnoreCase);
            }
        }
#endif

        public string SceneName
        {
            get
            {
#if UNITY_EDITOR
                ValidateSceneName();
#endif
                return _sceneName;
            }
        }

        public void LoadScene(LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (string.IsNullOrEmpty(_sceneName))
            {
                Debug.LogError("Scene name is not set. Ensure you have assigned a valid scene.");
                return;
            }

            SceneManager.LoadScene(_sceneName, mode);
        }

#if UNITY_EDITOR
        private void ValidateSceneName()
        {
            if (IsValidSceneAsset)
            {
                var assetPath = UnityEditor.AssetDatabase.GetAssetPath(_sceneAsset);
                _sceneName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            }
            else
            {
                _sceneName = string.Empty;
            }
        }
#endif
    }
}