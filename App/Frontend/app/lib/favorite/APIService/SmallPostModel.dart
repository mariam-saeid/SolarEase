import 'package:app/api_objects/url.dart';
import 'package:app/homeuser.dart';
import 'package:dio/dio.dart';

class Smallpostmodel {
  int PostID;
  String UserProfileImage;
  String UserName;
  String date;
  String ProductImage;
  String ProductName;
  String ProductPrice;
  String City;
  bool isFavorite;

  Smallpostmodel({
    required this.PostID,
    required this.UserProfileImage,
    required this.UserName,
    required this.date,
    required this.ProductImage,
    required this.ProductName,
    required this.ProductPrice,
    required this.City,
    required this.isFavorite,
  });

  factory Smallpostmodel.fromJson(json) {
    return Smallpostmodel(
      PostID: json['id'],
      UserProfileImage: json['profileImageUrl'],
      UserName: json['personName'],
      date: json['createdOn'],
      ProductImage: json['productImageUrl'],
      ProductName: json['compositeName'],
      ProductPrice: json['priceStr'],
      City: json['city'],
      isFavorite: json['isFavorite'],
    );
  }
}

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

class SmallPostService {

  final Dio dio;
  SmallPostService(this.dio);
  String sortBy = 'd'; //sortBy=$sortBy
  int ID = user.id!;

  Future<List<Smallpostmodel>> getSmallPosts(String category) async {
    print(category);
    try {
      Response response = await a._dio.get(
          hosturl+'api/Posts/SmallPosts/$ID?sortBy=d&categoryQuery=$category&cityQuery=');

      List<dynamic> jsonData = response.data;
      List<Smallpostmodel> listOfPosts = [];

      for (var item in jsonData) {
        Smallpostmodel smallpostmodel = Smallpostmodel.fromJson(item);
        listOfPosts.add(smallpostmodel);
      }

      return listOfPosts;
    } on DioException catch (e) {
      throw Exception("Oops, there was an error, try later");
    } catch (e) {
      throw Exception("Oops, there was an error, try later");
    }
  }
}
