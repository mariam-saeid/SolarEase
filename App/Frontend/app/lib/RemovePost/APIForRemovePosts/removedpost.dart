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

class RemovedPost {
  int ID;
  String RemovedImage;
  String RemovedCategory;
  String RemovedPrice;
  String RemovedDate;
  String RemovedLocation;
  bool PostStatus;

  RemovedPost({
    required this.ID,
    required this.RemovedCategory,
    required this.RemovedDate,
    required this.PostStatus,
    required this.RemovedImage,
    required this.RemovedLocation,
    required this.RemovedPrice,
  });

  factory RemovedPost.fromJson(json) {
    return RemovedPost(
        ID: json['id'],
        RemovedCategory: json['categoryName'],
        RemovedDate: json['createdOn'],
        RemovedImage: json['productImageUrl'],
        RemovedLocation: json['city'],
        RemovedPrice: json['priceStr'],
        PostStatus: json['active']);
  }
}

class RemovedService {
  final Dio dio;
  RemovedService(this.dio);
  //String sortBy = 'd'; //sortBy=$sortBy
  int ID = user.id!;

  Future<List<RemovedPost>> getAllPosts() async {
    try {
      Response response = await a._dio.get(
          hosturl+'api/Posts/PersonInfo/$ID?sortBy=a');
      print(response);
      List<dynamic> jsonData = response.data;
      List<RemovedPost> listAllPosts = [];

      for (var item in jsonData) {
        RemovedPost removedPost = RemovedPost.fromJson(item);
        listAllPosts.add(removedPost);
        print(response.statusCode);
      }
      return listAllPosts;
    } on DioException catch (e) {
      throw Exception("Oops, there was an dio error, try later");
    } catch (e) {
      throw Exception("Oops, there was an error, try later");
    }
  }

  // -----------------------------

  Future<void> deletePost(int id) async {
    final String url = hosturl+'api/Posts/Delete/$id';
    try {
      await a._dio.delete(url);

      print("code run succesfully");
    } on DioException catch (e) {
      throw Exception("Oops, there was a Dio error, try later");
    } catch (e) {
      throw Exception("Oops, there was an error, try later");
    }
  }

  // ----------------------------------
}
