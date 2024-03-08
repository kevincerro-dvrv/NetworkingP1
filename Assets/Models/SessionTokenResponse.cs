using System;
using UnityEngine;

[Serializable]
public class SessionTokenResponse
{
    public int response_code;
    public string response_message;
    public string token;

    public static SessionTokenResponse FromJson(string json) {
        return JsonUtility.FromJson<SessionTokenResponse>(json);
    }   
}
