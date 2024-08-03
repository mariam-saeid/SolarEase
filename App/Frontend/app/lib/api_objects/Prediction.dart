import 'dart:async';
import 'package:app/api_objects/url.dart';
import 'package:app/homeuser.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';

////////////////////////////////
class Hour {
  String start;
  num total;
  Hour({required this.start, required this.total});
}

///////////////////////////////
class Day {
  String name;
  num totalEnergy;
  Day({required this.name, required this.totalEnergy});
}

////////////////////////////////
class ApiService {
  final Dio _dio = Dio();
  String _token = user.useKey!;

  ApiService() {
    _dio.interceptors.add(
      InterceptorsWrapper(
        onRequest: (options, handler) {
          if (_token != null) {
            options.headers['Authorization'] = 'Bearer $_token';
          }
          return handler.next(options); // Continue with the request
        },
        onError: (DioError e, handler) {
          // Handle errors globally
          print(e.response?.data);
          return handler.next(e); // Continue with the error
        },
      ),
    );
  }
}

/***********************api for get Five Days ************************/
Future<List<Day>> getFiveDays() async {
  ApiService a = ApiService();
  List<Day> days = [];
  int id = user.id!;
  print("prediction***************" + id.toString());

  try {
    var response = await a._dio.get(
        hosturl+"api/DailyEnergyPrediction/DailyPredictions/$id");

    for (var day in response.data) {
      Day temp = Day(name: '', totalEnergy: 0);
      temp.name = day['date'];
      temp.totalEnergy = day['predictedEnergy'];
      days.add(temp);
    }
    return days;
  } catch (e) {
    print(e);
    return [];
  }
}

/************************api for get hours of 5 Days*************************/
Future<List<Hour>> getHours() async {
  ApiService a = ApiService();
  List<Hour> hours = [];
  int id = user.id!;

  print("iam waiting hours");
  try {
    var response = await a._dio.get(
        hosturl+"api/HourlyEnergyPrediction/GetHourlyPrediction/$id");
    for (var hour in response.data) {
      Hour temp = Hour(start: "0", total: 0);
      temp.start = hour['hour'];
      temp.total = hour['predictedEnergy'];
      hours.add(temp);
    }

    return hours;
  } catch (e) {
    print(e);
    return [];
  }
}

/********************api for high value*****************/
Future<int> highLoad() async {
  ApiService a = ApiService();
  int id = user.id!;
  try {
    var response = await a._dio.get(
        hosturl+"api/DailyEnergyPrediction/HighEnergy/$id");
    print(response.data);
    return response.data;
  } catch (e) {
    print(e);
    return 0;
  }
}
