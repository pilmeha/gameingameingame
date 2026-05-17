using UnityEngine;

public class game : MonoBehaviour
{
    public bool secretConditionMet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        secretConditionMet = true;
        if (secretConditionMet)
        {
            GameManager.Instance.zeldaKey = true;
        }
    }


}
