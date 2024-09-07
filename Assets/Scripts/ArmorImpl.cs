using Abstractions;
using UnityEngine;

public class ArmorImpl : Armor
{
    private enum ArmorCondition { Pristine, Damaged, Ineffective }
    private double Durability { get; set; }
    private double PhysicalResistance { get; set; }
    private int Poise { get; set; }

    public ArmorImpl(string name, double durability, double physicalResistance, int poise)
    {
        Name = name;
        Init(durability, physicalResistance, poise);
    }

    private void Init(double durability, double physicalResistance, int poise)
    {
        SetDurability(durability);
        SetPhysicalResistance(physicalResistance);
        SetPoise(poise);
    }

    private ArmorCondition Condition => Durability >= 66 ? ArmorCondition.Pristine : Durability >= 33 ? ArmorCondition.Damaged : ArmorCondition.Ineffective;
    private int ArmorConditionBonus => Condition == ArmorCondition.Pristine ? 5 : ArmorCondition.Damaged == Condition ? 3 : 0;

    public override double HandlePhysicalDamage(double damage)
    {
        var actualDamage = (damage - ArmorConditionBonus) * (PhysicalResistance / 100);
        Debug.Log("Actual Damage: " + actualDamage);
        return actualDamage < 0 ? 0 : actualDamage;
    }
    
    public override void HandleDurabilityDamage(double damage)
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


    protected override void SetDurability(double value)
    {
        Durability = value;
    }

    protected override void SetPhysicalResistance(double value)
    {
        PhysicalResistance = value;
    }

    protected override void SetPoise(int value)
    {
        Poise = value;
    }
}
