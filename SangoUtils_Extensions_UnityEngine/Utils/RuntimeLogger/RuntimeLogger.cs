using System;
using System.Collections.Generic;
#if UNITY_ENV
using UnityEngine;

namespace SangoUtils_Extensions_UnityEngine.Utils
{
    public class RuntimeLogger : MonoBehaviour
    {
        #region Config
        private const int _maxLinesPerPage = 50;
        private const int _maxStrLengthPerLine = 80;
        private const int _fontSize = 25;
        private readonly Color _fontColor = Color.blue;
        #endregion

        private string _logStr = "";
        private readonly List<string> _lines = new List<string>();

        void OnEnable() { Application.logMessageReceived += Log; }
        void OnDisable() { Application.logMessageReceived -= Log; }

        public void Log(string logString, string stackTrace, LogType type)
        {
            foreach (var line in logString.Split('\n'))
            {
                if (line.Length <= _maxStrLengthPerLine)
                {
                    _lines.Add(line);
                    continue;
                }
                var lineCount = line.Length / _maxStrLengthPerLine + 1;
                for (int i = 0; i < lineCount; i++)
                {
                    if ((i + 1) * _maxStrLengthPerLine <= line.Length)
                    {
                        _lines.Add(line.Substring(i * _maxStrLengthPerLine, _maxStrLengthPerLine));
                    }
                    else
                    {
                        _lines.Add(line.Substring(i * _maxStrLengthPerLine, line.Length - i * _maxStrLengthPerLine));
                    }
                }
            }
            if (_lines.Count > _maxLinesPerPage)
            {
                _lines.RemoveRange(0, _lines.Count - _maxLinesPerPage);
            }
            _logStr = string.Join("\n", _lines);
        }

        private void OnGUI()
        {
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
               new Vector3(1f, 1f, 1.0f));
            GUI.Label(new Rect(10, 10, 600, 370), _logStr, new GUIStyle() { fontSize = Math.Max(10, _fontSize), normal = new GUIStyleState() { textColor = _fontColor } });
        }
    }
}
#endif