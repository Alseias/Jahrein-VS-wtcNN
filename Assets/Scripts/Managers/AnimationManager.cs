using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimationManager : MonoBehaviour {

    public Text textArea;
    public string [] strings;
    public float speed = 0.1f;

    int stringIndex = 0;
    int characterIndex = 0;

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(DisplayTimer());
        textArea.DOText("We are a few game developers trying to make this gaming world a better place! Follow us from: perfectillusiongames.com", 2.3f, true, ScrambleMode.None, null);
    }

   /* IEnumerator DisplayTimer()
    {
        while(1 == 1)
        {
            yield return new WaitForSeconds(speed);
            
            if(characterIndex > strings[stringIndex].Length)
            {
                continue;
            }
            textArea.text = strings[stringIndex].Substring(0, characterIndex);
            characterIndex++;
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
