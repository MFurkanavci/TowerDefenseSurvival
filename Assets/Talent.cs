[System.Serializable]
public class Talent
{
    public string name;
    public string description;
    public int requiredLevel;
    public int requiredPointsInPreviousTalent;
    public bool isUnlocked;
    public bool isActive;

    // Add other properties like effects.
    public void Unlock()
    {
        isUnlocked = true;
    }

    public void Activate()
    {
        isActive = true;

        // Add effects.
        
    }
}
