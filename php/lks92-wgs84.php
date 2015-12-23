<?php

class LKS92WGS84
{
    // Koordinātu pārveidojumos izmantotās konstantes
    private static $A_AXIS = 6378137;           // Elipses modeļa lielā ass (a)
    private static $B_AXIS = 6356752.31414;     // Elipses modeļa mazā ass (b)
    private static $CENTRAL_MERIDIAN = M_PI;    // Centrālais meridiāns
    private static $OFFSET_X = 500000;          // Koordinātu nobīde horizontālās (x) ass virzienā
    private static $OFFSET_Y = -6000000;        // Koordinātu nobīde vertikālās (y) ass virzienā
    private static $SCALE = 0.9996;             // Kartes mērogojuma faktors (reizinātājs)

    // Aprēķina loka garumu no ekvatora līdz dotā punkta ģeogrāfiskajam platumam
    private static function getArcLengthOfMeridian($phi)
    {
        $n = (self::$A_AXIS - self::$B_AXIS) / (self::$A_AXIS + self::$B_AXIS);
        $alpha = ((self::$A_AXIS + self::$B_AXIS) / 2) * (1 + (pow($n, 2) / 4) + (pow($n, 4) / 64));
        $beta = (-3 * $n / 2) + (9 * pow($n, 3) / 16) + (-3 * pow($n, 5) / 32);
        $gamma = (15 * pow($n, 2) / 16) + (-15 * pow($n, 4) / 32);
        $delta = (-35 * pow($n, 3) / 48) + (105 * pow($n, 5) / 256);
        $epsilon = (315 * pow($n, 4) / 512);

        return $alpha * ($phi + ($beta * sin(2 * $phi)) + ($gamma * sin(4 * $phi)) + ($delta * sin(6 * $phi)) + ($epsilon * sin(8 * $phi)));
    }

    // Aprēķina ģeogrāfisko platumu centrālā meridiāna punktam
    private static function getFootpointLatitude($y)
    {
        $n = (self::$A_AXIS - self::$B_AXIS) / (self::$A_AXIS + self::$B_AXIS);
        $alpha = ((self::$A_AXIS + self::$B_AXIS) / 2) * (1 + (pow($n, 2) / 4) + (pow($n, 4) / 64));
        $yd = $y / $alpha;
        $beta = (3 * $n / 2) + (-27 * pow($n, 3) / 32) + (269 * pow($n, 5) / 512);
        $gamma = (21 * pow($n, 2) / 16) + (-55 * pow($n, 4) / 32);
        $delta = (151 * pow($n, 3) / 96) + (-417 * pow($n, 5) / 128);
        $epsilon = (1097 * pow($n, 4) / 512);

        return $yd + ($beta * sin(2 * $yd)) + ($gamma * sin(4 * $yd)) + ($delta * sin(6 * $yd)) + ($epsilon * sin(8 * $yd));
    }

    // Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (bez pārvietojuma un mērogojuma)
    private static function convertMapLatLngToXY($phi, $lambda, $lambda0)
    {
        $xy = [0, 0];

        $ep2 = (pow(self::$A_AXIS, 2) - pow(self::$B_AXIS, 2)) / pow(self::$B_AXIS, 2);
        $nu2 = $ep2 * pow(cos($phi), 2);
        $N = pow(self::$A_AXIS, 2) / (self::$B_AXIS * sqrt(1 + $nu2));
        $t = tan($phi);
        $t2 = $t * $t;

        $l = $lambda - $lambda0;
        $l3coef = 1 - $t2 + $nu2;
        $l4coef = 5 - $t2 + 9 * $nu2 + 4 * ($nu2 * $nu2);
        $l5coef = 5 - 18 * $t2 + ($t2 * $t2) + 14 * $nu2 - 58 * $t2 * $nu2;
        $l6coef = 61 - 58 * $t2 + ($t2 * $t2) + 270 * $nu2 - 330 * $t2 * $nu2;
        $l7coef = 61 - 479 * $t2 + 179 * ($t2 * $t2) - ($t2 * $t2 * $t2);
        $l8coef = 1385 - 3111 * $t2 + 543 * ($t2 * $t2) - ($t2 * $t2 * $t2);

        // x koordināta
        $xy[0] = $N * cos($phi) * $l + ($N / 6 * pow(cos($phi), 3) * $l3coef * pow($l, 3)) + ($N / 120 * pow(cos($phi), 5) * $l5coef * pow($l, 5)) + ($N / 5040 * pow(cos($phi), 7) * $l7coef * pow($l, 7));

        // y koordināta
        $xy[1] = self::getArcLengthOfMeridian($phi) + ($t / 2 * $N * pow(cos($phi), 2) * pow($l, 2)) + ($t / 24 * $N * pow(cos($phi), 4) * $l4coef * pow($l, 4)) + ($t / 720 * $N * pow(cos($phi), 6) * $l6coef * pow($l, 6)) + ($t / 40320 * $N * pow(cos($phi), 8) * $l8coef * pow($l, 8));
        return $xy;
    }

    // Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (bez pārvietojuma un mērogojuma)
    private static function convertMapXYToLatLon($x, $y, $lambda0)
    {
        $latLng = [0, 0];

        $phif = self::getFootpointLatitude($y);
        $ep2 = (pow(self::$A_AXIS, 2) - pow(self::$B_AXIS, 2)) / pow(self::$B_AXIS, 2);
        $cf = cos($phif);
        $nuf2 = $ep2 * pow($cf, 2);
        $Nf = pow(self::$A_AXIS, 2) / (self::$B_AXIS * sqrt(1 + $nuf2));
        $Nfpow = $Nf;

        $tf = tan($phif);
        $tf2 = $tf * $tf;
        $tf4 = $tf2 * $tf2;

        $x1frac = 1 / ($Nfpow * $cf);

        $Nfpow *= $Nf;   // Nf^2
        $x2frac = $tf / (2 * $Nfpow);

        $Nfpow *= $Nf;   // Nf^3
        $x3frac = 1 / (6 * $Nfpow * $cf);

        $Nfpow *= $Nf;   // Nf^4
        $x4frac = $tf / (24 * $Nfpow);

        $Nfpow *= $Nf;   // Nf^5
        $x5frac = 1 / (120 * $Nfpow * $cf);

        $Nfpow *= $Nf;   // Nf^6
        $x6frac = $tf / (720 * $Nfpow);

        $Nfpow *= $Nf;   // Nf^7
        $x7frac = 1 / (5040 * $Nfpow * $cf);

        $Nfpow *= $Nf;   // Nf^8
        $x8frac = $tf / (40320 * $Nfpow);

        $x2poly = -1 - $nuf2;
        $x3poly = -1 - 2 * $tf2 - $nuf2;
        $x4poly = 5 + 3 * $tf2 + 6 * $nuf2 - 6 * $tf2 * $nuf2 - 3 * ($nuf2 * $nuf2) - 9 * $tf2 * ($nuf2 * $nuf2);
        $x5poly = 5 + 28 * $tf2 + 24 * $tf4 + 6 * $nuf2 + 8 * $tf2 * $nuf2;
        $x6poly = -61 - 90 * $tf2 - 45 * $tf4 - 107 * $nuf2 + 162 * $tf2 * $nuf2;
        $x7poly = -61 - 662 * $tf2 - 1320 * $tf4 - 720 * ($tf4 * $tf2);
        $x8poly = 1385 + 3633 * $tf2 + 4095 * $tf4 + 1575 * ($tf4 * $tf2);

        // Ģeogrāfiskais platums
        $latLng[0] = $phif + $x2frac * $x2poly * ($x * $x) + $x4frac * $x4poly * pow($x, 4) + $x6frac * $x6poly * pow($x, 6) + $x8frac * $x8poly * pow($x, 8);

        // Ģeogrāfiskais garums
        $latLng[1] = $lambda0 + $x1frac * $x + $x3frac * $x3poly * pow($x, 3) + $x5frac * $x5poly * pow($x, 5) + $x7frac * $x7poly * pow($x, 7);

        return $latLng;
    }

    // Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (ar pārvietojumu un mērogojumu)
    public static function convertLatLonToXY(array $coordinates)
    {
        $lat = deg2rad($coordinates[0]);
        $lng = deg2rad($coordinates[1]);
        $xy = self::convertMapLatLngToXY($lat, $lng, 24 / 180 * self::$CENTRAL_MERIDIAN);

        $xy[0] = $xy[0] * self::$SCALE + self::$OFFSET_X;
        $xy[1] = $xy[1] * self::$SCALE + self::$OFFSET_Y;

        if ($xy[1] < 0) {
            $xy[1] += 10000000;
        }

        return $xy;
    }

    // Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (ar pārvietojumu un mērogojumu)
    public static function convertXYToLatLon(array $coordinates)
    {
        $x = ($coordinates[0] - self::$OFFSET_X) / self::$SCALE;
        $y = ($coordinates[1] - self::$OFFSET_Y) / self::$SCALE;
        $latLng = self::convertMapXYToLatLon($x, $y, 24 / 180 * self::$CENTRAL_MERIDIAN);

        $latLng[0] = rad2deg($latLng[0]);
        $latLng[1] = rad2deg($latLng[1]);

        return $latLng;
    }
}
