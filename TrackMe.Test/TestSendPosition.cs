using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using TrackMe.Core.Models;
using TrackMe.Core.Services;
using Assert = NUnit.Framework.Assert;

namespace TrackMe.Test
{
    [TestFixture]
    public class TestSendPosition
    {
        private SendPositionService _service;
        private const int Sessions = 10000;
        private const int Requests = 100;

        [SetUp]
        public void Init()
        {
            _service = new SendPositionService();
        }

        [Test]
        [Ignore]
        public async Task TestMethod1()
        {
            for (int i = 0; i < Sessions; i++)
            {
                try
                {
                    var result = await _service.GetToken(new Position(i % 49, i % 49), 240);
                    SendPosition(result.Result.PublicToken);
                    Debug.WriteLine(result.Result.PublicToken + " " + result.Result.PrivateToken);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                } 
            }

            Assert.IsTrue(true);
        }

        [Ignore]
        private async Task SendPosition(string token)
        {
            try
            {
                for (int i = 0; i < Requests; i++)
                {
                    var result = await _service.SendPosition(token, new Position(i % 49, i % 49), "adress", 11);
                    await Task.Delay(5000);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
     
        }


    }

    [TestFixture]
    public class Test2
    {
        [Test]
        public void TestSerialization()
        {
            var json = "{\"IsError\":false,\"ErrorMessage\":null,\"Result\":{\"PublicToken\":\"a54844f2-d4ba-487b-b807-e496ba170fdc\",\"PrivateToken\":\"2df3cd21 - dfca - 4123 - 813f - 60f471772429\"}}";
            var converted = JsonConvert.DeserializeObject<WebResult<TokenPair>>(json);

            Assert.IsNotNull(converted.Result);
        }
    }
}
