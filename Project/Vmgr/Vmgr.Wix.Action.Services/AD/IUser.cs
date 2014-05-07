using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Vmgr.Wix.Action.Services.AD
{
    /// <summary>
    /// The interface for an Active Directory user object
    /// </summary>
    public interface IUser : IComparable, ISerializable
    {
        /// <summary>
        /// The EID of this user
        /// </summary>
        string EID { get; set; }

        /// <summary>
        /// The first name of this user
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// The middle initials of this user
        /// </summary>
        string Initials { get; set; }

        /// <summary>
        /// The last name of this user
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// The display name of this user
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// The title of this user
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The department of this user
        /// </summary>
        string Department { get; set; }

        /// <summary>
        /// The business unit of this user
        /// </summary>
        string BusinessUnit { get; set; }

        /// <summary>
        /// The business category of this user
        /// </summary>
        string BusinessCategory { get; set; }

        /// <summary>
        /// The code of conduct of this user
        /// </summary>
        string CodeOfConductGroup { get; set; }

        /// <summary>
        /// The secondary code of conduct group of this user
        /// </summary>
        string CodeOfConductGroupSecondary { get; set; }

        /// <summary>
        /// The secondary code of conduct group of this user
        /// </summary>
        string CodeOfConductGroupTertiary { get; set; }

        /// <summary>
        /// The office phone number of this user
        /// </summary>
        string OfficePhone { get; set; }

        /// <summary>
        /// The tileline phone number of this user
        /// </summary>
        string TieLine { get; set; }

        /// <summary>
        /// The mobile phone number of this user
        /// </summary>
        string MobilePhone { get; set; }

        /// <summary>
        /// The pager information of this user
        /// </summary>
        string Pager { get; set; }

        /// <summary>
        /// The office location of this user
        /// </summary>
        string OfficeLocation { get; set; }

        /// <summary>
        /// The floor number of this user
        /// </summary>
        string Floor { get; set; }

        /// <summary>
        /// The state of this user
        /// </summary>
        string State { get; set; }

        /// <summary>
        /// The Active Directory path of this user object
        /// </summary>
        string ActiveDirectoryPath { get; set; }

        /// <summary>
        /// The email of this user
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// The directory path that was searched to find this user object
        /// </summary>
        string DirectoryPath { get; set; }

        /// <summary>
        /// The EID of this user's manager
        /// </summary>
        string ManagerEid { get; set; }


        /// <summary>
        /// Gets a User object for this user's manager
        /// </summary>
        IUser Manager { get; }

        /// <summary>
        /// Gets the list of IGroup objects that this user is a memeber of
        /// </summary>
        List<IGroup> Groups { get; }
    }
}
