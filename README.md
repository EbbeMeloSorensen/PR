# PR (Person Register)

Dette projekt tog udgangspunkt i Neil Cummings Udemy projekt "Complete guide to building an app with .Net Core and React", specifikt i projektet "Reactivities". Her handler det blot om at administrere kontaktinformation for personer.

Projektet her udgør en merging af en tilpasset version af Reactivities projektet med en tidligere verson af PR projektet, der ikke omfattede noget API eller nogen web klient. Denne tidligere version af PR projektet trak også på en version af Craft-SDK'et.

Bemærk, at løsningen omfatter 2 forskellige applikationslag, hvor det ene bruges af API'et, mens det andet bruges af både en WPF-applikation og en konsolapplikation. Dette er ikke hensigtsmæssigt, da man kun bør have ét applikationslag, som fundament for alle tænkelige overbygninger. Dette er gjort bedre i løsningen Temple.

Bemærk også, at denne version af PR ikke er bitemporal i modsætning til den senere version "PR_Trimmed". PR har til gengæld support for relationer mellem personer og eksport at person netværk til graf, hvad PR_Trimmed ikke har endnu.