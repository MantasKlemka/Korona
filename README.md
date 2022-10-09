# Projektas „Korona“

## Sistemos paskirtis 
Projekto tikslas – sukurti sistemą, kuri lengvintu Koronos viruso užsikrėtusių asmenų registraciją bei stebėjimą.
Veikimo principas – kuriamą platformą sudaro dvi dalys: internetinė aplikacija, kuria naudosis gydytojai, pacientai ir administratorius bei aplikacijų programavimo sąsaja (angl. trump. API). Gydytojas, norėdamas naudotis šia platforma, prie jos prisiregistruos ir po sėkmingo paskyros sukūrimo, prisijungs. Prisijungęs gydytojas gali užregistruoti naujus pacientus, pacientam paskirti izoliaciją, parinkti izoliacijos tipą. Gydytojas, gali matyti visų esamų pacientų informaciją įvedus jų asmens kodą, bei matyti visus savo užregistruotus pacientus. Neprisijungęs vartotojas gali peržiūrėti asmens izoliacijos informaciją, pateikus asmens kodą pagrindiniame puslapyje. Administratorius turės galimybę patvirtinti gydytojų registracijas.

## Funkciniai reikalavimai 
Neregistruotas sistemos naudotojas galės: 
1.	Peržiūrėti platformos pagrindinį puslapį; 
2.	Peržiūrėti izoliacijų sąrašą pagal asmens kodą;


Gydytojas galės: 
1.	Atsijungti nuo internetinės aplikacijos; 
2.	Prisijungti prie platformos; 
3.	Užregistruoti pacientą;
4.  Redaguoti paciento informaciją;
5.  Peržiūrėti visų pacientų sąrašą;
6.	Peržiūrėti savo užregistruotų pacientų sąrašą;
7.  Peržiūrėti konkretaus paciento informaciją;
8.	Užregistruoti paciento izoliaciją;
9.  Redaguoti paciento izoliaciją;
10. Ištrinti paciento izoliaciją;
11. Peržiūrėti visas paciento izoliacijas;
12. Peržiūrėti visas esamas izoliacijas;
13. Peržiūrėti konkrečios izoliacijos informaciją;
14.	Užregistruoti izoliacijos testą;
15.  Redaguoti izoliacijos testą;
16.  Ištrinti izoliacijos testą;
17.  Peržiūrėti konkretaus testo informaciją;
18.  Peržiūrėti visų esamų testų informaciją;
19. Peržiūrėti visų konkrečios izoliacijos testų sąrašą;
20.	Peržiūrėti visų esamų gydytojų sąrašą;


Administratorius galės: 
1.	Patvirtinti gydytojo registraciją;
2.	Ištrinti gydytojo paskyrą;
3.  Ištrinti paciento paskyrą;
 
## Sistemos architektūra 
Sistemos sudedamosios dalys: 
-   Kliento pusė – naudojant React.js; 
-	Serverio pusė – naudojant ASP.NET.
-	Duomenų bazė – MySQL. 

1 pav. pavaizduota kuriamos sistemos diegimo diagrama. Sistemos talpinimui yra
naudojamas Azure serveris. Kiekviena sistemos dalis yra diegiama tame pačiame serveryje.
Internetinė aplikacija yra pasiekiama per HTTP protokolą. Šios sistemos veikimui yra
reikalingas Korona API, kuris pasiekiamas per aplikacijų programavimo sąsają. Pats Korona
API vykdo duomenų mainus su duomenų baze - tam naudojama ORM sąsaja. 

![image](https://user-images.githubusercontent.com/78092109/191050345-a04125cb-e087-450d-b6b7-edf7897545e4.png)

1 pav.

## API aprašas

Endpoints:
- Doctor
    - GET       /api/Doctor/All
    - POST      /api/Doctor/
    - PUT       /api/Doctor/Activate/{personalCode}
    - DELETE    /api/Doctor/{personalCode}

- Pacient
    - GET       /api/Pacient/All/
    - POST      /api/Pacient/
    - GET       /api/Pacient/{personalCode}
    - PUT       /api/Pacient/{personalCode}
    - DELETE    /api/Pacient/{personalCode}
    - GET       /api/Pacient/All/{personalCode}
    
- Isolation
    - GET       /api/Isolation/All
    - GET       /api/Isolation/All/{personalCode}
    - POST      /api/Isolation/
    - GET       /api/Isolation/{isolationID}
    - PUT       /api/Isolation/{isolationID}
    - DELETE    /api/Isolation/{isolationID}
    
- Test
    - GET       /api/Test/All
    - GET       /api/Test/All/{isolationID}
    - POST      /api/Test/
    - GET       /api/Test/{testID}
    - PUT       /api/Test/testID
    - DELETE    /api/Test/testID
