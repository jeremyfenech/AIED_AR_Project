using System.Linq;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class QuestionPicker : MonoBehaviour
{
    public int Correct_Ans = 0;
    public bool Question_Complete = false;
    public int Current_Question = 0;
    public int Question_Count = 0;

    private string[] Answers = new string[2];
    private IOrderedEnumerable<XmlNode> Randomized_Questions;


    // Start is called before the first frame update
    void Start()
    {
        TextAsset xmlFile = (TextAsset)Resources.Load("Questions/Questions");
        GameObject.FindGameObjectWithTag("question_text").GetComponent<TextMeshProUGUI>().text = "Question txt Started";
        XmlDocument doc = new();
        doc.LoadXml(xmlFile.text);

        var questionNodes = doc.SelectNodes("questions/question");
        Question_Count = questionNodes.Count;
        Debug.Log(Question_Count);
        Debug.Log(Current_Question);
        // Randomize the order of the answers
        var rnd = new System.Random();
        Randomized_Questions = questionNodes.OfType<XmlNode>().OrderBy(x => rnd.Next());
        GetQuestion();
    }

    public void GetQuestion()
    {
        Question_Complete = false;
        XmlNode questionNode = Randomized_Questions.ElementAt(Current_Question++);
        string questionText = questionNode.SelectSingleNode("text").InnerText;
        GameObject.FindGameObjectWithTag("question_text").GetComponent<TextMeshProUGUI>().text = questionText;

        XmlNodeList answerNodes = questionNode.SelectNodes("answers/answer");
        int i = 0;

        // Randomize the order of the answers
        var rnd = new System.Random();
        var randomizedAnswers = answerNodes.OfType<XmlNode>().OrderBy(x => rnd.Next());

        foreach (XmlNode answerNode in randomizedAnswers)
        {
            string answerText = answerNode.InnerText;
            Answers[i++] = answerText;
            bool isCorrect = answerNode.Attributes["correct"].Value == "true";
            Correct_Ans = isCorrect ? i : Correct_Ans;
        }
    }

    public void GameFinished(int Points)
    {
        string Score = (Points.ToString() + '/' + Question_Count.ToString());
        GameObject.FindGameObjectWithTag("question_text").GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
        GameObject.FindGameObjectWithTag("question_text").GetComponent<TextMeshProUGUI>().text = "Quiz Finished! You scored " + Score;

        GameObject.FindGameObjectWithTag("s_right").GetComponent<Renderer>().enabled = false;
        GameObject.FindGameObjectWithTag("ans_1").GetComponent<Renderer>().enabled = false;
        GameObject.FindGameObjectWithTag("s_left").GetComponent<Renderer>().enabled = false;
        GameObject.FindGameObjectWithTag("ans_2").GetComponent<Renderer>().enabled = false;
    }

    private void Update()
    {
        GameObject.FindGameObjectWithTag("ans_1").GetComponent<TextMeshPro>().text = Answers[0];
        GameObject.FindGameObjectWithTag("ans_2").GetComponent<TextMeshPro>().text = Answers[1];


    }
}
