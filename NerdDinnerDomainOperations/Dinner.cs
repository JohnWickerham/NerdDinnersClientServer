// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Dinner.cs" company="AppliedIS">
//   All rights reserved
// </copyright>
// <summary>
//   Defines the Dinner type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NerdDinnerDomain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Dinner domain class
    /// </summary>
    [Serializable]
    public class Dinner
    {
        /// <summary>
        /// Gets or sets DinnerId.
        /// </summary>
        public int DinnerId { get; set; }

        /// <summary>
        /// Gets or sets CreatedBy.
        /// </summary>
        [StringLength(50)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets Title.
        /// </summary>
        [Required(ErrorMessage = "Please enter a Dinner Title")]
        [StringLength(20, ErrorMessage = "Title is too long")]
        [Display(Name = "Dinner Title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets EventDate.
        /// </summary>
        [Required(ErrorMessage = "Please enter the Date of the Dinner")]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Gets or sets Address.
        /// </summary>
        [Required(ErrorMessage = "Please enter the location of the Dinner")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets HostedBy.
        /// </summary>
        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Hosted By")]
        public string HostedBy { get; set; }

        /// <summary>
        /// Gets or sets Country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets RsvpCount.
        /// </summary>
        public int RsvpCount { get; set; }
        ////public ICollection<RSVP> RSVPs { get; set; }
    }

    /// <summary>
    /// Rsvp domain class
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    [Serializable]
    public class Rsvp
    {
        /// <summary>
        /// Gets or sets RsvpId.
        /// </summary>
        public int RsvpId { get; set; }

        /// <summary>
        /// Gets or sets AttendeeEmail.
        /// </summary>
        public string AttendeeEmail { get; set; }

        /// <summary>
        /// Gets or sets Dinner.
        /// </summary>
        public Dinner Dinner { get; set; }

        /// <summary>
        /// Gets or sets DinnerId.
        /// </summary>
        public int DinnerId { get; set; }
    }

    /// <summary>
    /// DinnerSet domain class
    /// </summary>
    [Serializable]
    public class DinnerSet
    {
        /// <summary>
        /// Gets or sets TotalDinnerCount.
        /// </summary>
        public int TotalDinnerCount { get; set; }

        /// <summary>
        /// Gets or sets FilteredDinnerCount.
        /// </summary>
        public int FilteredDinnerCount { get; set; }

        /// <summary>
        /// Gets or sets Dinners.
        /// </summary>
        public ICollection<Dinner> Dinners { get; set; }
    }

    /// <summary>
    /// Dinner set domain class
    /// </summary>
    [Serializable]
    public class RsvpSet
    {
        /// <summary>
        /// Gets or sets TotalRSVPCount.
        /// </summary>
        public int TotalRsvpCount { get; set; }

        /// <summary>
        /// Gets or sets FilteredRSVPCount.
        /// </summary>
        public int FilteredRsvpCount { get; set; }

        /// <summary>
        /// Gets or sets RSVPs.
        /// </summary>
        public ICollection<Rsvp> Rsvps { get; set; }
    }
}