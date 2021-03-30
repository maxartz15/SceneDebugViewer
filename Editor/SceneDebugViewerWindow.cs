using UnityEngine;
using UnityEditor;

namespace TAO.SceneDebugViewer.Editor
{
    public class SceneDebugViewerWindow : EditorWindow
    {
        [MenuItem("Window/SceneDebugViewer")]
        static void Init()
        {
            SceneDebugViewerWindow window = (SceneDebugViewerWindow)EditorWindow.GetWindow(typeof(SceneDebugViewerWindow));
            window.Show();
        }

        private void OnGUI()
        {
            
        }
    }
}
