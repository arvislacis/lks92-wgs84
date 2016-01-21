class LKS92WGS84
{
    // Koordinātu pārveidojumos izmantotās konstantes
    private static PI: number = Math.PI;                                    // Skaitlis pi
    private static A_AXIS: number = 6378137;                                // Elipses modeļa lielā ass (a)
    private static B_AXIS: number = 6356752.31414;                          // Elipses modeļa mazā ass (b)
    private static CENTRAL_MERIDIAN: number = LKS92WGS84.PI * 24 / 180;     // Centrālais meridiāns
    private static OFFSET_X: number = 500000;                               // Koordinātu nobīde horizontālās (x) ass virzienā
    private static OFFSET_Y: number = -6000000;                             // Koordinātu nobīde vertikālās (y) ass virzienā
    private static SCALE: number = 0.9996;                                  // Kartes mērogojuma faktors (reizinātājs)

    // Aprēķina loka garumu no ekvatora līdz dotā punkta ģeogrāfiskajam platumam
    private static getArcLengthOfMeridian(phi: number): number
    {
        var alpha: number, beta: number, gamma: number, delta: number, epsilon: number, n: number;

        n = (LKS92WGS84.A_AXIS - LKS92WGS84.B_AXIS) / (LKS92WGS84.A_AXIS + LKS92WGS84.B_AXIS);
        alpha = ((LKS92WGS84.A_AXIS + LKS92WGS84.B_AXIS) / 2) * (1 + (Math.pow(n, 2) / 4) + (Math.pow(n, 4) / 64));
        beta = (-3 * n / 2) + (9 * Math.pow(n, 3) / 16) + (-3 * Math.pow(n, 5) / 32);
        gamma = (15 * Math.pow(n, 2) / 16) + (-15 * Math.pow(n, 4) / 32);
        delta = (-35 * Math.pow(n, 3) / 48) + (105 * Math.pow(n, 5) / 256);
        epsilon = (315 * Math.pow(n, 4) / 512);

        return alpha * (phi + (beta * Math.sin(2 * phi)) + (gamma * Math.sin(4 * phi)) + (delta * Math.sin(6 * phi)) + (epsilon * Math.sin(8 * phi)));
    }

    // Aprēķina ģeogrāfisko platumu centrālā meridiāna punktam
    private static getFootpointLatitude(y): number
    {
        var yd: number, alpha: number, beta: number, gamma: number, delta: number, epsilon: number, n: number;

        n = (LKS92WGS84.A_AXIS - LKS92WGS84.B_AXIS) / (LKS92WGS84.A_AXIS + LKS92WGS84.B_AXIS);
        alpha = ((LKS92WGS84.A_AXIS + LKS92WGS84.B_AXIS) / 2) * (1 + (Math.pow(n, 2) / 4) + (Math.pow(n, 4) / 64));
        yd = y / alpha;
        beta = (3 * n / 2) + (-27 * Math.pow(n, 3) / 32) + (269 * Math.pow(n, 5) / 512);
        gamma = (21 * Math.pow(n, 2) / 16) + (-55 * Math.pow(n, 4) / 32);
        delta = (151 * Math.pow(n, 3) / 96) + (-417 * Math.pow(n, 5) / 128);
        epsilon = (1097 * Math.pow(n, 4) / 512);

        return yd + (beta * Math.sin(2 * yd)) + (gamma * Math.sin(4 * yd)) + (delta * Math.sin(6 * yd)) + (epsilon * Math.sin(8 * yd));
    }

    // Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (bez pārvietojuma un mērogojuma)
    private static convertMapLatLngToXY(phi: number, lambda: number, lambda0: number): number[]
    {
        var N: number, nu2: number, ep2: number, t: number, t2: number, l: number,
            l3coef: number, l4coef: number, l5coef: number, l6coef: number, l7coef: number, l8coef: number,
            xy: number[] = [0, 0];

        ep2 = (Math.pow(LKS92WGS84.A_AXIS, 2) - Math.pow(LKS92WGS84.B_AXIS, 2)) / Math.pow(LKS92WGS84.B_AXIS, 2);
        nu2 = ep2 * Math.pow(Math.cos(phi), 2);
        N = Math.pow(LKS92WGS84.A_AXIS, 2) / (LKS92WGS84.B_AXIS * Math.sqrt(1 + nu2));
        t = Math.tan(phi);
        t2 = t * t;

        l = lambda - lambda0;
        l3coef = 1 - t2 + nu2;
        l4coef = 5 - t2 + 9 * nu2 + 4 * (nu2 * nu2);
        l5coef = 5 - 18 * t2 + (t2 * t2) + 14 * nu2 - 58 * t2 * nu2;
        l6coef = 61 - 58 * t2 + (t2 * t2) + 270 * nu2 - 330 * t2 * nu2;
        l7coef = 61 - 479 * t2 + 179 * (t2 * t2) - (t2 * t2 * t2);
        l8coef = 1385 - 3111 * t2 + 543 * (t2 * t2) - (t2 * t2 * t2);

        // x koordināta
        xy[0] = N * Math.cos(phi) * l + (N / 6 * Math.pow(Math.cos(phi), 3) * l3coef * Math.pow(l, 3)) + (N / 120 * Math.pow(Math.cos(phi), 5) * l5coef * Math.pow(l, 5)) + (N / 5040 * Math.pow(Math.cos(phi), 7) * l7coef * Math.pow(l, 7));

        // y koordināta
        xy[1] = LKS92WGS84.getArcLengthOfMeridian(phi) + (t / 2 * N * Math.pow(Math.cos(phi), 2) * Math.pow(l, 2)) + (t / 24 * N * Math.pow(Math.cos(phi), 4) * l4coef * Math.pow(l, 4)) + (t / 720 * N * Math.pow(Math.cos(phi), 6) * l6coef * Math.pow(l, 6)) + (t / 40320 * N * Math.pow(Math.cos(phi), 8) * l8coef * Math.pow(l, 8));
        return xy;
    }

    // Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (bez pārvietojuma un mērogojuma)
    private static convertMapXYToLatLon(x: number, y: number, lambda0: number): number[]
    {
        var phif: number, Nf: number, Nfpow: number, nuf2: number, ep2: number, tf: number, tf2: number, tf4: number, cf: number,
            x1frac: number, x2frac: number, x3frac: number, x4frac: number, x5frac: number, x6frac: number, x7frac: number, x8frac: number,
            x2poly: number, x3poly: number, x4poly: number, x5poly: number, x6poly: number, x7poly: number, x8poly: number,
            latLng: number[] = [0, 0];

        phif = LKS92WGS84.getFootpointLatitude(y);
        ep2 = (Math.pow(LKS92WGS84.A_AXIS, 2) - Math.pow(LKS92WGS84.B_AXIS, 2)) / Math.pow(LKS92WGS84.B_AXIS, 2);
        cf = Math.cos(phif);
        nuf2 = ep2 * Math.pow(cf, 2);
        Nf = Math.pow(LKS92WGS84.A_AXIS, 2) / (LKS92WGS84.B_AXIS * Math.sqrt(1 + nuf2));
        Nfpow = Nf;

        tf = Math.tan(phif);
        tf2 = tf * tf;
        tf4 = tf2 * tf2;

        x1frac = 1 / (Nfpow * cf);

        Nfpow *= Nf;    // Nf^2
        x2frac = tf / (2 * Nfpow);

        Nfpow *= Nf;    // Nf^3
        x3frac = 1 / (6 * Nfpow * cf);

        Nfpow *= Nf;    // Nf^4
        x4frac = tf / (24 * Nfpow);

        Nfpow *= Nf;    // Nf^5
        x5frac = 1 / (120 * Nfpow * cf);

        Nfpow *= Nf;    // Nf^6
        x6frac = tf / (720 * Nfpow);

        Nfpow *= Nf;    // Nf^7
        x7frac = 1 / (5040 * Nfpow * cf);

        Nfpow *= Nf;    // Nf^8
        x8frac = tf / (40320 * Nfpow);

        x2poly = -1 - nuf2;
        x3poly = -1 - 2 * tf2 - nuf2;
        x4poly = 5 + 3 * tf2 + 6 * nuf2 - 6 * tf2 * nuf2 - 3 * (nuf2 * nuf2) - 9 * tf2 * (nuf2 * nuf2);
        x5poly = 5 + 28 * tf2 + 24 * tf4 + 6 * nuf2 + 8 * tf2 * nuf2;
        x6poly = -61 - 90 * tf2 - 45 * tf4 - 107 * nuf2 + 162 * tf2 * nuf2;
        x7poly = -61 - 662 * tf2 - 1320 * tf4 - 720 * (tf4 * tf2);
        x8poly = 1385 + 3633 * tf2 + 4095 * tf4 + 1575 * (tf4 * tf2);

        // Ģeogrāfiskais platums
        latLng[0] = phif + x2frac * x2poly * (x * x) + x4frac * x4poly * Math.pow(x, 4) + x6frac * x6poly * Math.pow(x, 6) + x8frac * x8poly * Math.pow(x, 8);

        // Ģeogrāfiskais garums
        latLng[1] = lambda0 + x1frac * x + x3frac * x3poly * Math.pow(x, 3) + x5frac * x5poly * Math.pow(x, 5) + x7frac * x7poly * Math.pow(x, 7);

        return latLng;
    };

    // Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (ar pārvietojumu un mērogojumu)
    public static convertLatLonToXY(coordinates): number[]
    {
        var lat: number = coordinates[0] * LKS92WGS84.PI / 180,
            lng: number = coordinates[1] * LKS92WGS84.PI / 180,
            xy: number[] = LKS92WGS84.convertMapLatLngToXY(lat, lng, LKS92WGS84.CENTRAL_MERIDIAN);

        xy[0] = xy[0] * LKS92WGS84.SCALE + LKS92WGS84.OFFSET_X;
        xy[1] = xy[1] * LKS92WGS84.SCALE + LKS92WGS84.OFFSET_Y;

        if (xy[1] < 0) {
            xy[1] += 10000000;
        }

        return xy;
    }

    // Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (ar pārvietojumu un mērogojumu)
    public static convertXYToLatLon(coordinates): number[]
    {
        var x: number = (coordinates[0] - LKS92WGS84.OFFSET_X) / LKS92WGS84.SCALE,
            y: number = (coordinates[1] - LKS92WGS84.OFFSET_Y) / LKS92WGS84.SCALE,
            latLng: number[] = LKS92WGS84.convertMapXYToLatLon(x, y, LKS92WGS84.CENTRAL_MERIDIAN);

        latLng[0] = latLng[0] / LKS92WGS84.PI * 180;
        latLng[1] = latLng[1] / LKS92WGS84.PI * 180;

        return latLng;
    }
}
