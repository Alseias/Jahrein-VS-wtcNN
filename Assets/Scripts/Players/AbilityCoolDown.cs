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
    private bool btnTriggered;


    void Start() {
        tempDuration = durationTime;
    }

    public void Initialize( GameObject weaponHolder) {
        myButtonImage = GetComponent<Image>();
        abilitySource = GetComponent<AudioSource>();

        AbilityReady();
    }

    // Update is called once per frame
    void Update() {
        bool coolDownComplete = (Time.time > nextReadyTime);

        if(coolDownComplete) {
            itsReady = true;
            AbilityReady();
            if(Input.GetButtonDown(abilityButtonAxisName)&&!isPassive) {
                btnTriggered = true;
                ButtonTriggered();
            }else {
                if(passiveTriggered) {
                    passiveTriggered = false;
                    ButtonTriggered();
                }
            }
        } else {
            itsReady = false;
            CoolDown();
        }
        
        if(btnTriggered && !isPassive) {
            durationTime -= Time.deltaTime;
            if(durationTime <= 0) {
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
}