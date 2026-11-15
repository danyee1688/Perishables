using UnityEngine;

public class Status
{
    private int statusId;
    private string statusName;
    private string statusDescription;
    private int stacks;
    private StatusType statusType;
    private StatusPolarity polarity;
    private float baseDuration;
    private float duration;

    public int StatusId
    {
        get
        {
            return statusId;
        }
    }

    public string StatusName
    {
        get 
        { 
            return statusName; 
        }
    }

    public int Stacks
    {
        get
        {
            return stacks;
        }
        set
        {
            stacks = value;
        }
    }


    public float Duration
    {
        get
        {
            return duration;
        }
        set
        {
            duration = value;
        }
    }

    public float BaseDuration
    {
        get
        {
            return baseDuration;
        }
        set
        {
            baseDuration = value;
        }
    }

    public StatusType StatusType
    {
        get
        {
            return statusType;  
        }
    }

    public StatusPolarity StatusPolarity
    {
        get
        {
            return polarity;
        }
    }

    public Status(int statusId, string statusName, StatusType statusType, StatusPolarity statusPolarity, float baseDuration)
    {
        this.statusId = statusId;
        this.statusName = statusName;
        this.statusType = statusType;
        polarity = statusPolarity;
        this.baseDuration = baseDuration;
        this.duration = baseDuration;
    }

    public Status(Status status)
    {
        this.statusId = status.statusId;
        this.statusName = status.statusName;
        this.statusType = status.statusType;
        polarity = status.polarity;
        this.baseDuration = status.baseDuration;
        this.duration = baseDuration;
    }

    public override bool Equals(object other)
    {
        if (other == null)
        {
            return false;
        }

        Status status = other as Status;

        if (status.statusId == statusId)
        {
            return true;
        }

        return false;
    }

    public Sprite GetSprite()
    {
        if (statusId < SpriteDB.Instance.statusSprites.Count)
        {
            return SpriteDB.Instance.statusSprites[statusId];
        }

        return SpriteDB.Instance.statusSprites[0];
    }
}

[System.Serializable]
public enum StatusType
{
    Stacking,
    StackingAndRefreshing,
    Permanent,
    NonstackingAndRefreshing,
    Nonstacking
}

[System.Serializable]
public enum StatusPolarity
{
    Buff,
    Debuff,
}