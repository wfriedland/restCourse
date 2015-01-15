﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using FluentAssertions;

namespace HelloWorldService.Tests
{
    public class Contact
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("date-added")]
        public DateTime DateAdded { get; set; }
        [JsonProperty("phones")]
        public Phone[] Phones { get; set; }
    }

    public class Phone
    {
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("phone-type")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public PhoneType PhoneType { get; set; }
    }

    public enum PhoneType
    {
        Nil,
        Home,
        Mobile,
    }

    [TestClass]
    public class UnitTest1
    {
        HttpClient client;

        [TestInitialize]
        public void SetUp()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost.fiddler:49270/api/");
        }

        [TestMethod]
        public void TestAddNewContact()
        {
            var newContact = new Contact
            {
                Name = "New Name",
                Phones = new[] { 
                    new Phone { 
                        Number = "425-111-2222",
                        PhoneType = PhoneType.Mobile
                    }
                }
            };
            var newJson = JsonConvert.SerializeObject(newContact);

            var postContent = new StringContent(newJson, Encoding.UTF8, "application/json");

            var postResult = client.PostAsync("contacts", postContent).Result;

            //Assert.AreEqual(System.Net.HttpStatusCode.NoContent, postResult.StatusCode, "Status code was not NoContent");
            postResult.StatusCode.Should().Be("");
        }
    }
}