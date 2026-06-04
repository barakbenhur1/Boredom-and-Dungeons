#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools
{
    public static class BDRuntimeEditorDependencyValidator
    {
        [MenuItem("Boredom & Dungeons/Validation/Check Runtime UnityEditor References")]
        public static void CheckRuntimeUnityEditorReferences()
        {
            string runtimeRoot = "Assets/_Project/Scripts/Runtime";
            List<string> offenders = new List<string>();

            if (Directory.Exists(runtimeRoot))
            {
                foreach (string path in Directory.GetFiles(runtimeRoot, "*.cs", SearchOption.AllDirectories))
                {
                    string text = File.ReadAllText(path);
                    if (text.Contains("using UnityEditor") || text.Contains("UnityEditor."))
                        offenders.Add(path);
                }
            }

            if (offenders.Count == 0)
            {
                Debug.Log("B&D validation passed: no UnityEditor references found in Runtime scripts.");
                return;
            }

            Debug.LogError("B&D validation failed: Runtime scripts reference UnityEditor:\n" + string.Join("\n", offenders));
        }
    }
}
#endif
