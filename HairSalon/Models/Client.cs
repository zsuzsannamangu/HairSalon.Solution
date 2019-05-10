using System.Collections.Generic;
using MySql.Data.MySqlClient;
//this above accesses the SQL methods used in ClearAll()

namespace HairSalon.Models
{
  public class Client
  {
    private string _name;
    private int _phone;
    private int _id;
    public Client (string name, int phone, int id = 0)
    {
      _name = name;
      _phone = phone;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public void SetName(string newName)
    {
      _name = newName;
    }

    public int GetPhone()
    {
      return _phone;
    }

    public void SetPhone(int newPhone)
    {
      _phone = newPhone;
    }

    public int GetId()
    {
      return _id;
    }

    public static List<Client> GetAll()
    {
      List<Client> allClients = new List<Client> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM clients;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
        int clientId = rdr.GetInt32(0);
        string clientName = rdr.GetString(1);
        int clientPhone = rdr.GetInt32(2);
        Client newClient = new Client(clientName, clientId);
        allClients.Add(newClient);
        }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allClients;
    }

  }
}
