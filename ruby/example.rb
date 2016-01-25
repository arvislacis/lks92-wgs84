#!/usr/bin/env ruby
# -*- coding: utf-8 -*-

load 'LKS92WGS84.rb'

def test_case(coordinates)
  converted = LKS92WGS84.convert_xy_to_lat_lon(LKS92WGS84.convert_lat_lon_to_xy(coordinates))

  if converted[0].round(8) == coordinates[0].round(8) and converted[1].round(8) == coordinates[1].round(8)
    result = 'izpildās'
  else
    result = 'neizpildās'
  end

  result
end

def test_case2(coordinates, lks_validate)
  converted = LKS92WGS84.convert_lat_lon_to_xy(coordinates)

  if converted[0].round(2) == lks_validate[0] and converted[1].round(2) == lks_validate[1]
    result = 'izpildās'
  else
    result = 'neizpildās'
  end

  result
end

coordinates = [58.079501574948, 25.189986971284]
puts '"Baltās naktis" - Latvijas tālākais ziemeļu punkts [' + coordinates[0].to_s + ',' + coordinates[1].to_s + '] => ' + test_case(coordinates) + ' => ' + test_case2(coordinates, [570181.00, 438180.00])
coordinates = [56.172282784562, 28.095216442873]
puts '"Austras koks" - Latvijas tālākais austrumu punkts [' + coordinates[0].to_s + ',' + coordinates[1].to_s + '] => ' + test_case(coordinates) + ' => ' + test_case2(coordinates, [754190.00, 232806.00])
coordinates = [55.675228242509, 26.580528487143]
puts '"Saules puķe" - Latvijas tālākais dienvidu punkts [' + coordinates[0].to_s + ',' + coordinates[1].to_s + '] => ' + test_case(coordinates) + ' => ' + test_case2(coordinates, [662269.00, 172953.00])
coordinates = [56.377008455189, 20.979185882058]
puts '"Zaļais stars" - Latvijas galējais rietumu punkts [' + coordinates[0].to_s + ',' + coordinates[1].to_s + '] => ' + test_case(coordinates) + ' => ' + test_case2(coordinates, [313470.00, 252137.00])
