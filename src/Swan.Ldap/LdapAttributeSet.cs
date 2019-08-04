namespace Swan.Ldap
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A set of LdapAttribute objects.
    /// An LdapAttributeSet is a collection of LdapAttribute
    /// classes as returned from an LdapEntry on a search or read
    /// operation. LdapAttributeSet may be also used to construct an entry
    /// to be added to a directory.  
    /// </summary>
    /// <seealso cref="LdapAttribute"></seealso>
    /// <seealso cref="LdapEntry"></seealso>
    [Serializable]
    public class LdapAttributeSet : Dictionary<string, LdapAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LdapAttributeSet"/> class.
        /// </summary>
        public LdapAttributeSet()
            : base(StringComparer.OrdinalIgnoreCase)
        {
            // placeholder
        }

        /// <summary>
        /// Creates a new attribute set containing only the attributes that have
        /// the specified subtypes.
        /// For example, suppose an attribute set contains the following
        /// attributes:
        /// <ul><li>    cn</li><li>    cn;lang-ja</li><li>    sn;phonetic;lang-ja</li><li>    sn;lang-us</li></ul>
        /// Calling the <c>getSubset</c> method and passing lang-ja as the
        /// argument, the method returns an attribute set containing the following
        /// attributes:.
        /// <ul><li>cn;lang-ja</li><li>sn;phonetic;lang-ja</li></ul>
        /// </summary>
        /// <param name="subtype">Semi-colon delimited list of subtypes to include. For
        /// example:
        /// <ul><li> "lang-ja" specifies only Japanese language subtypes</li><li> "binary" specifies only binary subtypes</li><li>
        /// "binary;lang-ja" specifies only Japanese language subtypes
        /// which also are binary
        /// </li></ul>
        /// Note: Novell eDirectory does not currently support language subtypes.
        /// It does support the "binary" subtype.</param>
        /// <returns>
        /// An attribute set containing the attributes that match the
        /// specified subtype.
        /// </returns>
        public LdapAttributeSet GetSubset(string subtype)
        {
            // Create a new tempAttributeSet
            var tempAttributeSet = new LdapAttributeSet();

            foreach (var kvp in this)
            {
                if (kvp.Value.HasSubtype(subtype))
                    tempAttributeSet.Add(kvp.Value.Clone());
            }

            return tempAttributeSet;
        }

        /// <summary>
        /// Returns <c>true</c> if this set contains an attribute of the same name
        /// as the specified attribute.
        /// </summary>
        /// <param name="attr">Object of type <c>LdapAttribute</c>.</param>
        /// <returns>
        /// true if this set contains the specified attribute.
        /// </returns>
        public bool Contains(object attr) => ContainsKey(((LdapAttribute)attr).Name);

        /// <summary>
        /// Adds the specified attribute to this set if it is not already present.
        /// If an attribute with the same name already exists in the set then the
        /// specified attribute will not be added.
        /// </summary>
        /// <param name="attr">Object of type <c>LdapAttribute</c>.</param>
        /// <returns>
        /// <c>true</c> if the attribute was added.
        /// </returns>
        public bool Add(LdapAttribute attr)
        {
            var name = attr.Name;

            if (ContainsKey(name))
                return false;

            this[name] = attr;
            return true;
        }

        /// <summary>
        /// Removes the specified object from this set if it is present.
        /// If the specified object is of type <c>LdapAttribute</c>, the
        /// specified attribute will be removed.  If the specified object is of type
        /// string, the attribute with a name that matches the string will
        /// be removed.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>
        /// true if the object was removed.
        /// </returns>
        public bool Remove(LdapAttribute entry) => Remove(entry.Name);

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var retValue = new StringBuilder("LdapAttributeSet: ");
            var first = true;

            foreach (var attr in this)
            {
                if (!first)
                {
                    retValue.Append(" ");
                }

                first = false;
                retValue.Append(attr);
            }

            return retValue.ToString();
        }
    }
}