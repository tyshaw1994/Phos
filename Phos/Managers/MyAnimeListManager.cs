using System;
using Phos.Models;
using Newtonsoft.Json;
using System.Configuration;
using System.Xml;

namespace Phos.Managers
{
    public static class MyAnimeListManager
    {
        public static string SearchForShow(string title)
        {
            //    ICredentialContext creds = new CredentialContext
            //    {
            //        UserName = ConfigurationManager.AppSettings["MalUserName"],
            //        Password = ConfigurationManager.AppSettings["MalPassword"]
            //    };

            //var searchMethods = new SearchMethods(creds);

            //var searchResponse = searchMethods.SearchAnime(title);
            
            //if(string.IsNullOrEmpty(searchResponse))
            //{
            //    return "Show not found on MAL";
            //}

            //var doc = new XmlDocument();
            //doc.LoadXml(searchResponse);
            ////var responseJson = JsonConvert.SerializeXmlNode(doc);

            //foreach(XmlNode node in doc.SelectNodes("entry"))
            //{
            //    var responseJson = JsonConvert.SerializeXmlNode(node);
            //    var show = JsonConvert.DeserializeObject<MalShow>(responseJson);
            //}

            return "";
        }
    }
}
