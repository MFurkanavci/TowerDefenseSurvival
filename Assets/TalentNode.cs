using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentNode : MonoBehaviour
{
    public Talent talent;
    public TalentNode[] requiredTalents;
    public TalentNode[] nextTalents;

    Player player;

    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void Unlock()
    {
        talent.isUnlocked = true;
    }

    public void Activate()
    {
        talent.isActive = true;

        player.maxHealth += 30;
    }

    public void Deactivate()
    {
        talent.isActive = false;
    }
}
