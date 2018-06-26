using NUnit.Framework;
using BaseXClient;
using System;
using System.IO;
using System.Net.Sockets;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private Session session;
	private MemoryStream ms;

        [SetUp]
        public void Setup()
        {
	    ms = new MemoryStream();
        }

        [Test]
        public void Test1()
        {
            try {
                session = new Session("localhost", 1999, "admin", "admin");
            } catch (SocketException) {
                Assert.True(true, "success");
            }
        }

        [Test]
        public void Test2()
        {
	    try {
               session = new Session("localhost", 1984, "noonehere", "noonehere");
            } catch (IOException) {
               Assert.True(true, "success");
            }
        }	

        [Test]
        public void Test3()
        {
            session = new Session("localhost", 1984, "admin", "admin");
            Assert.IsNotNull(session);
        }

	[Test]
        public void Test4()
        {
            string result = session.Execute("xquery 1");
            Assert.IsTrue(string.Equals("1", result));
        }

	[Test]
        public void Test5()
        {
            string input = "declare variable $name external;" +
          	"for $i in 1 to 1 return element { $name } { $i }";
		
            Query query = session.Query(input);
            query.Bind("$name", "number");

	    string result = query.Execute();
	    query.Close();
            Assert.IsTrue(string.Equals("<number>1</number>", result));
        }

	[Test]
        public void Test6()
        {
            string input = "for $i in 1 to 1 return <xml>Text { $i }</xml>";

            Query query = session.Query(input);
            string result = string.Empty;
            while (query.More()) 
            {
               result = query.Next();
            }	    
            query.Close();
            Assert.IsTrue(string.Equals("<xml>Text 1</xml>", result));
        }

        [Test]
        public void Test7()
        {
	    session.Execute("xquery 2", ms);
            session.Close();

            Assert.IsTrue(string.Equals("2", System.Text.Encoding.UTF8.GetString(ms.ToArray())));
        }
    }
}
