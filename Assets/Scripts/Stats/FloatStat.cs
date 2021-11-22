using UnityEngine;

namespace Entity
{
    [System.Serializable]
    public class FloatStat
    {
        [SerializeField]
        private float baseValue;

        public float BaseValue
        {
            get { return baseValue; }
            set { baseValue = value; }
        }

        public static implicit operator float(FloatStat stat)
        {
            return stat.BaseValue;
        }
    }
}
