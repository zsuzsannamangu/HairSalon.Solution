using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Stylist
  {
    private static List<Stylist> _instances = new List<Stylist> {};
    private string _name;
    private int _id;
    private List<Client> _clients;

    public Stylist(string stylistName)
    {
      _name = stylistName;
      _instances.Add(this);
      _id = _instances.Count;
      _clients = new List<Client>{};
    }

    public string GetName()
    {
      return _name;
    }

    public int GetId()
    {
      return _id;
    }

    public void AddClient(Client client)
    {
      _clients.Add(client);
    }

    public static void ClearAll()
    {
      _instances.Clear();
    }

    public static Stylist Find(int searchId)
    {
      return _instances[searchId-1];
    }

    public List<Client> GetClients()
    {
      return _clients;
    }

    public static List<Stylist> GetAll()
    {
      List<Stylist> allStylists = new List<Stylist> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM stylists;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int Id = rdr.GetInt32(0);
        string stylistName = rdr.GetString(1);
        allStylists.Add(new Stylist(stylistName));
        }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStylists;
    }

    public override bool Equals(System.Object otherStylist)
    {
      if (!(otherStylist is Stylist))
      {
        return false;
      }
      else
      {
        Stylist newStylist = (Stylist) otherStylist;
        bool idEquality = (this.GetId() == newStylist.GetId());
        bool nameEquality = (this.GetName() == newStylist.GetName());
        return (idEquality && nameEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO stylists (name) VALUES (@stylistName);";
      // cmd.Parameters.AddWithValue("@stylistName", _name);
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@stylistName";
      name.Value = this._name;
      cmd.Parameters.Add(name);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
