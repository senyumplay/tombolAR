using UnityEngine;

[System.Serializable]
public class HadithData
{
    public string id;
    public string judul;

    [TextArea(3, 10)]
    public string arab;
    [TextArea(3, 10)]
    public string indo;
    [TextArea(3, 10)]
    public string inggris;

    public AudioClip[] audioClips;
}
