using Abstractions;
using UnityEngine;

public class ArmorImpl : Armor
{
    private enum ArmorCondition { Pristine, Damaged, Ineffective }
    private float Durability { get; set; }
    private float MaxDurability { get; set; }
    private float PhysicalResistance { get; set; }
    private int Poise { get; set; }

    public ArmorImpl(string name, float durability, float physicalResistance, int poise)
    {
        Name = name;
        Init(durability, physicalResistance, poise);
    }

    private void Init(float durability, float physicalResistance, int poise)
    {
        SetDurability(durability);
        SetPhysicalResistance(physicalResistance);
        SetPoise(poise);
    }

    private ArmorCondition Condition => Durability >= 66 ? ArmorCondition.Pristine : Durability >= 33 ? ArmorCondition.Damaged : ArmorCondition.Ineffective;
    private int ArmorConditionBonus => Condition == ArmorCondition.Pristine ? 5 : ArmorCondition.Damaged == Condition ? 3 : 0;

    public override float HandlePhysicalDamage(float damage)
    {
        var actualDamage = (damage - ArmorConditionBonus) * ((100 - PhysicalResistance) / 100);
        return actualDamage < 0 ? 0 : actualDamage;
    }
    
    public override void HandleDurabilityDamage(float damage)
    {
        var durabilityDamage = HandlePhysicalDamage(damage) / 5;
        SetDurability(Durability - durabilityDamage);
        Debug.Log("Durability after attack: " + Durability);
    }


    public override void HandlePoiseDamage(Weapon weapon)
    {
        var poiseDamage = weapon.HandlePoiseDamage();
        Poise -= poiseDamage;
    }


    protected override void SetDurability(float value)
    {
        Durability = value;
    }
    
    protected override void SetMaxDurability(float value)
    {
        MaxDurability = value;
    }

    protected override void SetPhysicalResistance(float value)
    {
        PhysicalResistance = value;
    }

    protected override void SetPoise(int value)
    {
        Poise = value;
    }
}
