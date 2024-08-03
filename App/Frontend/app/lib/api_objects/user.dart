import 'dart:io';
import 'package:app/api_objects/url.dart';
import 'package:app/homeuser.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';

/////////////////////////////////////////////////////////////
class ApiService {
  final Dio _dio = Dio();

  ApiService() {
    String _token = user.useKey!;
    _dio.interceptors.add(
      InterceptorsWrapper(
        onRequest: (options, handler) {
          if (_token != null) {
            options.headers['Authorization'] = 'Bearer $_token';
          }
          return handler.next(options); 
        },
        onError: (DioError e, handler) {
          // Handle errors globally
          print(e.response?.data);
          return handler.next(e); 
        },
      ),
    );
  }
}

/////////////////////////////////////////////////////////
class User {
  Dio dio = Dio();
  String username;
  String password;
  String email;
  String city;
  String location;
  String phoneNumber;
  String? useKey;
  int? id;
  String? systemsize;
  String image = hosturl+"ProfileImages/profile.png";

  User({
    required this.username,
    required this.password,
    required this.email,
    required this.location,
    required this.phoneNumber,
    required this.city,
  });
  //////////////////////////////////////
  bool isempty() {
    if (this.username != "" &&
        this.password != "" &&
        this.email != "" &&
        this.location != "" &&
        this.phoneNumber != "" &&
        this.city != "")
      return false;
    else
      return true;
  }
  ///////////////////////////////////////
  Map<String, dynamic> tojsonLogin() {
    return {
      "email": this.email,
      "password": this.password,
    };
  }
 ////////////////////////////////////////
  Map<String, dynamic> tojsonSignup() {
    return {
      "Email": this.email,
      "Password": this.password,
      'Name': this.username,
      "Location": this.location,
      'City': this.city,
      "PhoneNumber": this.phoneNumber
    };
  }

  /******************api for signup***************************/
  Future<String> createuse() async {
    var response;
    try {
      print(this.email);
      print(this.password);
      print(this.username);
      print(this.location);
      print(this.city);
      print(this.phoneNumber);
      response = await dio.post(
          hosturl+"api/AuthPersons/Register/",
          data: FormData.fromMap(tojsonSignup()));
      return "Ok";
    } on DioException catch (e) {
      print(e.response);
      if (e.response?.statusCode == 400) {
        return e.response?.data['errors'].values.first[0]
            .replaceAll('Username', 'Email');
      } else if (e.response?.statusCode == 500){
        return e.response?.data[0]['description']
            .replaceAll('Username', 'Email');
      }
        return "Oops! Something went wrong.";
    }
  }

  /******************api for login***************************/
  Future<String> checkuser() async {
    try {
      print(this.password);
      print(this.email);
      var response = await dio.post(
          hosturl +"api/AuthPersons/Login/",
          data: tojsonLogin());
      this.username = response.data["name"];
      this.location = response.data["location"];
      this.phoneNumber = response.data["phoneNumber"];
      this.city = response.data["city"];
      this.useKey = response.data["jwtToken"];
      this.id = response.data["id"];
      this.systemsize = response.data["systemSize"].toString();
      this.image= hosturl + response.data['imageUrl'];

      return "Ok";
    } on DioException catch (e) {
      print(e.response);
      if (e.response?.statusCode == 400) {
        return e.response?.data['errors'].values.first[0];
      } else if (e.response?.statusCode == 401){
        return e.response.toString();
      }
        return "Oops! Something went wrong.";
    }
  }

/*********************api for login gor admin**************************/
  Future<String> checkAdmin() async {
    try {
      print(this.password);
      print(this.email);
      var response = await dio.post(
          hosturl+"api/AuthAdmins/Login",
          data: tojsonLogin());
      this.username = response.data["name"];
      this.location = response.data["location"];
      this.phoneNumber = response.data["phoneNumber"];
      this.city = response.data["city"];
      this.useKey = response.data["jwtToken"];
      this.id = response.data["id"];
      this.image= hosturl + response.data['imageUrl'];

      return "Ok";
    } on DioException catch (e) {
      print(e.response);
        if (e.response?.statusCode == 400) {
        return e.response?.data['errors'].values.first[0];
      } else if (e.response?.statusCode == 401) {
        return e.response.toString();
      }
      return "Oops! Something went wrong.";
    }
  }

  /****************************api for update user profile************************/
  Future<String> updateUserData(
      File? image,
      String name,
      String email,
      String? password,
      String phone,
      String location,
      String systemsize,
      String city,
      bool update) async {
    ApiService a = ApiService();
    try {
      print(image);
      print(email);
      print(password);
      print(name);
      print(location);
      print(city + '****');
      print(phone);
      print(systemsize);
      print(user.id);

      var response = await a._dio
          .put(hosturl+"api/Persons/Update/$id",
              data: FormData.fromMap({
                "Email": email,
                "Password": password,
                "Name": name,
                "Location": location,
                "City": city,
                "PhoneNumber": phone,
                "SystemSize": systemsize,
                "Image": image == null
                    ? null
                    : await MultipartFile.fromFile(image.path,
                        filename: image.path.split('/').last),
                "ImageUpdated": update
              }));
      user.username = name;
      user.email = email;
      user.phoneNumber = phone;
      user.location = location;
      user.city = city;
      user.systemsize = systemsize;
        user.image =
            hosturl + response.data['imageUrl'];

      return 'Ok';
    } on DioException catch (e) {
      print(e.response.toString() );
      if (e.response?.statusCode == 400) {
        return e.response?.data['errors'].values.first[0]
            .replaceAll('Username', 'Email');
      } else if (e.response?.statusCode == 500) {
        print(
            e.response?.data[0]['description'].replaceAll('Username', 'Email'));
        return e.response?.data[0]['description']
            .replaceAll('Username', 'Email');
      } else {
        return "Oops! Something went wrong.";
      }
    }
  }

/****************************delete user account*****************************/
  Future<bool> delete() async {
    var response;
    int id = user.id!;
    ApiService a = ApiService();
    try {
      response = await a._dio.delete(
          hosturl+"api/Persons/$id",
          data: FormData.fromMap(tojsonSignup()));
      return true;
    } on DioException catch (e) {
      print(e.response);
      return false;
    }
  }
}
