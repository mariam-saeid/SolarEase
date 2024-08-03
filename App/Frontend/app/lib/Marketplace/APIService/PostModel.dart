import 'package:app/api_objects/url.dart';
import 'package:app/homeuser.dart';
import 'package:dio/dio.dart';


class UserPostModel {
  final String UserName;
  final String PostDate;
  final String UserImage;
  final bool IsUsed;
  final String PostBrand;
  final String PostCategory;
  final String? Volume;
  final String PostCapacity;
  final String PostPrice;
  final String PhoneNumber;
  final String Location;
  final String? Description;
  final String PostImage;
  UserPostModel(
      {required this.UserName,
      required this.PostDate,
      required this.UserImage,
      required this.PhoneNumber,
      required this.PostImage,
      required this.IsUsed,
      required this.PostCategory,
      required this.PostCapacity,
      required this.PostBrand,
      required this.PostPrice,
      required this.Location,
      required this.Description,
      required this.Volume});

  factory UserPostModel.fromJson(json) {
    return UserPostModel(
        UserName: json['personName'],
        PostDate: json['createdOn'],
        UserImage: json['profileImageUrl'],
        PhoneNumber: json['phoneNumber'],
        PostImage: json['productImageUrl'],
        IsUsed: json['isUsed'],
        PostCategory: json['categoryName'],
        PostCapacity: json['capacityStr'],
        PostBrand: json['brand'],
        PostPrice: json['priceStr'],
        Location: json['compositeLocation'],
        Description: json['description'],
        Volume: json['voltStr']);
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

class UserPostService {
  final Dio dio;
  UserPostService(this.dio);
 
  Future<void> AddFvoritePost(int postId) async {
    print(postId);
    int ID = user.id!;
    try {
      final response = await a._dio.put(
        hosturl+'api/FavoritePosts/ToggleFavorite/$ID/$postId', // Replace with your API endpoint
      );
      print(response.statusCode);
      if (response.statusCode == 200) {
        // Handle successful response
        print('Favorite status updated successfully');
      } else {
        // Handle other status codes
        print('Failed to update favorite status');
      }
    } catch (e) {
      // Handle error
      print('Error updating favorite status: $e');
    }
  }
}
