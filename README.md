# LKS92-WGS84 #

LKS92-WGS84 ir neliela, dažādās programmēšanas valodās uzrakstīta ģeogrāfisko koordinātu pārveidošanas klase,
kas nodrošina Latvijas koordinātu sistēmā (LKS-92) norādīto koordinātu pārveidošanu vispasaules ģeodēzisko
koordinātu sistēmā (WGS-84) un otrādi.

## Klases izmantošana ##

1. Atkarībā no vēlamās programmēšanas valodas izvēlas pirmkoda direktoriju (piemēram, `javascript`).
2. Izvēlētā pirmkoda direktorija satur divas datnes:
    - `LKS92WGS84.*` - atbilstošajā programmēšanas valodā uzrakstītā koordinātu pārveidošanas klase;
    - `example.*` - klases izmantošanas paraugs ar Latvijas galējo punktu (skatīt zemāk) koordinātu pārveidojumu testpiemēriem.
3. Vēlamo koordinātu pārveidošanas klasi `LKS92WGS84.*` lejupielādē/saglabā, veicot tās pirmkoda aplūkošanu un noklikšķinot
uz spiedpogas `Raw`, pēc tam izsaucot lapas/datnes saglabāšanas funkcionalitāti no pārlūkprogrammas.
4. Lejupielādēto datni iekļauj un izmanto vēlamajā projektā, vadoties pēc `example.*` parauga; ja nepieciešams, maina datnes nosaukumu.

## Papildu piezīmes ##

- Koordinātu pārveidojumos tiek pielietots LKS-92 variants, kurā punkta koordinātas nobīdītas par `-600 km = -600000 m` ziemeļu virzienā,
mērogojuma faktors - `0.9996`, bet centrālais ass meridiāns - `24 E`.
Šos parametrus iespējams mainīt klases konstantes mainīgajos `OFFSET_Y`, `SCALE` un `CENTRAL_MERIDIAN`; tāpat tiek izmantoti
arī citi konstantie mainīgie, kas var ietekmēt gala rezultātu precizitāti un atbilstību testa datiem.
- Koordinātu pārveidojumos ieteicams izmantot tikai to punktu koordinātas, kuri atrodas Latvijas teritorijā.
- WGS-84 rezultātu kļūda **nav lielāka** par **`10^(-8) grāda`**, bet LKS-92 rezultātu - **`10^(-2) metra`**, tādēļ šos **pārveidojumus
nav ieteicams izmantot precīzos mērījumos un precīzu aprēķinu veikšanā** vai tml.

## Testpiemēru atskaites punkti ##

Lai salīdzinātu koordinātu pārveidojumu rezultātus, īpaši veicot jaunu programmēšanas valodu pievienošanu, tiek izmantotas
[Latvijas galējo punktu koordinātas](http://www.vietas.lv/index.php?p=34&gid=15):

| Punkta nosaukums | Virziens |  WGS-84 platums      |  WGS-84 garums      |  LKS-92 X      |  LKS-92 Y      |
|:----------------:|:--------:|:--------------------:|:-------------------:|:--------------:|:--------------:|
| Baltās naktis    | Z        |  **58.07950157**4948 | **25.18998697**1284 | **570181.00**0 | **438180.00**0 |
| Austras koks     | A        |  **56.17228278**4562 | **28.09521644**2873 | **754190.00**3 | **232806.00**0 |
| Saules puķe      | D        |  **55.67522824**2509 | **26.58052848**7143 | **662269.00**0 | **172953.00**0 |
| Zaļais stars     | R        |  **56.37700845**5189 | **20.97918588**2058 | **313470.00**0 | **252137.00**0 |

## Programmēšanas valodu atbalsts ##

Saraksts ar projektā pašlaik pieejamajām programmēšanas valodām - valodas, kurām izstrādāta koordinātu pārveidošanas klase `LKS92WGS84.*`:

|                                Programmēšanas valoda                                 |                 Klases autors                    | Pēdējo būtisko izmaiņu datums |
|:------------------------------------------------------------------------------------:|:------------------------------------------------:|:-----------------------------:|
| [C++](https://github.com/arvislacis/lks92-wgs84/tree/master/c%2B%2B)                 | [Arvis Lācis](https://github.com/arvislacis)     | 21.01.2016.                   |
| [C#](https://github.com/arvislacis/lks92-wgs84/tree/master/c%23)                     | [Arvis Lācis](https://github.com/arvislacis)     | 28.12.2015.                   |
| [Java](https://github.com/arvislacis/lks92-wgs84/tree/master/java)                   | [Arvis Lācis](https://github.com/arvislacis)     | 11.01.2016.                   |
| [JavaScript](https://github.com/arvislacis/lks92-wgs84/tree/master/javascript)       | [Arvis Lācis](https://github.com/arvislacis)     | 22.01.2016.                   |
| [PHP](https://github.com/arvislacis/lks92-wgs84/tree/master/php)                     | [Arvis Lācis](https://github.com/arvislacis)     | 23.12.2015.                   |
| [Python2/Python3](https://github.com/arvislacis/lks92-wgs84/tree/master/python)      | [Dāvis Mičulis](https://github.com/DavisMiculis) | 24.12.2015.                   |
| [Ruby](https://github.com/arvislacis/lks92-wgs84/tree/master/ruby)                   | [Arvis Lācis](https://github.com/arvislacis)     | 25.01.2016.                   |
| [TypeScript](https://github.com/arvislacis/lks92-wgs84/tree/master/typescript)       | [Arvis Lācis](https://github.com/arvislacis)     | 22.01.2016.                   |
| [Vala](https://github.com/arvislacis/lks92-wgs84/tree/master/vala)                   | [Arvis Lācis](https://github.com/arvislacis)     | 24.01.2016.                   |
| [Visual Basic](https://github.com/arvislacis/lks92-wgs84/tree/master/visual%20basic) | [Arvis Lācis](https://github.com/arvislacis)     | 21.01.2016.                   |

Laika gaitā plānots projektu papildināt ar citām, mazāk vai vairāk, populārām programmēšanas valodām gan no projekta autora,
gan citu interesentu puses.

Jebkuram interesentam ir iespējams iesniegt - gan izmantojot GitHub *[Pull requests sadaļu](https://github.com/arvislacis/lks92-wgs84/pulls)*, gan rakstot personīgi -
jaunu koordinātu pārveidošanas klasi citā, viņam labi zināmā, programmēšanas valodā, ievērojot sekojošus nosacījumus:
- **Nedublēt esošās programmēšanas valodas.** Ja esošajos risinājumos tiek pamanīta kļūda, tad nepieciešams izveidot jaunu problēmas
ziņojumu *([Issues sadaļā](https://github.com/arvislacis/lks92-wgs84/issues))*, nevis pārstrādāt vai veidot no jauna esoša risinājuma variantu.
- **Stingri ievērot projekta autora veidoto klašu pierakstu** - komentāri, funkciju secība, funkciju ieejas un izejas
parametri, vērtības utt. Atkāpes no iepriekš minētajām normām pieļaujamas tikai tad, ja izvēlētajā programmēšanas valodā nav iespējams
izmantot tāda paša veida risinājumu, kas vairumā gadījumu ir apšaubāmi.
- **Izvēlētajā programmēšanas valodā atļauts izmantot unikālos operatorus un iebūvētās funkcijas** - gan kā alternatīvu, gan atkārtojošā,
liekā pirmkoda aizvietošanas nolūkiem -, piemēram, izmantojot valodā iebūvēto funkciju grādu pārveidošanai par radiāniem un otrādi, kas
JavaScript valodā nav pieejama utml. Šādu operatoru, funkciju izmantojums nedrīkst pārlieku sarežģīt klases satura pārskatāmību un
visiem funkciju ieejas, izejas parametriem jāpaliek nemainīgiem.
- **Klases realizācijā izvairīties no papildu bibliotēku vai klašu izmantošanas**, ja tas nav iespējams vai ir ļoti apgrūtinoši, tad
pieļaujama standarta bibliotēku iekļaušana.
- Ja vien to pieļauj programmēšanas valoda, **pirmkods jāstrukturē klases veidā ar statiski izsaucamām funkcijām**, kas nodrošina
klases vienkāršu izmantošanu un atjaunināšanu, kā arī koordinātu pārveidošanas funkciju izsaukšanai nav nepieciešams veidot jaunu klases objektu.
- **Iesniegtajam pirmkodam jāsatur gan klases datne `LKS92WGS84.*`, gan klases izmantošanas parauga un testpiemēru datne `example.*`.**
Neskaidrību gadījumā ieteicams vadīties pēc projektā esošo datņu paraugiem.

Ieteikumu, uzlabojumu vai cita veida kļūdu atklāšanas gadījumā vēlams izveidot jaunu problēmas ziņojumu *([Issues sadaļā](https://github.com/arvislacis/lks92-wgs84/issues))*.

## Izmantotie avoti ##

Lai izveidotu projekta sākotnējo koordinātu pārveidošanas klasi JavaScript programmēšanas valodā, tika izmantoti šādi informācijas avoti:

1. [Koordinātu pārrēķinātājs - NeoGeo.lv](http://neogeo.lv/ekartes/koord2/)
2. [Transverse Mercator: Redfearn series - Wikipedia](https://en.wikipedia.org/wiki/Transverse_Mercator:_Redfearn_series)

## MIT licence ##

    Copyright (c) 2015-2016 Arvis Lācis

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
