using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestionsResponse
{
    public int response_code;
    public List<Question> results;

    public static QuestionsResponse FromJson(string json) {
        return JsonUtility.FromJson<QuestionsResponse>(json);
    }
}
