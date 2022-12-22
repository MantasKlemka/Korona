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

<p align="center">1 pav. </p>

## API aprašas

- ### Daktaro endpointai:
    #### **GET**       /api/Doctor/All
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
     #### **POST**      /api/Doctor
     **Paskirtis:** sukurti daktaro paskyrą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400 <br>
     **Autorizuoti vartotojai:** neregistruotas vartotojas<br>
     
    **Užklausos sėkmingo atsakymo pavyzdys:** 
    <pre>
    Doctor Gytis Stankevicius (gytusx@gmail.com) created
    </pre>
    -------
    
    #### **PUT**       /api/Doctor/Activate/{id}
     **Paskirtis:** aktyvuoti daktaro paskyrą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401, 304 <br>
     **Autorizuoti vartotojai:** administratorius <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Doctor (164) activated
     </pre>
    -------
    #### **DELETE**    /api/Doctor/{id}
     **Paskirtis:** ištrinti daktaro paskyrą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Doctor (164) deleted
     </pre>
    -------
    #### **GET**       /api/Doctor/{doctorID}/Pacients
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

- ### Paciento endpointai:
    #### **GET**       /api/Pacient/All
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
    #### **POST**      /api/Pacient
     **Paskirtis:** sukurti naują pacientą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Pacient Tamas Tomasiunas (542112151) created
     </pre>
     -------
    #### **GET**       /api/Pacient/{id}
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
     ####  **PUT**       /api/Pacient/{id}
     **Paskirtis:** paredaguoti paciento informaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401, 304 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Pacient (11) updated
     </pre>
     -------
     #### **DELETE**    /api/Pacient/{id}
     **Paskirtis:** ištrinti pacientą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Pacient (49) deleted
     </pre>
     -------
    ####  **GET**       /api/Pacient/{pacientID}/Isolations
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
- ### Izoliacijos endpointai:
    #### **GET**       /api/Isolation/All
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
    #### **POST**      /api/Isolation/
     **Paskirtis:** sukurti naują izoliaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Isolation (9) created
     </pre>
    -------
    #### **GET**       /api/Isolation/{id}
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
    #### **PUT**       /api/Isolation/{id}
     **Paskirtis:** redaguoti izoliacijos informaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401, 304 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Isolation (9) updated
     </pre>
    -------
    #### **DELETE**    /api/Isolation/{id}
     **Paskirtis:** ištrinti izoliaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Isolation (9) deleted
     </pre>
    -------
    #### **GET**       /api/Isolation/{isolationID}/Tests
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
    #### **GET**       /api/Isolation/Check/{isolationCode}
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
    
- ### Testo endpointai:
    #### **GET**       /api/Test/All
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
    #### **POST**      /api/Test/
     **Paskirtis:** sukurti naują testą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Test (5) created
     </pre>
    -------
    #### **GET**       /api/Test/{id}
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
    #### **PUT**       /api/Test/{id}
     **Paskirtis:** redaguoti testo informaciją <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401, 304 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Test (5) updated
     </pre>
    -------
    #### **DELETE**    /api/Test/{id}
     **Paskirtis:** ištrinti testą <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400, 401 <br>
     **Autorizuoti vartotojai:** administratorius, daktaras <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Test (5) deleted
     </pre>
    -------
    
- ### Pagrindinio lango endpointai:
    #### **POST**      /api/Main/Login
     **Paskirtis:** prisijungti prie sistemos <br>
     **Sėkmingo atsakymo kodas:** 200 <br>
     **Galimi nesėkmingo atsakymo kodai:** 400 <br>
     **Autorizuoti vartotojai:** neregistruotas vartotojas <br>
     
     **Užklausos sėkmingo atsakymo pavyzdys:** 
     <pre>
     Berear eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJwb3BhcyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluaXN0cmF0b3IiLCJqdGkiOiI2MjE1MjA0MS1mMDFkLTQ5ODMtYjczMi1hZjEwOTQ1MDQwYzAiLCJleHAiOjE2NzE3MjE4NTQsImlzcyI6Ik1hbnRhc0tsZW1rYSIsImF1ZCI6Ik1hbnRhc0tsZW1rYSJ9.DYnmPr8eKBhGrpQf9-e9nagpcFC8K9XUg3H3iASlMG0
     </pre>
     
## Projektuojamos sąsajos langų wireframe`ai bei realizaciją
- ### Pagrindinis langas
    #### Wireframe:
    ![image](https://user-images.githubusercontent.com/78092109/209142735-5ec05277-5097-4c56-9c26-9a982324aef2.png)
    <p align="center">2 pav. </p>
    
    #### Realizacija:
    ![image](https://user-images.githubusercontent.com/78092109/209143082-9725177a-2ea5-4848-b67c-3b4872add684.png)
    <p align="center">3 pav. </p>
    
    -------
    
- ### Prisijungimo langas
    #### Wireframe:
    ![image](https://user-images.githubusercontent.com/78092109/209143231-721818e4-06ed-44ea-8aa8-bbf368ffc54d.png)
    <p align="center">4 pav. </p>
    
    #### Realizacija:
    ![image](https://user-images.githubusercontent.com/78092109/209143269-b33b2edf-3b51-429c-a4ef-ef461530e9fb.png)
    <p align="center">5 pav. </p>
    
     -------
     
- ### Registracijos langas
    #### Wireframe:
    ![image](https://user-images.githubusercontent.com/78092109/209143699-d53f99e0-3f15-4171-b9c2-e8dbabd39b52.png)
    <p align="center">6 pav. </p>
    
    #### Realizacija:
    ![image](https://user-images.githubusercontent.com/78092109/209143757-130edb92-1e38-4f2d-8be1-e0c1c674ccc2.png)
    <p align="center">7 pav. </p>
    
     -------
     
- ### Pagrindinis sistemos langas
    #### Wireframe:
    ![image](https://user-images.githubusercontent.com/78092109/209143865-bac921ba-680b-4f75-85ef-d4e2fcf9d1dc.png)
    <p align="center">8 pav. </p>
    
    #### Realizacija:
    ![image](https://user-images.githubusercontent.com/78092109/209143929-8d9a2e84-838d-492b-ac82-e2ee5f7b69ca.png)
    <p align="center">9 pav. </p>
    
     -------
     
- ### Daktarų sistemos langas
    #### Wireframe:
    ![image](https://user-images.githubusercontent.com/78092109/209144166-21b13ef6-5a35-4d48-86b5-d4fbb3355860.png)
    <p align="center">10 pav. </p>
    
    #### Realizacija:
    ![image](https://user-images.githubusercontent.com/78092109/209144214-853baeef-6ba7-4c7c-9e8e-bb5c86e03e9b.png)
    <p align="center">11 pav. </p>
    
     -------
     
- ### Pacientų sistemos langas
    #### Wireframe:
    ![image](https://user-images.githubusercontent.com/78092109/209144259-1cd1c36c-74f9-4441-a65e-76aaf94d10c1.png)
    <p align="center">12 pav. </p>
    
    #### Realizacija:
    ![image](https://user-images.githubusercontent.com/78092109/209144484-1f34c261-bd1e-4633-b319-833ccb0d56b1.png)
    <p align="center">13 pav. </p>
    
     -------
     
- ### Izoliacijų sistemos langas
    #### Wireframe:
    ![image](https://user-images.githubusercontent.com/78092109/209144555-a53e5525-b946-4f20-9bf7-0fa382c300af.png)
    <p align="center">14 pav. </p>
    
    #### Realizacija:
    ![image](https://user-images.githubusercontent.com/78092109/209144631-4125eec6-955d-4772-96d0-98b6bd36fa46.png)
    <p align="center">15 pav. </p>
    
     -------
     
- ### Testų sistemos langas
    #### Wireframe:
    ![image](https://user-images.githubusercontent.com/78092109/209144711-07c87932-421a-464c-a99f-af142cf76290.png)
    <p align="center">16 pav. </p>
    
    #### Realizacija:
    ![image](https://user-images.githubusercontent.com/78092109/209144794-e93275dd-f10e-4c4b-8f55-fde6caa464a4.png)
    <p align="center">17 pav. </p>
