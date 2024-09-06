public abstract class MagicalArmor : Armor, IMagicalRpgObject
{
    protected abstract void SetMagicResistance(double value);
    protected abstract void SetResistanceElement(string value);
    
    public abstract int HandleMagicalDamage(int damage);
    
    public double MagicFactor { set => SetMagicResistance(value); }
    public string ElementalFactor { set => SetResistanceElement(value); }
}
