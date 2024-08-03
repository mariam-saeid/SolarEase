import 'dart:convert';
import 'package:app/api_objects/url.dart';
import 'package:app/homeuser.dart';
import 'package:dio/dio.dart';

class Company {
  Dio dio = Dio();
  String name;
  int id;
  List<String> phone;
  List<String> email;
  List<String> location;
  String city;
  num distanceFromUser;
  num latitude;
  num longitude;
  Company({
    required this.name,
    required this.id,
    required this.phone,
    required this.email,
    required this.location,
    required this.city,
    required this.distanceFromUser,
    required this.latitude,
    required this.longitude,
  });
}

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
          print(e.response?.data);
          return handler.next(e); // Continue with the error
        },
      ),
    );
  }
}

/****************api nearst************************/
Future<bool> getcomponies(
    {required List<Company> componies,
    String search = "",
    String filter = ""}) async {
  ApiService a = ApiService();
  componies.clear();

  int? id = user.id;
  try {
    var response = await a._dio.get(
        hosturl+"api/SolarInstallers/Sorted/$id",
        queryParameters: {
          "nameQuery": search,
          "cityQuery": filter,
        });

    for (Map<String, dynamic> com in response.data) {
      Company temp = Company(
          name: com["name"],
          id: com["id"],
          phone: com["phoneNumber"].split(';').toList(),
          email: com["email"].split(';').toList(),
          location: com["address"].split(';').toList(),
          city: com["city"],
          distanceFromUser: com["distanceFromUser"],
          latitude: com["latitude"],
          longitude: com["longitude"]);
      componies.add(temp);
    }

    return true;
  } catch (e) {
    print(e);
    return false;
  }
}
