using UnityEngine;

[CreateAssetMenu(menuName = "HarvestingGame/Fruit")]
public class FruitData : ScriptableObject
{
    public Sprite Sprite;
    public Sprite WhiteSprite;
    public float GravityScale;
    public int Value;
}
