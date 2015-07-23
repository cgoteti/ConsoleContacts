using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using QueryCSVsamples.RightNowServiceReference;
using System.ServiceModel;

namespace QueryCSVsamples
{
    class QueryCSV
    {
        private RightNowSyncPortClient _client;
        public QueryCSV()
        {
            _client = new RightNowSyncPortClient();
            _client.ClientCredentials.UserName.UserName = "cgoteti";
            _client.ClientCredentials.UserName.Password = "goEts#2015";
        }

        #region QueryCSV with Where clause        
        public void QueryCSVwithWhere() {
            ClientInfoHeader clientInfoHeader = new ClientInfoHeader();
            clientInfoHeader.AppID = "CSV query on Contacts with where clause";

            // string queryString = "SELECT contact.LookupName from contact where contact.ID>6";
             string queryString = "SELECT * FROM contact";
           // string queryString = "SELECT I.CustomFields.CO FROM Incident I WHERE ID = 27;";

            try
            {
                byte[] rawResult = null;
                CSVTableSet queryCSVwithWhere = _client.QueryCSV(clientInfoHeader, queryString, 1000, ",", false, true, out rawResult);
                CSVTable[] csvTables = queryCSVwithWhere.CSVTables;
                foreach (CSVTable table in csvTables)
                {
                    System.Console.WriteLine("Name: " + table.Name);
                    System.Console.WriteLine("Columns: " + table.Columns);
                    String[] rowData = table.Rows;

                    foreach (String data in rowData)
                    {
                        System.Console.WriteLine("Row Data: " + data);
                    }
                }
            }
            catch (System.ServiceModel.FaultException ex)
            {
                Console.WriteLine(ex.Code);
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region QueryObject
        public void queryObjects()
        {
            ClientInfoHeader clientHeaderInfo = new ClientInfoHeader();
            clientHeaderInfo.AppID = "Chandu test query objects";
             string queryString = "SELECT O.Opportunities FROM Organization O WHERE O.ID = 1;";
           // string queryString = "Select * from contact";
            
            Opportunity opportunity = new Opportunity();

            RNObject[] objectTemplates = new RNObject[] { opportunity };

            try
            {
                QueryResultData[] queryObjects = _client.QueryObjects(clientHeaderInfo, queryString, objectTemplates, 10000);
                RNObject[] rnObjects = queryObjects[0].RNObjectsResult;

                foreach (RNObject obj in rnObjects)
                {                    
                    Opportunity Opportunity = (Opportunity)obj;                    
                    Console.WriteLine("Opportunity of the organization is - " + Opportunity.Name);
                }

            }
            catch (System.ServiceModel.FaultException ex)
            {
                Console.WriteLine(ex.Code);
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region update an object    
        public void updateContact(long id, string updatedfirstname) {
            Contact contacttobeUpdated = new Contact();
            ID contactID = new ID();
            contactID.id = id;
            contactID.idSpecified = true;
            contacttobeUpdated.ID = contactID;

            PersonName pn = new PersonName();
            pn.First = updatedfirstname;
            

            contacttobeUpdated.Name = pn;
            RNObject[] rnobjects = new RNObject[]{ contacttobeUpdated };
            UpdateProcessingOptions options = new UpdateProcessingOptions();
            options.SuppressExternalEvents = false;
            options.SuppressRules = false;
            try
            {
                ClientInfoHeader clientInfoHeader = new ClientInfoHeader();
                clientInfoHeader.AppID = "Basic Update";

                //Invoke the Update operation
                _client.Update(clientInfoHeader, rnobjects, options);
            }
            catch (FaultException ex)
            {
                Console.WriteLine(ex.Code);
                Console.WriteLine(ex.Message);
            }          
        } 
        #endregion


        static void Main(string[] args)
        {
            QueryCSV proxyclient = new QueryCSV();
            try
            {
                proxyclient.QueryCSVwithWhere();
                // proxyclient.queryObjects();
                Console.WriteLine();
                Console.WriteLine("Enter the ID of Contact you want to update: ");
                long _id = Convert.ToInt64(Console.ReadLine());
                Console.WriteLine("Enter first name you want to update for the contact:");
                string _fname = Console.ReadLine();
               // long _id1 = Convert.ToInt64(_id);
                proxyclient.updateContact(_id, _fname);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
