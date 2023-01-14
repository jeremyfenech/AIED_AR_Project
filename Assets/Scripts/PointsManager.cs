using UnityEngine;

public class PointsManager : MonoBehaviour
{
    private HeadTiltChecker Head_Tilt_Checker;
    private QuestionPicker Question_Picker;

    private bool Correct = false;
    private int Points = 0;

    private Material Default_Colour;
    private Material Red;
    private Material Green;

    // Start is called before the first frame update
    void Start()
    {
        Default_Colour = Resources.Load<Material>("Materials/Default");
        Red = Resources.Load<Material>("Materials/Red");
        Green = Resources.Load<Material>("Materials/Green");

        GameObject ARSessionOrigin = GameObject.Find("AR Session Origin");
        Head_Tilt_Checker = ARSessionOrigin.GetComponent<HeadTiltChecker>();

        GameObject QuestionManager = GameObject.Find("QuestionManager");
        Question_Picker = QuestionManager.GetComponent<QuestionPicker>();
    }

    // Update is called once per frame
    void Update()
    {
        int tilt = Head_Tilt_Checker.Tilt_Side;
        int ans = Question_Picker.Correct_Ans;
        Material Material_Colour;

        if (tilt != 0)
        {
            Correct = (tilt == ans);
            Material_Colour = (tilt == ans) ? Green : Red;
            switch (tilt)
            {
                case 1:
                    SquareMaterialChanger(Default_Colour, Material_Colour);
                    break;
                case 2:
                    SquareMaterialChanger(Material_Colour, Default_Colour);
                    break;
            }
            Question_Picker.Question_Complete = true;
        }
        else
        {
            SquareMaterialChanger(Default_Colour, Default_Colour);

            if (Question_Picker.Current_Question <= Question_Picker.Question_Count)
            {
                if (Question_Picker.Question_Complete == true)
                {
                    Points += (Correct) ? 1 : 0;
                    Correct = false;
                    Question_Picker.GetQuestion();
                }
            }
            else
            {
                Question_Picker.GameFinished(Points);
            }

        }

    }

    void SquareMaterialChanger(Material Mat1, Material Mat2)
    {
        GameObject.FindGameObjectWithTag("s_right").GetComponent<Renderer>().material = Mat1;
        GameObject.FindGameObjectWithTag("s_left").GetComponent<Renderer>().material = Mat2;
    }
}
