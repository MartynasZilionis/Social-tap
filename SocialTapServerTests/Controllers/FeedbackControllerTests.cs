using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialTapServer.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialTapServer.Controllers.Tests
{
    [TestClass]
    public class FeedbackControllerTests
    {
        private string conStr = @"Data Source = (localdb)\ProjectsV13;Initial Catalog = master; Integrated Security = True; Connect Timeout = 5; Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection conn;

        [TestInitialize]
        public void Setup()
        {
            conn = new SqlConnection(conStr);
            conn.Open();
            InitDb();
            CleanDb();
        }

        private void InitDb()
        {
            using (SqlCommand cmd = new SqlCommand
                (
                    @"IF OBJECT_ID('Feedback') IS NULL
                        CREATE TABLE [Feedback](
                            [Id] uniqueidentifier default newid(),
                            [Text] varchar(8000) not null,
                            [Date] datetime not null
                        )", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void CleanDb()
        {
            using (SqlCommand cmd = new SqlCommand("DELETE FROM [Feedback]", conn))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter da = new SqlDataAdapter("select [Id], [Text], [Date] from [Feedback]", conn))
                {
                    da.DeleteCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Feedback");
                    while (ds.Tables[0].Rows.Count > 0)
                        ds.Tables[0].Rows[0].Delete();
                    da.Update(ds.Tables[0]);
                }
            }
        }

        [TestCleanup]
        public void Teardown()
        {
            CleanDb();
            conn.Close();
            conn.Dispose();
        }

        [TestMethod]
        public void TestUploadFeedback()
        {
            var ctrl = new FeedbackController();
            ctrl.Post("test").Wait();
            ctrl.Post("test2").Wait();

            DataSet ds = new DataSet();
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT [Id], [Date], [Text] FROM [Feedback] ORDER BY [Date] DESC", conn))
            {
                da.Fill(ds, "Feedback");
            }
            Assert.AreEqual("test2", ds.Tables[0].Rows[0]["Text"]);
            Assert.AreEqual("test", ds.Tables[0].Rows[1]["Text"]);
        }
    }
}