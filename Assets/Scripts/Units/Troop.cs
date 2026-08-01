using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTroop", menuName = "GGame/Units/Troop")]
public class Troop : ScriptableObject, Unit
{
    [SerializeField] private int _hp;
    [SerializeField] private int _power;
    [SerializeField] private int _shield;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private List<Resource> _costToCreate = new List<Resource>();
    [SerializeField] private int _overlandSpeed;

    public int hp => _hp;
    public int power => _power;
    public int shield => _shield;
    public Sprite sprite => _sprite;
    public ResourceCollection costToCreate => new ResourceCollection(_costToCreate);
    public int overlandSpeed => _overlandSpeed;

    public string GetCost()
    {
        string s = "";
        foreach (var resource in costToCreate)
        {
            s += resource + "\n";

        }
        return s;
    }

    public override string ToString()
    {
        return name;
    }
}
