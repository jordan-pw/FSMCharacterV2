using UnityEngine;

namespace Entity
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField]
        private int baseValue;

        public int BaseValue
        {
            get { return baseValue; }
            set { baseValue = value; }
        }

        public static implicit operator int(Stat stat)
        {
            return stat.BaseValue;
        }
    }
}
