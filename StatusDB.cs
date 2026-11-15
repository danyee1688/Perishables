using System.Collections.Generic;
using UnityEngine;

public class StatusDB 
{
    public static List<Status> statusList = new List<Status>()
    {
        new Status(0, "Strength", StatusType.Stacking, StatusPolarity.Buff, 4),
    };
}
