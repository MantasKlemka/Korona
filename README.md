# Projektas „Korona“

## Sistemos paskirtis 
Projekto tikslas – sukurti sistemą, kuri lengvintu Koronos viruso užsikrėtusių asmenų registraciją bei stebėjimą.
Veikimo principas – kuriamą platformą sudaro dvi dalys: internetinė aplikacija, kuria naudosis gydytojai, neregistruotas sistemos naudotojas ( pacientas ) ir administratorius bei aplikacijų programavimo sąsaja. Gydytojas, norėdamas naudotis šia platforma, prie jos prisiregistruos ir po sėkmingo paskyros sukūrimo, prisijungs. Prisijungęs gydytojas gali užregistruoti naujus pacientus, pacientam paskirti izoliaciją, parinkti izoliacijos tipą. Gydytojas, gali matyti visus esamus pacientus arba visus savo užregistruotus pacientus. Neprisijungęs vartotojas gali peržiūrėti asmens izoliacijos informaciją, pateikus asmens kodą pagrindiniame puslapyje. Administratorius turės galimybę patvirtinti gydytojų registracijas, bei ištrinti paskyras.

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

Endpoints:
- Doctor
    - **GET**       /api/Doctor/All
    - **POST**      /api/Doctor/
    - **PUT**       /api/Doctor/Activate/{personalCode}
    - **DELETE**    /api/Doctor/{personalCode}

- Pacient
    - **GET**       /api/Pacient/All/
    - **POST**      /api/Pacient/
    - **GET**       /api/Pacient/{personalCode}
    - **PUT**       /api/Pacient/{personalCode}
    - **DELETE**    /api/Pacient/{personalCode}
    - **GET**       /api/Pacient/All/{personalCode}
    
- Isolation
    - **GET**       /api/Isolation/All
    - **GET**       /api/Isolation/All/{personalCode}
    - **POST**      /api/Isolation/
    - **GET**       /api/Isolation/{isolationID}
    - **PUT**       /api/Isolation/{isolationID}
    - **DELETE**    /api/Isolation/{isolationID}
    
- Test
    - **GET**       /api/Test/All
    - **GET**       /api/Test/All/{isolationID}
    - **POST**      /api/Test/
    - **GET**       /api/Test/{testID}
    - **PUT**       /api/Test/testID
    - **DELETE**    /api/Test/testID
