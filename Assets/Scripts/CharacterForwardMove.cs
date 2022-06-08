using StarterAssets;
using UnityEngine;

public class CharacterForwardMove : MonoBehaviour
{
    [Range(0, 10)] [SerializeField] private float staticCharacterSpeed = 1f;
    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private bool  IsRunning;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    public void StartRunning()
    {
	IsRunning = true;
	Update();

    }

    public void Stop()
    {
	    IsRunning =false;
	    thirdPersonController.MoveForward(0);
		
    }

    private void Update()
    {
	    if(IsRunning){
		thirdPersonController.MoveForward(staticCharacterSpeed);
	    }
	    else{
		    Stop();

	    }
    }
}
