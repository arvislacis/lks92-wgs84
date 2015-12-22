#!/usr/bin/env python
# -*- coding: utf-8 -*-
import math
import lks92_wgs84

LKS92WGS84 = lks92_wgs84.LKS92WGS84()

def TestCase(coordinates):
	converted = LKS92WGS84.convertXYToLatLon(LKS92WGS84.convertLatLonToXY(coordinates))

	if round(converted[0] * math.pow(10, 8)) / math.pow(10, 8) == round(coordinates[0] * math.pow(10, 8)) / math.pow(10, 8) and round(converted[1] * math.pow(10, 8)) / math.pow(10, 8) == round(coordinates[1] * math.pow(10, 8)) / math.pow(10, 8):
		result = "izpildās"
	else:
		result = "neizpildās"

	return result

coordinates = [58.079501574948, 25.189986971284]
print "\"Baltās naktis\" - Latvijas tālākais ziemeļu punkts [" + str(coordinates[0]) + "," + str(coordinates[1]) + "] => " + TestCase(coordinates)
coordinates = [56.172282784562, 28.095216442873]
print "\"Austras koks\" - Latvijas tālākais austrumu punkts [" + str(coordinates[0]) + "," + str(coordinates[1]) + "] => " + TestCase(coordinates)
coordinates = [55.675228242509, 26.580528487143]
print "\"Saules puķe\" - Latvijas tālākais dienvidu punkts [" + str(coordinates[0]) + "," + str(coordinates[1]) + "] => " + TestCase(coordinates)
coordinates = [56.377008455189, 20.979185882058]
print "\"Zaļais stars\" - Latvijas galējais rietumu punkts [" + str(coordinates[0]) + "," + str(coordinates[1]) + "] => " + TestCase(coordinates)
