using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HaditsColelction", menuName = "Hadits/HaditsCollection")]
public class HaditsCollection : ScriptableObject
{
    public List<HaditsSO> haditsList;
}
