using UnityEngine;

public class Pulse : MonoBehaviour
{
    public GameObject tile1;
    public GameObject tile2;
    public float r, g, b;
    public float alphaChange;

    private SpriteRenderer tileRend1;
    private SpriteRenderer tileRend2;
    private bool decrease;

    void Start()
    {
        tileRend1 = tile1.GetComponent<SpriteRenderer>();

        tileRend2 = tile2.GetComponent<SpriteRenderer>();

        decrease = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (decrease)
        {
            tileRend1.color = new Color(r, g, b, tileRend1.color.a - alphaChange);
            tileRend2.color = new Color(r, g, b, tileRend2.color.a - alphaChange);

            if (tileRend1.color.a <= 0)
            {
                decrease = false;
            }
        }
        else
        {
            tileRend1.color = new Color(r, g, b, tileRend1.color.a + alphaChange);
            tileRend2.color = new Color(r, g, b, tileRend2.color.a + alphaChange);

            if (tileRend1.color.a >= 0.2)
            {
                decrease = true;
            }
        }
    }
}
