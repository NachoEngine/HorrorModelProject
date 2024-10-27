using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Reflection;
public class CreateHorrorModel : MonoBehaviour
{
    static private string prefabPath;
    static public string filePath;

    //[MenuItem("Assets/Create Misc")]
    static public void BuildAssetBundle()
    {
        GameObject obj = Selection.activeGameObject;

        HorrorModelDescriptor horrormodel = obj.GetComponent<HorrorModelDescriptor>();
        if (horrormodel.NameTag == "" || horrormodel.model == null || horrormodel.Rig == null)
        {
            Debug.LogError($"HorrorModelDescriptor on {obj.name}: Make sure it has a name and that the Model and Rig objects are assigned.");
            return;
        }

        prefabPath = "Assets/Temp/" + "horrormodel" + ".prefab";
        filePath = "Assets/";

        if (!Directory.Exists("Assets/Temp"))
        {
            Directory.CreateDirectory("Assets/Temp");
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        var prefabAsset = PrefabUtility.SaveAsPrefabAsset(obj.gameObject, prefabPath);

        GameObject contentsRoot = PrefabUtility.LoadPrefabContents(prefabPath);

        contentsRoot.name = "HorrorModel";

        contentsRoot.GetComponent<HorrorModelDescriptor>().model.name = "model";

        contentsRoot.GetComponent<HorrorModelDescriptor>().Rig.name = "HorrorRig";


        Object.DestroyImmediate(contentsRoot.GetComponent<HorrorModelDescriptor>());
        PrefabUtility.SaveAsPrefabAsset(contentsRoot, prefabPath);
        PrefabUtility.UnloadPrefabContents(contentsRoot);

        AssetImporter.GetAtPath(prefabPath).SetAssetBundleNameAndVariant(horrormodel.NameTag, "");

        string assetBundleDirectory = "Assets/Export";

        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        if (File.Exists(prefabPath))
        {
            File.Delete(prefabPath);
            File.Delete(prefabPath + ".meta");
        }

        AssetDatabase.Refresh();

        AssetDatabase.Refresh();
        string oldfile = Path.Combine(assetBundleDirectory, horrormodel.NameTag.ToLower());
        string newfile = Path.Combine(assetBundleDirectory, horrormodel.NameTag.ToUpper());

        File.Move(oldfile, newfile);

        AssetDatabase.Refresh();

        string[] bundleFiles = Directory.GetFiles(assetBundleDirectory);
        foreach (string file in bundleFiles)
        {
            if (file.EndsWith(".manifest") || file.EndsWith("Export"))//file.EndsWith(horrormodel.NameTag))
            {
                File.Delete(file + ".meta");
                File.Delete(file);
                
            }
        }


        AssetDatabase.Refresh();


    }
}
