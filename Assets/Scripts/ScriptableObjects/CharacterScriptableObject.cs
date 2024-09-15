using System.Collections.Generic;
using Abstractions;
using Interfaces;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterScriptableObject", order = 1)]
    public class CharacterScriptableObject : ScriptableObject
    {
        public Character Character;
        public List<IRpgObject> Items;
    }
}