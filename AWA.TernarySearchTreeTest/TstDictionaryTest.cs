using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AWA.TernarySearchTree;
using System.IO;
using System.Reflection;


namespace AWA.TernarySearchTreeTest
{
    /// <summary>
    /// Summary description for TstDictionaryTest
    /// </summary>
    [TestClass]
    public class TstDictionaryTest
    {
        TstDictionary<string, string> dictionary;
        public TstDictionaryTest()
        {            
            loadDictionary();
        }

        private void loadDictionary()
        {
            dictionary = new TstDictionary<string, string>();
            StreamReader SR;
            string S;
            var dr = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //SR = File.OpenText("C:\\Users\\adam\\Documents\\TWL06.txt");
            SR = File.OpenText(dr + "\\TWL06.txt");
            S = SR.ReadLine();
            while (S != null)
            {
                dictionary[S] = S;
                S = SR.ReadLine();
            }
            SR.Close();
            dictionary.BalanceSearchTree();
        }
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestFind()
        {
            //
            // TODO: Add test logic here
            //
            Assert.IsTrue(dictionary["FROM"] == "FROM");
        }

        [TestMethod]
        public void TestNearSearch()
        {
            //
            // TODO: Add test logic here
            //
            IList<KeyValuePair<string, string>> list = dictionary.NearSearch("FROM", 1);
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void TestPartialMatch()
        {
            //
            // TODO: Add test logic here
            //
            IList<KeyValuePair<string, string>> list = dictionary.PartialKeyMatch("F..M");
            Assert.IsTrue(list.Count > 0);
            list = dictionary.PartialKeyMatch("FRO*");
            Assert.IsTrue(list.Count > 0);
            list = dictionary.PartialKeyMatch("F*M");
            Assert.IsTrue(list.Count > 0);
            list = dictionary.PartialKeyMatch("F*OM");
            Assert.IsTrue(list.Count > 0);
            list = dictionary.PartialKeyMatch("F*.M");
            Assert.IsTrue(list.Count > 0);
        }
    }
}
