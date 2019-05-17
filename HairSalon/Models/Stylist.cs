using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Stylist
  {
    private string _name;
    private int _id;
    private string _bio;

    public Stylist(string stylistName, string bio)
    {
      _name = stylistName;
      _bio = bio;
    }

    public string GetStylistName()
    {
      return _name;
    }

    public string GetBio()
    {
      return _bio;
    }

    public int GetId()
    {
      return _id;
    }

    public void SetId(int id)
    {
      _id = id;
    }


    public static Stylist Find(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM stylists WHERE id = @searchId;";
      cmd.Parameters.AddWithValue("@searchId", searchId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      rdr.Read();
      int id = rdr.GetInt32(0);
      string name = rdr.GetString(1);
      string bio = rdr.GetString(2);
      Stylist foundStylist = new Stylist(name, bio);
      foundStylist.SetId(id);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return foundStylist;
      // return _instances[searchId-1];
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM stylists;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
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
        string bio = rdr.GetString(2);
        Stylist newStylist = new Stylist(stylistName, bio);
        newStylist.SetId(Id);
        allStylists.Add(newStylist);
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
        bool nameEquality = (this.GetStylistName() == newStylist.GetStylistName());
        return (idEquality && nameEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO stylists (name, bio) VALUES (@stylistName, @bio);";
      // cmd.Parameters.AddWithValue("@stylistName", _name);
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@stylistName";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      MySqlParameter bio = new MySqlParameter();
      bio.ParameterName = "@bio";
      bio.Value = this._bio;
      cmd.Parameters.Add(bio);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Client> GetClients()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM clients WHERE stylist_id = @id;";
      cmd.Parameters.AddWithValue("@id", _id);
      List<Client> clients = new List<Client>{};
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string phone = rdr.GetString(2);
        int stylist_id = rdr.GetInt32(3);
        Client newClient = new Client(name, phone, stylist_id);
        newClient.SetId(id);
        clients.Add(newClient);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return clients;
    }

    public void AddSpecialty(Specialty newSpecialty)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"INSERT INTO services (stylists_id, specialties_id) VALUES (@StylistId, @SpecialtyId);";

      MySqlParameter stylists_id = new MySqlParameter();
      stylists_id.ParameterName = "@StylistId";
      stylists_id.Value = _id;
      cmd.Parameters.Add(stylists_id);

      MySqlParameter specialties_id = new MySqlParameter();
      specialties_id.ParameterName = "@SpecialtyId";
      specialties_id.Value = newSpecialty._specialtyId;
      cmd.Parameters.Add(specialties_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Specialty> GetSpecialties()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

        cmd.CommandText = @"SELECT specialties.* FROM stylists
            JOIN services ON (stylists.id = services.stylists_id)
            JOIN specialties ON (services.specialties_id = specialties.id)
            WHERE stylists.id = @StylistId;";

        MySqlParameter stylistIdParameter = new MySqlParameter();
        stylistIdParameter.ParameterName = "@StylistId";
        stylistIdParameter.Value = _id;
        cmd.Parameters.Add(stylistIdParameter);

        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<Specialty> specialties = new List<Specialty>{};
        while(rdr.Read())
        {
          // int specialtiesId = rdr.GetInt32(0);
          string specialtyName = rdr.GetString(1);
          string specialtyPrice = rdr.GetString(2);
          Specialty newSpecialty = new Specialty(specialtyName, specialtyPrice);
          specialties.Add(newSpecialty);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return specialties;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM stylists WHERE id = @StylistId; DELETE FROM services WHERE stylists_id = @StylistId;";
      MySqlParameter stylistIdParameter = new MySqlParameter();
      stylistIdParameter.ParameterName = "@StylistId";
      stylistIdParameter.Value = this.GetId();
      cmd.Parameters.Add(stylistIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
