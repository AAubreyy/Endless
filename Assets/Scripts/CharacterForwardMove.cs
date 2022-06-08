using StarterAssets;
using UnityEngine;

public class CharacterForwardMove : MonoBehaviour
{
    [Range(0, 10)] [SerializeField] private float staticCharacterSpeed = 1f;
    [SerializeField] private ThirdPersonController thirdPersonController;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        thirdPersonController.MoveForward(staticCharacterSpeed);
    }
}