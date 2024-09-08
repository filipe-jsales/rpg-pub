using Abstractions;
using UnityEngine;

public class WeaponImpl : Weapon
{
    private double Durability { get; set; }
    private double Damage { get; set; }
    private int PoiseDamage { get; set; }
    private Sprite Sprite;

    public WeaponImpl(string name, double durability, double damage, int poiseDamage)
    { 
        Name = name;
        Init( durability, damage, poiseDamage);
    }

    private void Init(double durability, double damage, int poiseDamage)
    {
        SetDurability(durability);
        SetDamage(damage);
        SetPoiseDamage(poiseDamage);
    }

    public override void SetSprite(Sprite value)
    {
        Sprite = value;
    }

    public override Sprite GetSprite()
    {
        return Sprite;
    }

    public override double HandlePhysicalDamage(double baseDamage)
    {
        return Damage + baseDamage;
    }
    
    public override void HandleDurabilityDamage(Armor armor)
    {
        var attemptedDamage = armor.HandlePhysicalDamage(Damage);
        SetDurability(Durability - (Damage / attemptedDamage));
    }


    public override int HandlePoiseDamage()
    {
        return PoiseDamage;
    }


    protected override void SetDurability(double value)
    {
        Durability = value;
    }

    protected override void SetDamage(double value)
    {
        Damage = value;
    }

    protected override void SetPoiseDamage(int value)
    {
        PoiseDamage = value;
    }
}
