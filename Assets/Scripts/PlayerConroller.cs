using System.Collections;
using System.Collections.Generic;
using Unity.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConroller : MonoBehaviour

{
    private const string HORIZONTAL = "Horizontal";
    private float HorizontalInput;
    private int facingDirection;
    private Rigidbody rb;
    
    //Numbers
    [SerializeField] private float motorforce;
    [SerializeField] private float maxSteeringAngle;
    private bool IsDestroyed;



    //SCORE AND MONEY
    [SerializeField] public int actualScore;
    [SerializeField] public float actualDistance;
    public const string prefScore = "PrefScore";
    [SerializeField] public float HighScore;



    private float screenWidth;
    private float currentSteerAngle;
        //Speed Variables
        [SerializeField] private float fowardSpeed;
        private float MaxSpeed = 2.8f;
        

    //Wheel COlliders
    [SerializeField] private WheelCollider FrontLeftCollider;
    [SerializeField] private WheelCollider RearLeftCollider;
    [SerializeField] private WheelCollider FrontRightCollider;
    [SerializeField] private WheelCollider RearRightCollider;

    //Wheel Transforms
    [SerializeField] private Transform FrontLeftTransform;
    [SerializeField] private Transform FrontRightTransform;
    [SerializeField] private Transform RearLeftTransform;
    [SerializeField] private Transform RearRightTransform;

    //Post Proccesing
    public PostProcessVolume volume;
    private LensDistortion lensDistortion;
        //ParticleSystem
        [SerializeField] private ParticleSystem SpeedParticles;
        [SerializeField] private ParticleSystem SmokeParticles;
        [SerializeField] private ParticleSystem ExplosionParticles;
        [SerializeField] private ParticleSystem BrokenParticles;
    
    
    //UI
    public Text moneyText;
    public Text ScoreText;
    [SerializeField] public GameObject gameOverCanvas;




    private void Awake() 
    {
        HighScore=PlayerPrefs.GetFloat(prefScore); 
    }

    private void Start() {
        rb = this.GetComponent<Rigidbody>();
        screenWidth = Screen.width/2;
        fowardSpeed = 1.4f;
        volume.profile.TryGetSettings(out lensDistortion);
        SpeedParticles.Pause();
        SmokeParticles.Pause();
        ExplosionParticles.Pause();
        BrokenParticles.Pause();
        IsDestroyed=false;
        actualScore=0;
        gameOverCanvas.SetActive(false);
    }

    private void FixedUpdate() 
    {
        touchControl();
        if (IsDestroyed==false)
        {
            IncreaseSpeed();
            IncreaseLens();
            HandleMotor();
            UpdateWheels();
            HandleSteering();
            moneyText.text = actualScore.ToString();
            ScoreText.text = (this.transform.position.z + 128).ToString("0");
        }
       
        StartSpeedParticles();
    }
    
    
    private void HandleMotor()
    {
        FrontLeftCollider.motorTorque = motorforce * fowardSpeed;
        FrontRightCollider.motorTorque = motorforce * fowardSpeed;
        RearLeftCollider.motorTorque = motorforce * fowardSpeed;
        RearRightCollider.motorTorque = motorforce * fowardSpeed;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle/2 * facingDirection;
        FrontRightCollider.steerAngle = currentSteerAngle;
        FrontLeftCollider.steerAngle = currentSteerAngle;
    }


    private void UpdateWheels()
    {
        UpdateSingleWheel(FrontLeftCollider , FrontLeftTransform);
        UpdateSingleWheel(FrontRightCollider , FrontRightTransform);
        UpdateSingleWheel(RearLeftCollider , RearLeftTransform);
        UpdateSingleWheel(RearRightCollider , RearRightTransform);
        
    
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider , Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    void touchControl()
    {
        if (Input.touchCount > 0) 
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x < screenWidth) {
                facingDirection = -1;
            }

            else if (touch.position.x > screenWidth) {
                facingDirection = 1;
            }

        }
    }

    private void IncreaseSpeed() 
    {
        if (fowardSpeed < MaxSpeed) {
            fowardSpeed+= 0.08f * Time.deltaTime/2;
            
           
            
        }
        else return;
    }

    private void IncreaseLens()
    {
        if (fowardSpeed > 1.8f)
        {
            
            if (lensDistortion.intensity.value > -50.0f)
             {
                 lensDistortion.intensity.value-= 2.5f * Time.deltaTime;
             }
        }
    }

    private void StartSpeedParticles(){
        if (fowardSpeed >= 2.4f  && SpeedParticles.isPaused &&SmokeParticles.isPaused)
        {
            SmokeParticles.Play();
            SpeedParticles.Play();
    
        } 
        
        else return;
        
    }


    //Destroy the player
    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Obstacle") 
        {
            GameOver();
        }
        
    }

    private void OnTriggerEnter(Collider trigger) 
    {
        if (trigger.gameObject.tag == "Coin")
        {
            actualScore++;
        }
    }
    

    IEnumerator WaitForCrashSeconds()
        {
            yield return new WaitForSeconds(2);
        }


    public void CheckNewHighScore()
        {
            if (this.transform.position.z + 128 > HighScore)
            {
                //new higscore
                PlayerPrefs.SetFloat(prefScore , this.transform.position.z + 128);
            }

            else return;

        }

    public void GameOver()
    {
                    //Car System
            IsDestroyed=true;
            
            rb.constraints=RigidbodyConstraints.FreezePositionX;
            rb.constraints=RigidbodyConstraints.FreezePositionY;

            //Save Player Money and Check for new HighScore
            PlayerMoney.Instance.saveMoney();
            CheckNewHighScore();

           //Particles
            ExplosionParticles.Play(); 
            SpeedParticles.Stop();
            Destroy(ExplosionParticles, 0.4f);
            SmokeParticles.Stop();
            SmokeParticles.gameObject.SetActive(false);
            BrokenParticles.Play();

            //Show Game Over Canvas
            gameOverCanvas.SetActive(true);
    }
}
