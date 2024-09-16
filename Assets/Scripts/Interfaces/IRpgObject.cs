using UnityEngine;
using UnityEngine.Events;

namespace Interfaces
{
    public interface IRpgObject
    {
        //Common name attributes
        string Name {  get; set; }
        
        // Unity attributes
        Sprite Sprite {  get; set; }
        UnityEvent OnInteract { get; set; }
        
        //Different name per implementation attributes
        protected void SetHealthFactor(float value);
        protected void SetMaxHealthFactor(float value);
        protected void SetPoiseFactor(float value);
        protected void SetMaxPoiseFactor(float value);
        protected void SetDamageFactor(float value);
    }
}