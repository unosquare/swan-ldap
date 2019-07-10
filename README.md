
# <img src="https://github.com/unosquare/swan-ldap/raw/master/swan-logo-32.png"></img> SWAN: Stuff We All Need - LDAP Client

**SWAN LDAP Client** was previously included as part of the main SWAN project, but since there is not too much products using an LDAP Client we removed as standalone library.


*:star: Please star this project if you find it useful!*

SWAN stands for Stuff We All Need

Repeating code and reinventing the wheel is generally considered bad practice. At [Unosquare](https://www.unosquare.com) we are committed to beautiful code and great software. Swan is a collection of classes and extension methods that we (and other good developers) have written and evolved over the years. We found ourselves copying and pasting the same code for every project every time we started them. We decided to kill that cycle once and for all. This is the result of that idea. Our philosophy is that Swan should have no external dependencies, it should be cross-platform, and it should be useful.

## Installation:

[![NuGet version](https://badge.fury.io/nu/Unosquare.Swan.Ldap.svg)](https://badge.fury.io/nu/Unosquare.Swan.Ldap)

```
PM> Install-Package Unosquare.Swan.Ldap
```

## The `LDAPConnection` class
The **Lightweight Directory Access Protocol** or LDAP is a network protocol for querying and modifying items in directory service providers like [Active Directory](https://en.wikipedia.org/wiki/Active_Directory) which provide a systematic set of records organized in a hierarchical structure. Active Directory stores information about users, computers, groups and other objects that are part of a `domain`.

[LdapConnection API Doc](https://unosquare.github.io/swan/api/Unosquare.Swan.Networking.Ldap.LdapConnection.html)

#### Operations
LDAP has a couple of operations that can be executed


* **Bind**: binds the current connection to a set of  credentials
* **Unbind or Disconnect**: signals the server that the connection is about to close then the server proceeds to close the connection to the client
* **Modify**: this operation is used by LDAP clients to request a change to be performed to the already existing database. This operation is used in combination with one of following :
  * **Add**: inserts a new entry into the directory 
  * **Delete**: deletes an entry from the directory
   * **Replace**: modifies an existing property value
   
#### Example 1: Connecting to a LDAP Server
A connection to a LDAP server is a two-step process, first we `connect` to a server but that connection is unauthenticated so we need to bind it to a set of credentials. The reason for breaking down the connection process into a two-step action allows us to reset the authorization state using the same connection. 

```csharp
 // Create a  LdapConnection variable
 var connection = new LdapConnection();
 
 // Connect to a server with a deafult port 
 await connection.Connect("ldap.forumsys.com", 389);
 
 // Set up the credentials 
 await connection.Bind("cn=read-only-admin,dc=example,dc=com", "password");
```
#### Example 2: Reading all the properties of an entry
After establishing a connection you can use the connection's Read method to retrieve all properties of an entry
```csharp
// Get all properties of 'tesla'
 var properties = await connection.Read("uid=tesla,dc=example,dc=com");
 
 // After getting all properties from an entry select its email and print it
 properties.GetAttribute("mail").StringValue.Info();
```
#### Example 3: Searching entries
 There are three scopes for searching entries :
1. **ScopeBase**: searches only at the base dn
2. **ScopeOne**: searches all entries one level under the specified dn
3. **ScopeSub**: as mentioned above this allows to search entries at all levels

```csharp
// Retrieve all entries that have the specified email using ScopeSub 
// which searches all entries at all levels under and including the specified base DN
var searchResult = await connection.Search("dc=example,dc=com",LdapConnection.ScopeSub,"(cn=Isaac Newton)");

// If there are more entries remaining
  while (searchResult.HasMore())
  {
      // Point to the next entry
      var entry = searchResult.Next();
      
      // Get all attributes 
      var entryAttributes = entry.GetAttributeSet();
      
      // Select its email and print it
      entryAttributes.GetAttribute("cn").StringValue.Info();
  }
```
 #### Example 4: Modifying an entry attribute 
An easy way to deal with attributes modification is by calling the Modify method with a `LdapModificationOp` such as:
* **Replace**: overrides an attribute value. 
   * If the attribute does not exist it creates a new one
   * If no value is passed the entire attribute is deleted
* **Delete** : deletes a value from an attribute.
   * If no values are listed or if all of them are the entire attribute is removed
* **Add**: adds a new value to an attribute 
   * If the attribute does not exist a new one is created
 ```csharp
 // Modify Tesla and sets its email as tesla@email.com
 connection.Modify("uid=tesla,dc=example,dc=com", 
    new[] { new LdapModification(LdapModificationOp.Replace, "mail", "tesla@email.com") });
    
   // Deletes the listed values from the given attribute
 connection.Modify("uid=tesla,dc=example,dc=com", 
    new[] { new LdapModification(LdapModificationOp.Delete, "mail", "tesla@email.com") });

 // Add back the recently deleted property
 connection.Modify("uid=tesla,dc=example,dc=com", 
    new[] { new LdapModification(LdapModificationOp.Add, "mail", "tesla@email.com") });


 // disconnect from the LDAP server
 connection.Disconnect();
 ```
