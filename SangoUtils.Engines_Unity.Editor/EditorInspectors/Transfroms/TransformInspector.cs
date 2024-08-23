using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SangoUtils.Behaviours_Unity.Editors
{
    [CustomEditor(typeof(Transform))]
    public class TransformInspector : Editor
    {
        private SerializedProperty _localPosition;
        private SerializedProperty _localRotation;
        private SerializedProperty _localScale;

        private Transform _targetComponent;

        private void OnEnable()
        {
            serializedObject.Update();

            _targetComponent = (Transform)target;

            _localPosition = serializedObject.FindProperty("m_LocalPosition");
            _localRotation = serializedObject.FindProperty("m_LocalRotation");
            _localScale = serializedObject.FindProperty("m_LocalScale");
        }

        public override void OnInspectorGUI()
        {
            EditorGUIUtility.labelWidth = 50;
            EditorGUIUtility.fieldWidth = 50;

            EditorGUI.BeginChangeCheck();

            EditorGUI.indentLevel = 0;
            Vector3 position = EditorGUILayout.Vector3Field("Position", _targetComponent.localPosition);
            Vector3 eulerAngles = EditorGUILayout.Vector3Field("Rotation", _targetComponent.localEulerAngles);
            Vector3 scale = EditorGUILayout.Vector3Field("Scale", _targetComponent.localScale);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_targetComponent, "Transform Change");

                _targetComponent.localPosition = FixIfNaN(ref position);
                _targetComponent.localEulerAngles = FixIfNaN(ref eulerAngles);
                _targetComponent.localScale = FixIfNaN(ref scale);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("SaveData"))
                SaveData(_targetComponent.gameObject);
            if (GUILayout.Button("LoadData"))
                LoadData(_targetComponent.gameObject);
            if (GUILayout.Button("ClearData"))
                ClearData();
            EditorGUILayout.EndHorizontal();
        }

        private Vector3 FixIfNaN(ref Vector3 v)
        {
            if (float.IsNaN(v.x))
                v.x = 0.0f;
            if (float.IsNaN(v.y))
                v.y = 0.0f;
            if (float.IsNaN(v.z))
                v.z = 0.0f;
            return v;
        }

        private string GetFolderPath()
        {
            string folderPath = Path.Combine(Path.GetTempPath(), "SangoUtilsTemp", "KeepTransformTemp");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        private void SaveData(GameObject baseObject)
        {
            List<string> saveDatas = new List<string>();

            saveDatas.Add(this.GetInstanceID().ToString());

            saveDatas.Add(baseObject.transform.localPosition.x.ToString());
            saveDatas.Add(baseObject.transform.localPosition.y.ToString());
            saveDatas.Add(baseObject.transform.localPosition.z.ToString());

            saveDatas.Add(baseObject.transform.localRotation.eulerAngles.x.ToString());
            saveDatas.Add(baseObject.transform.localRotation.eulerAngles.y.ToString());
            saveDatas.Add(baseObject.transform.localRotation.eulerAngles.z.ToString());

            saveDatas.Add(baseObject.transform.localScale.x.ToString());
            saveDatas.Add(baseObject.transform.localScale.y.ToString());
            saveDatas.Add(baseObject.transform.localScale.z.ToString());

            string filePath = Path.Combine(GetFolderPath(), baseObject.name + "_" + baseObject.GetInstanceID() + ".keepTransform.txt");
            File.WriteAllLines(filePath, saveDatas.ToArray());
        }

        private void LoadData(GameObject baseObject)
        {
            string filePath = Path.Combine(GetFolderPath(), baseObject.name + "_" + baseObject.GetInstanceID() + ".keepTransform.txt");
            if (!File.Exists(filePath))
                return;

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                _localPosition.vector3Value = new Vector3(System.Convert.ToSingle(lines[1]), System.Convert.ToSingle(lines[2]), System.Convert.ToSingle(lines[3]));
                _localRotation.quaternionValue = Quaternion.Euler(System.Convert.ToSingle(lines[4]), System.Convert.ToSingle(lines[5]), System.Convert.ToSingle(lines[6]));
                _localScale.vector3Value = new Vector3(System.Convert.ToSingle(lines[7]), System.Convert.ToSingle(lines[8]), System.Convert.ToSingle(lines[9]));

                File.Delete(filePath);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ClearData()
        {
            string[] files = Directory.GetFiles(GetFolderPath());
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
    }
}