﻿namespace Unosquare.Swan.Ldap
{
    using System.IO;

    /// <summary>
    /// Represents an Ldap Modify Request.
    /// <pre>
    /// ModifyRequest ::= [APPLICATION 6] SEQUENCE {
    /// object          LdapDN,
    /// modification    SEQUENCE OF SEQUENCE {
    /// operation       ENUMERATED {
    /// add     (0),
    /// delete  (1),
    /// replace (2) },
    /// modification    AttributeTypeAndValues } }
    /// </pre>
    /// </summary>
    /// <seealso cref="Asn1Sequence" />
    /// <seealso cref="IRfcRequest" />
    internal sealed class RfcModifyRequest 
        : Asn1Sequence, IRfcRequest
    {
        public RfcModifyRequest(string obj, Asn1SequenceOf modification)
            : base(2)
        {
            Add(obj);
            Add(modification);
        }
        
        public Asn1SequenceOf Modifications => (Asn1SequenceOf)Get(1);
        
        public override Asn1Identifier GetIdentifier() => new Asn1Identifier(LdapOperation.ModifyRequest);

        public string GetRequestDN() => ((Asn1OctetString)Get(0)).StringValue();
    }

    internal class RfcModifyResponse : RfcLdapResult
    {
        public RfcModifyResponse(Stream stream, int len) 
            : base(stream, len)
        {
        }
        
        public override Asn1Identifier GetIdentifier() => new Asn1Identifier(LdapOperation.ModifyResponse);
    }
}