using UnityEngine;
[CreateAssetMenu(fileName = "Interactable",menuName ="InteractableType")]
public class InteractionType : ScriptableObject
{
    public float selectionRadius;
    public bool unselectable;
    public enum Type{OBJECT, STRUCTURE, ORGANIC, ENEMY, PLAYER}
    public Type type;

}
