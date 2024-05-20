using UnityEditor;
using UnityEngine;

namespace SangoUtils.Engines_Unity.Editor
{
    public class SessionsHelperEditor
    {
        [MenuItem("GameObject/SangoUtils/Sessions/AddInteractableObjectSession", false, 10)]
        private static void AddInteractableObjectSession()
        {
            GameObject session = new GameObject("InteractableObjectSession");
            if (Selection.activeTransform != null)
            {
                session.transform.SetParent(Selection.activeTransform);
            }
            Undo.RegisterCreatedObjectUndo(session, "Add InteractableObjectSession");
            Selection.activeObject = session;
            session.AddComponent<InteractableObjectSession>();
        }

        [MenuItem("GameObject/SangoUtils/Sessions/AddRecognizableObjectSession", false, 10)]
        private static void AddRecognizableObjectSession()
        {
            GameObject session = new GameObject("RecognizableObjectSession");
            if (Selection.activeTransform != null)
            {
                session.transform.SetParent(Selection.activeTransform);
            }
            Undo.RegisterCreatedObjectUndo(session, "Add RecognizableObjectSession");
            Selection.activeObject = session;
            session.AddComponent<RecognizableObjectSession>();
        }
    }
}
