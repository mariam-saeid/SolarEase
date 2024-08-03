import 'package:app/api_objects/url.dart';
import 'package:app/favorite/APIService/ProductModel.dart';
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
          print(e.response?.data);
          return handler.next(e); // Continue with the error
        },
      ),
    );
  }
}

ApiService a = ApiService();

class PanelService {
  final Dio dio;
  PanelService(this.dio);
  String query = 'Panel';
  Future<List<FavoritePanel>> getFavoritePanel() async {
    int ID = user.id!;

    try {
      Response response = await a._dio.get(
          hosturl+'api/FavoriteProducts/ByPerson/$ID?query=$query');
      List<dynamic> jsonData = response.data;

      List<FavoritePanel> PanelList = [];

      for (var item in jsonData) {
        FavoritePanel panelModel = FavoritePanel.fromJson(item);
        PanelList.add(panelModel);
      }

      return PanelList;
    } on DioException catch (e) {
      throw Exception("Oops, there was an error, try later");
    } catch (e) {
      throw Exception("Oops, there was an error, try later");
    }
  }

// ------------ ------------------ ----------------------------

  Future<void> ToggleFavorite(int ID) async {
    // print(ID);
    int userID = user.id!;

    try {
      final response = await a._dio.put(
        hosturl+'api/FavoriteProducts/ToggleFavorite/$userID/$ID', // Replace with your API endpoint
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
// ------  --------------- ------------------------ --------------------------

class BatteryService {
  final Dio dio;
  BatteryService(this.dio);
  String query = 'Battery';
  int ID = user.id!;

  Future<List<FavoriteBattery>> getFavoriteBattery() async {
    try {
      Response response = await a._dio.get(
          hosturl+'api/FavoriteProducts/ByPerson/$ID?query=$query');
      List<dynamic> jsonData = response.data;

      List<FavoriteBattery> BatteryList = [];

      for (var item in jsonData) {
        FavoriteBattery favoriteBattery = FavoriteBattery.fromJson(item);
        BatteryList.add(favoriteBattery);
      }

      return BatteryList;
    } on DioException catch (e) {
      throw Exception("Oops, there was an error, try later");
    } catch (e) {
      throw Exception("Oops, there was an error, try later");
    }
  }
}

//- -----------------------------------------------------------------------------------

class InverterService {
  final Dio dio;
  InverterService(this.dio);
  String query = 'Inverter';
  int ID = user.id!;

  Future<List<FavoriteInverter>> getFavoriteInverter() async {
    try {
      Response response = await a._dio.get(
          hosturl+'api/FavoriteProducts/ByPerson/$ID?query=$query');

      // Assuming response.data is a List of maps
      List<dynamic> jsonData = response.data;

      List<FavoriteInverter> PanelList = [];

      for (var item in jsonData) {
        FavoriteInverter favoriteInverter = FavoriteInverter.fromJson(item);
        PanelList.add(favoriteInverter);
      }

      return PanelList;
    } on DioException catch (e) {
      throw Exception("Oops, there was an error, try later");
    } catch (e) {
      throw Exception("Oops, there was an error, try later");
    }
  }
}
