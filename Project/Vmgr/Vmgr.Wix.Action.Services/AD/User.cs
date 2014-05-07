using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Runtime.Serialization;

namespace Vmgr.Wix.Action.Services.AD
{
    /// <summary>
    /// An Active Directory user
    /// </summary>
    [Serializable]
    class User : IUser
    {
        /// <summary>
        /// The manager of this User
        /// </summary>
        private IUser manager;

        /// <summary>
        /// The list of group this User is a member of
        /// </summary>
        private List<IGroup> groups;

        //The default constructor
        public User() { }

        /// <summary>
        /// The constructor used for deserialiazation
        /// </summary>
        /// <param name="info">The store for the deserialization of the data</param>
        /// <param name="context">The StreamingContext for the deserialization</param>
        public User(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");

            //Deserialize string properties
            this.EID = (string)info.GetValue("EID", typeof(string));
            this.FirstName = (string)info.GetValue("FirstName", typeof(string));
            this.Initials = (string)info.GetValue("Initials", typeof(string));
            this.LastName = (string)info.GetValue("LastName", typeof(string));
            this.DisplayName = (string)info.GetValue("DisplayName", typeof(string));
            this.Title = (string)info.GetValue("Title", typeof(string));
            this.Department = (string)info.GetValue("Department", typeof(string));
            this.BusinessUnit = (string)info.GetValue("BusinessUnit", typeof(string));
            this.BusinessCategory = (string)info.GetValue("BusinessCategory", typeof(string));
            this.CodeOfConductGroup = (string)info.GetValue("CodeOfConductGroup", typeof(string));
            this.CodeOfConductGroupSecondary = (string)info.GetValue("CodeOfConductGroupSecondary", typeof(string));
            this.CodeOfConductGroupTertiary = (string)info.GetValue("CodeOfConductGroupTertiary", typeof(string));
            this.OfficePhone = (string)info.GetValue("OfficePhone", typeof(string));
            this.TieLine = (string)info.GetValue("TieLine", typeof(string));
            this.MobilePhone = (string)info.GetValue("MobilePhone", typeof(string));
            this.Pager = (string)info.GetValue("Pager", typeof(string));
            this.OfficeLocation = (string)info.GetValue("OfficeLocation", typeof(string));
            this.Floor = (string)info.GetValue("Floor", typeof(string));
            this.State = (string)info.GetValue("State", typeof(string));
            this.ActiveDirectoryPath = (string)info.GetValue("ActiveDirectoryPath", typeof(string));
            this.Email = (string)info.GetValue("Email", typeof(string));
            this.DirectoryPath = (string)info.GetValue("DirectoryPath", typeof(string));
            this.ManagerEid = (string)info.GetValue("ManagerEid", typeof(string));

        }

        /// <summary>
        /// The EID of this user
        /// </summary>
        public string EID { get; set; }

        /// <summary>
        /// The first name of this user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The middle initials of this user
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// The last name of this user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The display name of this user
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The title of this user
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The department of this user
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// The business unit of this user
        /// </summary>
        public string BusinessUnit { get; set; }
        
        /// <summary>
        /// The business category of this user
        /// </summary>
        public string BusinessCategory { get; set; }

        /// <summary>
        /// The code of conduct of this user
        /// </summary>
        public string CodeOfConductGroup { get; set; }

        /// <summary>
        /// The secondary code of conduct group of this user
        /// </summary>
        public string CodeOfConductGroupSecondary { get; set; }

        /// <summary>
        /// The tertiary code of conduct group of this user
        /// </summary>
        public string CodeOfConductGroupTertiary { get; set; }

        /// <summary>
        /// The office phone number of this user
        /// </summary>
        public string OfficePhone { get; set; }

        /// <summary>
        /// The tileline phone number of this user
        /// </summary>
        public string TieLine { get; set; }
        
        /// <summary>
        /// The mobile phone number of this user
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// The pager information of this user
        /// </summary>
        public string Pager { get; set; }

        /// <summary>
        /// The office location of this user
        /// </summary>
        public string OfficeLocation { get; set; }

        /// <summary>
        /// The floor number of this user
        /// </summary>
        public string Floor { get; set; }

        /// <summary>
        /// The state of this user
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The Active Directory path of this user object
        /// </summary>
        public string ActiveDirectoryPath { get; set; }

        /// <summary>
        /// The email of this user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The directory path that was searched to find this user object
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// The EID of this user's manager
        /// </summary>
        public string ManagerEid { get; set; }


        /// <summary>
        /// Gets a IUser object for this user's manager.  This object is lazy loaded when requested.
        /// </summary>
        public IUser Manager
        {
            get
            {
                if (manager == null && ManagerEid != "")
                {
                    using (ActiveDirectoryContext context = new ActiveDirectoryContext(this.DirectoryPath))
                    {
                        manager = context.Users.SelectByProperty(UserSearchableProperties.EID, this.ManagerEid).FirstOrDefault();
                    }
                }
                return manager;
            }
        }

        /// <summary>
        /// Gets the list of IGroup objects that this user is a memeber of.  This list is lazy loaded when requested.
        /// </summary>
        public List<IGroup> Groups
        {
            get
            {
                if (groups == null)
                {
                    using (ActiveDirectoryContext context = new ActiveDirectoryContext(this.DirectoryPath))
                    {
                        groups = context.Groups.SelectByMembership(this);
                    }
                }
                return groups;
            }
        }

        /// <summary>
        /// Compares one User object to another for sorting by Last Name/First Name.  
        /// If an object not of type IUser is passed the method will throw an exception.
        /// </summary>
        /// <param name="obj">The IUser to compare to</param>
        /// <returns>The result of the comparison</returns>
        public int CompareTo(object obj)
        {
            if (!obj.GetType().GetInterfaces().Contains(typeof(IUser)))
                throw new Exception("Users objects can only be compared to other User objects");

            User userToCompare = obj as User;

            if (this.LastName != userToCompare.LastName)
                return this.LastName.CompareTo(userToCompare.LastName);
            else if (this.FirstName != userToCompare.FirstName)
                return this.FirstName.CompareTo(userToCompare.FirstName);
            else
                return this.EID.CompareTo(userToCompare.EID);
        }

        /// <summary>
        /// Checks equality of two IUser objects by comparing EID
        /// If an object not of type IUser is passed the method will throw an exception.
        /// </summary>
        /// <param name="obj">The IUser to compare to</param>
        /// <returns>The result of the equality check</returns>
        public override bool Equals(object obj)
        {
            if (!obj.GetType().GetInterfaces().Contains(typeof(IUser)))
                throw new Exception("Users objects can only be checked for equality to other User objects");

            User userToCompare = obj as User;

            return this.EID == userToCompare.EID;
        }

        /// <summary>
        /// Override of GetHashCode, called base.GetHashCode.  Overridden because of compiler warning when overriding Equals
        /// </summary>
        /// <returns>The object's hash</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets object data for seraliation
        /// </summary>
        /// <param name="info">The store for the serialization data</param>
        /// <param name="context">The StreamingContext for the serialization</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");

            //Serialize string properties
            info.AddValue("EID", this.EID);
            info.AddValue("FirstName", this.FirstName);
            info.AddValue("Initials", this.Initials);
            info.AddValue("LastName", this.LastName);
            info.AddValue("DisplayName", this.DisplayName);
            info.AddValue("Title", this.Title);
            info.AddValue("Department", this.Department);
            info.AddValue("BusinessUnit", this.BusinessUnit);
            info.AddValue("BusinessCategory", this.BusinessCategory);
            info.AddValue("CodeOfConductGroup", this.CodeOfConductGroup);
            info.AddValue("CodeOfConductGroupSecondary", this.CodeOfConductGroupSecondary);
            info.AddValue("CodeOfConductGroupTertiary", this.CodeOfConductGroupTertiary);
            info.AddValue("OfficePhone", this.OfficePhone);
            info.AddValue("TieLine", this.TieLine);
            info.AddValue("MobilePhone", this.MobilePhone);
            info.AddValue("Pager", this.Pager);
            info.AddValue("OfficeLocation", this.OfficeLocation);
            info.AddValue("Floor", this.Floor);
            info.AddValue("State", this.State);
            info.AddValue("ActiveDirectoryPath", this.ActiveDirectoryPath);
            info.AddValue("Email", this.Email);
            info.AddValue("DirectoryPath", this.DirectoryPath);
            info.AddValue("ManagerEid", this.ManagerEid);
        }
    }
}
