using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HadithDatabase", menuName = "Database/Hadith Database")]
public class HadithDatabase : ScriptableObject
{
    public List<HadithData> hadithList = new List<HadithData>();
}
