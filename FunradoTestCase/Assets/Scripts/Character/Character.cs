using UnityEngine;

namespace Character
{
    // This is the base class for all characters in the game.
    public abstract class Character : MonoBehaviour
    {
        public int level; // The level of the character.
        public FloatingText floatingText;  // The floating text object that shows the level of the character.
        public GameObject floatingTextObject;  // Game object that include floating text.

        public void UpdateLevelText(int level)
        {
            // Update the level of the character.
            floatingText.SetText(level.ToString());
        }

        public abstract void Combat(Collider other);
        // This method is called when the character is in combat.
    }
}