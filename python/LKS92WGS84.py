#!/usr/bin/env python
# -*- coding: utf-8 -*-

from __future__ import division
import math

class LKS92WGS84:
    # Koordinātu pārveidojumos izmantotās konstantes
    __PI = math.pi                              # Skaitlis pi
    __A_AXIS = 6378137                          # Elipses modeļa lielā ass (a)
    __B_AXIS = 6356752.31414                    # Elipses modeļa mazā ass (b)
    __CENTRAL_MERIDIAN = math.pi * 24 / 180     # Centrālais meridiāns
    __OFFSET_X = 500000                         # Koordinātu nobīde horizontālās (x) ass virzienā
    __OFFSET_Y = -6000000                       # Koordinātu nobīde vertikālās (y) ass virzienā
    __SCALE = 0.9996                            # Kartes mērogojuma faktors (reizinātājs)

    # Aprēķina loka garumu no ekvatora līdz dotā punkta ģeogrāfiskajam platumam
    @staticmethod
    def __getArcLengthOfMeridian(phi):
        n = (LKS92WGS84.__A_AXIS - LKS92WGS84.__B_AXIS) / (LKS92WGS84.__A_AXIS + LKS92WGS84.__B_AXIS)
        alpha = ((LKS92WGS84.__A_AXIS + LKS92WGS84.__B_AXIS) / 2) * (1 + (math.pow(n, 2) / 4) + (math.pow(n, 4) / 64))
        beta = (-3 * n / 2) + (9 * math.pow(n, 3) / 16) + (-3 * math.pow(n, 5) / 32)
        gamma = (15 * math.pow(n, 2) / 16) + (-15 * math.pow(n, 4) / 32)
        delta = (-35 * math.pow(n, 3) / 48) + (105 * math.pow(n, 5) / 256)
        epsilon = (315 * math.pow(n, 4) / 512)

        return alpha * (phi + (beta * math.sin(2 * phi)) + (gamma * math.sin(4 * phi)) + (delta * math.sin(6 * phi)) + (epsilon * math.sin(8 * phi)))

    # Aprēķina ģeogrāfisko platumu centrālā meridiāna punktam
    @staticmethod
    def __getFootpointLatitude(y):
        n = (LKS92WGS84.__A_AXIS - LKS92WGS84.__B_AXIS) / (LKS92WGS84.__A_AXIS + LKS92WGS84.__B_AXIS)
        alpha = ((LKS92WGS84.__A_AXIS + LKS92WGS84.__B_AXIS) / 2) * (1 + (math.pow(n, 2) / 4) + (math.pow(n, 4) / 64))
        yd = y / alpha
        beta = (3 * n / 2) + (-27 * math.pow(n, 3) / 32) + (269 * math.pow(n, 5) / 512)
        gamma = (21 * math.pow(n, 2) / 16) + (-55 * math.pow(n, 4) / 32)
        delta = (151 * math.pow(n, 3) / 96) + (-417 * math.pow(n, 5) / 128)
        epsilon = (1097 * math.pow(n, 4) / 512)

        return yd + (beta * math.sin(2 * yd)) + (gamma * math.sin(4 * yd)) + (delta * math.sin(6 * yd)) + (epsilon * math.sin(8 * yd))

    # Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (bez pārvietojuma un mērogojuma)
    @staticmethod
    def __convertMapLatLngToXY(phi, lambda1, lambda0):
        xy = [0, 0]

        ep2 = (math.pow(LKS92WGS84.__A_AXIS, 2) - math.pow(LKS92WGS84.__B_AXIS, 2)) / math.pow(LKS92WGS84.__B_AXIS, 2)
        nu2 = ep2 * math.pow(math.cos(phi), 2)
        N = math.pow(LKS92WGS84.__A_AXIS, 2) / (LKS92WGS84.__B_AXIS * math.sqrt(1 + nu2))
        t = math.tan(phi)
        t2 = t * t

        l = lambda1 - lambda0
        l3coef = 1 - t2 + nu2
        l4coef = 5 - t2 + 9 * nu2 + 4 * (nu2 * nu2)
        l5coef = 5 - 18 * t2 + (t2 * t2) + 14 * nu2 - 58 * t2 * nu2
        l6coef = 61 - 58 * t2 + (t2 * t2) + 270 * nu2 - 330 * t2 * nu2
        l7coef = 61 - 479 * t2 + 179 * (t2 * t2) - (t2 * t2 * t2)
        l8coef = 1385 - 3111 * t2 + 543 * (t2 * t2) - (t2 * t2 * t2)

        # x koordināta
        xy[0] = N * math.cos(phi) * l + (N / 6 * math.pow(math.cos(phi), 3) * l3coef * math.pow(l, 3)) + (N / 120 * math.pow(math.cos(phi), 5) * l5coef * math.pow(l, 5)) + (N / 5040 * math.pow(math.cos(phi), 7) * l7coef * math.pow(l, 7))

        # y koordināta
        xy[1] = LKS92WGS84.__getArcLengthOfMeridian(phi) + (t / 2 * N * math.pow(math.cos(phi), 2) * math.pow(l, 2)) + (t / 24 * N * math.pow(math.cos(phi), 4) * l4coef * math.pow(l, 4)) + (t / 720 * N * math.pow(math.cos(phi), 6) * l6coef * math.pow(l, 6)) + (t / 40320 * N * math.pow(math.cos(phi), 8) * l8coef * math.pow(l, 8))

        return xy

    # Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (bez pārvietojuma un mērogojuma)
    @staticmethod
    def __convertMapXYToLatLon(x, y, lambda0):
        latLng = [0, 0]

        phif = LKS92WGS84.__getFootpointLatitude(y)
        ep2 = (math.pow(LKS92WGS84.__A_AXIS, 2) - math.pow(LKS92WGS84.__B_AXIS, 2)) / math.pow(LKS92WGS84.__B_AXIS, 2)
        cf = math.cos(phif)
        nuf2 = ep2 * math.pow(cf, 2)
        Nf = math.pow(LKS92WGS84.__A_AXIS, 2) / (LKS92WGS84.__B_AXIS * math.sqrt(1 + nuf2))
        Nfpow = Nf

        tf = math.tan(phif)
        tf2 = tf * tf
        tf4 = tf2 * tf2

        x1frac = 1 / (Nfpow * cf)

        Nfpow *= Nf     # Nf^2
        x2frac = tf / (2 * Nfpow)

        Nfpow *= Nf     # Nf^3
        x3frac = 1 / (6 * Nfpow * cf)

        Nfpow *= Nf     # Nf^4
        x4frac = tf / (24 * Nfpow)

        Nfpow *= Nf     # Nf^5
        x5frac = 1 / (120 * Nfpow * cf)

        Nfpow *= Nf     # Nf^6
        x6frac = tf / (720 * Nfpow)

        Nfpow *= Nf     # Nf^7
        x7frac = 1 / (5040 * Nfpow * cf)

        Nfpow *= Nf     # Nf^8
        x8frac = tf / (40320 * Nfpow)

        x2poly = -1 - nuf2
        x3poly = -1 - 2 * tf2 - nuf2
        x4poly = 5 + 3 * tf2 + 6 * nuf2 - 6 * tf2 * nuf2 - 3 * (nuf2 * nuf2) - 9 * tf2 * (nuf2 * nuf2)
        x5poly = 5 + 28 * tf2 + 24 * tf4 + 6 * nuf2 + 8 * tf2 * nuf2
        x6poly = -61 - 90 * tf2 - 45 * tf4 - 107 * nuf2 + 162 * tf2 * nuf2
        x7poly = -61 - 662 * tf2 - 1320 * tf4 - 720 * (tf4 * tf2)
        x8poly = 1385 + 3633 * tf2 + 4095 * tf4 + 1575 * (tf4 * tf2)

        # Ģeogrāfiskais platums
        latLng[0] = phif + x2frac * x2poly * (x * x) + x4frac * x4poly * math.pow(x, 4) + x6frac * x6poly * math.pow(x, 6) + x8frac * x8poly * math.pow(x, 8)

        # Ģeogrāfiskais garums
        latLng[1] = lambda0 + x1frac * x + x3frac * x3poly * math.pow(x, 3) + x5frac * x5poly * math.pow(x, 5) + x7frac * x7poly * math.pow(x, 7)

        return latLng

    # Pārveido punkta ģeogrāfiskā platuma, garuma koordinātas par x, y koordinātām (ar pārvietojumu un mērogojumu)
    @staticmethod
    def convertLatLonToXY(coordinates):
        lat = coordinates[0] * LKS92WGS84.__PI / 180
        lng = coordinates[1] * LKS92WGS84.__PI / 180
        xy = LKS92WGS84.__convertMapLatLngToXY(lat, lng, LKS92WGS84.__CENTRAL_MERIDIAN)

        xy[0] = xy[0] * LKS92WGS84.__SCALE + LKS92WGS84.__OFFSET_X
        xy[1] = xy[1] * LKS92WGS84.__SCALE + LKS92WGS84.__OFFSET_Y

        if xy[1] < 0:
            xy[1] += 10000000

        return xy

    # Pārveido punkta x, y koordinātas par ģeogrāfiskā platuma, garuma koordinātām (ar pārvietojumu un mērogojumu)
    @staticmethod
    def convertXYToLatLon(coordinates):
        x = (coordinates[0] - LKS92WGS84.__OFFSET_X) / LKS92WGS84.__SCALE
        y = (coordinates[1] - LKS92WGS84.__OFFSET_Y) / LKS92WGS84.__SCALE
        latLng = LKS92WGS84.__convertMapXYToLatLon(x, y, LKS92WGS84.__CENTRAL_MERIDIAN)

        latLng[0] = latLng[0] / LKS92WGS84.__PI * 180
        latLng[1] = latLng[1] / LKS92WGS84.__PI * 180

        return latLng
