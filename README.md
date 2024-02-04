![ER-Schema miniprojekt api](https://github.com/Sami3Goran/MiniProjektAPI/assets/146171510/3068e7f0-f0bd-4afa-a88f-2967b2623250)

Hämta alla personer:
Metod: GET
Endpoint: http://localhost:5285/people
Beskrivning: Hämtar alla personer med deras intressen och länkar.


Hämta en persons intressen:
Metod: GET
Endpoint: http://localhost:5285/person/{id}/interests
Beskrivning: Hämtar intressen för en specifik person baserat på personens ID.


Hämta en persons länkar:
Metod: GET
Endpoint: http://localhost:5285/person/{id}/links
Beskrivning: Hämtar länkar för en specifik person baserat på personens ID.


Lägga till en ny person:
Metod: POST
Endpoint: http://localhost:5285/new/person
Beskrivning: Skapar en ny person och lägger till intressen och länkar om det finns.
--->Exempel nedan:
ändra body till JSON
skriv i body:
{
  "firstName": "sami",
  "lastName": "goran",
  "phoneNumber": "0763562651",
  "interests": [
    {
      "title": "cats",
      "descriptions": "purr",
      "interestLinks": [
        {
          "url": "https://cat.com"
        }
      ]
    }
  ]
}


Lägga till ett nytt intresse för en befintlig person:
Metod: POST
Endpoint: http://localhost:5285/person/{personId}/interest
Beskrivning: Lägger till ett nytt intresse för en befintlig person baserat på personens ID.
--->Exempel nedan:
ändra body till JSON
skriv i body:
{
  "Title": "playing darksouls",
  "Descriptions": "god its so hard"
}


Lägga till en ny länk för en person och ett intresse:
Metod: POST
Endpoint: http://localhost:5285/person/{personId}/interest/{interestId}/link
Beskrivning: Lägger till en ny länk för en befintlig person och ett befintligt intresse baserat på deras ID.
--->Exempel nedan:
ändra body till JSON
skriv i body:
{
  "Url": "https://www.Darksouls.com"
}

---------->UML_DIAGRAM NEDAN<-------------

![UML-diagram MiniProjekt API](https://github.com/Sami3Goran/MiniProjektAPI/assets/146171510/f8b1f1f3-858f-4531-9c41-5da8133c352b)












