import 'package:app/api_objects/url.dart';
import 'package:dio/dio.dart';

/**************************************************************/
Future<String> checkEmail(String email) async {
  Dio dio = Dio();
  try {
    var response = await dio.post(
        hosturl+"api/AuthPersons/ForgetPassword/",
        data: {'Email': email});

    return "Ok";
  } on DioException catch (e) {
    print(e.response);
    if (e.response?.statusCode == 500) {
      return "Oops! Something went wrong";
    }
    return "Email not valid";
  }
}

/******************api for check code****************************/
Future<String> checkCode(String code, String email) async {
  Dio dio = Dio();
  try {
    var response = await dio.post(
        hosturl+"api/AuthPersons/ValidateCode/",
        data: {"ValidationCode": code, 'Email': email});

    return "Ok";
  } on DioException catch (e) {
    print(e.response);
    if (e.response?.statusCode == 401) {
      return e.response!.data.toString();
    }
    return "Oops! Something went wrong.";
  }
}

/**********************api for reset*************************/
Future<String> Reset(String password, String email) async {
  Dio dio = Dio();
  try {
    var response = await dio.put(
        hosturl+"api/AuthPersons/ResetPassword/",
        data: {'Password': password, 'Email': email});

    return "Ok";
  } on DioException catch (e) {
    print(e.response);
    if(e.response!.statusCode==500)
    {
      return e.response?.data[0]['description'].replaceAll('Username', 'Email');
    }
     
    return "Oops! Something went wrong";
  }
}
