using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ampl.Configuration.Tests.EntityFramework
{
  [TestClass]
  public class AppConfigTest
  {
    private AmplTestContext _db;
    private DbContextTransaction _tr;
    private AppConfig _cfg;

    private static string _testString = "Это тестовая строка.";
    private static int _testInt = 567;
    private static bool _testBool = true;
    private static decimal _testDecimal = 1234.5670M;

    private static int? _testNullableInt = 567;
    private static bool? _testNullableBool = true;
    private static decimal? _testNullableDecimal = 1234.5670M;


    [TestInitialize]
    public void TestInitialize()
    {
      _db = new AmplTestContext();
      _tr = _db.Database.BeginTransaction();

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingString", Value = _testString });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingInt", Value = _testInt.ToString() });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingBool", Value = _testBool.ToString() });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingDecimal", Value = _testDecimal.ToString(new CultureInfo("en-US")) });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingNullableIntNotNull", Value = _testNullableInt.ToString() });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingNullableIntNull", Value = ((int?)null).ToString() });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingNullableBoolNotNull", Value = _testNullableBool.ToString() });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingNullableBoolNull", Value = ((bool?)null).ToString() });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingNullableDecimalNotNull", Value = _testNullableDecimal.Value.ToString(new CultureInfo("en-US")) });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingNullableDecimalNull", Value = ((decimal?)null).ToString() });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TwoConfig.Value", Value = _testInt.ToString() });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "OneConfig.Value", Value = (_testInt * 10).ToString() });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingStringList[0]", Value = "Zero" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingStringList[1]", Value = "One" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingStringList[2]", Value = "Two" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingStringList[3]", Value = "Three" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingStringList[33]", Value = "Thirty Three" });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingIntList[0]", Value = "0" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingIntList[1]", Value = "-1" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingIntList[2]", Value = "2" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingIntList[3]", Value = "3" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestingIntList[33]", Value = "333" });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "Defaults.UserName", Value = "Default" });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClass.UserName", Value = "Mike" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClass.Password", Value = "Pass" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClass.UserPass", Value = "Oops" });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[0].UserName", Value = "John" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[0].Password", Value = "Doe" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[0].Port", Value = "25" });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[1].UserName", Value = "Alex" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[1].Password", Value = "Donkey" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[1].Port", Value = "110" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[1].Ports[0]", Value = "995" });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[2].UserName", Value = "Lara" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[2].Password", Value = "Croft" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[2].Port", Value = "6666" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[2].Ports[0]", Value = "6667" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "TestClasses[2].Ports[1]", Value = "7000" });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "Email.Password", Value = "Pass" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "Email.Ports[0]", Value = "110" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "Email.Ports[1]", Value = "995" });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Doe].UserName", Value = "John" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Doe].Password", Value = "Doe" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Doe].Port", Value = "25" });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Donkey].UserName", Value = "Alex" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Donkey].Password", Value = "Donkey" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Donkey].Port", Value = "110" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Donkey].Ports[0]", Value = "995" });

      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Croft].UserName", Value = "Lara" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Croft].Password", Value = "Croft" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Croft].Port", Value = "6666" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Croft].Ports[0]", Value = "6667" });
      _db.AppConfigItems.Add(new AppConfigItem() { Key = "ObjectDictionary[Croft].Ports[1]", Value = "7000" });


      _db.AppConfigKeyResolvers.Add(new AppConfigKeyResolver() {
        FromKey = "SomeNestedConfig.TestingString",
        ToKey = "TestingString"
      });
      _db.AppConfigKeyResolvers.Add(new AppConfigKeyResolver() {
        FromKey = "SomeNestedConfig.AnotherNestedConfig.TestingString",
        ToKey = "SomeNestedConfig.TestingString"
      });

      _db.AppConfigKeyResolvers.Add(new AppConfigKeyResolver() {
        FromKey = "TwoConfig.Value",
        ToKey = "OneConfig.Value"
      });
      _db.AppConfigKeyResolvers.Add(new AppConfigKeyResolver() {
        FromKey = "ThreeConfig.Value",
        ToKey = "TwoConfig.Value"
      });

      _db.AppConfigKeyResolvers.Add(new AppConfigKeyResolver() {
        FromKey = "TestingStringList2",
        ToKey = "TestingStringList"
      });
      _db.AppConfigKeyResolvers.Add(new AppConfigKeyResolver() {
        FromKey = "Email.UserName",
        ToKey = "Defaults.UserName"
      });

      _db.SaveChanges();

      _cfg = new AppConfig(_db, _db);
    }

    [TestCleanup]
    public void TestCleanup()
    {
      _tr.Rollback();
      //_tr.Commit();
      _tr.Dispose();
      _db.Dispose();
    }

    [TestMethod]
    public void Get_String_Exising()
    {
      Assert.AreEqual(_testString, _cfg.Get<string>("TestingString"));
    }

    [TestMethod]
    public void Get_String_NotExisting()
    {
      Assert.AreEqual(default(string), _cfg.Get<string>("NonExistingString"));
    }

    #region int

    [TestMethod]
    public void Get_Int_Existing()
    {
      Assert.AreEqual(_testInt, _cfg.Get<int>("TestingInt"));
    }

    [TestMethod]
    public void Get_Int_NonExisting()
    {
      Assert.AreEqual(default(int), _cfg.Get<int>("NonExistingInt"));
    }

    #endregion

    #region int?

    [TestMethod]
    public void Get_NullableInt_Existing_NotNull()
    {
      Assert.AreEqual(_testNullableInt, _cfg.Get<int?>("TestingNullableIntNotNull"));
    }

    [TestMethod]
    public void Get_NullableInt_Existing_Null()
    {
      Assert.AreEqual(null, _cfg.Get<int?>("TestingNullableIntNull"));
    }

    [TestMethod]
    public void Get_NullableInt_NonExisting()
    {
      Assert.AreEqual(null, _cfg.Get<int?>("NonExistingNullableInt"));
    }

    #endregion

    #region bool

    [TestMethod]
    public void Get_Bool_Existing()
    {
      Assert.AreEqual(_testBool, _cfg.Get<bool>("TestingBool"));
    }

    [TestMethod]
    public void Get_Bool_NonExisting()
    {
      Assert.AreEqual(default(bool), _cfg.Get<bool>("NonExistingBool"));
    }

    #endregion

    #region bool?

    [TestMethod]
    public void Get_NullableBool_Existing_NotNull()
    {
      Assert.AreEqual(_testNullableBool, _cfg.Get<bool?>("TestingNullableBoolNotNull"));
    }

    [TestMethod]
    public void Get_NullableBool_Existing_Null()
    {
      Assert.AreEqual(null, _cfg.Get<bool?>("TestingNullableBoolNull"));
    }

    [TestMethod]
    public void Get_NullableBool_NonExisting()
    {
      Assert.AreEqual(null, _cfg.Get<bool?>("NonExistingNullableBool"));
    }

    #endregion

    #region decimal

    [TestMethod]
    public void Get_Decimal_Existing()
    {
      Assert.AreEqual(_testDecimal, _cfg.Get<decimal>("TestingDecimal"));
    }

    [TestMethod]
    public void Get_Decimal_NonExisting()
    {
      Assert.AreEqual(default(decimal), _cfg.Get<decimal>("NonExistingDecimal"));
    }

    #endregion

    #region decimal?

    [TestMethod]
    public void Get_NullableDecimal_Existing_NotNull()
    {
      Assert.AreEqual(_testNullableDecimal, _cfg.Get<decimal?>("TestingNullableDecimalNotNull"));
    }

    [TestMethod]
    public void Get_NullableDecimal_Existing_Null()
    {
      Assert.AreEqual(null, _cfg.Get<decimal?>("TestingNullableDecimalNull"));
    }

    [TestMethod]
    public void Get_NullableDecimal_NonExisting()
    {
      Assert.AreEqual(null, _cfg.Get<decimal?>("NonExistingNullableDecimal"));
    }

    #endregion

    [TestMethod]
    public void Get_String_Resolves()
    {
      Assert.AreEqual(_testString, _cfg.Get<string>("SomeNestedConfig.TestingString"));
      Assert.AreEqual(null, _cfg.Get<string>("SomeNestedConfig.TestingString", false));
      //Dictionary<string, object> dic;
    }

    [TestMethod]
    public void Get_String_ResolvesNested()
    {
      Assert.AreEqual(_testString, _cfg.Get<string>("SomeNestedConfig.AnotherNestedConfig.TestingString"));
    }

    [TestMethod]
    public void Get_String_ResolvesNested2()
    {
      Assert.AreEqual(_testInt, _cfg.Get<int>("ThreeConfig.Value"));
    }

    [TestMethod]
    public void Get_List_String()
    {
      List<string> list = _cfg.Get<List<string>>("TestingStringList");
      Assert.AreEqual(5, list.Count);
    }

    [TestMethod]
    public void Get_List_String_Resolve()
    {
      List<string> list = _cfg.Get<List<string>>("TestingStringList2");
      Assert.AreEqual(5, list.Count);
    }

    [TestMethod]
    public void Get_List_String_NonExistent()
    {
      List<string> list = _cfg.Get<List<string>>("NonExistentStringList");
      Assert.IsNotNull(list);
      Assert.AreEqual(0, list.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(MissingMethodException))]
    public void Get_List_String_Interface()
    {
      ICollection<string> list = _cfg.Get<ICollection<string>>("TestingStringList");
    }

    [TestMethod]
    public void Get_List_Int()
    {
      List<string> list = _cfg.Get<List<string>>("TestingIntList");
      Assert.AreEqual(5, list.Count);
    }


    public class TestClass1
    {
      public string UserName { get; set; }
      public string Password { get; set; }

      public string UserPass
      {
        get
        {
          return UserName + " " + Password;
        }
      }

      public int Port;

      public List<int> Ports { get; set; }
    }

    public class TestClass2
    {
      public string UserName { get; set; }
      public string Password { get; set; }

      public int Port;

      public Dictionary<string, int> Ports { get; set; }
    }


    [TestMethod]
    public void Get_Class_Existing()
    {
      var obj = _cfg.Get<TestClass1>("TestClass");
      Assert.IsTrue(obj.Password == "Pass");
      Assert.IsTrue(obj.UserPass == "Mike Pass"); // won't set RO properties
      Assert.IsTrue(obj.Port == 0); // won't set fields
    }

    [TestMethod]
    public void Get_Class_NonExisting()
    {
      var obj = _cfg.Get<TestClass1>("TestClassNotExisting");
      Assert.AreEqual(default(TestClass1), obj); // return null for non-existing objects
    }

    [TestMethod]
    public void Get_Class_Existing_Resolvers()
    {
      var obj = _cfg.Get<TestClass1>("Email");
      Assert.IsTrue(obj.UserName == "Default");
    }

    [TestMethod]
    public void Get_Class_NonExistingCollections()
    {
      var obj = _cfg.Get<TestClass1>("TestClass");
      Assert.AreEqual(0, obj.Ports.Count);
    }

    [TestMethod]
    public void Get_Class_ExistingCollections()
    {
      var obj = _cfg.Get<TestClass1>("Email");
      Assert.AreEqual(2, obj.Ports.Count);
    }

    [TestMethod]
    public void Get_CollectionOfClasses()
    {
      var obj = _cfg.Get<List<TestClass1>>("TestClasses");
      Assert.AreEqual(3, obj.Count);
      Assert.AreEqual(0, obj[0].Ports.Count);
      Assert.AreEqual(1, obj[1].Ports.Count);
      Assert.AreEqual(2, obj[2].Ports.Count);
    }

    [TestMethod]
    public void Get_DictionaryOfClasses()
    {
      var obj = _cfg.Get<Dictionary<string, TestClass1>>("TestClasses");
      Assert.AreEqual(3, obj.Count);
      Assert.AreEqual(0, obj["0"].Ports.Count);
      Assert.AreEqual(1, obj["1"].Ports.Count);
      Assert.AreEqual(2, obj["2"].Ports.Count);
    }

    [TestMethod]
    public void Get_ListOfClasses_With_Dictionaries()
    {
      var obj = _cfg.Get<List<TestClass2>>("TestClasses");
      Assert.AreEqual(3, obj.Count);
      Assert.AreEqual(0, obj[0].Ports.Count);
      Assert.AreEqual(1, obj[1].Ports.Count);
      Assert.AreEqual(2, obj[2].Ports.Count);
    }

    [TestMethod]
    public void Get_DictionaryOfClasses_With_Dictionaries()
    {
      var obj = _cfg.Get<Dictionary<string, TestClass2>>("ObjectDictionary");
      Assert.AreEqual(3, obj.Count);
      Assert.AreEqual(0, obj["Doe"].Ports.Count);
      Assert.AreEqual(1, obj["Donkey"].Ports.Count);
      Assert.AreEqual(2, obj["Croft"].Ports.Count);
      Assert.IsFalse(obj.ContainsKey("NonExistentKey"));
    }



    [TestMethod]
    public void Get_Class_FromCollection_ByIndex()
    {
      var obj = _cfg.Get<TestClass1>("TestClasses[0]");
      Assert.IsTrue(obj.UserName == "John");
    }

    [TestMethod]
    public void Set_Primitive_Creates()
    {
      Assert.IsTrue(_db.AppConfigItems.FirstOrDefault(x => x.Key == "SomeClass.UserName") == null);
      _cfg.Set("SomeClass.UserName", "John");
      Assert.IsTrue(_db.AppConfigItems.FirstOrDefault(x => x.Key == "SomeClass.UserName").Value == "John");
    }


    [TestMethod]
    public void Set_Primitive_Overwrites()
    {
      Assert.IsTrue(_db.AppConfigItems.FirstOrDefault(x => x.Key == "TestClass.UserName").Value == "Mike");
      _cfg.Set("TestClass.UserName", "John");
      Assert.IsTrue(_db.AppConfigItems.FirstOrDefault(x => x.Key == "TestClass.UserName").Value == "John");
      Assert.IsTrue(_cfg.Get<string>("TestClass.UserName") == "John");
    }

    [TestMethod]
    public void Set_Collection_Creates()
    {
      Assert.IsFalse(_db.AppConfigItems.Any(x => x.Key.StartsWith("TestCollection[")));
      _cfg.Set("TestCollection", new[] { 1, 2, 3, 4, 5 });
      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("TestCollection[")) == 5);
      Assert.IsTrue(_cfg.Get<List<string>>("TestCollection").Count == 5);
    }

    [TestMethod]
    public void Set_Object_Creates()
    {
      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("TestObject")) == 0);
      TestClass1 testClass1 = new TestClass1() { UserName = "User", Password = null, Ports = new List<int>() { 1, 2, 3 } };
      _cfg.Set("TestObject", testClass1);
      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("TestObject")) == 5);
    }

    [TestMethod]
    public void Set_Null_DeletesExisting_CreatesOne()
    {
      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("TestObject")) == 0);
      TestClass1 testClass1 = new TestClass1() { UserName = "User", Password = null, Ports = new List<int>() { 1, 2, 3 } };
      _cfg.Set("TestObject", testClass1);
      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("TestObject")) == 5);

      //
      // will contain
      //    TestObject.UserName = "User"
      //    TestObject.Password = null;
      // TestObject.Ports deleted
      //
      testClass1.Ports = null;
      _cfg.Set("TestObject", testClass1);
      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("TestObject")) == 2);
    }

    [TestMethod]
    public void Set_Collection_Of_Classes()
    {
      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("ClassList")) == 0);
      var list = new[] {
        new TestClass1() { UserName = "1", Password = null, Port = 3, Ports = null },
        new TestClass1() { UserName = "2", Password = "22", Port = 4, Ports = new List<int>() { 1, 2, 3 } },
      };
      _cfg.Set("ClassList", list);

      //
      // won't set public fields
      //
      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("ClassList")) == 7);
    }

    [TestMethod]
    public void Set_ObjectWithCollection()
    {
      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("ObjectWithCollection")) == 0);
      var obj = new TestClass1() {
        UserName = "1",
        Password = "2",
        Port = 3,
        Ports = new List<int>() { 1, 2, 3 }
      };
      _cfg.Set("ObjectWithCollection", obj);

      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("ObjectWithCollection")) == 5);
    }

    [TestMethod]
    public void Set_ObjectWithDictionary()
    {
      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("ObjectWithDictionary")) == 0);
      var obj = new TestClass2() {
        UserName = "1",
        Password = "2",
        Port = 3,
        Ports = new Dictionary<string, int>() { { "a", 1 }, { "aa", 2 }, { "abc", 3 } }
      };

      
      _cfg.Set("ObjectWithDictionary", obj);

      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("ObjectWithDictionary")) == 5);
    }

    [TestMethod]
    public void Set_Dictionary_Of_Objects_WithDictionary()
    {
      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("DicObjDic")) == 0);

      var obj = new Dictionary<string, TestClass2>() {
        { "one", new TestClass2() {
                    UserName = "1", Password = "2",
                    Ports = new Dictionary<string, int>() { { "a", 1 }, { "aa", 2 }, { "abc", 3 } }
                 }
        },
        { "two", new TestClass2() {
                    UserName = "3", Password = "4",
                    Ports = new Dictionary<string, int>() { { "x", 1 }, { "y", 2 } }
                 }
        },
      };

      _cfg.Set("DicObjDic", obj);

      Assert.IsTrue(_db.AppConfigItems.Count(x => x.Key.StartsWith("DicObjDic")) == 9);
    }


  }
}
