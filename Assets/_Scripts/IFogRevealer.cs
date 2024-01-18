using UnityEngine;

namespace ReplayValue
{
    public interface IFogRevealer
    {
        public float ViewDistance { get; }
        public Vector3 Position { get; }
    }
}
