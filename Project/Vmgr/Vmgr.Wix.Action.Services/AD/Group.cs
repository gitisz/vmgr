using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Vmgr.Wix.Action.Services.AD
{
    /// <summary>
    /// An Active Directory group
    /// </summary>
    [Serializable]
    class Group : IGroup
    {
        /// <summary>
        /// The list of IUsers in this Group
        /// </summary>
        private List<IUser> users;

        /// <summary>
        /// The default constructor
        /// </summary>
        public Group() { }

        /// <summary>
        /// The constructor used for deserialiazation
        /// </summary>
        /// <param name="info">The store for the deserialization of the data</param>
        /// <param name="context">The StreamingContext for the deserialization</param>
        public Group(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");

            //Deserialize string properties
            this.Name = (string)info.GetValue("Name", typeof(string));
            this.ActiveDirectoryPath = (string)info.GetValue("ActiveDirectoryPath", typeof(string));
            this.DirectoryPath = (string)info.GetValue("DirectoryPath", typeof(string));
        }

        /// <summary>
        /// The name of the Group
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Active Directory path of this Group object
        /// </summary>
        public string ActiveDirectoryPath { get; set; }

        /// <summary>
        /// The directory path that was searched to find this Group object
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// Gets a list of IUser objects for this Group's membership.  This list is lazy loaded when requested.
        /// </summary>
        public List<IUser> Users
        {
            get
            {
                if (users == null)
                {
                    using (ActiveDirectoryContext context = new ActiveDirectoryContext(this.DirectoryPath))
                    {
                        users = context.Users.SelectByGroup(this.Name);
                    }
                }
                return users;
            }
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
            info.AddValue("Name", this.Name);
            info.AddValue("ActiveDirectoryPath", this.ActiveDirectoryPath);
            info.AddValue("DirectoryPath", this.DirectoryPath);
        }
    }
}
