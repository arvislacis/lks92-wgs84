#include <cmath>

class LKS92WGS84
{
    // Koordinātu pārveidojumos izmantotās konstantes
    constexpr static double PI = M_PI;                            // Skaitlis pi
    constexpr static double A_AXIS = 6378137;                     // Elipses modeļa lielā ass (a)
    constexpr static double B_AXIS = 6356752.31414;               // Elipses modeļa mazā ass (b)
    constexpr static double CENTRAL_MERIDIAN = PI * 24 / 180;     // Centrālais meridiāns
    constexpr static double OFFSET_X = 500000;                    // Koordinātu nobīde horizontālās (x) ass virzienā
    constexpr static double OFFSET_Y = -6000000;                  // Koordinātu nobīde vertikālās (y) ass virzienā
    constexpr static double SCALE = 0.9996;                       // Kartes mērogojuma faktors (reizinātājs)

    // Aprēķina loka garumu no ekvatora līdz dotā punkta ģeogrāfiskajam platumam
    static double getArcLengthOfMeridian(double phi)
    {
        double n = (A_AXIS - B_AXIS) / (A_AXIS + B_AXIS);
        double alpha = ((A_AXIS + B_AXIS) / 2) * (1 + (pow(n, 2) / 4) + (pow(n, 4) / 64));
        double beta = (-3 * n / 2) + (9 * pow(n, 3) / 16) + (-3 * pow(n, 5) / 32);
        double gamma = (15 * pow(n, 2) / 16) + (-15 * pow(n, 4) / 32);
        double delta = (-35 * pow(n, 3) / 48) + (105 * pow(n, 5) / 256);
        double epsilon = (315 * pow(n, 4) / 512);

        return alpha * (phi + (beta * sin(2 * phi)) + (gamma * sin(4 * phi)) + (delta * sin(6 * phi)) + (epsilon * sin(8 * phi)));
    }

    // Aprēķina ģeogrāfisko platumu centrālā meridiāna punktam
    static double getFootpointLatitude(double y)
    {
        double n = (A_AXIS - B_AXIS) / (A_AXIS + B_AXIS);
        double alpha = ((A_AXIS + B_AXIS) / 2) * (1 + (pow(n, 2) / 4) + (pow(n, 4) / 64));
        double yd = y / alpha;
        double beta = (3 * n / 2) + (-27 * pow(n, 3) / 32) + (269 * pow(n, 5) / 512);
        double gamma = (21 * pow(n, 2) / 16) + (-55 * pow(n, 4) / 32);
        double delta = (151 * pow(n, 3) / 96) + (-417 * pow(n, 5) / 128);
        double epsilon = (1097 * pow(n, 4) / 512);

        return yd + (beta * sin(2 * yd)) + (gamma * sin(4 * yd)) + (delta * sin(6 * yd)) + (epsilon * sin(8 * yd));
    }

    // Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (bez pārvietojuma un mērogojuma)
    static double *convertMapLatLngToXY(double phi, double lambda, double lambda0)
    {
        double *xy = new double[2] {0, 0};

        double ep2 = (pow(A_AXIS, 2) - pow(B_AXIS, 2)) / pow(B_AXIS, 2);
        double nu2 = ep2 * pow(cos(phi), 2);
        double N = pow(A_AXIS, 2) / (B_AXIS * sqrt(1 + nu2));
        double t = tan(phi);
        double t2 = t * t;

        double l = lambda - lambda0;
        double l3coef = 1 - t2 + nu2;
        double l4coef = 5 - t2 + 9 * nu2 + 4 * (nu2 * nu2);
        double l5coef = 5 - 18 * t2 + (t2 * t2) + 14 * nu2 - 58 * t2 * nu2;
        double l6coef = 61 - 58 * t2 + (t2 * t2) + 270 * nu2 - 330 * t2 * nu2;
        double l7coef = 61 - 479 * t2 + 179 * (t2 * t2) - (t2 * t2 * t2);
        double l8coef = 1385 - 3111 * t2 + 543 * (t2 * t2) - (t2 * t2 * t2);

        // x koordināta
        xy[0] = N * cos(phi) * l + (N / 6 * pow(cos(phi), 3) * l3coef * pow(l, 3)) + (N / 120 * pow(cos(phi), 5) * l5coef * pow(l, 5)) + (N / 5040 * pow(cos(phi), 7) * l7coef * pow(l, 7));

        // y koordināta
        xy[1] = getArcLengthOfMeridian(phi) + (t / 2 * N * pow(cos(phi), 2) * pow(l, 2)) + (t / 24 * N * pow(cos(phi), 4) * l4coef * pow(l, 4)) + (t / 720 * N * pow(cos(phi), 6) * l6coef * pow(l, 6)) + (t / 40320 * N * pow(cos(phi), 8) * l8coef * pow(l, 8));
        return xy;
    }

    // Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (bez pārvietojuma un mērogojuma)
    static double *convertMapXYToLatLon(double x, double y, double lambda0)
    {
        double *latLng = new double[2] {0, 0};

        double phif = getFootpointLatitude(y);
        double ep2 = (pow(A_AXIS, 2) - pow(B_AXIS, 2)) / pow(B_AXIS, 2);
        double cf = cos(phif);
        double nuf2 = ep2 * pow(cf, 2);
        double Nf = pow(A_AXIS, 2) / (B_AXIS * sqrt(1 + nuf2));
        double Nfpow = Nf;

        double tf = tan(phif);
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
        latLng[0] = phif + x2frac * x2poly * (x * x) + x4frac * x4poly * pow(x, 4) + x6frac * x6poly * pow(x, 6) + x8frac * x8poly * pow(x, 8);

        // Ģeogrāfiskais garums
        latLng[1] = lambda0 + x1frac * x + x3frac * x3poly * pow(x, 3) + x5frac * x5poly * pow(x, 5) + x7frac * x7poly * pow(x, 7);

        return latLng;
    }

    // Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (ar pārvietojumu un mērogojumu)
    public: static double *convertLatLonToXY(double *coordinates)
    {
        double lat = coordinates[0] * PI / 180;
        double lng = coordinates[1] * PI / 180;
        double *xy = convertMapLatLngToXY(lat, lng, CENTRAL_MERIDIAN);

        xy[0] = xy[0] * SCALE + OFFSET_X;
        xy[1] = xy[1] * SCALE + OFFSET_Y;

        if (xy[1] < 0) {
            xy[1] += 10000000;
        }

        return xy;
    }

    // Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (ar pārvietojumu un mērogojumu)
    public: static double *convertXYToLatLon(double *coordinates)
    {
        double x = (coordinates[0] - OFFSET_X) / SCALE;
        double y = (coordinates[1] - OFFSET_Y) / SCALE;
        double *latLng = convertMapXYToLatLon(x, y, CENTRAL_MERIDIAN);

        latLng[0] = latLng[0] / PI * 180;
        latLng[1] = latLng[1] / PI * 180;

        return latLng;
    }
};
