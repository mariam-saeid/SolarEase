import 'package:app/api_objects/url.dart';
import 'package:app/homeuser.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';

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

class FavoritePostmodel {
  int ID;
  String FImage;
  String FCategory;
  String Fprice;
  String RemovedDate;
  String Flocation;
  bool PostStatus;

  FavoritePostmodel({
    required this.ID,
    required this.FCategory,
    required this.RemovedDate,
    required this.PostStatus,
    required this.FImage,
    required this.Flocation,
    required this.Fprice,
  });

  factory FavoritePostmodel.fromJson(json) {
    return FavoritePostmodel(
        ID: json['id'],
        FCategory: json['categoryName'],
        RemovedDate: json['createdOn'],
        FImage: json['productImageUrl'],
        Flocation: json['city'],
        Fprice: json['priceStr'],
        PostStatus: json['active']);
  }
}

class FavoriteService {
  final Dio dio;

  FavoriteService(this.dio);
  int ID = user.id!;

  //String sortBy = 'd'; //sortBy=$sortBy
  Future<List<FavoritePostmodel>> getFavoritePosts(
      List<FavoritePostmodel> listAllPosts) async {
    try {
      Response response = await a._dio
          .get(hosturl+'api/FavoritePosts/ByPerson/$ID');
      List<dynamic> jsonData = response.data;
      listAllPosts = [];

      for (var item in jsonData) {
        FavoritePostmodel removedPost = FavoritePostmodel.fromJson(item);
        listAllPosts.add(removedPost);
        print(response.statusCode);
      }
      print(listAllPosts);
      return listAllPosts;
    } on DioException catch (e) {
      throw Exception("Oops, there was an dio error, try later");
    } catch (e) {
      throw Exception("Oops, there was an error, try later");
    }
  }

}
