using System;
using UnityEngine;

public class LittlePerson : MonoBehaviour
{
    [SerializeField] private Material matRed, matGreen, matBlue;

    public static event Action<SmallPersonsPlacer.EntityColorCode> OnColorTrigger;

    public SmallPersonsPlacer.EntityColorCode npcColorCode;

    private void OnTriggerEnter(Collider other)
    {
        switch (npcColorCode)
        {
            case SmallPersonsPlacer.EntityColorCode.Red:
                OnColorTrigger?.Invoke(SmallPersonsPlacer.EntityColorCode.Red);
                break;
            case SmallPersonsPlacer.EntityColorCode.Green:
                OnColorTrigger?.Invoke(SmallPersonsPlacer.EntityColorCode.Green);
                break;
            case SmallPersonsPlacer.EntityColorCode.Blue:
                OnColorTrigger?.Invoke(SmallPersonsPlacer.EntityColorCode.Blue);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (other.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }

    private void Update()
    {
        //I should really use object pooling for this but screw that.
        if ((transform.position - GameManager.PlayerTransform.position).magnitude > 200f)
        {
            Destroy(gameObject);
        }
    }

    public void SetColorCode(SmallPersonsPlacer.EntityColorCode type)
    {
        npcColorCode = type;
        gameObject.GetComponent<Renderer>().material = type switch
        {
            SmallPersonsPlacer.EntityColorCode.Red => matRed,
            SmallPersonsPlacer.EntityColorCode.Green => matGreen,
            SmallPersonsPlacer.EntityColorCode.Blue => matBlue,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}