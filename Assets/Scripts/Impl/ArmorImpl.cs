using System;
using Abstractions;
using UnityEngine;
using UnityEngine.Events;

namespace Impl
{
    public class ArmorImpl : Armor
    {
        public ArmorImpl(string name, Sprite sprite, UnityEvent onInteract, float durability, float maxDurability, float physicalResistance, int poise, int maxPoise)
        {
            Name = name;
            Sprite = sprite;
            OnInteract = onInteract;
            Durability = durability;
            MaxDurability = maxDurability;
            PhysicalResistance = physicalResistance;
            Poise = poise;
            MaxPoise = maxPoise;
            // TODO: create actual implementation
            ObtainedDate = DateTime.Now;
        }
    }
}

