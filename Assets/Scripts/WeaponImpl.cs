using Abstractions;
using UnityEngine;

public class WeaponImpl : Weapon
{
    private float Durability { get; set; }
    private float MaxDurability { get; set; }
    private float Damage { get; set; }
    private int PoiseDamage { get; set; }

    public WeaponImpl(string name, float durability, float damage, int poiseDamage)
    { 
        Name = name;
        Knockback = new Vector2(10f, 10f);
        Init( durability, damage, poiseDamage);
    }

    private void Init(float durability, float damage, int poiseDamage)
    {
        SetDurability(durability);
        SetDamage(damage);
        SetPoiseDamage(poiseDamage);
    }

    public override float HandlePhysicalDamage(float baseDamage)
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


    protected override void SetDurability(float value)
    {
        Durability = value;
    }
    
    protected override void SetMaxDurability(float value)
    {
        Durability = value;
    }

    protected override void SetDamage(float value)
    {
        Damage = value;
    }

    protected override void SetPoiseDamage(int value)
    {
        PoiseDamage = value;
    }
}
