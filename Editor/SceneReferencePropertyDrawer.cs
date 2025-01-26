using k.LevelService.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace k.LevelService.Editor {
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferencePropertyDrawer : PropertyDrawer {
        private const string SCENE_ASSET_PROPERTY = "_sceneAsset";
        private const string SCENE_NAME_PROPERTY = "_sceneName";

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var root = new VisualElement();

            var sceneAssetProperty = property.FindPropertyRelative(SCENE_ASSET_PROPERTY);
            var sceneAssetField = new PropertyField(sceneAssetProperty, property.displayName);
            root.Add(sceneAssetField);

            var sceneNameProperty = property.FindPropertyRelative(SCENE_NAME_PROPERTY);
            var sceneNameLabel = new Label { name = "scene-name-label" };
            root.Add(sceneNameLabel);

            var hasScene = !string.IsNullOrEmpty(sceneNameProperty.stringValue);
            var isSceneActive = SceneExistsInBuildSettings(sceneNameProperty.stringValue, out var buildIndex);

            var warningBox = new HelpBox(
                isSceneActive
                    ? $"Scene: {sceneNameProperty.stringValue} is active with build index: {buildIndex}"
                    : "This scene is not in your build settings.",
                isSceneActive
                    ? HelpBoxMessageType.None
                    : HelpBoxMessageType.Error) {
                name = "scene-warning-box",
                visible = hasScene
            };
            root.Add(warningBox);
            if (hasScene && !isSceneActive) {
                var addButton = new Button(() =>
                    AddSceneToBuildSettings(sceneAssetProperty.objectReferenceValue as SceneAsset)) {
                    text = "Add scene to build settings",
                    name = "add-scene-button"
                };
                root.Add(addButton);
            }

            return root;
        }

        private void AddSceneToBuildSettings(SceneAsset scene) {
            if (scene == null) return;
            var scenePath = AssetDatabase.GetAssetPath(scene);
            var scenes = EditorBuildSettings.scenes;
            var sceneToAdd = new EditorBuildSettingsScene(scenePath, true);
            ArrayUtility.Add(ref scenes, sceneToAdd);
            EditorBuildSettings.scenes = scenes;
        }

        private bool SceneExistsInBuildSettings(string sceneName, out int buildIndex) {
            var scenes = EditorBuildSettings.scenes;
            for (var i = 0; i < scenes.Length; i++) {
                var scene = scenes[i];
                if (scene.path.Contains(sceneName)) {
                    buildIndex = i;
                    return true;
                }
            }

            buildIndex = -1;
            return false;
        }
    }
}