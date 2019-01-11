using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Account {

    public int Userid;

    public string Username;

    public string Password;

    public Account(int userid,string username,string password)
    {
        this.Userid = userid;
        this.Username = username;
        this.Password = password;
    }
}
