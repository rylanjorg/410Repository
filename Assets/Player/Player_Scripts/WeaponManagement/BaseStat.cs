using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat : MonoBehaviour
{
    public List<StatBonus> BaseAdditives { get; set; }
    public int BaseValue { get; set; }
    public string StatName { get; set; }
    public string StatDescription { get; set; }
    public int FinalValue { get; set; }
}
