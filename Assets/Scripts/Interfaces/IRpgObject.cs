namespace Interfaces
{
    public interface IRpgObject
    {
        double HealthFactor { set; } // either actual health or durability
        double DamageFactor { set; } // either damage/damage modifier or resistance
        int PoiseFactor { set; } // either poise damage modifier or poise total modifier
    }
}