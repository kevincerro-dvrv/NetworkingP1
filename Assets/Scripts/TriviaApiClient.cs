using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class TriviaApiClient : MonoBehaviour
{
    private const string GetSessionTokenEndpoint = "https://opentdb.com/api_token.php?command=request";
    
    private const string GetQuestionsEndpoint = "https://opentdb.com/api.php?amount={0}&token={1}";

    private string sessionToken = null;
    private List<Question> questions = null;

    void Start()
    {
        StartCoroutine(GetSessionToken());
    }

    IEnumerator GetSessionToken()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(GetSessionTokenEndpoint))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error: " + webRequest.error);
            } else {
                string response = webRequest.downloadHandler.text;
                SessionTokenResponse sessionTokenResponse = SessionTokenResponse.FromJson(response);
                if (sessionTokenResponse.response_code == 0) {
                    sessionToken = sessionTokenResponse.token;

                    // TODO Move to an event so can be outside here
                    StartCoroutine(GetQuestions(10));
                }
            }
        }
    }

    IEnumerator GetQuestions(int amount)
    {
        object[] args = new object[] { amount, sessionToken};
        string url = string.Format(GetQuestionsEndpoint, args);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error: " + webRequest.error);
            } else {
                string response = webRequest.downloadHandler.text;
                QuestionsResponse questionsResponse = QuestionsResponse.FromJson(response);
                if (questionsResponse.response_code == 0) {
                    questions = questionsResponse.results;
                    PrintQuestions(amount, questions);
                }
            }
        }
    }

    private void PrintQuestions(int amount, List<Question> questions)
    {
        Debug.Log("Preguntas solicitadas: " + amount);

        foreach(Question question in questions) {
            Debug.Log("Pregunta: " + question.question);
            Debug.Log("Tipo: " + question.type);
            Debug.Log("Categoria: " + question.category);
            Debug.Log("Dificultad: " + question.difficulty);
            Debug.Log("Respuesta correcta: " + question.correct_answer);
        }
    }
}
