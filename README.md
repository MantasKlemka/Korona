# Projektas „Korona“

## Sistemos paskirtis 
Projekto tikslas – sukurti sistemą, kuri lengvintu Koronos viruso užsikrėtusių asmenų registraciją bei stebėjimą.
Veikimo principas – kuriamą platformą sudaro dvi dalys: internetinė aplikacija, kuria naudosis gydytojai, neregistruotas sistemos naudotojas ( pacientas ) ir administratorius bei aplikacijų programavimo sąsaja. Gydytojas, norėdamas naudotis šia platforma, prie jos prisiregistruos ir po sėkmingo paskyros sukūrimo, prisijungs. Prisijungęs gydytojas gali užregistruoti naujus pacientus, pacientam paskirti izoliaciją, parinkti izoliacijos tipą. Gydytojas, gali matyti visus esamus pacientus arba visus savo užregistruotus pacientus. Neprisijungęs vartotojas gali peržiūrėti asmens izoliacijos informaciją, pateikus izoliacijos kodą pagrindiniame puslapyje. Administratorius turės prieigą prie viso sistemos funkcionalumo, bei papildomą galimybę patvirtinti gydytojų registracijas, bei ištrinti jų paskyras.

## Funkciniai reikalavimai 
**Sistemos naudotojai:**
-   Neregistruotas sistemos naudotojas ( pacientas )
-   Gydytojas
-   Administratorius

**Taikomosios srities objektai susieti hierarchiniu ryšiu:**<br/>
Pacientas -> Izoliacija -> Testas
<br>
### Funkcionalumas
***Neregistruoto sistemos naudotojo funkcionalumas:***
-	Peržiūrėti platformos pagrindinį puslapį; 
-	Peržiūrėti izoliacijų sąrašą pagal asmens kodą;


***Gydytojo funkcionalumas:***
-	Prisijungti prie platformos; 
-	Atsijungti nuo internetinės aplikacijos; 
-	Peržiūrėti visų esamų gydytojų sąrašą;
-   **Veiksmai su pacientais:**
    -	Užregistruoti pacientą;
    -   Redaguoti paciento informaciją;
    -   Peržiūrėti visų pacientų sąrašą;
    -	Peržiūrėti savo užregistruotų pacientų sąrašą;
    -   Peržiūrėti konkretaus paciento informaciją;
-   **Veiksmai su pacientų izoliacijomis:**
    -	Užregistruoti paciento izoliaciją;
    -   Redaguoti paciento izoliaciją;
    -   Ištrinti paciento izoliaciją;
    -   Peržiūrėti visas paciento izoliacijas;
    -   Peržiūrėti visas esamas izoliacijas;
    -   Peržiūrėti konkrečios izoliacijos informaciją;
-   **Veiksmai su izoliacijų testais:**
    -	Užregistruoti izoliacijos testą;
    -   Redaguoti izoliacijos testą;
    -   Ištrinti izoliacijos testą;
    -   Peržiūrėti konkretaus testo informaciją;
    -   Peržiūrėti visų esamų testų informaciją;
    -   Peržiūrėti visų konkrečios izoliacijos testų sąrašą;


***Administratoriaus funkcionalumas:***
-	Patvirtinti gydytojo registraciją;
-	Ištrinti gydytojo paskyrą;
-   Ištrinti paciento paskyrą;
 
## Sistemos architektūra 
Sistemos sudedamosios dalys: 
-   Kliento pusė – naudojant React.js; 
-	Serverio pusė – naudojant ASP.NET.
-	Duomenų bazė – MySQL ( MariaDB )

1 pav.

## API aprašas

- ## Daktaro endpointai:
    ### **GET**       /api/Doctor/All
    **Paskirtis:** gražinti visų daktarų sąrašą <br>
    **Sėkmingo atsakymo kodas:** 200 <br>
    **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
    **Autorizuoti vartotojai:** administratorius, daktaras <br>
    
    **Užklausos sėkmingo atsakymo pavyzdys:**
    <pre>
    {
        "id": 150,
        "email": "gytux@gmail.com",
        "name": "Gytis",
        "surname": "Stankevicius",
        "password": "123456789",
        "activated": true
    }</pre>
    
    -------
     ### **POST**      /api/Doctor
     **Paskirtis:** sukurti daktaro paskyrą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400 <br>
     **Autorizuoti vartotojai:** neregistruotas vartotojas<br>
     
    **Užklausos sėkmingo atsakymo pavyzdys:** 
    <pre>
    Doctor Gytis Stankevicius (gytusx@gmail.com) created
    </pre>
    -------
    
    ### **PUT**       /api/Doctor/Activate/{id}
     **Paskirtis:** aktyvuoti daktaro paskyrą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401, 304 <br>
     **Autorizuoti vartotojai:** administratorius <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Doctor (164) activated
     </pre>
    -------
    ### **DELETE**    /api/Doctor/{id}
     **Paskirtis:** ištrinti daktaro paskyrą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Doctor (164) deleted
     </pre>
    -------
    ### **GET**       /api/Doctor/{doctorID}/Pacients
     **Paskirtis:** gauti visų daktaro pacientų sąrašą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     {
        "id": 150,
        "email": "gytux@gmail.com",
        "name": "Gytis",
        "surname": "Stankevicius",
        "password": "123456789",
        "activated": true
    }
     </pre>

- ## Paciento endpointai:
    ### **GET**       /api/Pacient/All
     **Paskirtis:** gauti visų pacientų sąrašą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     {
        "id": 11,
        "identificationCode": "54212151",
        "name": "Tamas",
        "surname": "Tomasiunas",
        "birthDate": "12/15/1992",
        "phoneNumber": "+37062485839",
        "address": "Ilgenu g. 11, Vilnius",
        "doctor": 150
     }
     </pre>
     -------
    ### **POST**      /api/Pacient
     **Paskirtis:** sukurti naują pacientą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Pacient Tamas Tomasiunas (542112151) created
     </pre>
     -------
    ### **GET**       /api/Pacient/{id}
     **Paskirtis:** gauti paciento informaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     {
        "id": 11,
        "identificationCode": "54212151",
        "name": "Tamas",
        "surname": "Tomasiunas",
        "birthDate": "12/15/1992",
        "phoneNumber": "+37062485839",
        "address": "Ilgenu g. 11, Vilnius",
        "doctor": 150
     }
     </pre>
     -------
     ###  **PUT**       /api/Pacient/{id}
     **Paskirtis:** paredaguoti paciento informaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401, 304 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Pacient (11) updated
     </pre>
     -------
     ### **DELETE**    /api/Pacient/{id}
     **Paskirtis:** ištrinti pacientą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Pacient (49) deleted
     </pre>
     -------
    ###  **GET**       /api/Pacient/{pacientID}/Isolations
     **Paskirtis:** gauti visų paciento izoliacijų sąrašą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     {
        "id": 9,
        "cause": "Diagnosed with COVID-11",
        "startDate": "10/7/2022",
        "amountOfDays": 8,
        "pacient": 12,
        "code": "1222111111"
     }
     </pre>
- ## Izoliacijos endpointai
    ### **GET**       /api/Isolation/All
     **Paskirtis:** gauti visų izoliacijų sąrašą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     {
        "id": 9,
        "cause": "Diagnosed with COVID-11",
        "startDate": "10/7/2022",
        "amountOfDays": 8,
        "pacient": 12,
        "code": "1222111111"
     }
     </pre>
    -------
    ### **POST**      /api/Isolation/
     **Paskirtis:** sukurti naują izoliaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Isolation (9) created
     </pre>
    -------
    ### **GET**       /api/Isolation/{id}
     **Paskirtis:** gauti izoliacijos informaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     {
        "id": 9,
        "cause": "Diagnosed with COVID-11",
        "startDate": "10/7/2022",
        "amountOfDays": 8,
        "pacient": 12,
        "code": "1222111111"
     }
     </pre>
    -------
    ### **PUT**       /api/Isolation/{id}
     **Paskirtis:** redaguoti izoliacijos informaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401, 304 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Isolation (9) updated
     </pre>
    -------
    ### **DELETE**    /api/Isolation/{id}
     **Paskirtis:** ištrinti izoliaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Isolation (9) deleted
     </pre>
    -------
    ### **GET**       /api/Isolation/{isolationID}/Tests
     **Paskirtis:** gauti visų izoliacijos testų sąrašą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     {
        "id": 5,
        "date": "10/9/2022",
        "type": "SELF TEST",
        "result": "NEGATIVE",
        "isolation": 9
     }
     </pre>
    -------
    ### **GET**       /api/Isolation/Check/{isolationCode}
     **Paskirtis:** gauti izoliacijos informaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400 <br>
     **Autorizuoti vartotojai:** neregistruotas vartotojas <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     {
          "Cause": "Diagnosed with COVID-11",
          "StartDate": "10/7/2022",
          "AmountOfDays": 8
     }
     </pre>
    
- ## Testo endpointai:
    ### **GET**       /api/Test/All
     **Paskirtis:** gauti visų testų sąrašą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     {
        "id": 5,
        "date": "10/9/2022",
        "type": "SELF TEST",
        "result": "NEGATIVE",
        "isolation": 9
     }
     </pre>
    -------
    ### **POST**      /api/Test/
     **Paskirtis:** sukurti naują testą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Test (5) created
     </pre>
    -------
    ### **GET**       /api/Test/{id}
     **Paskirtis:** gauti testo informaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     {
        "id": 5,
        "date": "10/9/2022",
        "type": "SELF TEST",
        "result": "NEGATIVE",
        "isolation": 9
     }
     </pre>
    -------
    ### **PUT**       /api/Test/{id}
     **Paskirtis:** redaguoti testo informaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401, 304 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Test (5) updated
     </pre>
    -------
    ### **DELETE**    /api/Test/{id}
     **Paskirtis:** ištrinti testą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Test (5) deleted
     </pre>
    -------
    
- Pagrindinio lango:
    ### **POST**      /api/Main/Login
     **Paskirtis:** prisijungti prie sistemos <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400 <br>
     **Autorizuoti vartotojai:** neregistruotas vartotojas <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Berear eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJwb3BhcyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluaXN0cmF0b3IiLCJqdGkiOiI2MjE1MjA0MS1mMDFkLTQ5ODMtYjczMi1hZjEwOTQ1MDQwYzAiLCJleHAiOjE2NzE3MjE4NTQsImlzcyI6Ik1hbnRhc0tsZW1rYSIsImF1ZCI6Ik1hbnRhc0tsZW1rYSJ9.DYnmPr8eKBhGrpQf9-e9nagpcFC8K9XUg3H3iASlMG0
     </pre>
