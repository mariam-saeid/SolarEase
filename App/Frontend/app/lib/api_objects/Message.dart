import 'package:app/api_objects/url.dart';
import 'package:app/homeuser.dart';
import 'package:dio/dio.dart';

class Message {
  String title;
  String body;
  String time;
  Message({required this.body, required this.time, required this.title});
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
          // Handle errors globally
          print(e.response?.data);
          return handler.next(e); // Continue with the error
        },
      ),
    );
  }
}

/***********************api for get all messages*****************************/
Future<bool> allMessages(List<Message> messages) async {
  ApiService a = ApiService();
  messages.clear();

  int? id = user.id;

  try {
    var response = await a._dio.get(
      hosturl+"api/Messages/ByPerson/$id",
    );

    for (var m in response.data) {
      Message temp =
          Message(title: m["title"], body: m['body'], time: m['sentDate']);
      messages.add(temp);
    }
    return true;
  } catch (e) {
    print(e);
    return false;
  }
}
