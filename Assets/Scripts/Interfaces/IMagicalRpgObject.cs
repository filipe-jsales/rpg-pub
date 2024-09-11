namespace Interfaces
{
    public interface IMagicalRpgObject
    {
        float MagicFactor { set; } // either damage or resistance modifier
        string ElementalFactor { set; }
    }
}