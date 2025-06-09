#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class CreateHaditsAsset
{
    [MenuItem("Tools/Create Hadits Arbain Asset")]
    public static void CreateHadits()
    {
        HaditsSO asset = ScriptableObject.CreateInstance<HaditsSO>();

        asset.id = 1;
        asset.judul = "Hadits Niat";
        asset.arab = "إِنَّمَا الأَعْمَالُ بِالنِّيَّاتِ...";
        asset.indo = "Dari Amirul Mukminin Abu Hafsh Umar bin Al Khaththab...";
        asset.inggris = "Actions are judged by intentions...";

        // contoh menambah audio (harus punya AudioClip di project)
        // asset.audios.Add(new HaditsAudio { id = 1, reciter = "Abdul Basit", audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/abdul_basit_001.wav") });

        string path = "Assets/Resources/HaditsArbainAsset.asset";
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();

        Debug.Log("Created Hadits Arbain asset at " + path);
    }
}
#endif
