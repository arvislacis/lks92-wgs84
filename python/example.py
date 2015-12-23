#!/usr/bin/env python
# -*- coding: utf-8 -*-

import math
from lks92_wgs84 import LKS92WGS84

def testCase(coordinates):
    converted = LKS92WGS84.convertXYToLatLon(LKS92WGS84.convertLatLonToXY(coordinates))

    if round(converted[0] * math.pow(10, 8)) / math.pow(10, 8) == round(coordinates[0] * math.pow(10, 8)) / math.pow(10, 8) and round(converted[1] * math.pow(10, 8)) / math.pow(10, 8) == round(coordinates[1] * math.pow(10, 8)) / math.pow(10, 8):
        result = "izpildās"
    else:
        result = "neizpildās"

    return result

def testCase2(coordinates, lksValidate):
    converted = LKS92WGS84.convertLatLonToXY(coordinates)

    if round(converted[0], 2) == lksValidate[0] and round(converted[1], 2) == lksValidate[1]:
        result = "izpildās"
    else:
        result = "neizpildās"

    return result

coordinates = [58.079501574948, 25.189986971284]
print("\"Baltās naktis\" - Latvijas tālākais ziemeļu punkts [" + str(coordinates[0]) + "," + str(coordinates[1]) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, [570181.00, 438180.00]))
coordinates = [56.172282784562, 28.095216442873]
print("\"Austras koks\" - Latvijas tālākais austrumu punkts [" + str(coordinates[0]) + "," + str(coordinates[1]) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, [754190.00, 232806.00]))
coordinates = [55.675228242509, 26.580528487143]
print("\"Saules puķe\" - Latvijas tālākais dienvidu punkts [" + str(coordinates[0]) + "," + str(coordinates[1]) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, [662269.00, 172953.00]))
coordinates = [56.377008455189, 20.979185882058]
print("\"Zaļais stars\" - Latvijas galējais rietumu punkts [" + str(coordinates[0]) + "," + str(coordinates[1]) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, [313470.00, 252137.00]))
