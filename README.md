# LKS92-WGS84 #

LKS92-WGS84 ir neliela, dažādās programmēšanas valodās uzrakstīta ģeogrāfisko koordinātu pārveidošanas klase,
kas nodrošina Latvijas koordinātu sistēmā (LKS-92) norādīto koordinātu pārveidošanu vispasaules ģeodēzisko
koordinātu sistēmā (WGS-84) un otrādi.

## Klases izmantošana ##

1. Atkarībā no vēlamās programmēšanas valodas izvēlas pirmkoda direktoriju (piemēram, `javascript`).
2. Izvēlētā pirmkoda direktorija satur divas datnes:
    - `lks92-wgs84.*` - atbilstošajā programmēšanas valodā uzrakstītā koordinātu pārveidošanas klase;
    - `example.*` - klases izmantošanas paraugs ar Latvijas galējo punktu (skatīt zemāk) koordinātu pārveidojumu testpiemēriem.
3. Vēlamo koordinātu pārveidošanas klasi `lks92-wgs84.*` lejupielādē/saglabā, veicot tās pirmkoda aplūkošanu un noklikšķinot
uz spiedpogas `Raw`, pēc tam izsaucot lapas/datnes saglabāšanas funkcionalitāti no pārlūkprogrammas.
4. Lejupielādēto datni iekļauj un izmanto vēlamajā projektā, vadoties pēc `example.*` parauga; ja nepieciešams, maina datnes nosaukumu.

## Papildu piezīmes ##

- Koordinātu pārveidojumos tiek pielietots LKS-92 variants, kurā punkta koordinātas nobīdītas par `-600 km = -600000 m` ziemeļu virzienā,
mērogojuma faktors - `0.9996`, bet centrālais ass meridiāns - `24 E`.
Šos parametrus iespējams mainīt klases konstantes mainīgajos `OFFSET_Y`, `SCALE` un `CENTRAL_MERIDIAN`; tāpat tiek izmantoti
arī citi konstantie mainīgie, kas var ietekmēt gala rezultātu precizitāti un atbilstību testa datiem.
- Koordinātu pārveidojumos ieteicams izmantot tikai to punktu koordinātas, kuri atrodas Latvijas teritorijā.
- WGS-84 rezultātu kļūda **nav mazāka** par `10^(-8) grāda`, bet LKS-92 rezultātu - `10^(-2) metra`, tādēļ šos pārveidojumus
nav ieteicams izmantot precīzos mērījumos un precīzu aprēķinu veikšanā vai tml.

## Testpiemēru atskaites punkti ##

Lai salīdzinātu koordinātu pārveidojumu rezultātus, īpaši veicot jaunu programmēšanas valodu pievienošanu, tiek izmantotas
[Latvijas galējo punktu koordinātas](http://www.vietas.lv/index.php?p=34&gid=15):

| Punkta nosaukums | Virziens |  WGS-84 platums  |  WGS-84 garums  |  LKS-92 X  |  LKS-92 Y  |
|:----------------:|:--------:|:----------------:|:---------------:|:----------:|:----------:|
| "Baltās naktis"  | Z        |  58.079501574948 | 25.189986971284 | 570181.000 | 438180.000 |
| "Austras koks"   | A        |  56.172282784562 | 28.095216442873 | 754190.003 | 232806.000 |
| "Saules puķe"    | D        |  55.675228242509 | 26.580528487143 | 662269.000 | 172953.000 |
| "Zaļais stars"   | R        |  56.377008455189 | 20.979185882058 | 313470.000 | 252137.000 |

## Programmēšanas valodu pieejamība ##

Projektā pašlaik pieejamās programmēšanas valodas - valodas, kurām izstrādāta koordinātu pārveidošanas klase `lks92-wgs84.*`:

| Programmēšanas valoda |                 Klases autors                | Pēdējo izmaiņu datums |
|:---------------------:|:--------------------------------------------:|:---------------------:|
| JavaScript            | [Arvis Lācis](https://github.com/arvislacis) | 22.12.2015.           |

Laika gaitā plānots projektu papildināt arī ar citām, mazāk vai vairāk, populārām programmēšanas valodām gan no projekta autora,
gan no citu interesentu puses.

Jebkuram interesentam ir iespējams iesniegt - gan izmantojot GitHub *Pull requests* sistēmu, gan rakstot personīgi -
jaunu koordinātu pārveidošanas klasi citā, viņam labi zināmā, programmēšanas valodā, ievērojot sekojošus nosacījumus:
- **Nedublējiet jau esošās programmēšanas valodas.** Ja esošajos risinājumos pamanāt kļūdu, tad izveidojiet jaunu problēmas
ziņojumu *(Issues)*, nevis pārstrādājiet vai veidojiet jaunu esoša risinājuma variantu.
- **Stingri ievērojiet projekta autora veidoto klašu pierakstu** - komentāri, funkciju secība, funkciju ieejas un izejas
parametri, vērtības utt. Atkāpes no iepriekš uzskaitītajām normām pieļaujamas tikai tad, ja izvēlētajā programmēšanas valodā nav iespējams
izmantot tāda paša veida risinājumu, kas ir apšaubāmi.
- **Atļauts izmantot izvēlētajā programmēšanas valodā unikālos operatorus un iebūvētās funkcijas** - gan kā alternatīvu, gan atkārtojošā,
liekā pirmkoda aizvietošanas nolūkiem -, piemēram, izmantojot valodā iebūvēto funkciju grādu pārveidošanai par radiāniem un otrādi, kas
JavaScript valodā nav pieejama utml. Šādu operatoru, funkciju izmantojums nedrīkst pārlieku sarežģīt koordinātu pārveidošanas klasi un
visiem funkciju ieejas, izejas parametriem jāpaliek nemainīgiem.
- **Klases realizācijā stingri jāizvairās no papildus bibliotēku vai klašu izmantošanas**, ja tas nav iespējams, tad pieļaujama
standarta bibliotēku iekļaušana.
- Ja vien to pieļauj programmēšanas valoda, **pirmkods jāstrukturē klases veidā ar statiski izsaucamām funkcijām**, kas nodrošina
koordinātu pārveidošanas klases vienkāršu izmantošanu un atjaunināšanu, tāpat pārveidošanas funkcionalitātes nodrošināšanai
nav nepieciešams veidot jaunu klases objektu.
- **Iesniegtajam pirmkodam jāsatur gan klases datne `lks92-wgs84.*`, gan klases izmantošanas parauga un testpiemēru datne `example.*`.**
Neskaidrību gadījumā ieteicams vadīties pēc projektā esošo datņu paraugiem.

Ieteikumu, uzlabojumu vai cita veida kļūdu atklāšanas gadījumā ieteicams izveidot jaunu problēmas ziņojumu *(Issues)*.

## Izmantotie avoti ##

Lai izveidotu projekta sākotnējo koordinātu pārveidošanas klasi (JavaScript), tika izmantoti šādi informācijas avoti:

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
