using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSwitchEditor
{
    [MenuItem("Scenes/MainMenu")]
    public static void LoadMainMenu()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    }

    [MenuItem("Scenes/Game")]
    public static void LoadGame()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    }
}
