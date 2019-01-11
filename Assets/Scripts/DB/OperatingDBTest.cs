using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatingDBTest : MonoBehaviour
{
    public static int id = 200;

    public void SaveAccount()
    {
        OperatingDB.Instance.SaveAccount("liruirui", "123456");
    }

    public void InsertInfo()
    {
        OperatingDB.Instance.SaveCheckItemData(id++, id++, id++,id++, DateTime.Now);
        Debug.Log("增加一条数据");
    }

    public void UpdateInfo()
    {
        OperatingDB.Instance.UpdateCheckItemData(200, 200, 200, 200, DateTime.Now);
        Debug.Log("更新一条数据");
    }

    public void DeleteInfo()
    {
        OperatingDB.Instance.DeleteCheckItem();
        Debug.Log("删除一条数据");
    }

    public void QueryInfo()
    {
        Debug.Log("查询数据");
        List<CheckItem> list = OperatingDB.Instance.GetCheckItemData(201, 203);
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i].CheckItemId);
            Debug.Log(list[i].Answer);
            Debug.Log(list[i].AirPlaneId);
            Debug.Log(list[i].UserId);
            Debug.Log(list[i].Time);
        }
    }
}
