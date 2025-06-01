using UnityEngine;

namespace com.game.utilities
{
    public interface IObject
    {
        public Transform transform { get; }
        public GameObject gameObject { get; }
    }
}
