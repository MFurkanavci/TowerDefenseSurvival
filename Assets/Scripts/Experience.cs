using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    public int experience;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out Player player))
        {
            player.AddExperience(experience);
            if (player.IsLevelUp()) player.LevelUp();
            ObjectPooler.Instance.ReturnObject(gameObject, gameObject);
        }
    }
}
