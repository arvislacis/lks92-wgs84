var LKS92WGS84 = function() {
    // publicStatic objekts paredzēts tikai JavaScript un tam līdzīgo programmēšanas valodu risinājumiem
    var publicStatic = {},
    // Koordinātu pārveidojumos izmantotās konstantes
        PI = Math.PI,                       // Skaitlis pi
        A_AXIS = 6378137,                   // Elipses modeļa lielā ass (a)
        B_AXIS = 6356752.31414,             // Elipses modeļa mazā ass (b)
        CENTRAL_MERIDIAN = 24 / 180 * PI,   // Centrālais meridiāns
        OFFSET_X = 500000,                  // Koordinātu nobīde horizontālās (x) ass virzienā
        OFFSET_Y = -6000000,                // Koordinātu nobīde vertikālās (y) ass virzienā
        SCALE = 0.9996,                     // Kartes mērogojuma faktors (reizinātājs)

    // Aprēķina loka garumu no ekvatora līdz dotā punkta ģeogrāfiskajam platumam
    getArcLengthOfMeridian = function(phi) {
        var alpha, beta, gamma, delta, epsilon, n;

        n = (A_AXIS - B_AXIS) / (A_AXIS + B_AXIS);
        alpha = ((A_AXIS + B_AXIS) / 2) * (1 + (Math.pow(n, 2) / 4) + (Math.pow(n, 4) / 64));
        beta = (-3 * n / 2) + (9 * Math.pow(n, 3) / 16) + (-3 * Math.pow(n, 5) / 32);
        gamma = (15 * Math.pow(n, 2) / 16) + (-15 * Math.pow(n, 4) / 32);
        delta = (-35 * Math.pow(n, 3) / 48) + (105 * Math.pow(n, 5) / 256);
        epsilon = (315 * Math.pow(n, 4) / 512);

        return alpha * (phi + (beta * Math.sin(2 * phi)) + (gamma * Math.sin(4 * phi)) + (delta * Math.sin(6 * phi)) + (epsilon * Math.sin(8 * phi)));
    },

    // Aprēķina ģeogrāfisko platumu centrālā meridiāna punktam
    getFootpointLatitude = function(y) {
        var y_, alpha_, beta_, gamma_, delta_, epsilon_, n;

        n = (A_AXIS - B_AXIS) / (A_AXIS + B_AXIS);
        alpha_ = ((A_AXIS + B_AXIS) / 2) * (1 + (Math.pow(n, 2) / 4) + (Math.pow(n, 4) / 64));
        y_ = y / alpha_;
        beta_ = (3 * n / 2) + (-27 * Math.pow(n, 3) / 32) + (269 * Math.pow(n, 5) / 512);
        gamma_ = (21 * Math.pow(n, 2) / 16) + (-55 * Math.pow(n, 4) / 32);
        delta_ = (151 * Math.pow(n, 3) / 96) + (-417 * Math.pow(n, 5) / 128);
        epsilon_ = (1097 * Math.pow(n, 4) / 512);

        return y_ + (beta_ * Math.sin(2 * y_)) + (gamma_ * Math.sin(4 * y_)) + (delta_ * Math.sin(6 * y_)) + (epsilon_ * Math.sin(8 * y_));
    },

    // Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (bez pārvietojuma un mērogojuma)
    convertMapLatLngToXY = function(phi, lambda, lambda0) {
        var N, nu2, ep2, t, t2, l,
            l3coef, l4coef, l5coef, l6coef, l7coef, l8coef,
            xy = [0, 0];

        ep2 = (Math.pow(A_AXIS, 2) - Math.pow(B_AXIS, 2)) / Math.pow(B_AXIS, 2);
        nu2 = ep2 * Math.pow(Math.cos(phi), 2);
        N = Math.pow(A_AXIS, 2) / (B_AXIS * Math.sqrt(1 + nu2));
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
        xy[1] = getArcLengthOfMeridian(phi) + (t / 2 * N * Math.pow(Math.cos(phi), 2) * Math.pow(l, 2)) + (t / 24 * N * Math.pow(Math.cos(phi), 4) * l4coef * Math.pow(l, 4)) + (t / 720 * N * Math.pow(Math.cos(phi), 6) * l6coef * Math.pow(l, 6)) + (t / 40320 * N * Math.pow(Math.cos(phi), 8) * l8coef * Math.pow(l, 8));
        return xy;
    },

    // Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (bez pārvietojuma un mērogojuma)
    convertMapXYToLatLon = function(x, y, lambda0) {
        var phif, Nf, Nfpow, nuf2, ep2, tf, tf2, tf4, cf,
            x1frac, x2frac, x3frac, x4frac, x5frac, x6frac, x7frac, x8frac,
            x2poly, x3poly, x4poly, x5poly, x6poly, x7poly, x8poly,
            latLng = [0, 0];

        phif = getFootpointLatitude(y);
        ep2 = (Math.pow(A_AXIS, 2) - Math.pow(B_AXIS, 2)) / Math.pow(B_AXIS, 2);
        cf = Math.cos(phif);
        nuf2 = ep2 * Math.pow(cf, 2);
        Nf = Math.pow(A_AXIS, 2) / (B_AXIS * Math.sqrt(1 + nuf2));
        Nfpow = Nf;

        tf = Math.tan(phif);
        tf2 = tf * tf;
        tf4 = tf2 * tf2;

        x1frac = 1 / (Nfpow * cf);

        Nfpow *= Nf;   // Nf^2
        x2frac = tf / (2 * Nfpow);

        Nfpow *= Nf;   // Nf^3
        x3frac = 1 / (6 * Nfpow * cf);

        Nfpow *= Nf;   // Nf^4
        x4frac = tf / (24 * Nfpow);

        Nfpow *= Nf;   // Nf^5
        x5frac = 1 / (120 * Nfpow * cf);

        Nfpow *= Nf;   // Nf^6
        x6frac = tf / (720 * Nfpow);

        Nfpow *= Nf;   // Nf^7
        x7frac = 1 / (5040 * Nfpow * cf);

        Nfpow *= Nf;   // Nf^8
        x8frac = tf / (40320 * Nfpow);

        x2poly = -1 - nuf2;
        x3poly = -1 - 2 * tf2 - nuf2;
        x4poly = 5 + 3 * tf2 + 6 * nuf2 - 6 * tf2 * nuf2 - 3 * (nuf2 *nuf2) - 9 * tf2 * (nuf2 * nuf2);
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
    publicStatic.convertLatLonToXY = function(coordinates) {
        var lat = coordinates[0] * PI / 180,
            lng = coordinates[1] * PI / 180,
            xy = convertMapLatLngToXY(lat, lng, CENTRAL_MERIDIAN);

        xy[0] = xy[0] * SCALE + OFFSET_X;
        xy[1] = xy[1] * SCALE + OFFSET_Y;

        if (xy[1] < 0) {
            xy[1] = xy[1] + 10000000;
        }

        return xy;
    };

    // Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (ar pārvietojumu un mērogojumu)
    publicStatic.convertXYToLatLon = function(coordinates) {
        var x = (coordinates[0] - OFFSET_X) / SCALE,
            y = (coordinates[1] - OFFSET_Y) / SCALE,
            latLng = convertMapXYToLatLon(x, y, CENTRAL_MERIDIAN);

        latLng[0] = latLng[0] / PI * 180;
        latLng[1] = latLng[1] / PI * 180;

        return latLng;
    };

    return publicStatic;
};

LKS92WGS84 = LKS92WGS84();
