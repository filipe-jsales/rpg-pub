using UnityEngine;
using UnityEngine.Events;

namespace Interfaces
{
    // TODO: add sprites here
    public interface IRpgObject
    {
        string Name {  get; set; }
        double HealthFactor { set; } // either actual health or durability
        // TODO add maxhealth
        double DamageFactor { set; } // either damage/damage modifier or resistance
        int PoiseFactor { set; } // either poise damage modifier or poise total modifier
        
        Sprite Sprite {  get; set; }
        UnityEvent OnInteract { get; set; }
    }
}