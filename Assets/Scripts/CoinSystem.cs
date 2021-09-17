using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinSystem : MonoBehaviour
{
   
    [SerializeField] private ParticleSystem PickUpParticles;
    private Renderer render;
    public int actualScore;


    private void Start() {
        PickUpParticles.Pause();
        render=GetComponentInChildren<Renderer>();
        actualScore=0;
        //actualScore=0;
    }
    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            PlayerMoney.Instance.AddMoney(1);
            if(PickUpParticles.isPaused){
                actualScore++;
                PickUpParticles.Play();
                render.enabled=false;
                //actualScore++;
                Destroy(this.gameObject , 0.8f);
            }
                
        }
    }
}
