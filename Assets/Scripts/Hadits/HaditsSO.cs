using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HaditsSO", menuName = "Hadits/HaditsSO")]
public class HaditsSO : ScriptableObject
{
    public int id;
    public string judul;
    [TextArea(3, 10)] public string arab;
    [TextArea(3, 10)] public string indo;
    [TextArea(3, 10)] public string inggris;
    public List<HaditsAudio> audios = new List<HaditsAudio>();
}
