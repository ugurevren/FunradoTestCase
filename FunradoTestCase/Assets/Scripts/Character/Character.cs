using UnityEngine;

namespace Character
{
    public abstract class Character : MonoBehaviour
    {
        public int level;
        public FloatingText _floatingText;
        public GameObject _floatingTextObject;

        public void UpdateLevelText(int level)
        {
            _floatingText.SetText(level.ToString());
        }

        public abstract void Combat(Collider other);
    }
}