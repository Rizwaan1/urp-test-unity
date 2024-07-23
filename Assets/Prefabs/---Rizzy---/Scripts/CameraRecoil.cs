
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GunScript gunScript;
    [SerializeField] private BulletPerfect bulletPerfect;

    //Bools
    private bool isAimingrecoil;

    private Vector3 currentRotation;
    private Vector3 targetRotation;


   


    void Start()
    {
        
    }

    
    void Update()
    {
        isAimingrecoil = playerController.isAiming;


       targetRotation = Vector3.Lerp(targetRotation,Vector3.zero,gunScript.returnSpeed * Time.deltaTime);
       currentRotation = Vector3.Slerp(currentRotation , targetRotation , gunScript.snappiness * Time.deltaTime);
       transform.localRotation = Quaternion.Euler(currentRotation);
    }


    public void RecoilFire()
    {
        
        if (isAimingrecoil)
        {
            targetRotation += new Vector3(gunScript.recoilX, Random.RandomRange(-gunScript.aimrecoilY, gunScript.   aimrecoilY), Random.Range(-gunScript.aimrecoilZ, gunScript.aimrecoilZ));
            Debug.Log("isAimingAcc");
        }
        else
        {
           targetRotation += new Vector3(gunScript.recoilX, Random.RandomRange(-gunScript.recoilY, gunScript.recoilY), Random.Range(-gunScript.recoilZ, gunScript.recoilZ));
            Debug.Log("isHipFireAcc");

        }

    }


   
}
