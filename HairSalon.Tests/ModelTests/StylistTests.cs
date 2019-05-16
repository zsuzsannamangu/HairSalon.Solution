using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Models;
using System.Collections.Generic;
using System;

namespace HairSalon.Tests
{
  [TestClass]
  public class StylistTest : IDisposable
  {

    public StylistTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=zsuzsanna_mangu_test;";
    }

    public void Dispose()
    {
      Stylist.ClearAll();
      Client.ClearAll();
    }

    [TestMethod]
    public void StylistConstructor_CreatesInstanceOfStylist_Stylist()
    {
      Stylist newStylist = new Stylist("test stylist");
      Assert.AreEqual(typeof(Stylist), newStylist.GetType());
    }

    [TestMethod]
    public void GetStylistName_ReturnsName_String()
    {
      //Arrange
      string name = "Test Stylist";
      Stylist newStylist = new Stylist(name);

      //Act
      string result = newStylist.GetStylistName();

      //Assert
      Assert.AreEqual(name, result);
    }

    [TestMethod]
    public void GetAll_ReturnsAllStylistObjects_StylistList()
    {
      //Arrange
      string name01 = "Laci";
      string name02 = "Ben";
      Stylist newStylist1 = new Stylist(name01);
      newStylist1.Save();
      Stylist newStylist2 = new Stylist(name02);
      newStylist2.Save();
      List<Stylist> newList = new List<Stylist> { newStylist1, newStylist2 };

      //Act
      List<Stylist> result = Stylist.GetAll();

      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void Find_ReturnsStylistInDatabase_Stylist()
    {
      //Arrange
      Stylist testStylist = new Stylist("Ben");
      testStylist.Save();

      //Act
      Stylist foundStylist = Stylist.Find(testStylist.GetId());

      //Assert
      Assert.AreEqual(testStylist, foundStylist);
    }

    [TestMethod]
    public void GetClients_ReturnsEmptyClientList_ClientList()
    {
      //Arrange
      string name = "Laci";
      Stylist newStylist = new Stylist(name);
      List<Client> newList = new List<Client> { };

      //Act
      List<Client> result = newStylist.GetClients();

      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void GetAll_StylistsEmptyAtFirst_List()
    {
      //Arrange, Act
      int result = Stylist.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfNamesAreTheSame_Stylist()
    {
      //Arrange, Act
      Stylist firstStylist = new Stylist("Ben");
      Stylist secondStylist = new Stylist("Ben");

      //Assert
      Assert.AreEqual(firstStylist, secondStylist);
    }

    [TestMethod]
    public void Save_SavesStylistToDatabase_StylistList()
    {
      //Arrange
      Stylist testStylist = new Stylist("Laci");
      testStylist.Save();

      //Act
      List<Stylist> result = Stylist.GetAll();
      List<Stylist> testList = new List<Stylist>{testStylist};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_DatabaseAssignsIdToStylist_Id()
    {
      //Arrange
      Stylist testStylist = new Stylist("Laci");
      testStylist.Save();

      //Act
      Stylist savedStylist = Stylist.GetAll()[0];

      int result = savedStylist.GetId();
      int testId = testStylist.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    // [TestMethod]
    // public void Test_AddClient_AddsClientToStylist()
    // {
    //   //Arrange
    //   Stylist testStylist = new Stylist("Ben");
    //   testStylist.Save();
    //   Client testClient = new Client("jon");
    //   testClient.Save();
    //   Client testClient2 = new Client("Rob");
    //   testClient2.Save();
    //
    //   //Act
    //   testStylist.AddClient(testClient);
    //   testStylist.AddClient(testClient2);
    //   List<Client> result = testStylist.GetClients();
    //   List<Client> testList = new List<Client>{testClient, testClient2};
    //
    //   //Assert
    //   CollectionAssert.AreEqual(testList, result);
    // }
    //
    // [TestMethod]
    // public void GetClients_ReturnsAllStylistClients_ClientList()
    // {
    //   //Arrange
    //   Stylist testStylist = new Stylist("Ben");
    //   testStylist.Save();
    //   Client testClient1 = new Client("jon");
    //   testClient1.Save();
    //   Client testClient2 = new Client("Rob");
    //   testClient2.Save();
    //
    //   //Act
    //   testStylist.AddClient(testClient1);
    //   List<Client> savedClients = testStylist.GetClients();
    //   List<Client> testList = new List<Client> {testClient1};
    //
    //   //Assert
    //   CollectionAssert.AreEqual(testList, savedClients);
    // }

  }
}
