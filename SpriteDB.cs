using System.Collections.Generic;
using UnityEngine;

public class SpriteDB : MonoBehaviour
{
    public static SpriteDB Instance;

    [SerializeField]
    public List<Sprite> entitySprites = new List<Sprite>();
    [SerializeField]
    public List<Sprite> encounterSprites = new List<Sprite>();
    [SerializeField]
    public List<Sprite> itemSprites = new List<Sprite>();
    [SerializeField]
    public List<Sprite> statusSprites = new List<Sprite>();

    private void Awake()
    {
        Instance = this;
    }
}
