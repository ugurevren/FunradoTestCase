using UnityEngine;

public interface ICharacter
{
    void Combat(Collider other);
    void UpdateLevel(int level);
}