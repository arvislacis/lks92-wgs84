#!/usr/bin/env ruby
# -*- coding: utf-8 -*-

class LKS92WGS84
  # Koordinātu pārveidojumos izmantotās konstantes
  CPI = Math::PI                      # Skaitlis pi
  A_AXIS = 6378137                    # Elipses modeļa lielā ass (a)
  B_AXIS = 6356752.31414              # Elipses modeļa mazā ass (b)
  CENTRAL_MERIDIAN = CPI * 24 / 180   # Centrālais meridiāns
  OFFSET_X = 500000                   # Koordinātu nobīde horizontālās (x) ass virzienā
  OFFSET_Y = -6000000                 # Koordinātu nobīde vertikālās (y) ass virzienā
  SCALE = 0.9996                      # Kartes mērogojuma faktors (reizinātājs)

  def self.get_arc_length_of_meridian(phi)
    n = (A_AXIS - B_AXIS) / (A_AXIS + B_AXIS)
    alpha = ((A_AXIS + B_AXIS) / 2) * (1 + (n ** 2 / 4) + (n ** 4 / 64))
    beta = (-3 * n / 2) + (9 * n ** 3 / 16) + (-3 * n ** 5 / 32)
    gamma = (15 * n ** 2 / 16) + (-15 * n ** 4 / 32)
    delta = (-35 * n ** 3 / 48) + (105 * n ** 5 / 256)
    epsilon = (315 * n ** 4 / 512)

    alpha * (phi + (beta * Math.sin(2 * phi)) + (gamma * Math.sin(4 * phi)) + (delta * Math.sin(6 * phi)) + (epsilon * Math.sin(8 * phi)))
  end

  def self.get_footpoint_latitude(y)
    n = (A_AXIS - B_AXIS) / (A_AXIS + B_AXIS)
    alpha = ((A_AXIS + B_AXIS) / 2) * (1 + (n ** 2 / 4) + (n ** 4 / 64))
    yd = y / alpha
    beta = (3 * n / 2) + (-27 * n ** 3 / 32) + (269 * n ** 5 / 512)
    gamma = (21 * n ** 2 / 16) + (-55 * n ** 4 / 32)
    delta = (151 * n ** 3 / 96) + (-417 * n ** 5 / 128)
    epsilon = (1097 * n ** 4 / 512)

    yd + (beta * Math.sin(2 * yd)) + (gamma * Math.sin(4 * yd)) + (delta * Math.sin(6 * yd)) + (epsilon * Math.sin(8 * yd))
  end

  def self.convert_map_lat_lng_to_xy(phi, lambda1, lambda0)
    xy = [0, 0]

    ep2 = (A_AXIS ** 2 - B_AXIS ** 2) / B_AXIS ** 2
    nu2 = ep2 * Math.cos(phi) ** 2
    n = A_AXIS ** 2 / (B_AXIS * Math.sqrt(1 + nu2))
    t = Math.tan(phi)
    t2 = t * t

    l = lambda1 - lambda0
    l3coef = 1 - t2 + nu2
    l4coef = 5 - t2 + 9 * nu2 + 4 * (nu2 * nu2)
    l5coef = 5 - 18 * t2 + (t2 * t2) + 14 * nu2 - 58 * t2 * nu2
    l6coef = 61 - 58 * t2 + (t2 * t2) + 270 * nu2 - 330 * t2 * nu2
    l7coef = 61 - 479 * t2 + 179 * (t2 * t2) - (t2 * t2 * t2)
    l8coef = 1385 - 3111 * t2 + 543 * (t2 * t2) - (t2 * t2 * t2)

    # x koordināta
    xy[0] = n * Math.cos(phi) * l + (n / 6 * Math.cos(phi) ** 3 * l3coef * l ** 3) + (n / 120 * Math.cos(phi) ** 5 * l5coef * l ** 5) + (n / 5040 * Math.cos(phi) ** 7 * l7coef * l ** 7)

    # y koordināta
    xy[1] = get_arc_length_of_meridian(phi) + (t / 2 * n * Math.cos(phi) ** 2 * l ** 2) + (t / 24 * n * Math.cos(phi) ** 4 * l4coef * l ** 4) + (t / 720 * n * Math.cos(phi) ** 6 * l6coef * l ** 6) + (t / 40320 * n * Math.cos(phi) ** 8 * l8coef * l ** 8)

    xy
  end

  def self.convert_map_xy_to_lat_lon(x, y, lambda0)
    lat_lng = [0, 0]

    phif = get_footpoint_latitude(y)
    ep2 = (A_AXIS ** 2 - B_AXIS ** 2) / B_AXIS ** 2
    cf = Math.cos(phif)
    nuf2 = ep2 * cf ** 2
    nf = A_AXIS ** 2 / (B_AXIS * Math.sqrt(1 + nuf2))
    nfpow = nf

    tf = Math.tan(phif)
    tf2 = tf * tf
    tf4 = tf2 * tf2

    x1frac = 1 / (nfpow * cf)

    nfpow *= nf   # Nf^2
    x2frac = tf / (2 * nfpow)

    nfpow *= nf   # Nf^3
    x3frac = 1 / (6 * nfpow * cf)

    nfpow *= nf   # Nf^4
    x4frac = tf / (24 * nfpow)

    nfpow *= nf   # Nf^5
    x5frac = 1 / (120 * nfpow * cf)

    nfpow *= nf   # Nf^6
    x6frac = tf / (720 * nfpow)

    nfpow *= nf   # Nf^7
    x7frac = 1 / (5040 * nfpow * cf)

    nfpow *= nf   # Nf^8
    x8frac = tf / (40320 * nfpow)

    x2poly = -1 - nuf2
    x3poly = -1 - 2 * tf2 - nuf2
    x4poly = 5 + 3 * tf2 + 6 * nuf2 - 6 * tf2 * nuf2 - 3 * (nuf2 * nuf2) - 9 * tf2 * (nuf2 * nuf2)
    x5poly = 5 + 28 * tf2 + 24 * tf4 + 6 * nuf2 + 8 * tf2 * nuf2
    x6poly = -61 - 90 * tf2 - 45 * tf4 - 107 * nuf2 + 162 * tf2 * nuf2
    x7poly = -61 - 662 * tf2 - 1320 * tf4 - 720 * (tf4 * tf2)
    x8poly = 1385 + 3633 * tf2 + 4095 * tf4 + 1575 * (tf4 * tf2)

    # Ģeogrāfiskais platums
    lat_lng[0] = phif + x2frac * x2poly * (x * x) + x4frac * x4poly * x ** 4 + x6frac * x6poly * x ** 6 + x8frac * x8poly * x ** 8

    # Ģeogrāfiskais garums
    lat_lng[1] = lambda0 + x1frac * x + x3frac * x3poly * x ** 3 + x5frac * x5poly * x ** 5 + x7frac * x7poly * x ** 7

    lat_lng
  end

  def self.convert_lat_lon_to_xy(coordinates)
    lat = coordinates[0] * CPI / 180
    lng = coordinates[1] * CPI / 180
    xy = convert_map_lat_lng_to_xy(lat, lng, CENTRAL_MERIDIAN)

    xy[0] = xy[0] * SCALE + OFFSET_X
    xy[1] = xy[1] * SCALE + OFFSET_Y

    if xy[1] < 0
      xy[1] += 10000000
    end

    xy
  end

  def self.convert_xy_to_lat_lon(coordinates)
    x = (coordinates[0] - OFFSET_X) / SCALE
    y = (coordinates[1] - OFFSET_Y) / SCALE
    lat_lng = convert_map_xy_to_lat_lon(x, y, CENTRAL_MERIDIAN)

    lat_lng[0] = lat_lng[0] / CPI * 180
    lat_lng[1] = lat_lng[1] / CPI * 180

    lat_lng
  end

  private_class_method :get_arc_length_of_meridian
  private_class_method :get_footpoint_latitude
  private_class_method :convert_map_lat_lng_to_xy
  private_class_method :convert_map_xy_to_lat_lon
end