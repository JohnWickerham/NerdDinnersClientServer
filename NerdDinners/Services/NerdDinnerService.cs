namespace NerdDinners.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using Microsoft.Runtime.Serialization;
    using NerdDinnerDomain;

    /// <summary>
    /// Implements INerdDInnerService, providing access to nerd dinners agnostically.
    /// </summary>
    public class NerdDinnerService : INerdDinnerService
    {
        ////private const string BaseUri = "http://localhost:44301/RestfulNerds/NerdService/";
        ////const string BaseUri = "http://localhost:3602/NerdService/";

        private const string BaseUri = "http://localhost/RestfulNerds/NerdService/";
        
        public DinnerSet GetFilteredDinners(int start, int count, string sortType, string sortColumn, string filter)
        {
            var client = GetAuthorizedHttpClient();
            var uri = BaseUri + "DinnerSearch/" + start + "/" + count + "/" + sortType + "/" + sortColumn + "?filter=" + filter;
             
            var responseGet = client.Get(uri);
            var result = GetDinnerSetFromResponse(responseGet);

            return result;  
        }

        public List<Dinner> GetDinners()
        {
            var client = GetAuthorizedHttpClient();
            var responseGet = client.Get(BaseUri + "Dinner/");
            var dinners = GetDinnersFromResponse(responseGet);

            return dinners;
        }

        public Dinner GetDinner(int dinnerId)
        {
            var client = GetAuthorizedHttpClient();
            var responseGet = client.Get(BaseUri + "Dinner/" + dinnerId);
            if (!responseGet.IsSuccessStatusCode)
            {
                return null;
            }

            var dinner = GetSingleDinnerFromResponse(responseGet);
            return dinner;
        }

        public Rsvp AttendDinner(Rsvp rsvp)
        {
            var client = GetAuthorizedHttpClient();
            HttpContent content = CreateRsvpContent(rsvp);
            var response = client.Post(BaseUri + "Rsvp/", content);
            return GetSingleRsvpFromResponse(response);
        }

        public RsvpSet GetRsvps(int dinnerId, int start, int count, string sortType, string sortColumn, string filter )
        {
            var client = GetAuthorizedHttpClient();
            var qst = BaseUri + "RsvpSearch/" + dinnerId + "/" + start + "/" + count + "/" + sortType + "/" + sortColumn + "?filter=" + filter;

            var responseGet = client.Get(qst);
            if (!responseGet.IsSuccessStatusCode)
            {
                return null;
            }

            var rsvpSet = GetRsvpSetFromResponse(responseGet);
            return rsvpSet;
        }

        public Dinner UpdateDinner(Dinner dinner)
        {
            var client = GetAuthorizedHttpClient();
            HttpContent content = this.CreateDinnerContent(dinner);
            var response = client.Put(BaseUri + "Dinner/", content);
            return GetSingleDinnerFromResponse(response);
        }


        public Dinner CreateDinner(Dinner dinner)
        {
            var client = GetAuthorizedHttpClient();
            HttpContent content = this.CreateDinnerContent(dinner);
            var response = client.Post(BaseUri + "Dinner/", content);
            return GetSingleDinnerFromResponse(response);
        }

        public void DeleteDinner(int dinnerId)
        {
            var client = GetAuthorizedHttpClient();
            var responseGet = client.Delete(BaseUri + "Dinner/" + dinnerId);
            if (responseGet.IsSuccessStatusCode)
            {
                return;
            }

            return;
        }

        #region Private Methods

        private static HttpClient GetAuthorizedHttpClient()
        {
            //Set authString to string64 encoding of the password.
            //uncomment line to add it to the header
            var authString = "put password here as string64 encoded";
            var client = new HttpClient {MaxResponseContentBufferSize = 196608};
            //client.DefaultRequestHeaders.Add("Authorization", "Basic " + authString);
            return client;
        }

        private static DinnerSet GetDinnerSetFromResponse(HttpResponseMessage response)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(DinnerSet));
            return (DinnerSet)serializer.ReadObject(response.Content.ContentReadStream);
        }

        private static List<Dinner> GetDinnersFromResponse(HttpResponseMessage response)
        {
            DataContractSerializer listSerializer = new DataContractSerializer(typeof(List<Dinner>));
            return (List<Dinner>)listSerializer.ReadObject(response.Content.ContentReadStream);
        }

        private static Dinner GetSingleDinnerFromResponse(HttpResponseMessage response)
        {
            DataContractSerializer listSerializer = new DataContractSerializer(typeof(Dinner));
            return (Dinner)listSerializer.ReadObject(response.Content.ContentReadStream);
        }

        private static Rsvp GetSingleRsvpFromResponse(HttpResponseMessage response)
        {
            DataContractSerializer listSerializer = new DataContractSerializer(typeof(Rsvp));
            return (Rsvp)listSerializer.ReadObject(response.Content.ContentReadStream);
        }

        private static RsvpSet GetRsvpSetFromResponse(HttpResponseMessage response)
        {
            DataContractSerializer listSerializer = new DataContractSerializer(typeof(RsvpSet));
            return (RsvpSet)listSerializer.ReadObject(response.Content.ContentReadStream);
        }

        private HttpContent CreateDinnerContent(Dinner dinnerToUpdate)
        {
            DataContractSerializer singleSerializer = new DataContractSerializer(typeof(Dinner));
            HttpContent content = dinnerToUpdate.ToContentUsingDataContractSerializer(singleSerializer);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
            return content;
        }

        private HttpContent CreateRsvpContent(Rsvp rsvpToUpdate)
        {
            DataContractSerializer singleSerializer = new DataContractSerializer(typeof(Rsvp));
            HttpContent content = rsvpToUpdate.ToContentUsingDataContractSerializer(singleSerializer);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
            return content;
        }
        #endregion
    }
}