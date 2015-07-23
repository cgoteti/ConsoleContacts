using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using ConsoleContacts.RightNowService;

namespace ConsoleContacts
{
    class sampleClient
    {
        //RightNowSyncPortClient is the class exposed by the rightNow webservice.
        private RightNowSyncPortClient _service;

        public sampleClient()
        {
            _service = new RightNowSyncPortClient();
            _service.ClientCredentials.UserName.UserName = "cgoteti";
            _service.ClientCredentials.UserName.Password = "goEts#2015";
        }

        public RNObject[] CreateContact(string FirstName, string LastName,string EmailAddress)
        {           
                Contact newContact = populateContactInfo(FirstName, LastName,EmailAddress);
                //Set the application ID in the client info header
                ClientInfoHeader clientInfoHeader = new ClientInfoHeader();
                clientInfoHeader.AppID = ".NET Getting Started test in log API";
                //Set the create processing options, allow external events and rules to execute
                CreateProcessingOptions createProcessingOptions = new CreateProcessingOptions();
                createProcessingOptions.SuppressExternalEvents = false;
                createProcessingOptions.SuppressRules = false;
                RNObject[] createObjects = new RNObject[] { newContact };
                GetProcessingOptions options = new GetProcessingOptions();
                options.FetchAllNames = false;
                //Invoke the create operation on the server. This will create a contact 
                RNObject[] createResults = _service.Create(clientInfoHeader, createObjects, createProcessingOptions);         
                newContact = createResults[0] as Contact;
                RNObject[] rnObjects = _service.Get(clientInfoHeader, createResults, options);                
                return rnObjects;      
         }

        private Contact populateContactInfo(string fname,string lname,string email)
        {
            Contact newContact = new Contact();
            PersonName personName = new PersonName();
            personName.First = fname;
            personName.Last = lname;
            newContact.Name = personName;
            Email[] emailArray = new Email[1];
            emailArray[0] = new Email();
            emailArray[0].action = ActionEnum.add;
            emailArray[0].actionSpecified = true;
            emailArray[0].Address = email;
            NamedID addressType = new NamedID();
            ID addressTypeID = new ID();
            addressTypeID.id = 1;
            addressType.ID = addressTypeID;
            addressType.ID.idSpecified = true;
            emailArray[0].AddressType = addressType;
            emailArray[0].Invalid = false;
            emailArray[0].InvalidSpecified = true;
            newContact.Emails = emailArray;
            return newContact;
        }

        static void Main(string[] args)
        {
            sampleClient client = new sampleClient();
            Console.WriteLine("Enter FirstName of contact:");
            string fname = Console.ReadLine();
            Console.WriteLine("Enter LastName of contact:");
            string lname = Console.ReadLine();
            Console.WriteLine("Enter Email Address");
            string email = Console.ReadLine();
            
            try { 
          
                 RNObject[] createdContact = client.CreateContact(fname,lname,email);
                if (createdContact!=null && createdContact.Count()>0)
                {
                    foreach (var item in createdContact)
                    {
                        Console.WriteLine("Contact created for " + item.LookupName + " with ID " + item.ID.id);
                    }
                }
                else
                {
                    Console.WriteLine("No contact created");
                }
                
            }
            catch (System.ServiceModel.FaultException ex)
            {
                 Console.WriteLine(ex.Code);
                 Console.WriteLine(ex.Message);
            }

            System.Console.Read();

        }
    }
}
