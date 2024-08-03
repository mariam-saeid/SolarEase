import 'package:app/api_objects/url.dart';
import 'package:app/homeuser.dart';
import 'package:dio/dio.dart';
import 'package:path/path.dart';

////////////////////////////////////////////////////
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
ApiService a = ApiService();

/*****************api to get initial chat and buttons************************/
Future<String> sendText(String Question) async {
  int id = user.id!;

  try {
    var response = await a._dio.post(
        hosturl+"api/ChatbotMessages/Answer/$id",
        data: {"question": Question});
    print(response.data);
    return response.data["answer"];
  } catch (e) {
    print(e);
    return "Error";
  }
}
