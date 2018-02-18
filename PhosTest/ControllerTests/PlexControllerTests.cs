using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phos.Logging;
using Phos.Controllers;
using Phos.Models;
using Newtonsoft.Json;
using System.Net.Http;
using Phos.Managers;
using System.Net;
using System.IO;
using System.Xml;
using System.Text;

namespace PhosTest.ControllerTests
{
    [TestClass]
    public class PlexControllerTests
    {
        [TestMethod]
        public void TestMalAuth()
        {

        }


        private string GetSamplePayload()
        {
            return "{'event':'media.scrobble','user':true,'owner':true,'Account':{'id':1,'thumb':'https://plex.tv/users/bb964a231134a00e/avatar?c=1517771621','title':'shaw.tyler94@gmail.com'},'Server':{'title':'Rem','uuid':'2b72d2bdcf768fb38f1716ee74b3a968c10f471e'},'Player':{'local':true,'publicAddress':'65.175.216.53','title':'Plex Web (Chrome)','uuid':'yxvzc6tcn3i4q3s7d782cyll'},'Metadata':{'librarySectionType':'show','ratingKey':'670','key':'/library/metadata/670','parentRatingKey':'669','grandparentRatingKey':'668','guid':'com.plexapp.agents.thetvdb://332979/1/1?lang=en','librarySectionID':1,'librarySectionKey':'/library/sections/1','librarySectionTitle':'Archive','type':'episode','title':'Sakuya Konohana','grandparentKey':'/library/metadata/668','parentKey':'/library/metadata/669','grandparentTitle':'Konohana Kitan','parentTitle':'Season 1','contentRating':'TV-14','summary':'Yuzu is the new live-in employee at Konohanatei, a hot spring hotel run by fox girls. It\\'s tough work, but she makes new friends, and gets plenty of help adjusting.','index':1,'parentIndex':1,'viewOffset':258000,'lastViewedAt':1517779027,'year':2017,'thumb':'/library/metadata/670/thumb/1516432781','art':'/library/metadata/668/art/1516432781','grandparentThumb':'/library/metadata/668/thumb/1516432781','grandparentArt':'/library/metadata/668/art/1516432781','grandparentTheme':'/library/metadata/668/theme/1516432781','originallyAvailableAt':'2017-10-04','addedAt':1507391408,'updatedAt':1516432781,'Director':[{'id':860,'filter':'director=860','tag':'Hideki Okamoto'},{'id':861,'filter':'director=861','tag':'Keiko Kurosawa'}],'Writer':[{'id':432,'filter':'writer=432','tag':'Takao Yoshioka'},{'id':859,'filter':'writer=859','tag':'Hideki Okamoto'}]}}";
        }
    }
}
