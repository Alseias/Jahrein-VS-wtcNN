using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityCoolDown : MonoBehaviour {

    public string abilityButtonAxisName = "Fire1";
    public Image darkMask;
    public Text coolDownTextDisplay;
    public float coolDownDuration,durationTime;
    public bool itsReady,durationEnd,isPassive,passiveTriggered;


    private GameObject weaponHolder;
    private Image myButtonImage;
    private AudioSource abilitySource;
    private float nextReadyTime;
    private float coolDownTimeLeft,tempDuration;
    private bool btnTriggered=false;
    private bool canUse=true;
    private Player target;

    public void setTarget(Player pc) {
        target = pc;
    }

    void Start() {
        tempDuration = durationTime;
        AbilityReady();
    }

    public void Initialize( GameObject weaponHolder) {
        myButtonImage = GetComponent<Image>();
        abilitySource = GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update() {
        
        bool coolDownComplete = (Time.time > nextReadyTime);

        if(coolDownComplete ) {
            itsReady = true;
            AbilityReady();
            
        } else {
            itsReady = false;
            CoolDown();
        }
        
        if(btnTriggered && !isPassive) {
            durationTime -= Time.deltaTime;
            if(durationTime <= 0) {
                //Debug.Log("duration end");

                btnTriggered = false;
                durationEnd = true;
                durationTime = tempDuration;
            } else {


                durationEnd = false;
            }
        }
    }
    
    private void AbilityReady() {
        coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
        
    }

    private void CoolDown() {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCd = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCd.ToString();
        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    private void ButtonTriggered() {
        
            nextReadyTime = coolDownDuration + Time.time;
            coolDownTimeLeft = coolDownDuration;
            darkMask.enabled = true;
            coolDownTextDisplay.enabled = true;
         
    }

    public void use() {
        if(!isPassive) {
            //Debug.Log("Button triggered");
            btnTriggered = true;
            ButtonTriggered();

        } else {
            if(passiveTriggered) {
                passiveTriggered = false;
                ButtonTriggered();
            }
        }
    }
}