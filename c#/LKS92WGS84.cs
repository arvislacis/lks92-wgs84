using System;

static class LKS92WGS84
{
    // Koordinātu pārveidojumos izmantotās konstantes
    private static double PI = Math.PI;                         // Skaitlis pi
    private static double A_AXIS = 6378137;                     // Elipses modeļa lielā ass (a)
    private static double B_AXIS = 6356752.31414;               // Elipses modeļa mazā ass (b)
    private static double CENTRAL_MERIDIAN = PI * 24 / 180;     // Centrālais meridiāns
    private static double OFFSET_X = 500000;                    // Koordinātu nobīde horizontālās (x) ass virzienā
    private static double OFFSET_Y = -6000000;                  // Koordinātu nobīde vertikālās (y) ass virzienā
    private static double SCALE = 0.9996;                       // Kartes mērogojuma faktors (reizinātājs)

    // Aprēķina loka garumu no ekvatora līdz dotā punkta ģeogrāfiskajam platumam
    private static double getArcLengthOfMeridian(double phi)
    {
        double n = (A_AXIS - B_AXIS) / (A_AXIS + B_AXIS);
        double alpha = ((A_AXIS + B_AXIS) / 2) * (1 + (Math.Pow(n, 2) / 4) + (Math.Pow(n, 4) / 64));
        double beta = (-3 * n / 2) + (9 * Math.Pow(n, 3) / 16) + (-3 * Math.Pow(n, 5) / 32);
        double gamma = (15 * Math.Pow(n, 2) / 16) + (-15 * Math.Pow(n, 4) / 32);
        double delta = (-35 * Math.Pow(n, 3) / 48) + (105 * Math.Pow(n, 5) / 256);
        double epsilon = (315 * Math.Pow(n, 4) / 512);

        return alpha * (phi + (beta * Math.Sin(2 * phi)) + (gamma * Math.Sin(4 * phi)) + (delta * Math.Sin(6 * phi)) + (epsilon * Math.Sin(8 * phi)));
    }

    // Aprēķina ģeogrāfisko platumu centrālā meridiāna punktam
    private static double getFootpointLatitude(double y)
    {
        double n = (A_AXIS - B_AXIS) / (A_AXIS + B_AXIS);
        double alpha = ((A_AXIS + B_AXIS) / 2) * (1 + (Math.Pow(n, 2) / 4) + (Math.Pow(n, 4) / 64));
        double yd = y / alpha;
        double beta = (3 * n / 2) + (-27 * Math.Pow(n, 3) / 32) + (269 * Math.Pow(n, 5) / 512);
        double gamma = (21 * Math.Pow(n, 2) / 16) + (-55 * Math.Pow(n, 4) / 32);
        double delta = (151 * Math.Pow(n, 3) / 96) + (-417 * Math.Pow(n, 5) / 128);
        double epsilon = (1097 * Math.Pow(n, 4) / 512);

        return yd + (beta * Math.Sin(2 * yd)) + (gamma * Math.Sin(4 * yd)) + (delta * Math.Sin(6 * yd)) + (epsilon * Math.Sin(8 * yd));
    }

    // Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (bez pārvietojuma un mērogojuma)
    private static double[] convertMapLatLngToXY(double phi, double lambda, double lambda0)
    {
        double[] xy = new double[2] {0, 0};

        double ep2 = (Math.Pow(A_AXIS, 2) - Math.Pow(B_AXIS, 2)) / Math.Pow(B_AXIS, 2);
        double nu2 = ep2 * Math.Pow(Math.Cos(phi), 2);
        double N = Math.Pow(A_AXIS, 2) / (B_AXIS * Math.Sqrt(1 + nu2));
        double t = Math.Tan(phi);
        double t2 = t * t;

        double l = lambda - lambda0;
        double l3coef = 1 - t2 + nu2;
        double l4coef = 5 - t2 + 9 * nu2 + 4 * (nu2 * nu2);
        double l5coef = 5 - 18 * t2 + (t2 * t2) + 14 * nu2 - 58 * t2 * nu2;
        double l6coef = 61 - 58 * t2 + (t2 * t2) + 270 * nu2 - 330 * t2 * nu2;
        double l7coef = 61 - 479 * t2 + 179 * (t2 * t2) - (t2 * t2 * t2);
        double l8coef = 1385 - 3111 * t2 + 543 * (t2 * t2) - (t2 * t2 * t2);

        // x koordināta
        xy[0] = N * Math.Cos(phi) * l + (N / 6 * Math.Pow(Math.Cos(phi), 3) * l3coef * Math.Pow(l, 3)) + (N / 120 * Math.Pow(Math.Cos(phi), 5) * l5coef * Math.Pow(l, 5)) + (N / 5040 * Math.Pow(Math.Cos(phi), 7) * l7coef * Math.Pow(l, 7));

        // y koordināta
        xy[1] = getArcLengthOfMeridian(phi) + (t / 2 * N * Math.Pow(Math.Cos(phi), 2) * Math.Pow(l, 2)) + (t / 24 * N * Math.Pow(Math.Cos(phi), 4) * l4coef * Math.Pow(l, 4)) + (t / 720 * N * Math.Pow(Math.Cos(phi), 6) * l6coef * Math.Pow(l, 6)) + (t / 40320 * N * Math.Pow(Math.Cos(phi), 8) * l8coef * Math.Pow(l, 8));
        return xy;
    }

    // Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (bez pārvietojuma un mērogojuma)
    private static double[] convertMapXYToLatLon(double x, double y, double lambda0)
    {
        double[] latLng = new double[2] {0, 0};

        double phif = getFootpointLatitude(y);
        double ep2 = (Math.Pow(A_AXIS, 2) - Math.Pow(B_AXIS, 2)) / Math.Pow(B_AXIS, 2);
        double cf = Math.Cos(phif);
        double nuf2 = ep2 * Math.Pow(cf, 2);
        double Nf = Math.Pow(A_AXIS, 2) / (B_AXIS * Math.Sqrt(1 + nuf2));
        double Nfpow = Nf;

        double tf = Math.Tan(phif);
        double tf2 = tf * tf;
        double tf4 = tf2 * tf2;

        double x1frac = 1 / (Nfpow * cf);

        Nfpow *= Nf;   // Nf^2
        double x2frac = tf / (2 * Nfpow);

        Nfpow *= Nf;   // Nf^3
        double x3frac = 1 / (6 * Nfpow * cf);

        Nfpow *= Nf;   // Nf^4
        double x4frac = tf / (24 * Nfpow);

        Nfpow *= Nf;   // Nf^5
        double x5frac = 1 / (120 * Nfpow * cf);

        Nfpow *= Nf;   // Nf^6
        double x6frac = tf / (720 * Nfpow);

        Nfpow *= Nf;   // Nf^7
        double x7frac = 1 / (5040 * Nfpow * cf);

        Nfpow *= Nf;   // Nf^8
        double x8frac = tf / (40320 * Nfpow);

        double x2poly = -1 - nuf2;
        double x3poly = -1 - 2 * tf2 - nuf2;
        double x4poly = 5 + 3 * tf2 + 6 * nuf2 - 6 * tf2 * nuf2 - 3 * (nuf2 * nuf2) - 9 * tf2 * (nuf2 * nuf2);
        double x5poly = 5 + 28 * tf2 + 24 * tf4 + 6 * nuf2 + 8 * tf2 * nuf2;
        double x6poly = -61 - 90 * tf2 - 45 * tf4 - 107 * nuf2 + 162 * tf2 * nuf2;
        double x7poly = -61 - 662 * tf2 - 1320 * tf4 - 720 * (tf4 * tf2);
        double x8poly = 1385 + 3633 * tf2 + 4095 * tf4 + 1575 * (tf4 * tf2);

        // Ģeogrāfiskais platums
        latLng[0] = phif + x2frac * x2poly * (x * x) + x4frac * x4poly * Math.Pow(x, 4) + x6frac * x6poly * Math.Pow(x, 6) + x8frac * x8poly * Math.Pow(x, 8);

        // Ģeogrāfiskais garums
        latLng[1] = lambda0 + x1frac * x + x3frac * x3poly * Math.Pow(x, 3) + x5frac * x5poly * Math.Pow(x, 5) + x7frac * x7poly * Math.Pow(x, 7);

        return latLng;
    }

    // Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (ar pārvietojumu un mērogojumu)
    public static double[] convertLatLonToXY(double[] coordinates)
    {
        double lat = coordinates[0] * PI / 180;
        double lng = coordinates[1] * PI / 180;
        double[] xy = convertMapLatLngToXY(lat, lng, CENTRAL_MERIDIAN);

        xy[0] = xy[0] * SCALE + OFFSET_X;
        xy[1] = xy[1] * SCALE + OFFSET_Y;

        if (xy[1] < 0) {
            xy[1] += 10000000;
        }

        return xy;
    }

    // Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (ar pārvietojumu un mērogojumu)
    public static double[] convertXYToLatLon(double[] coordinates)
    {
        double x = (coordinates[0] - OFFSET_X) / SCALE;
        double y = (coordinates[1] - OFFSET_Y) / SCALE;
        double[] latLng = convertMapXYToLatLon(x, y, CENTRAL_MERIDIAN);

        latLng[0] = latLng[0] / PI * 180;
        latLng[1] = latLng[1] / PI * 180;

        return latLng;
    }
}