using System;
using UnityEngine;

namespace MyFw
{
    [Serializable]
    public class PairGameObject<GameObjectType>
        where GameObjectType : MonoBehaviour
    {
        [SerializeField] private GameObjectType objectA;
        [SerializeField] private GameObjectType objectB;
        public GameObjectType Current { get; private set; }
        public GameObjectType Next { get; private set; }

        public void Setup()
        {
            this.Current = this.objectA;
            this.Next = this.objectB;
        }

        public GameObjectType Spawn()
        {
            (this.Next, this.Current) = (this.Current, this.Next);
            return this.Current;
        }

        public void SetActive(bool enable)
        {
            this.objectA.gameObject.SetActive(enable);
            this.objectB.gameObject.SetActive(enable);
        }
    }
}