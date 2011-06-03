//-----------------------------------------------------------------------
// <copyright file="INerdService.cs" company="AppliedIS">
//     Copyright AppliedIS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace RestfulNerds
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Web;
    using NerdDinnerDomain;

    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [ServiceKnownType(typeof(ICollection<Rsvp>))]
    public interface INerdService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "Dinner/", Method = "POST")]
        NerdDinnerDomain.Dinner CreateDinner(NerdDinnerDomain.Dinner dinner);

        [OperationContract]
        [WebInvoke(UriTemplate = "Dinner/", Method = "PUT")]
        NerdDinnerDomain.Dinner UpdateDinner(NerdDinnerDomain.Dinner dinner);

        [OperationContract]
        [WebInvoke(UriTemplate = "Dinner/{dinnerId}", Method = "DELETE")]
        void DeleteDinner(string dinnerId);

        //[OperationContract]
        //[WebGet(UriTemplate = "Dinner/")]
        //List<NerdDinnerDomain.Dinner> GetDinners();

        [OperationContract]
        [WebGet(UriTemplate = "Dinner/{dinnerId}")]
        NerdDinnerDomain.Dinner GetDinner(string dinnerId);

        [OperationContract]
        [WebGet(UriTemplate = "DinnerSearch/{start}/{count}/{sortType}/{sortColumn}")]
        DinnerSet GetSortedDinners(string start, string count, string sortType, string sortColumn);

        [OperationContract]
        [WebGet(UriTemplate = "RsvpSearch/{dinnerid}/{start}/{count}/{sortType}/{sortColumn}")]
        RsvpSet GetSortedRsvps(string dinnerId, string start, string count, string sortType, string sortColumn);

        [OperationContract]
        [WebInvoke(UriTemplate = "Rsvp/", Method = "POST")]
        NerdDinnerDomain.Rsvp CreateRsvp(NerdDinnerDomain.Rsvp rsvp);
    }
}
