using Interfaces;
using UnityEngine;

namespace Abstractions
{
    public abstract class Character2D : Character, IAssetable<Texture2D>
    {
        protected abstract void SetAsset(Texture2D value);

        public Texture2D Asset
        {
            set => SetAsset(value);
        }
    }
}
