using UnityEngine;

[CreateAssetMenu(fileName = "NewHadithContent", menuName = "Content/HadithContentData")]
public class HadithContentData : ScriptableObject
{
    public string judulHaditsID;
    public string judulHaditsEN;
    public Sprite isiHaditsImage;
    public string translateID;
    public string translateEN;
    public AudioClip audioClip;
}
