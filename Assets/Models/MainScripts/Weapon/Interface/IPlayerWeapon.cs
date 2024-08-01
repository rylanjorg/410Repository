using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;


public interface IPlayerWeapon 
{
    FSMMovement _FsmMovement { get; set; }
    PlayerDataManagement _PlayerDataManagement { get; set; }
}
