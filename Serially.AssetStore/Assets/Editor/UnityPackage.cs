using UnityEditor;
using UnityEngine;

public static class UnityPackage
{
    const string packageName = "Serially";

    static readonly ExportPackageOptions exportOptions = ExportPackageOptions.Recurse
        | ExportPackageOptions.IncludeDependencies
        ;


    [MenuItem("Serially/Package")]
    public static void Build()
    {
        AssetDatabase.ExportPackage(
            assetPathName: "Assets/Serially",
            fileName: packageName + ".unitypackage",
            exportOptions
        );
    }
}
