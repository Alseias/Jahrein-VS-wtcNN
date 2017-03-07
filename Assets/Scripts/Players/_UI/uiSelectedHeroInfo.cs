using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiSelectedHeroInfo : MonoBehaviour {
    public hero[] AllHeros;
    public hero jahrein;
    public Sprite[] jahImg;
    public hero wtcn;
    public Sprite[] wtcnImg;
	// Use this for initialization
	void Start () {
        jahrein = new hero();
        jahrein.heroName = "Jahrein";
        jahrein.skills = new string[5];
        jahrein.skills[0] = "Jahrage";
        jahrein.skills[1] = "Jahra2ge";
        jahrein.skills[2] = "Jahra1ge";
        jahrein.skills[3] = "Jahra4ge";
        jahrein.skillImages = new Sprite[4];
        jahrein.setSkillImage(jahImg);

        wtcn = new hero();
        wtcn.heroName = "wtcN";
        wtcn.skills = new string[5];
        wtcn.skills[0] = "Casper";
        wtcn.skills[1] = "Casper2";
        wtcn.skills[2] = "Casper1";
        wtcn.skills[3] = "Caspe3r";
        wtcn.skillImages = new Sprite[4];
        wtcn.setSkillImage(wtcnImg);

        AllHeros = new hero[2];
        AllHeros[0] = jahrein;
        AllHeros[1] = wtcn;




    }

    // Update is called once per frame
    void Update () {
		
	}
}

public struct hero {
    public string heroName;
    public string[] skills;
    public Sprite[] skillImages;
    public void setSkillImage(Sprite[] _img) {
        for(int i = 0; i < _img.Length; i++) {
            skillImages[i] = _img[i];
        }
    }
    
}
