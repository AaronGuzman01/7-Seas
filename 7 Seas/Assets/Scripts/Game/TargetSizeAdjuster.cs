using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSizeAdjuster : MonoBehaviour
{

    private const float YSCALE = 5f;

    // Start is called before the first frame update
    void Start()
    {
        string difficulty = PlayerPrefs.GetString("Difficulty");
        Vector3 scale = gameObject.transform.localScale;
        switch (difficulty)
        {
            case "POWDER MONKEY":
                scale = gameObject.transform.localScale;
                scale.Set(4f, YSCALE, 4f);
                gameObject.transform.localScale = scale;
                Debug.Log(gameObject.transform.localScale.ToString());
                break;
            case "BOATSWAIN":
                scale = gameObject.transform.localScale;
                scale.Set(3f, YSCALE, 3f);
                gameObject.transform.localScale = scale;
                break;
            case "QUARTERMASTER":
                scale = gameObject.transform.localScale;
                scale.Set(2f, YSCALE, 2f);
                gameObject.transform.localScale = scale;
                break;
            case "CAPTAIN":
                scale = gameObject.transform.localScale;
                scale.Set(1f, YSCALE, 1f);
                gameObject.transform.localScale = scale;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
