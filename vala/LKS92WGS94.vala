using GLib;

class LKS92WGS84
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
        double alpha = ((A_AXIS + B_AXIS) / 2) * (1 + (Math.pow(n, 2) / 4) + (Math.pow(n, 4) / 64));
        double beta = (-3 * n / 2) + (9 * Math.pow(n, 3) / 16) + (-3 * Math.pow(n, 5) / 32);
        double gamma = (15 * Math.pow(n, 2) / 16) + (-15 * Math.pow(n, 4) / 32);
        double delta = (-35 * Math.pow(n, 3) / 48) + (105 * Math.pow(n, 5) / 256);
        double epsilon = (315 * Math.pow(n, 4) / 512);

        return alpha * (phi + (beta * Math.sin(2 * phi)) + (gamma * Math.sin(4 * phi)) + (delta * Math.sin(6 * phi)) + (epsilon * Math.sin(8 * phi)));
    }

    // Aprēķina ģeogrāfisko platumu centrālā meridiāna punktam
    private static double getFootpointLatitude(double y)
    {
        double n = (A_AXIS - B_AXIS) / (A_AXIS + B_AXIS);
        double alpha = ((A_AXIS + B_AXIS) / 2) * (1 + (Math.pow(n, 2) / 4) + (Math.pow(n, 4) / 64));
        double yd = y / alpha;
        double beta = (3 * n / 2) + (-27 * Math.pow(n, 3) / 32) + (269 * Math.pow(n, 5) / 512);
        double gamma = (21 * Math.pow(n, 2) / 16) + (-55 * Math.pow(n, 4) / 32);
        double delta = (151 * Math.pow(n, 3) / 96) + (-417 * Math.pow(n, 5) / 128);
        double epsilon = (1097 * Math.pow(n, 4) / 512);

        return yd + (beta * Math.sin(2 * yd)) + (gamma * Math.sin(4 * yd)) + (delta * Math.sin(6 * yd)) + (epsilon * Math.sin(8 * yd));
    }

    // Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (bez pārvietojuma un mērogojuma)
    private static double[] convertMapLatLngToXY(double phi, double lambda, double lambda0)
    {
        double[] xy = new double[] {0, 0};

        double ep2 = (Math.pow(A_AXIS, 2) - Math.pow(B_AXIS, 2)) / Math.pow(B_AXIS, 2);
        double nu2 = ep2 * Math.pow(Math.cos(phi), 2);
        double N = Math.pow(A_AXIS, 2) / (B_AXIS * Math.sqrt(1 + nu2));
        double t = Math.tan(phi);
        double t2 = t * t;

        double l = lambda - lambda0;
        double l3coef = 1 - t2 + nu2;
        double l4coef = 5 - t2 + 9 * nu2 + 4 * (nu2 * nu2);
        double l5coef = 5 - 18 * t2 + (t2 * t2) + 14 * nu2 - 58 * t2 * nu2;
        double l6coef = 61 - 58 * t2 + (t2 * t2) + 270 * nu2 - 330 * t2 * nu2;
        double l7coef = 61 - 479 * t2 + 179 * (t2 * t2) - (t2 * t2 * t2);
        double l8coef = 1385 - 3111 * t2 + 543 * (t2 * t2) - (t2 * t2 * t2);

        // x koordināta
        xy[0] = N * Math.cos(phi) * l + (N / 6 * Math.pow(Math.cos(phi), 3) * l3coef * Math.pow(l, 3)) + (N / 120 * Math.pow(Math.cos(phi), 5) * l5coef * Math.pow(l, 5)) + (N / 5040 * Math.pow(Math.cos(phi), 7) * l7coef * Math.pow(l, 7));

        // y koordināta
        xy[1] = getArcLengthOfMeridian(phi) + (t / 2 * N * Math.pow(Math.cos(phi), 2) * Math.pow(l, 2)) + (t / 24 * N * Math.pow(Math.cos(phi), 4) * l4coef * Math.pow(l, 4)) + (t / 720 * N * Math.pow(Math.cos(phi), 6) * l6coef * Math.pow(l, 6)) + (t / 40320 * N * Math.pow(Math.cos(phi), 8) * l8coef * Math.pow(l, 8));

        return xy;
    }

    // Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (bez pārvietojuma un mērogojuma)
    private static double[] convertMapXYToLatLon(double x, double y, double lambda0)
    {
        double[] latLng = new double[] {0, 0};

        double phif = getFootpointLatitude(y);
        double ep2 = (Math.pow(A_AXIS, 2) - Math.pow(B_AXIS, 2)) / Math.pow(B_AXIS, 2);
        double cf = Math.cos(phif);
        double nuf2 = ep2 * Math.pow(cf, 2);
        double Nf = Math.pow(A_AXIS, 2) / (B_AXIS * Math.sqrt(1 + nuf2));
        double Nfpow = Nf;

        double tf = Math.tan(phif);
        double tf2 = tf * tf;
        double tf4 = tf2 * tf2;

        double x1frac = 1 / (Nfpow * cf);

        Nfpow *= Nf;    // Nf^2
        double x2frac = tf / (2 * Nfpow);

        Nfpow *= Nf;    // Nf^3
        double x3frac = 1 / (6 * Nfpow * cf);

        Nfpow *= Nf;    // Nf^4
        double x4frac = tf / (24 * Nfpow);

        Nfpow *= Nf;    // Nf^5
        double x5frac = 1 / (120 * Nfpow * cf);

        Nfpow *= Nf;    // Nf^6
        double x6frac = tf / (720 * Nfpow);

        Nfpow *= Nf;    // Nf^7
        double x7frac = 1 / (5040 * Nfpow * cf);

        Nfpow *= Nf;    // Nf^8
        double x8frac = tf / (40320 * Nfpow);

        double x2poly = -1 - nuf2;
        double x3poly = -1 - 2 * tf2 - nuf2;
        double x4poly = 5 + 3 * tf2 + 6 * nuf2 - 6 * tf2 * nuf2 - 3 * (nuf2 * nuf2) - 9 * tf2 * (nuf2 * nuf2);
        double x5poly = 5 + 28 * tf2 + 24 * tf4 + 6 * nuf2 + 8 * tf2 * nuf2;
        double x6poly = -61 - 90 * tf2 - 45 * tf4 - 107 * nuf2 + 162 * tf2 * nuf2;
        double x7poly = -61 - 662 * tf2 - 1320 * tf4 - 720 * (tf4 * tf2);
        double x8poly = 1385 + 3633 * tf2 + 4095 * tf4 + 1575 * (tf4 * tf2);

        // Ģeogrāfiskais platums
        latLng[0] = phif + x2frac * x2poly * (x * x) + x4frac * x4poly * Math.pow(x, 4) + x6frac * x6poly * Math.pow(x, 6) + x8frac * x8poly * Math.pow(x, 8);

        // Ģeogrāfiskais garums
        latLng[1] = lambda0 + x1frac * x + x3frac * x3poly * Math.pow(x, 3) + x5frac * x5poly * Math.pow(x, 5) + x7frac * x7poly * Math.pow(x, 7);

        return latLng;
    }

    // Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (ar pārvietojumu un mērogojumu)
    public static double[] convertLatLonToXY(double[] coordinates)
    {
        double lat = coordinates[0] * Math.PI / 180;
        double lng = coordinates[1] * Math.PI / 180;
        double[] xy = convertMapLatLngToXY(lat, lng, Math.PI * 24 / 180);

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
        double[] latLng = convertMapXYToLatLon(x, y, Math.PI * 24 / 180);

        latLng[0] = latLng[0] / Math.PI * 180;
        latLng[1] = latLng[1] / Math.PI * 180;

        return latLng;
    }
}
