Module LKS92WGS84
    ' Koordinātu pārveidojumos izmantotās konstantes
    Dim PI As Double = Math.PI                          ' Skaitlis pi
    Dim A_AXIS As Double = 6378137                      ' Elipses modeļa lielā ass (a)
    Dim B_AXIS As Double = 6356752.31414                ' Elipses modeļa mazā ass (b)
    Dim CENTRAL_MERIDIAN As Double = PI * 24 / 180      ' Centrālais meridiāns
    Dim OFFSET_X As Double = 500000                     ' Koordinātu nobīde horizontālās (x) ass virzienā
    Dim OFFSET_Y As Double = -6000000                   ' Koordinātu nobīde vertikālās (y) ass virzienā
    Dim SCALE As Double = 0.9996                        ' Kartes mērogojuma faktors (reizinātājs)

    ' Aprēķina loka garumu no ekvatora līdz dotā punkta ģeogrāfiskajam platumam
    Private Function getArcLengthOfMeridian(phi As Double) As Double
        Dim n As Double = (A_AXIS - B_AXIS) / (A_AXIS + B_AXIS)
        Dim alpha As Double = ((A_AXIS + B_AXIS) / 2) * (1 + (Math.Pow(n, 2) / 4) + (Math.Pow(n, 4) / 64))
        Dim beta As Double = (-3 * n / 2) + (9 * Math.Pow(n, 3) / 16) + (-3 * Math.Pow(n, 5) / 32)
        Dim gamma As Double = (15 * Math.Pow(n, 2) / 16) + (-15 * Math.Pow(n, 4) / 32)
        Dim delta As Double = (-35 * Math.Pow(n, 3) / 48) + (105 * Math.Pow(n, 5) / 256)
        Dim epsilon As Double = (315 * Math.Pow(n, 4) / 512)

        Return alpha * (phi + (beta * Math.Sin(2 * phi)) + (gamma * Math.Sin(4 * phi)) + (delta * Math.Sin(6 * phi)) + (epsilon * Math.Sin(8 * phi)))
    End Function

    ' Aprēķina ģeogrāfisko platumu centrālā meridiāna punktam
    Private Function getFootpointLatitude(y As Double) As Double
        Dim n As Double = (A_AXIS - B_AXIS) / (A_AXIS + B_AXIS)
        Dim alpha As Double = ((A_AXIS + B_AXIS) / 2) * (1 + (Math.Pow(n, 2) / 4) + (Math.Pow(n, 4) / 64))
        Dim yd As Double = y / alpha
        Dim beta As Double = (3 * n / 2) + (-27 * Math.Pow(n, 3) / 32) + (269 * Math.Pow(n, 5) / 512)
        Dim gamma As Double = (21 * Math.Pow(n, 2) / 16) + (-55 * Math.Pow(n, 4) / 32)
        Dim delta As Double = (151 * Math.Pow(n, 3) / 96) + (-417 * Math.Pow(n, 5) / 128)
        Dim epsilon As Double = (1097 * Math.Pow(n, 4) / 512)

        Return yd + (beta * Math.Sin(2 * yd)) + (gamma * Math.Sin(4 * yd)) + (delta * Math.Sin(6 * yd)) + (epsilon * Math.Sin(8 * yd))
    End Function

    ' Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (bez pārvietojuma un mērogojuma)
    Private Function convertMapLatLngToXY(phi As Double, lambda As Double, lambda0 As Double) As Double()
        Dim xy = New Double() {0, 0}

        Dim ep2 As Double = (Math.Pow(A_AXIS, 2) - Math.Pow(B_AXIS, 2)) / Math.Pow(B_AXIS, 2)
        Dim nu2 As Double = ep2 * Math.Pow(Math.Cos(phi), 2)
        Dim N As Double = Math.Pow(A_AXIS, 2) / (B_AXIS * Math.Sqrt(1 + nu2))
        Dim t As Double = Math.Tan(phi)
        Dim t2 As Double = t * t

        Dim l As Double = lambda - lambda0
        Dim l3coef As Double = 1 - t2 + nu2
        Dim l4coef As Double = 5 - t2 + 9 * nu2 + 4 * (nu2 * nu2)
        Dim l5coef As Double = 5 - 18 * t2 + (t2 * t2) + 14 * nu2 - 58 * t2 * nu2
        Dim l6coef As Double = 61 - 58 * t2 + (t2 * t2) + 270 * nu2 - 330 * t2 * nu2
        Dim l7coef As Double = 61 - 479 * t2 + 179 * (t2 * t2) - (t2 * t2 * t2)
        Dim l8coef As Double = 1385 - 3111 * t2 + 543 * (t2 * t2) - (t2 * t2 * t2)

        ' x koordināta
        xy(0) = N * Math.Cos(phi) * l + (N / 6 * Math.Pow(Math.Cos(phi), 3) * l3coef * Math.Pow(l, 3)) + (N / 120 * Math.Pow(Math.Cos(phi), 5) * l5coef * Math.Pow(l, 5)) + (N / 5040 * Math.Pow(Math.Cos(phi), 7) * l7coef * Math.Pow(l, 7))

        ' y koordināta
        xy(1) = getArcLengthOfMeridian(phi) + (t / 2 * N * Math.Pow(Math.Cos(phi), 2) * Math.Pow(l, 2)) + (t / 24 * N * Math.Pow(Math.Cos(phi), 4) * l4coef * Math.Pow(l, 4)) + (t / 720 * N * Math.Pow(Math.Cos(phi), 6) * l6coef * Math.Pow(l, 6)) + (t / 40320 * N * Math.Pow(Math.Cos(phi), 8) * l8coef * Math.Pow(l, 8))
        Return xy
    End Function

    ' Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (bez pārvietojuma un mērogojuma)
    Private Function convertMapXYToLatLon(x As Double, y As Double, lambda0 As Double) As Double()
        Dim latLng = New Double() {0, 0}

        Dim phif As Double = getFootpointLatitude(y)
        Dim ep2 As Double = (Math.Pow(A_AXIS, 2) - Math.Pow(B_AXIS, 2)) / Math.Pow(B_AXIS, 2)
        Dim cf As Double = Math.Cos(phif)
        Dim nuf2 As Double = ep2 * Math.Pow(cf, 2)
        Dim Nf As Double = Math.Pow(A_AXIS, 2) / (B_AXIS * Math.Sqrt(1 + nuf2))
        Dim Nfpow As Double = Nf

        Dim tf As Double = Math.Tan(phif)
        Dim tf2 As Double = tf * tf
        Dim tf4 As Double = tf2 * tf2

        Dim x1frac As Double = 1 / (Nfpow * cf)

        Nfpow *= Nf    ' Nf^2
        Dim x2frac As Double = tf / (2 * Nfpow)

        Nfpow *= Nf    ' Nf^3
        Dim x3frac As Double = 1 / (6 * Nfpow * cf)

        Nfpow *= Nf    ' Nf^4
        Dim x4frac As Double = tf / (24 * Nfpow)

        Nfpow *= Nf    ' Nf^5
        Dim x5frac As Double = 1 / (120 * Nfpow * cf)

        Nfpow *= Nf    ' Nf^6
        Dim x6frac As Double = tf / (720 * Nfpow)

        Nfpow *= Nf    ' Nf^7
        Dim x7frac As Double = 1 / (5040 * Nfpow * cf)

        Nfpow *= Nf    ' Nf^8
        Dim x8frac As Double = tf / (40320 * Nfpow)

        Dim x2poly As Double = -1 - nuf2
        Dim x3poly As Double = -1 - 2 * tf2 - nuf2
        Dim x4poly As Double = 5 + 3 * tf2 + 6 * nuf2 - 6 * tf2 * nuf2 - 3 * (nuf2 * nuf2) - 9 * tf2 * (nuf2 * nuf2)
        Dim x5poly As Double = 5 + 28 * tf2 + 24 * tf4 + 6 * nuf2 + 8 * tf2 * nuf2
        Dim x6poly As Double = -61 - 90 * tf2 - 45 * tf4 - 107 * nuf2 + 162 * tf2 * nuf2
        Dim x7poly As Double = -61 - 662 * tf2 - 1320 * tf4 - 720 * (tf4 * tf2)
        Dim x8poly As Double = 1385 + 3633 * tf2 + 4095 * tf4 + 1575 * (tf4 * tf2)

        ' Ģeogrāfiskais platums
        latLng(0) = phif + x2frac * x2poly * (x * x) + x4frac * x4poly * Math.Pow(x, 4) + x6frac * x6poly * Math.Pow(x, 6) + x8frac * x8poly * Math.Pow(x, 8)

        ' Ģeogrāfiskais garums
        latLng(1) = lambda0 + x1frac * x + x3frac * x3poly * Math.Pow(x, 3) + x5frac * x5poly * Math.Pow(x, 5) + x7frac * x7poly * Math.Pow(x, 7)

        Return latLng
    End Function

    ' Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (ar pārvietojumu un mērogojumu)
    Public Function convertLatLonToXY(coordinates As Double()) As Double()
        Dim lat As Double = coordinates(0) * PI / 180
        Dim lng As Double = coordinates(1) * PI / 180
        Dim xy As Double() = convertMapLatLngToXY(lat, lng, CENTRAL_MERIDIAN)

        xy(0) = xy(0) * SCALE + OFFSET_X
        xy(1) = xy(1) * SCALE + OFFSET_Y

        If xy(1) < 0 Then
            xy(1) += 10000000
        End If

        Return xy
    End Function

    ' Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (ar pārvietojumu un mērogojumu)
    Public Function convertXYToLatLon(coordinates As Double()) As Double()
        Dim x As Double = (coordinates(0) - OFFSET_X) / SCALE
        Dim y As Double = (coordinates(1) - OFFSET_Y) / SCALE
        Dim latLng As Double() = convertMapXYToLatLon(x, y, CENTRAL_MERIDIAN)

        latLng(0) = latLng(0) / PI * 180
        latLng(1) = latLng(1) / PI * 180

        Return latLng
    End Function
End Module
