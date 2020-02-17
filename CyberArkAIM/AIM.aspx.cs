using System;
using System.Data.SqlClient;
using CyberArk.AIM.NetPasswordSDK;
using CyberArk.AIM.NetPasswordSDK.Exceptions;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace CyberArkAIM
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string connString = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtAppId.Text = "";
            //txtBaseUrl.Text = "";
            //txtSafe.Text = "";
            //txtObject.Text = "";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            //is this a CP or CCP call?
            if (rblProviderFlavor.SelectedValue == "CP")
            {
                getCPCreds(cbDualAcct.Checked);
            }
            else if (rblProviderFlavor.SelectedValue == "CCP")
            {
                getRESTCreds(cbDualAcct.Checked);
            }
            else
            {
                lblDBResult.Text = "Select which flavor of CyberArk's Application Identity Manager you want to test.";
            }

            //if we were able to build a connection string, attempt to log into the DB and return the time
            if (connString != null)
            {
                try
                {
                    SqlConnection cn = new SqlConnection(connString);
                    string sql = "SELECT CONVERT(TIME, GETDATE())";
                    SqlCommand sqlCmd;
                    SqlDataReader sqlReader;
                    cn.Open();
                    sqlCmd = new SqlCommand(sql, cn);
                    sqlReader = sqlCmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        lblDBResult.Text = "Current time in your database is: " + sqlReader.GetValue(0).ToString();
                    }

                    cn.Close();
                }
                catch (Exception ex)
                {
                    lblDBResult.Text = ex.Message;
                }
            }
        }
        private void getCPCreds(bool isDualAcct)
        {
            try
            {
                PSDKPasswordRequest passRequest = new
                PSDKPasswordRequest();
                PSDKPassword password;
                string dbPort = "";

                //Retreive values input by the user for items that exist in the vault
                passRequest.AppID = txtAppId.Text;
                passRequest.ConnectionTimeout = 30;
                passRequest.Safe = txtSafe.Text;
                passRequest.Folder = txtFolder.Text;

                //Are we retreiving a single or dual acct?
                if (isDualAcct)
                {
                    passRequest.Query = "VirtualUserName=" + txtObject.Text;
                }
                else
                {
                    passRequest.Object = txtObject.Text;
                }
                //Add the properties required to build our connection string
                passRequest.RequiredProperties.Add("userName");
                passRequest.RequiredProperties.Add("Address");
                passRequest.RequiredProperties.Add("Database");
                passRequest.RequiredProperties.Add("Port");

                // Sending the request to get the password
                password = PasswordSDK.GetPassword(passRequest);

                //set Port if available
                try {
                    dbPort = password.PasswordProperties["Port"].ToString();
                }
                catch
                {
                    dbPort = "1433";
                }
                
                //Build the connection string with values returned from the Credential Provider
                BuildConnString(password.Address, password.Database, password.UserName, password.Content, dbPort);
            }
            catch (PSDKException ex)
            {
                lblDBResult.Text = ex.Reason;
            }
        }

        private void getRESTCreds(bool isDualAccount)
        {
            try
            {
                //Build the GetPassword REST request
                var client = new RestClient("https://" + txtBaseUrl.Text + "/AIMWebService/api/Accounts");
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                var request = new RestRequest(Method.GET);
                request.AddHeader("content-type", "application/json");
                request.AddQueryParameter("AppID", txtAppId.Text);
                request.AddQueryParameter("Safe", txtSafe.Text);
                request.AddQueryParameter("Folder", txtFolder.Text);
                


                //Are we retreiving a single or dual acct?
                if (isDualAccount)
                {
                    request.AddQueryParameter("Query", "VirtualUserName=" + txtObject.Text);
                }
                else
                {
                    request.AddQueryParameter("Object", txtObject.Text);
                }

                IRestResponse response = client.Execute(request);

                //Parse the response so that we can use values later
                dynamic restReponse = JObject.Parse(response.Content);

                string dbPort = "";
                //set Port if available
                try
                {
                    dbPort = restReponse.Port.ToString();
                }
                catch
                {
                    dbPort = "1433";
                }

                //Build the connection string with values returned from the Central Credential Provider
                BuildConnString(restReponse.Address.ToString(), restReponse.Database.ToString(), restReponse.UserName.ToString(), restReponse.Content.ToString(), dbPort);
            }
            catch (Exception ex)
            {
                lblDBResult.Text = ex.Message;
            }
        }

        public void BuildConnString(string strDataSource, string strDatabase, string strUserName, string strPassword, string strPort)
        {
            connString = "Data Source=" + strDataSource + "," + strPort + ";Initial Catalog=" + strDatabase + ";User ID=" + strUserName + ";Password=" + strPassword;
        }
    }
}