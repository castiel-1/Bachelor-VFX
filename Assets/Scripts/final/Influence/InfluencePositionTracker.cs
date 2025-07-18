using UnityEngine;

public class InfluencePositionTracker : MonoBehaviour
{
    public Influence Influence { get; set; }

    private void Update()
    {
        if(transform.position != Influence.Position)
        {
            // debugging
            Debug.Log("influence has moved");

            Influence.Position = transform.position;
        }
    }
}
