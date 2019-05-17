using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Client
  {
    private int _id;
    private string _name;
    private string _phone;
    private int _stylist_id;

    public Client (string name, string phone, int stylist_id = 0)
    {
      _name = name;
      _phone = phone;
      _stylist_id = stylist_id;
    }

    public string GetClientName()
    {
      return _name;
    }

    public void SetName(string newName)
    {
      _name = newName;
    }

    public string GetPhone()
    {
      return _phone;
    }

    public void SetPhone(string newPhone)
    {
      _phone = newPhone;
    }

    public int GetId()
    {
      return _id;
    }

    public int GetStylistId()
    {
      return _stylist_id;
    }

    public void SetId(int id)
    {
      _id = id;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM clients;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Client Find(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM clients WHERE id = @searchId;";
      cmd.Parameters.AddWithValue("@searchId", searchId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      rdr.Read();
      int id = rdr.GetInt32(0);
      string name = rdr.GetString(1);
      string phone = rdr.GetString(2);
      Client foundClient = new Client(name, phone);
      foundClient.SetId(id);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return foundClient;
    }

    public static List<Client> GetAll()
    {
      List<Client> allClients = new List<Client> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT id, name, phone, stylist_id FROM clients;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
        int clientId = rdr.GetInt32(0);
        string clientName = rdr.GetString(1);
        string clientPhone = rdr.GetString(2);
        int stylist_id = rdr.GetInt32(3);
        Client newClient = new Client(clientName, clientPhone, stylist_id);
        allClients.Add(newClient);
        }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allClients;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO clients (name, phone, stylist_id) VALUES (@clientName, @clientPhone, @stylist_id);";
      cmd.Parameters.AddWithValue("@clientName", _name);
      cmd.Parameters.AddWithValue("@clientPhone", _phone);
      cmd.Parameters.AddWithValue("@stylist_id", _stylist_id);
      // MySqlParameter name = new MySqlParameter();
      // name.ParameterName = "@clientName";
      // name.Value = this._name;
      // cmd.Parameters.Add(name);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM clients WHERE id = @ClientId;";
      MySqlParameter clientIdParameter = new MySqlParameter();
      clientIdParameter.ParameterName = "@ClientId";
      clientIdParameter.Value = this.GetId();
      cmd.Parameters.Add(clientIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
