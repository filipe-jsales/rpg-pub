using Abstractions;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterScriptableObject", order = 1)]
    public class CharacterScriptableObject : ScriptableObject
    {
        public Character Character;
    }
}