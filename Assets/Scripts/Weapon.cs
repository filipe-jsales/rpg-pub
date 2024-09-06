public abstract class Weapon : IRpgObject
{
    protected abstract void SetDurability(double value);
    protected abstract void SetDamage(double value);
    protected abstract void SetPoiseDamage(int value);
    
    public abstract void HandleDurabilityDamage(Armor armor);
    public abstract double HandlePhysicalDamage(double baseDamage);
    public abstract int HandlePoiseDamage();

    public double HealthFactor { set => SetDurability(value); }
    public double DamageFactor { set => SetDamage(value); }
    public int PoiseFactor { set => SetPoiseDamage(value); }
}
