using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace SocialTapServer.Controllers
{
    public class FeedbackController : ApiController
    {
        // POST: api/Feedback
        public async Task Post([FromBody]string value)
        {
            #if DEBUG
                string conStr = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=master;Integrated Security=True;Connect Timeout=5;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
#else
                string conStr = @"Server=tcp:soctapserv.database.windows.net,1433;Initial Catalog=SocialTapDb;Persist Security Info=False;User ID={USERNAME};Password={PASSWORD};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
#endif
            using (SqlConnection conn = new SqlConnection(conStr))
            {
            conn.Open();

            SqlCommand command = new SqlCommand("INSERT INTO Feedback (Text, Date) VALUES (@txt, @date)", conn);
            command.Parameters.Add(new SqlParameter("@txt", SqlDbType.VarChar, 8000, "Text"));
            command.Parameters.Add(new SqlParameter("@date", SqlDbType.DateTime, 0, "Date"));

                using (SqlDataAdapter da = new SqlDataAdapter("Select Id, Text, Date from Feedback", conn))
                {
                    da.InsertCommand = command;

                    DataSet ds = new DataSet();
                    da.Fill(ds, "Feedback");

                    DataRow newRow = ds.Tables[0].NewRow();
                    newRow["Text"] = value;
                    newRow["Date"] = DateTime.UtcNow;
                    ds.Tables[0].Rows.Add(newRow);

                    da.Update(ds.Tables[0]);
                    conn.Close();
                }
            }
            //await command.ExecuteNonQueryAsync();
        }
    }
}
