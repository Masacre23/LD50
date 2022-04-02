using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    GASOIL,
    WOOD,
    HUMAN,
    DEFAULT
}

public class Item
{
    public float illumination = 0f;
    public ItemType type;
}