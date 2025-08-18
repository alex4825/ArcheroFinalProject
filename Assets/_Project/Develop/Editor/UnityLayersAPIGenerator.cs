using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Globalization;

namespace Assets._Project.Develop.Editor
{
    public class UnityLayersAPIGenerator
    {
        private const string AssemblyName = "Assembly-CSharp";

        private static string OutputPath
            => Path.Combine(Application.dataPath, "_Project/Develop/Runtime/Utilities/DataManagement/KeysStorage/UnityLayers.cs");


        [InitializeOnLoadMethod]
        [MenuItem("Tools/GenerateUnityLayersAPI")]
        private static void Generate()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"using UnityEngine;");
            sb.AppendLine();

            sb.AppendLine($"public static class UnityLayers");
            sb.AppendLine("{");

            Dictionary<string, string> layersToFormatted = GetLayersToFormatted();

            foreach (var layerToFormatted in layersToFormatted)
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                string capitalizedLayerName = textInfo.ToTitleCase(layerToFormatted.Key);

                sb.AppendLine($"\tpublic static readonly int Layer{layerToFormatted.Value}"
                    + $" = LayerMask.NameToLayer(\"{layerToFormatted.Key}\");");
            }

            sb.AppendLine();

            foreach (var layerToFormatted in layersToFormatted)
            {
                sb.AppendLine($"\tpublic static readonly int LayerMask{layerToFormatted.Value}"
                    + $" = 1 << Layer{layerToFormatted.Value};");
            }

            sb.AppendLine("}");

            File.WriteAllText(OutputPath, sb.ToString());

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private static Dictionary<string, string> GetLayersToFormatted()
        {
            List<string> layerNames = GetLayerNames();

            Dictionary<string, string> layersToFormatted = new(layerNames.Count);

            foreach (var layerName in layerNames)
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                string capitalizedLayerName = textInfo.ToTitleCase(layerName);

                string formattedLayerName = capitalizedLayerName.Replace(" ", "");

                layersToFormatted.Add(layerName, formattedLayerName);
            }

            return layersToFormatted;
        }

        private static List<string> GetLayerNames()
        {
            List<string> layerNames = new List<string>();

            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);

                if (string.IsNullOrEmpty(layerName))
                    continue;

                layerNames.Add(layerName);
            }

            return layerNames;
        }
    }
}
