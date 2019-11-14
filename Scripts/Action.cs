using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum selectedAnimation
{
    Buff,Debuff,Heal,Attack
};
public enum affectingStat
{
    Health,Speed
};
public enum target
{
    Self,Enemy
};

[System.Serializable]
public class Action
{
    public string name;
    public int cost;
    public string description;
    public selectedAnimation anim;
    public affectingStat effect;
    public bool drain;
    public int amount;
    public float time;
    public target target;

}