# Projektas „Korona“

## Sistemos paskirtis 
Projekto tikslas – sukurti sistemą, kuri lengvintu Koronos viruso užsikrėtusių asmenų registraciją bei stebėjimą.
Veikimo principas – kuriamą platformą sudaro dvi dalys: internetinė aplikacija, kuria naudosis gydytojai, pacientai ir administratorius bei aplikacijų programavimo sąsaja (angl. trump. API). Gydytojas, norėdamas naudotis šia platforma, prie jos prisiregistruos ir po sėkmingo paskyros sukūrimo, prisijungs. Prisijungęs gydytojas gali užregistruoti naujus pacientus, pacientam paskirti izoliaciją, parinkti izoliacijos tipą bei sunkiai sergančiam pacientui rezervuoti vietą atitinkamoje ligoninėje. Gydytojas, gali matyti visų esamų pacientų informaciją įvedus jų asmens kodą, bei matyti visus savo užregistruotus pacientus. Užregistruotas pacientas, turės galimybę prisijungti prie sistemos nurodydamas savo asmens kodą, bei slaptažodį ( slaptažodį susikuria pirmo prisijungimo metu ). Pacientas turi galimybę prisijungti, tik jei jau yra užregistruotas gydytojo. Prisijungęs pacientas, gali matyti detalesnę informaciją apie savo izoliaciją, jos trukmę bei priežastį. Neprisijungės vartotojas taip pat gali peržiūrėti asmens izoliacijos laikotarpį, pateikus asmens kodą pagrindiniame puslapyje. Administratorius turės galimybę patvirtinti gydytojų registracijas, įvesti naujas ligonines į sistemą.

## Funkciniai reikalavimai 
Neregistruotas sistemos naudotojas galės: 
1.	Peržiūrėti platformos pagrindinį puslapį; 
2.	Peržiūrėti asmens izoliacijos pabaigą pagal asmens kodą; 
Pacientas galės: 
1.	Atsijungti nuo internetinės aplikacijos; 
2.	Prisijungti prie platformos;
3.	Peržiūrėti detalesnę informaciją apie izoliaciją;

Gydytojas galės: 
1.	Atsijungti nuo internetinės aplikacijos; 
2.	Prisijungti prie platformos; 
3.	Užregistruoti pacientą:
    -	Pridėti paciento asmeninę informaciją;
    - Gyvenamą vietą;
4.	Užregistruoti paciento izoliaciją:
    - Įtraukti izoliacijos priežastį;
    - Pateikti izoliacijos laikotarpį;
5.	Rezervuoti pacientui vietą ligoninėje;
6.	Peržiūrėti kitus esamus pacientus, bei jų izoliacijas, įvedus jų asmens kodą;
7.	Peržiūrėti savo užregistruotų pacientų informaciją bei izoliacijas;
8.	Peržiūrėti kitų esamų gydytojų informaciją;

Administratorius galės: 
1.	Patvirtinti gydytojo registraciją;
2.	Registruoti naujas ligonines;
 
## Sistemos architektūra 
Sistemos sudedamosios dalys: 
-   Kliento pusė – naudojant React.js; 
-	Serverio pusė – naudojant ASP.NET.
-	Duomenų bazė – MySQL. 

2.1 pav. pavaizduota kuriamos sistemos diegimo diagrama. Sistemos talpinimui yra
naudojamas Azure serveris. Kiekviena sistemos dalis yra diegiama tame pačiame serveryje.
Internetinė aplikacija yra pasiekiama per HTTP protokolą. Šios sistemos veikimui yra
reikalingas Korona API, kuris pasiekiamas per aplikacijų programavimo sąsają. Pats Korona
API vykdo duomenų mainus su duomenų baze - tam naudojama ORM sąsaja. 
