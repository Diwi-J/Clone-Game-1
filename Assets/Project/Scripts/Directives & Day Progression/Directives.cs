using System.Collections.Generic;
using UnityEngine;

public enum DirectiveType
{
    IdentificationRequired,
    SupportingDocumentRequired,
    RejectMajorOffence,
    KillConfirmedExposure,
    KillSkinwalker,
    CitizensOnly,
    ForeignersRequireEntryPermit
}

public class Directives
{
    public static List<DirectiveType> GetDirectives(int day)
    {
        switch (day)
        {
            case 1:
                return new List<DirectiveType>
                {
                    DirectiveType.IdentificationRequired,
                    DirectiveType.CitizensOnly
                };

            case 2:
                return new List<DirectiveType>
                {
                    DirectiveType.IdentificationRequired,
                    DirectiveType.ForeignersRequireEntryPermit,
                    DirectiveType.KillSkinwalker
                };

            case 3:
                return new List<DirectiveType>
                {
                    DirectiveType.IdentificationRequired,
                    DirectiveType.ForeignersRequireEntryPermit,
                    DirectiveType.SupportingDocumentRequired, // clerance doc 
                    DirectiveType.KillConfirmedExposure,
                    DirectiveType.KillSkinwalker
                };

            default:
                return new List<DirectiveType>();
        }
    }

    public static bool IsActive(int day, DirectiveType directive)
    {
        return GetDirectives(day).Contains(directive);
    }
}
