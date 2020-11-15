// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTest1.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Aseem Anand"/>
// --------------------------------------------------------------------------------------------------------------------
namespace AddressBook_RestSharp
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Net;

    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            //Initialize the base URL to execute requests made by the instance
            client = new RestClient("http://localhost:4000");
        }
        private IRestResponse GetContactList()
        {
            //Arrange
            //Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("/contacts/list", Method.GET);
            //Act
            // Execute the request
            IRestResponse response = client.Execute(request);
            return response;
        }
        /// <summary>
        /// UC 22 : Reads the entries from json server.
        /// </summary>
        [TestMethod]
        public void ReadEntriesFromJsonServer()
        {
            IRestResponse response = GetContactList();
            // Check if the status code of response equals the default code for the method requested
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Convert the response object to list of employees
            List<Contact> contactList = JsonConvert.DeserializeObject<List<Contact>>(response.Content);
            Assert.AreEqual(4, contactList.Count);
            foreach (Contact c in contactList)
            {
                Console.WriteLine($"Id: {c.Id}\tFullName: {c.FirstName} {c.LastName}\tPhoneNo: {c.PhoneNo}\tAddress: {c.Address}\tCity: {c.City}\tState: {c.State}\tZip: {c.Zip}\tEmail: {c.Email}");
            }
        }

        /// <summary>
        /// UC 23 : Add multiple contacts to the address book JSON server and return the same
        /// </summary>
        [TestMethod]
        public void OnCallingPostAPIForAContactListWithMultipleContacts_ReturnContactObject()
        {
            // Arrange
            List<Contact> contactList = new List<Contact>();
            contactList.Add(new Contact { FirstName = "Virendra", LastName = "Sehwag", PhoneNo = "6777456345", Address = "Feroz Shah Kotla", City = "New Delhi", State = "New Delhi", Zip = "547677", Email = "vs@gmail.com" });
            contactList.Add(new Contact { FirstName = "Yuzvendra", LastName = "Chahal", PhoneNo = "3456723456", Address = "Chinnaswamy", City = "Bangalore", State = "Karnataka", Zip = "435627", Email = "yc@gmail.com" });
            contactList.Add(new Contact { FirstName = "KL", LastName = "Rahul", PhoneNo = "7654564345", Address = "Mohali", City = "Mohali", State = "Punjab", Zip = "113425", Email = "klr@gmail.com" });

            //Iterate the loop for each contact
            foreach (var v in contactList)
            {
                //Initialize the request for POST to add new contact
                RestRequest request = new RestRequest("/contacts/list", Method.POST);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("firstname", v.FirstName);
                jsonObj.Add("lastname", v.LastName);
                jsonObj.Add("phoneNo", v.PhoneNo);
                jsonObj.Add("address", v.Address);
                jsonObj.Add("city", v.City);
                jsonObj.Add("state", v.State);
                jsonObj.Add("zip", v.Zip);
                jsonObj.Add("email", v.Email);

                //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Contact contact = JsonConvert.DeserializeObject<Contact>(response.Content);
                Assert.AreEqual(v.FirstName, contact.FirstName);
                Assert.AreEqual(v.LastName, contact.LastName);
                Assert.AreEqual(v.PhoneNo, contact.PhoneNo);
                Console.WriteLine(response.Content);
            }
        }

        /// <summary>
        /// UC 24 : Update the phoneNo into the json file in json server
        /// </summary>
        [TestMethod]
        public void OnCallingPutAPI_ReturnContactObject()
        {
            //Arrange
            //Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/contacts/7", Method.PUT);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("firstname", "Leo");
            jsonObj.Add("lastname", "Messi");
            jsonObj.Add("phoneNo", "7858070934");
            jsonObj.Add("address", "FC Barcelona");
            jsonObj.Add("city", "Barcelona");
            jsonObj.Add("state", "Spain");
            jsonObj.Add("zip", "535678");
            jsonObj.Add("email", "leomessi@gmail.com");
            //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Contact contact = JsonConvert.DeserializeObject<Contact>(response.Content);
            Assert.AreEqual("Leo", contact.FirstName);
            Assert.AreEqual("Messi", contact.LastName);
            Assert.AreEqual("535678", contact.Zip);
            Console.WriteLine(response.Content);
        }

        /// <summary>
        /// UC 5 : Delete the contact details with given id
        /// </summary>
        [TestMethod]
        public void OnCallingDeleteAPI_ReturnSuccessStatus()
        {
            //Arrange
            //Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/contacts/7", Method.DELETE);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}
