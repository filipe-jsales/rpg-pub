using UnityEngine;

public class TestWeapon : Weapon
{
    
    private double Durability { get; set; }
    private double Damage { get; set; }
    private int PoiseDamage { get; set; }

    public TestWeapon(double durability, double damage, int poiseDamage)
    {
       Init(durability, damage, poiseDamage);
    }

    private void Init(double durability, double damage, int poiseDamage)
    {
        SetDurability(durability);
        SetDamage(damage);
        SetPoiseDamage(poiseDamage);
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
