import 'dart:io';
import 'package:app/api_objects/Button.dart';
import 'package:app/api_objects/url.dart';
import 'package:app/filterpost.dart';
import 'package:app/homeuser.dart';
import 'package:dio/dio.dart';

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
          print("losttttttttttttttttttttt");
          print(e.response?.data);
          return handler.next(e); // Continue with the error
        },
      ),
    );
  }
}


//////////////////////////////////////////////////////////////////////
class Post {
  Dio dio = Dio();
  int? id;
  bool isusedChange;
  String categoryChange;
  double priceChange;
  File? imageFile;
  String brandChange;
  double capacityChange;
  String unitChange;
  double? voltChange;
  String locationChange;
  String? phone;
  String photo = hosturl+"ProfileImages/profile.png";
  String cityChange;
  String descriptionChange;
  String? ownerPhoto =
      hosturl+"ProfileImages/profile.png";
  String? date;
  String? ownerName;
  Post(
      {required this.brandChange,
      required this.capacityChange,
      required this.categoryChange,
      required this.cityChange,
      required this.descriptionChange,
      required this.imageFile,
      required this.isusedChange,
      required this.locationChange,
      required this.priceChange,
      required this.unitChange,
      required this.voltChange});



/*********************************api create post*************************/
  Future<String> createPost() async {
    int ID = user.id!;
    // print("*************************************************************");
    // print("is used" + isusedChange.toString());
    // print("categoryChange" + categoryChange.toString());
    // print("priceChange" + priceChange.toString());
    // print("brandChange" + brandChange.toString());
    // print("unitChange" + unitChange.toString());
    // print("voltChange" + voltChange.toString());
    // print("locationChange" + locationChange.toString());
    // print("cityChange" + cityChange.toString());
    // print("descriptionChange" + descriptionChange.toString());
    // print("image" + imageFile.toString());
    // print("capcity" + capacityChange.toString());
    ApiService a = ApiService();
    try {
      var response = await a._dio
          .post(hosturl+"api/Posts/Create/$ID",
              data: FormData.fromMap({
                "CategoryName": this.categoryChange,
                "Price": this.priceChange,
                "Location": this.locationChange,
                "City": this.cityChange,
                "IsUsed": this.isusedChange,
                "Description": this.descriptionChange,
                "Brand": this.brandChange,
                "Capacity": this.capacityChange,
                "MeasuringUnit": this.unitChange,
                "Volt": (categoryChange == "Battery") ? this.voltChange : null,
                "ProductImageUrl": await MultipartFile.fromFile(imageFile!.path,
                    filename: imageFile?.path.split('/').last),
              }));

      return 'Ok';
    } on DioException catch (e) {
      print(e.response);
      if (e.response?.statusCode == 400) {
        return e.response?.data['errors'].values.first[0];
      }
      return "Oops! Something went wrong.";
    }
  }

/*****************************api edit*****************************/
  Future<String> edditPost(int postid) async {
    // print("from hereeeeeeeeeeeeeeeeeeeeeeeeeee");
    // print("is used" + isusedChange.toString());
    // print("categoryChange" + categoryChange.toString());
    // print("priceChange" + priceChange.toString());
    // print("brandChange" + brandChange.toString());
    // print("unitChange" + unitChange.toString());
    // print("voltChange" + voltChange.toString());
    // print("locationChange" + locationChange.toString());
    // print("cityChange" + cityChange.toString());
    // print("descriptionChange" + descriptionChange.toString());
    // print("image" + imageFile.toString());
    // print("capcity" + capacityChange.toString());
    // print(imageFile);
    ApiService a = ApiService(); //lock authurization for now
    try {
      var response = await a._dio
          .put(hosturl+"api/Posts/Update/$postid",
              data: FormData.fromMap({
                "CategoryName": this.categoryChange,
                "Price": this.priceChange,
                "Location": this.locationChange,
                "City": this.cityChange,
                "IsUsed": this.isusedChange,
                "Description": this.descriptionChange,
                "Brand": this.brandChange,
                "Capacity": this.capacityChange,
                "MeasuringUnit": this.unitChange,
                "Volt": (categoryChange == "Battery") ? this.voltChange : null,
                "ProductImageUrl": imageFile == null
                    ? null
                    : await MultipartFile.fromFile(imageFile!.path,
                        filename: imageFile!.path.split('/').last),
              }));

      return "Ok";
    }
     on DioException catch (e) {
      print(e.response);
      if (e.response?.statusCode == 400) {
        return e.response?.data['errors'].values.first[0];
      }
      return "Oops! Something went wrong.";
    }
  }

/**************************************api getpost*********************************/
  Future<String> getPost(int postId) async {
    ApiService a = ApiService(); //lock authurization for now
    try {
      var response = await a._dio.get(
        hosturl+"api/Posts/Info/$postId",
      );
      print(response);
      this.id = response.data["id"];
      this.isusedChange = response.data["isUsed"];

      this.categoryChange = response.data["categoryName"];

      this.priceChange = response.data["price"].toDouble();
      this.brandChange = response.data["brand"];

      this.capacityChange = response.data["capacity"].toDouble();
      this.unitChange = response.data["measuringUnit"];

      if (this.categoryChange == "Battery")
        this.voltChange = response.data["volt"].toDouble();
      this.locationChange = response.data["location"];
      this.phone = response.data["phoneNumber"];
      this.ownerPhoto =
          hosturl + response.data["profileImageUrl"];
      this.date = response.data["createdOn"];
      this.ownerName = response.data["personName"];
      this.photo =
          hosturl + response.data["productImageUrl"];
      print(this.photo);
      this.cityChange = response.data["city"];
      if (response.data["description"] != null)
        this.descriptionChange = response.data["description"];
      else {
        this.descriptionChange = "";
      }
      print(this.id);
      return "Ok";
    } catch (e) {
      print(e.toString());
      return "Opps! Someyhing went wrong.";
    }
  }
}

class ApiServicef {
  final Dio _dio = Dio();

  ApiServicef() {
    String _token = userf.useKey!;
    _dio.interceptors.add(
      InterceptorsWrapper(
        onRequest: (options, handler) {
          if (_token != null) {
            options.headers['Authorization'] = 'Bearer $_token';
          }
          return handler.next(options); 
        },
        onError: (DioError e, handler) {
          print(e.response?.data);
          return handler.next(e);
        },
      ),
    );
  }
}

/********************************Get posts for admin to filter***************************************** */
Future<String> getPoststoFilter(List<Post> posts) async {
  ApiServicef a = ApiServicef(); //lock authurization for now
  posts.clear();
  Dio dio = Dio();
  try {
    var response = await a._dio
        .get(hosturl+"api/Posts/InActive?sortBy=a");
    print(response);
    for (var p in response.data) {
      Post temp = Post(
          brandChange: '',
          capacityChange: 0,
          categoryChange: '',
          cityChange: '',
          descriptionChange: '',
          imageFile: null,
          isusedChange: true,
          locationChange: '',
          priceChange: 0,
          unitChange: '',
          voltChange: 0);
      temp.id = p['id'];
      temp.isusedChange = p["isUsed"];

      temp.categoryChange = p["categoryName"];

      temp.priceChange = p["price"].toDouble();

      temp.brandChange = p["brand"];

      temp.capacityChange = p["capacity"].toDouble();
      temp.unitChange = p["measuringUnit"];

      if (temp.categoryChange == "Battery")
        temp.voltChange = p["volt"].toDouble();
      temp.locationChange = p["location"];
      temp.phone = p["phoneNumber"];
      temp.ownerPhoto = hosturl + p["profileImageUrl"];
      temp.date = p["createdOn"];
      temp.ownerName = p["personName"];
      temp.photo = hosturl + p["productImageUrl"];

      temp.cityChange = p["city"];
      if (p["description"] != null) temp.descriptionChange = p["description"];
      posts.add(temp);
    }
    return "Ok";
  } catch (e) {
    print(e);
    return "Oops! Something went wrong.";
  }
}

/********************************** api for reject post****************************************** */
Future<String> rejectPost(int id) async {
  ApiServicef a = ApiServicef(); //lock authurization for now
  Dio dio = Dio();
  try {
    var response = await a._dio
        .delete(hosturl+"api/Posts/RejectPost/$id");
    print("rejected******************************");
    return "Ok";
  } catch (e) {
    print(e);
    return "Oops! Something went wrong.";
  }
}

/******************api for accept post******************************/
Future<String> acceptPost(int id) async {
  ApiServicef a = ApiServicef(); //lock authurization for now
  Dio dio = Dio();
  try {
    var response = await a._dio
        .put(hosturl+"api/Posts/ApprovePost/$id");
    print("Accept******************************");
    return "Ok";
  } catch (e) {
    print(e);
    return "Oops! Something went wrong.";
  }
}
