using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    public static PlayerList PL;
    [HideInInspector] public List<Player> PlayersList = new List<Player>();

    private void Awake()
    {
        PL = this;
    }
}
