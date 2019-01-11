using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OperatingDB : MonoBehaviour {

    private static OperatingDB m_instance;

    public static OperatingDB Instance
    {
        get
        {
            return m_instance;
        }
    }

    public DbAccess db;
    private string appDBPath;

    public void Awake()
    {
        m_instance = this;
    }

    //创建数据库
    public void CreateDataBase()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        appDBPath = Application.streamingAssetsPath + "/luhang.db";
#elif UNITY_ANDROID || UNITY_IPHONE
		appDBPath = Application.persistentDataPath + "/luhang.db";
		if(!File.Exists(appDBPath))
		{
			StartCoroutine(CopyDB());
		}
#endif
        // db = new DbAccess("URI=file:" + appDBPath);
        db = new DbAccess("data source=" + appDBPath);
    }

    IEnumerator CopyDB()
    {
        string loadPath = string.Empty;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        loadPath = Application.streamingAssetsPath + "/luhang.db";
#elif UNITY_ANDROID
		loadPath = "jar:file://" + Application.dataPath + "!/assets" + "/luhang.db";
#elif UNITY_IPHONE
		loadPath = + Application.dataPath + "/Raw" + "/luhang.db";
#endif
        WWW www = new WWW(loadPath);
        yield return www;
        File.WriteAllBytes(appDBPath, www.bytes);
    }

    public void SaveAccount(string username,string password)
    {
        CreateDataBase();
        //db.InsertInfo("T_Account", new string[] {"'" + username + "'","'" +  password + "'", '"' + DateTime.Now.ToString() + '"' });
        db.InsertIntoSpecific("T_Account", new string[] { "username", "password", "createtime" }, new string[] { "'" + username + "'", "'" + password + "'", '"' + DateTime.Now.ToString("s") + '"' });
        db.CloseSqlConnection();
    }

    //获取账号信息
    public Account GetRoleInfoDB(string username)
    {
        CreateDataBase();
        SqliteDataReader roleInfo = db.Select("T_Account", "username", username);
        Account account = null;
        while (roleInfo.Read())
        {
            account.Userid = int.Parse(roleInfo[0].ToString());
            account.Username = roleInfo[1].ToString();
            account.Password = roleInfo[2].ToString();
        }
        db.CloseSqlConnection();
        Debug.Log("account.password:" + account.Password);

        return account;
    }

    //修改密码
    public void ChangePassword(string username,string password)
    {
        CreateDataBase();
        db.UpdateInfo("T_Account", new string[] { "password" }, new string[] {password },
                        "usernmae", username);
        db.CloseSqlConnection();
    }

    public void SaveCheckItemData(int checkItemId,int userid,int answer,int airplaneId,DateTime dateTime)
    {
        CreateDataBase();
        db.InsertInfo("T_CheckItem", new string[] {checkItemId.ToString(),userid.ToString(),answer.ToString(),airplaneId.ToString(),'"'+ dateTime.ToString("s") + '"' });
        db.CloseSqlConnection();
    }

    public List<CheckItem> GetCheckItemData(int userId,int airplaneId)
    {
        List<CheckItem> list = new List<CheckItem>();
        CreateDataBase();
        SqliteDataReader reader = db.SelectWhere("T_CheckItem", new string[] {"checkitemid","answer","userid","airplaneid","time"}, new string[] { "userid", "airplaneid"}, new string[] { "=" ,"="}, new string[] { userId.ToString(), airplaneId.ToString()});
        while (reader.Read())
        {
            CheckItem item = new CheckItem();
            item.CheckItemId = int.Parse(reader["checkitemid"].ToString());
            item.Answer = int.Parse(reader["answer"].ToString());
            item.UserId = int.Parse(reader["userid"].ToString());
            item.AirPlaneId = int.Parse(reader["airplaneid"].ToString());
            item.Time = DateTime.Parse(reader["time"].ToString());
            list.Add(item);
        }
        db.CloseSqlConnection();
        return list;
    }

    public void UpdateCheckItemData(int checkItemId, int userid, int answer, int airplaneId, DateTime dateTime)
    {
        CreateDataBase();
        db.UpdateInfo("T_CheckItem",new string[] {"checkitemid","answer","userid","airplaneid","time" }, new string[] { checkItemId.ToString(), answer.ToString(), userid.ToString(), airplaneId.ToString(), '"' + dateTime.ToString("s") + '"' },"checkitemid","1");
        db.CloseSqlConnection();
    }

    public void DeleteCheckItem()
    {
        CreateDataBase();
        //db.Delete("T_CheckItem", new string[] { "checkitemid" }, new string[] { "10" });
        //db.DeleteContents("T_CheckItem");
        db.DeleteContents("T_Account");
        db.CloseSqlConnection();
    }
}
