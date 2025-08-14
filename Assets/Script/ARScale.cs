using UnityEngine;
using TMPro;

public class ARScale : MonoBehaviour
{
    public TMP_Text text;
    public TMP_Text text2;

    public Vector3 scale;
    public float startDistance;

    public GameObject SObject;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Debug Data";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            text.text = "Touch Data \n";
            // for debugging
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                text.text += "\n i : " + i + " , " + touch.position.ToString();
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit, 100))
            {
                text2.text = hit.transform.tag;
                SObject = hit.transform.gameObject;
            }


            if(Input.touchCount >= 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                if(touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    startDistance = Vector2.Distance(touch0.position, touch1.position);
                    scale = SObject.transform.localScale;
                }
                else
                {
                    Vector2 v1 = touch0.position;
                    Vector2 v2 = touch1.position;

                    float distance = Vector2.Distance(v1, v2);
                    text.text += " \n Distance " + distance;

                    float factor = distance / startDistance;

                    SObject.transform.localScale = scale * factor;

                }


            }
        }
    }
}
