using UnityEngine;
using System.Data;
using System;
using System.Collections;
using Mono.Data.Sqlite;

public class DbAccess
{

	private SqliteConnection dbConnection;

	private SqliteCommand dbCommand;

	private SqliteDataReader reader;

	public DbAccess (string connectionString)
	{

		OpenDB (connectionString);

	}
	public DbAccess ()
	{

	}

	/// <summary>
	/// 打开数据库
	/// </summary>
	/// <param name="connectionString">Connection string.</param>
	public void OpenDB (string connectionString)
	{
		try
		{
			dbConnection = new SqliteConnection (connectionString + "; Version = 3");

			dbConnection.Open ();

			Debug.Log ("Connected to db");
		}
		catch(Exception e)
		{
			string temp1 = e.ToString();
			Debug.Log(temp1);
		}
	}

	/// <summary>
	/// 关闭数据库
	/// </summary>
	public void CloseSqlConnection ()
	{

		if (dbCommand != null) {

			dbCommand.Dispose ();

		}

		dbCommand = null;

		if (reader != null) {

			reader.Dispose ();

		}

		reader = null;

		if (dbConnection != null) {

			dbConnection.Close ();

		}

		dbConnection = null;

		Debug.Log ("Disconnected from db.");
	}

	/// <summary>
	/// 执行sql语句
	/// </summary>
	/// <returns>The query.</returns>
	/// <param name="sqlQuery">查询语句.</param>
	public SqliteDataReader ExecuteQuery (string sqlQuery)
	{

		Debug.Log ("sql="+sqlQuery);
		dbCommand = dbConnection.CreateCommand ();

		dbCommand.CommandText = sqlQuery;

		reader = dbCommand.ExecuteReader ();

		return reader;

	}

	/// <summary>
	/// 查询整个table的数据
	/// </summary>
	/// <returns>The full table.</returns>
	/// <param name="tableName">表名.</param>
	public SqliteDataReader ReadFullTable (string tableName)
	{

		string query = "SELECT * FROM " + tableName;

		return ExecuteQuery (query);

	}

	/// <summary>
	/// 插入数据
	/// </summary>
	/// <returns>The into.</returns>
	/// <param name="tableName">表名</param>
	/// <param name="values">需要插入的字段内容，注意字符串需要添加单引号 如 ‘name’</param>
	public SqliteDataReader InsertInfo (string tableName, string[] values)
	{

		string query = "INSERT INTO " + tableName + " VALUES (" + values[0];

		for (int i = 1; i < values.Length; ++i) {

			query += ", " + values[i];

		}

		query += ")";


		return ExecuteQuery (query);

	}

	/// <summary>
	/// 更新table内容
	/// </summary>
	/// <returns>The into.</returns>
	/// <param name="tableName">Table 名称.</param>
	/// <param name="cols">需要更新的字段名称数组.</param>
	/// <param name="colsvalues">需要更新的字段对应的值.</param>
	/// <param name="selectkey">更新依据的字段.</param>
	/// <param name="selectvalue">更新依据字段对应的值</param>
	public SqliteDataReader UpdateInfo (string tableName, string []cols,string []colsvalues,string selectkey,string selectvalue)
	{

		string query = "UPDATE "+tableName+" SET "+cols[0]+" = "+colsvalues[0];

		for (int i = 1; i < colsvalues.Length; ++i) {

			query += ", " +cols[i]+" ="+ colsvalues[i];
		}

		query += " WHERE "+selectkey+" = "+selectvalue+" ";

		return ExecuteQuery (query);
	}

	/// <summary>
	/// 根据删除条件，删除对应的数据
	/// </summary>
	/// <param name="tableName">Table 名称.</param>
	/// <param name="cols">字段数组.</param>
	/// <param name="colsvalues">字段数组对应的值.</param>
	public SqliteDataReader Delete(string tableName,string []cols,string []colsvalues)
	{
		string query = "DELETE FROM "+tableName + " WHERE " +cols[0] +" = " + colsvalues[0];

		for (int i = 1; i < colsvalues.Length; ++i) {

			query += " or " +cols[i]+" = "+ colsvalues[i];
		}

		return ExecuteQuery (query);
	}

	/// <summary>
	/// 插入数据，只插入部分字段的数据
	/// </summary>
	/// <returns>The into specific.</returns>
	/// <param name="tableName">Table 名称.</param>
	/// <param name="cols">需要插入的字段数组.</param>
	/// <param name="values">需要插入的字段数组对应的值.</param>
	public SqliteDataReader InsertIntoSpecific (string tableName, string[] cols, string[] values)
	{

		if (cols.Length != values.Length) {

			throw new SqliteException ("columns.Length != values.Length");

		}

		string query = "INSERT INTO " + tableName + "(" + cols[0];

		for (int i = 1; i < cols.Length; ++i) {

			query += ", " + cols[i];

		}

		query += ") VALUES (" + values[0];

		for (int i = 1; i < values.Length; ++i) {

			query += ", " + values[i];

		}

		query += ")";

		return ExecuteQuery (query);

	}
	/// <summary>
	/// 根据表名，删除该表的全部数据
	/// </summary>
	/// <returns>The contents.</returns>
	/// <param name="tableName">Table name.</param>
	public SqliteDataReader DeleteContents (string tableName)
	{

		string query = "DELETE FROM " + tableName;

		return ExecuteQuery (query);

	}

	/// <summary>
	/// 创建一个数据表
	/// </summary>
	/// <returns>The table.</returns>
	/// <param name="name">Name.</param>
	/// <param name="col">Col.</param>
	/// <param name="colType">Col type.</param>
	public SqliteDataReader CreateTable (string name, string[] col, string[] colType)
	{

		if (col.Length != colType.Length) {

			throw new SqliteException ("columns.Length != colType.Length");

		}

		string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];

		for (int i = 1; i < col.Length; ++i) {

			query += ", " + col[i] + " " + colType[i];

		}

		query += ")";

		return ExecuteQuery (query);

	}

	/// <summary>
	/// 根据条件筛选数据
	/// </summary>
	/// <returns>The where.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="items">需要筛选的字段.</param>
	/// <param name="col">筛选条件的健.</param>
	/// <param name="operation">筛选符号，如 >,<,= </param>.</param>
	/// <param name="values">筛选条件的值.</param>
	public SqliteDataReader SelectWhere (string tableName, string[] items, string[] col, string[] operation, string[] values)
	{

		if (col.Length != operation.Length || operation.Length != values.Length) {

			throw new SqliteException ("col.Length != operation.Length != values.Length");

		}

		string query = "SELECT " + items[0];

		for (int i = 1; i < items.Length; ++i) {

			query += ", " + items[i];

		}

		query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";

		for (int i = 1; i < col.Length; ++i) {

			query += " AND " + col[i] + operation[i] + "'" + values[i] + "' ";

		}

		return ExecuteQuery (query);

	}

    /// <summary>
	/// 查询表
	/// </summary>
	public SqliteDataReader Select(string tableName, string col, string values)
    {
        string query = "SELECT * FROM " + tableName + " WHERE " + col + " = " + values;
        return ExecuteQuery(query);
    }

    public SqliteDataReader Select(string tableName, string col, string operation, string values)
    {
        string query = "SELECT * FROM " + tableName + " WHERE " + col + operation + values;
        return ExecuteQuery(query);
    }

    /// <summary>
    /// 升序查询
    /// </summary>
    public SqliteDataReader SelectOrderASC(string tableName, string col)
    {
        string query = "SELECT * FROM " + tableName + " ORDER BY " + col + " ASC";
        return ExecuteQuery(query);
    }

    /// <summary>
    /// 降序查询
    /// </summary>
    public SqliteDataReader SelectOrderDESC(string tableName, string col)
    {
        string query = "SELECT * FROM " + tableName + " ORDER BY " + col + " DESC";
        return ExecuteQuery(query);
    }

    /// <summary>
    /// 查询表行数
    /// </summary>
    public SqliteDataReader SelectCount(string tableName)
    {
        string query = "SELECT COUNT(*) FROM " + tableName;
        return ExecuteQuery(query);
    }
}